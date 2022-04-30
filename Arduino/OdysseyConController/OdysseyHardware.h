#include <SPI.h>

const int RESET_SETTLE_TIME_MS = 2;

typedef struct spot_gen {
// Write pins
  const int x_vcc_pin;
  const int x_gnd_pin;
  const int y_vcc_pin;
  const int y_gnd_pin;
  const int eng_vcc_pin;
  const int eng_gnd_pin;
  const int digi_pot_xy_ss_pin;
  const uint8_t digi_pot_x_addr;
  const uint8_t digi_pot_y_addr;
  const int digi_pot_eng_ss_pin;
  const uint8_t digi_pot_eng_addr;
  const int reset_write_pin;
  const int crowbar_reset_pin;

// Read pins
  const int x_read;
  const int y_read;

// State flags
  bool writing; // Whether or not the player is being written or read
                // Gates somes functions to prevent unwanted pinMode changes
                // Used by audio effects to know which lines to check for being reset
                // to avoid playing sound effects when they should not be
  uint8_t english_pos;
} spot_gen;

spot_gen p1_spot = {
                     .x_vcc_pin = 49,
                     .x_gnd_pin = 48,
                     .y_vcc_pin = 47,
                     .y_gnd_pin = 46,
                     .eng_vcc_pin = 41,
                     .eng_gnd_pin = 40,
                     .digi_pot_xy_ss_pin = 37,
                     .digi_pot_x_addr = 0x00,
                     .digi_pot_y_addr = 0x10,
                     .digi_pot_eng_ss_pin = 35,
                     .digi_pot_eng_addr = 0x00,
                     .reset_write_pin = 33,
                     .crowbar_reset_pin = A14,
                     .x_read = A0,
                     .y_read = A1,
                     .writing = false,
                     .english_pos = 0,
};

spot_gen p2_spot = {
                     .x_vcc_pin = 45,
                     .x_gnd_pin = 44,
                     .y_vcc_pin = 43,
                     .y_gnd_pin = 42,
                     .eng_vcc_pin = 41,         // Intentionally the same as P1
                     .eng_gnd_pin = 40,         // Intentionally the same as P1
                     .digi_pot_xy_ss_pin = 36,
                     .digi_pot_x_addr = 0x00,
                     .digi_pot_y_addr = 0x10,
                     .digi_pot_eng_ss_pin = 35, // Intentionally the same as P1
                     .digi_pot_eng_addr = 0x00, // Intentionally the same as P1
                     .reset_write_pin = 34,
                     .crowbar_reset_pin = A15,
                     .x_read = A2,
                     .y_read = A3,
                     .writing = false,
                     .english_pos = 0,
};

#define READ_ONLY_SPOT_DECL(spot_name, x_analog, y_analog) \
  spot_gen spot_name = {                      \
    .x_vcc_pin = 0,                           \
    .x_gnd_pin = 0,                           \
    .y_vcc_pin = 0,                           \
    .y_gnd_pin = 0,                           \
    .eng_vcc_pin = 0,                         \
    .eng_gnd_pin = 0,                         \
    .digi_pot_xy_ss_pin = 0,                  \
    .digi_pot_x_addr = 0,                     \
    .digi_pot_y_addr = 0,                     \
    .digi_pot_eng_ss_pin = 0,                 \
    .digi_pot_eng_addr = 0,                   \
    .reset_write_pin = 0,                     \
    .crowbar_reset_pin = 0,                   \
    .x_read = x_analog,                       \
    .y_read = y_analog,                       \
    .writing = false,                         \
    .english_pos = 0,                         \
};

READ_ONLY_SPOT_DECL(p3_spot, A4, A5)
READ_ONLY_SPOT_DECL(p4_spot, A6, A7)
READ_ONLY_SPOT_DECL(ball_spot, A8, A9)
READ_ONLY_SPOT_DECL(wall_spot, A10, A10) // There is no y_read on the board

const int NOT_CROWBAR_READ        = A11; // Crowbar state
const int NOT_ENGLISH_WEAK_Q_READ = A12; // Flip flop state
const int NOT_BALL_WEAK_Q_READ    = A13; // Flip flop state

#define READ_CROWBAR           (analogRead(NOT_CROWBAR_READ) < 720)
#define READ_ENGLISH_FLIP_FLOP (analogRead(NOT_ENGLISH_WEAK_Q_READ) < 512)
#define READ_BALL_FLIP_FLOP    (analogRead(NOT_BALL_WEAK_Q_READ) < 512)

/**
 * Initialize the pins of a player so they can be written to
 * @param player
 */
void init_player_as_writeable(spot_gen *player) {
    pinMode(player->x_vcc_pin, OUTPUT);
    pinMode(player->x_gnd_pin, OUTPUT);
    pinMode(player->y_vcc_pin, OUTPUT);
    pinMode(player->y_gnd_pin, OUTPUT);
    pinMode(player->eng_vcc_pin, INPUT);
    pinMode(player->eng_gnd_pin, INPUT);

    // Set digital pins to power digital potentiometer
    digitalWrite(player->x_vcc_pin, HIGH);
    digitalWrite(player->x_gnd_pin, LOW);
    digitalWrite(player->y_vcc_pin, HIGH);
    digitalWrite(player->y_gnd_pin, LOW);

    pinMode(player->digi_pot_xy_ss_pin, OUTPUT);
    digitalWrite(player->digi_pot_xy_ss_pin, HIGH);
    pinMode(player->digi_pot_eng_ss_pin, OUTPUT);
    digitalWrite(player->digi_pot_eng_ss_pin, HIGH);

    pinMode(player->reset_write_pin, INPUT);
    pinMode(player->crowbar_reset_pin, OUTPUT);
    digitalWrite(player->crowbar_reset_pin, LOW);

    player->writing = true;
}

void init_player_as_reading(spot_gen *player) {
    pinMode(player->x_vcc_pin, INPUT);
    pinMode(player->x_gnd_pin, INPUT);
    pinMode(player->y_vcc_pin, INPUT);
    pinMode(player->y_gnd_pin, INPUT);
    pinMode(player->eng_vcc_pin, INPUT);
    pinMode(player->eng_gnd_pin, INPUT);

    pinMode(player->digi_pot_xy_ss_pin, OUTPUT);
    digitalWrite(player->digi_pot_xy_ss_pin, HIGH);
    pinMode(player->digi_pot_eng_ss_pin, OUTPUT);
    digitalWrite(player->digi_pot_eng_ss_pin, HIGH);

    pinMode(player->reset_write_pin, INPUT);
    pinMode(player->crowbar_reset_pin, INPUT);

    player->writing = false;
}

/**
 * Triggers are reset of the console as the player passed in
 * @param player
 */
void reset_as_player(spot_gen *player) {
    // if (player->writing) {
    //     pinMode(player->reset_write_pin, OUTPUT);
    //     digitalWrite(player->reset_write_pin, LOW);
    //     pinMode(player->crowbar_reset_pin, INPUT_PULLUP);

    //     delay(RESET_SETTLE_TIME_MS);

    //     pinMode(player->reset_write_pin, INPUT);
    //     pinMode(player->crowbar_reset_pin, OUTPUT);
    //     digitalWrite(player->crowbar_reset_pin, LOW);
    // }

    
    pinMode(player->reset_write_pin, OUTPUT);
    digitalWrite(player->reset_write_pin, LOW);
    pinMode(player->crowbar_reset_pin, INPUT_PULLUP);

    delay(RESET_SETTLE_TIME_MS);

    pinMode(player->reset_write_pin, INPUT);
    pinMode(player->crowbar_reset_pin, OUTPUT);
    digitalWrite(player->crowbar_reset_pin, LOW);
    delay(RESET_SETTLE_TIME_MS);
    
}

bool read_player_reset(spot_gen *player) {
    return analogRead(player->crowbar_reset_pin) > 128;
}

/**
 * Write the knobs of a player
 * Writes directly to the digital potentiometer
 *
 * @param player
 * @param x
 * @param y
 * @param eng
 */
void write_player_knobs(spot_gen *player, uint8_t x, uint8_t y, uint8_t eng) {
    if (player->writing) {
        // Write the X position
        digitalWrite(player->digi_pot_xy_ss_pin, LOW);
        noInterrupts();
        SPI.transfer(player->digi_pot_x_addr);
        SPI.transfer(x);
        interrupts();
        digitalWrite(player->digi_pot_xy_ss_pin, HIGH);
        // Write the Y position
        digitalWrite(player->digi_pot_xy_ss_pin, LOW);
        noInterrupts();
        SPI.transfer(player->digi_pot_y_addr);
        SPI.transfer(y);
        interrupts();
        digitalWrite(player->digi_pot_xy_ss_pin, HIGH);

        // Save the english position to the spot
        // it will be handled by serve_english
        player->english_pos = eng;
    }
}

/**
 * Read the position of a player
 * returns the position using the pointers x_pos and y_pos
 * @param spot
 * @param x_pos
 * @param y_pos
 */
void read_player_position(spot_gen* spot, int* x_pos, int* y_pos) {
  *x_pos = analogRead(spot->x_read);
  *y_pos = analogRead(spot->y_read);
}

/*void print_spot_position(spot_gen *spot, char *name) {
    int x_pos, y_pos;
    read_player_position(spot, &x_pos, &y_pos);
    Serial.print(name);
    Serial.print(": ");
    Serial.print(x_pos);
    Serial.print(" ");
    Serial.print(y_pos);
    Serial.print("   ");
}*/

/**
 * Handle setting the english
 * There is only one english line so the english flip flop
 * must be read, and the writing status of the players inspected
 * in order to determine which players (if any's) english should be written
 */
void serve_english(void) {
    bool english_flip_flop = READ_ENGLISH_FLIP_FLOP;

    if (p1_spot.writing) {
        if (english_flip_flop) {
            pinMode(p2_spot.eng_vcc_pin, OUTPUT);
            pinMode(p2_spot.eng_gnd_pin, OUTPUT);
            digitalWrite(p1_spot.eng_vcc_pin, HIGH);
            digitalWrite(p1_spot.eng_gnd_pin, LOW);

            // Write the value to the digital pot
            digitalWrite(p1_spot.digi_pot_eng_ss_pin, LOW);
            noInterrupts();
            SPI.transfer(p1_spot.digi_pot_eng_addr);
            SPI.transfer(p1_spot.english_pos);
            interrupts();
            digitalWrite(p1_spot.digi_pot_eng_ss_pin, HIGH);
        } else {
            pinMode(p1_spot.eng_vcc_pin, INPUT);
            pinMode(p1_spot.eng_gnd_pin, INPUT);
        }
    }

    if (p2_spot.writing) {
        if (!english_flip_flop) {
            pinMode(p2_spot.eng_vcc_pin, OUTPUT);
            pinMode(p2_spot.eng_gnd_pin, OUTPUT);
            digitalWrite(p2_spot.eng_vcc_pin, HIGH);
            digitalWrite(p2_spot.eng_gnd_pin, LOW);

            // Write the value to the digital pot
            digitalWrite(p2_spot.digi_pot_eng_ss_pin, LOW);
            noInterrupts();
            SPI.transfer(p2_spot.digi_pot_eng_addr);
            SPI.transfer(p2_spot.english_pos);
            interrupts();
            digitalWrite(p2_spot.digi_pot_eng_ss_pin, HIGH);
        } else {
            pinMode(p2_spot.eng_vcc_pin, INPUT);
            pinMode(p2_spot.eng_gnd_pin, INPUT);
        }
    }
}

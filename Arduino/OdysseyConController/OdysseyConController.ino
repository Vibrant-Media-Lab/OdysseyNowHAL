/////////////////////////////////////////////////////////////////////////////////////////
// Arduino driver for Odysessey Arduino Interface Board R0.3
// Odyssey Restoration Project
// University of Pittsburgh
// 9-24-19
// History:
// * 4-21-19 - File copied from R0.2 - Levi Burner
// * 9-24-19 - Unity adoption - Grayson Wen
/////////////////////////////////////////////////////////////////////////////////////////

#include <SD.h>
#define SD_ChipSelectPin 53   // Used by TMRpcm library
#include <TMRpcm.h>
#include <SPI.h>

TMRpcm tmrpcm; // For outputting audio

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

// Initialize the pins of a player so they can be written to
void init_player_as_writeable(spot_gen* player) {
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

void init_player_as_reading(spot_gen* player) {
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

// Triggers are reset of the console as the player passed in
void reset_as_player(spot_gen* player) {
  if (player->writing) {
    pinMode(player->reset_write_pin, OUTPUT);
    digitalWrite(player->reset_write_pin, LOW);
    pinMode(player->crowbar_reset_pin, INPUT_PULLUP);
  
    delay(RESET_SETTLE_TIME_MS);
  
    pinMode(player->reset_write_pin, INPUT);
    pinMode(player->crowbar_reset_pin, OUTPUT);
    digitalWrite(player->crowbar_reset_pin, LOW);
  }
}

bool read_player_reset(spot_gen* player) {
  return analogRead(player->crowbar_reset_pin) > 128;
}

// Write the knobs of a player
// Writes directly to the digital potentiometer
void write_player_knobs(spot_gen* player, uint8_t x, uint8_t y, uint8_t eng) {
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

// Read the position of a player
// returns the position using the pointers x_pos and y_pos
void read_player_position(spot_gen* spot, int* x_pos, int* y_pos) {
  *x_pos = analogRead(spot->x_read);
  *y_pos = analogRead(spot->y_read);
}

// void print_spot_position(spot_gen* spot, char* name) {
//   int x_pos, y_pos;
//   read_player_position(spot, &x_pos, &y_pos);
//   Serial.print(name);
//   Serial.print(": ");
//   Serial.print(x_pos);
//   Serial.print(" ");
//   Serial.print(y_pos);
//   Serial.print("   ");
// }

// Handle setting the english
// There is only one english line so the english flip flop
// must be read, and the writing status of the players inspected
// in order to determine which players (if any's) english should be written
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

// Handle playing sound based on detections
void serve_audio(void) {
  // Turn on the ENGLISH disconnect relay if P2 has the ball
  // Prevents P1 from writing the english position
    int8_t english = READ_ENGLISH_FLIP_FLOP;
    int8_t ball = READ_BALL_FLIP_FLOP;

    static int8_t last_english = -1;
    static int8_t last_ball = -1;
    if (last_english == -1) last_english = english;
    if (last_ball == -1) last_ball = ball;

    if (english != last_english || ball != last_ball) {
      tmrpcm.play("bounce.wav");
      last_english = english;
      last_ball = ball;
    }

    static bool crowbar_was_inactive = false;
    bool crowbar = READ_CROWBAR;
    if (!crowbar) {
       crowbar_was_inactive = true;
    }

    // If spot is being read and reset is active, the reset triggered the crowbar sound
    // in this case, do not play the crowbar sound
    if ((!p1_spot.writing && read_player_reset(&p1_spot))
         || (!p2_spot.writing && read_player_reset(&p2_spot))){
      crowbar_was_inactive = false;
    }

    if (crowbar && crowbar_was_inactive) {
      crowbar_was_inactive = false;
      tmrpcm.play("crowbar.wav");
    }
}


float update_period = 1.0 / 120.0;

void setup() {
  Serial.begin(115200);

  // Initialize tmrpcm for sound
  tmrpcm.speakerPin = 11;
  if (!SD.begin(SD_ChipSelectPin)) {
    Serial.println("SD fail");  
  } else {
    Serial.println();
    Serial.println("SD begin success");
  }

  // Init P1 and P2 pins, all other spots are implicitly read only
  init_player_as_reading(&p1_spot);
  init_player_as_reading(&p2_spot);

}

int tmp_x, tmp_y;

void loop() {

  // Construct one piece of JSON message

  read_player_position(&p1_spot, &tmp_x, &tmp_y);
  Serial.print("{");
  Serial.print("\"P1_X_READ\": ");
  Serial.print(tmp_x);
  Serial.print(", ");

  Serial.print("\"P1_Y_READ\": ");
  Serial.print(tmp_y);
  Serial.print(", ");

  // Serial.print("\"P1_ENG_READ\": ");
  // Serial.print(0); //todo
  // Serial.print(", ");

  // Serial.print("\"P1_RESET_READ\": ");
  // Serial.print(read_player_reset(&p1_spot)); 
  // Serial.print(", ");

  read_player_position(&p2_spot, &tmp_x, &tmp_y);
  Serial.print("\"P2_X_READ\": ");
  Serial.print(tmp_x);
  Serial.print(", ");
  
  Serial.print("\"P2_Y_READ\": ");
  Serial.print(tmp_y);
  Serial.print(", ");

  // Serial.print("\"P2_ENG_READ\": ");
  // Serial.print(0); //todo
  // Serial.print(", ");

  // Serial.print("\"P2_RESET_READ\": ");
  // Serial.print(read_player_reset(&p2_spot)); 
  // Serial.print(", ");

  read_player_position(&ball_spot, &tmp_x, &tmp_y);
  Serial.print("\"BALL_X_READ\": ");
  Serial.print(tmp_x); 
  Serial.print(", ");

  Serial.print("\"BALL_Y_READ\": ");
  Serial.print(tmp_y); 
  Serial.print(", ");

  // Serial.print("\"BALL_SPEED_READ\": ");
  // Serial.print(1); // todo
  // Serial.print(", ");

  read_player_position(&wall_spot, &tmp_x, &tmp_y);
  Serial.print("\"WALL_X_READ\": ");
  Serial.print(tmp_x); 
  // Serial.print(", ");

  // Serial.print("\"SELECT_READ\": ");
  // Serial.print(0);
  // Serial.print(", ");

  // Serial.print("\"ENTER_READ\": ");
  // Serial.print(0);
  // Serial.print(", ");

  // Serial.print("\"CROWBAR_READ\": ");
  // Serial.print(0);
  // Serial.print(", ");

  // Serial.print("\"CROWBAR_RESET_READ\": ");
  // Serial.print(0);
  // Serial.print(", ");

  // Serial.print("\"ENCODER_READ\": ");
  // Serial.print(0);

  Serial.print("}");
  Serial.println();
 
    // #define READ_SPOT_PRINT(x) read_player_position(spot, &tmp_x, &tmp_y);
    // READ_SPOT_PRINT(&p1_spot)
    // READ_SPOT_PRINT(&p2_spot)
    // READ_SPOT_PRINT(&p3_spot)
    // READ_SPOT_PRINT(&p4_spot)
    // READ_SPOT_PRINT(&ball_spot)
    // READ_SPOT_PRINT(&wall_spot)
    // #define PRINT_DIGITAL(x) Serial.print(#x ": "); Serial.print(x); Serial.print("   ");
    // PRINT_DIGITAL(READ_CROWBAR)
    // PRINT_DIGITAL(READ_ENGLISH_FLIP_FLOP)
    // PRINT_DIGITAL(READ_BALL_FLIP_FLOP)
    // PRINT_DIGITAL(read_player_reset(&p1_spot))
    // PRINT_DIGITAL(read_player_reset(&p2_spot))
    // Serial.println();

    serve_audio();
    serve_english();
 
    /* // Enter characters in serial monitor to call different functions
    if(Serial.available() >= 2){
      char cmd = Serial.read();

      // Disable control of player spot
      if (cmd == 'd') {
        char player = Serial.read();
        if (player == '1') {
          init_player_as_reading(&p1_spot);
          Serial.println("Reading P1");
        } else if(player == '2') {
          init_player_as_reading(&p2_spot);
          Serial.println("Reading P2");
        }
      }

      if (cmd == 'r'){ // reset as a player
        char player = Serial.read();
        if (player == '1') {
          reset_as_player(&p1_spot);
          Serial.println("Reset as P1");
        } else if(player == '2') {
          reset_as_player(&p2_spot);
          Serial.println("Reset as P2");
        }
      }

      // Flush the input buffer
      while(Serial.available()) Serial.read();
      
    } */

    // TODO Replace with true rate limiting
    delay(update_period * 1000);
  
}

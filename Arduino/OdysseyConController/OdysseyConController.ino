/////////////////////////////////////////////////////////////////////////////////////////
// Arduino driver for Odysessey Arduino Interface Board R0.3
// Odyssey Restoration Project
// University of Pittsburgh
// 9-24-19
// History:
// * 4-21-19 - File copied from R0.2 - Levi Burner
// * 9-24-19 - Unity adoption - Grayson Wen
// * 10-4-19 - Communication protocol; Code refactoring;  - Grayson Wen
/////////////////////////////////////////////////////////////////////////////////////////
#include "OdysseyHardware.h"
#include "Sound.h"

#include <ArduinoJson.h>

float update_period = 1.0 / 120.0;

void setup() {
    Serial.begin(115200);

    // Initialize for sound
    if (!init_audio()) {
        Serial.println("Audio initialization fail (SD card failed).");
    }

    // Init P1 and P2 pins, all other spots are implicitly read only
    init_player_as_reading(&p1_spot);
    init_player_as_reading(&p2_spot);
}

// Change the size as needed. A handy link for this: https://arduinojson.org/v6/assistant/
const size_t msgCapacity = JSON_OBJECT_SIZE(9);
DynamicJsonDocument doc(msgCapacity);
int tmp_x, tmp_y;

void loop() {

    // Read/Write the player data accordingly
    if (p1_spot.writing) {

    } else {

    }

    // Construct one piece of JSON message
    read_player_position(&p1_spot, &tmp_x, &tmp_y);
    doc["P1_X_READ"] = tmp_x;
    doc["P1_Y_READ"] = tmp_y;

    read_player_position(&p2_spot, &tmp_x, &tmp_y);
    doc["P2_X_READ"] = tmp_x;
    doc["P2_Y_READ"] = tmp_y;

    read_player_position(&ball_spot, &tmp_x, &tmp_y);
    doc["BALL_X_READ"] = tmp_x;
    doc["BALL_Y_READ"] = tmp_y;

    read_player_position(&wall_spot, &tmp_x, &tmp_y);
    doc["WALL_X_READ"] = tmp_x;

    doc["P1_RESET_READ"] = read_player_reset(&p1_spot);
    doc["P2_RESET_READ"] = read_player_reset(&p2_spot);

    // Write json message to serial (->Unity3D)
    serializeJson(doc, Serial);
    Serial.println();

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

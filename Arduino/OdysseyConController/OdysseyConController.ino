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
#include <StreamUtils.h>

unsigned long update_period = 1000.0f / 50.0f; // in ms
unsigned long tsLastLoop = millis();
long tsUsed; // in ms

void setup() {
    Serial.begin(115200);

    // Initialize for sound
    if (!init_audio()) {
        Serial.println("Audio initialization fail (SD card failed).");
    }

    // Init P1 and P2 pins, all other spots are implicitly read only
    init_player_as_reading(&p1_spot);
    init_player_as_reading(&p2_spot);
    //read_player_reset(&p2_spot);//testing
}

// Change the size as needed. A handy link for this: https://arduinojson.org/v6/assistant/
DynamicJsonDocument docIn(JSON_OBJECT_SIZE(12));
DynamicJsonDocument docOut(JSON_OBJECT_SIZE(12));

// Temp static variables
static int tmp_x, tmp_y;
static DeserializationError parseError;

#define MAKE_WRITE(p) ( !p.writing ? init_player_as_writeable(&p) : void() )
#define MAKE_READ(p) ( p.writing ? init_player_as_reading(&p) : void() )

const char RECV_START = '<';
const char RECV_END = '>';
const int RECV_BUF_SIZE = 128;
char recvBuffer[RECV_BUF_SIZE];
int currPos = 0;
bool recvInProgress = false;
/**
 *
 * @return Is data available (in recvBuffer);
 */
bool recvSerialWithStartEnd() {
    while (Serial.available()) {
        char r = Serial.read();
        if (recvInProgress) {
            if (r == RECV_END) {
                recvInProgress = false;
                if ( currPos+1 < RECV_BUF_SIZE )
                    recvBuffer[currPos+1] = '\0';
                return true;
            } else {
                if (currPos >= RECV_BUF_SIZE) {
                    // error, message too long.
                    //      TODO what to do? currently we are throwing away all the buffer message and wait for next new start.
                    recvInProgress = false;
                    Serial.println("\n\n\nThrew Away Data");
                } else {
                    recvBuffer[currPos++] = r;
                }
            }
        } else if (r == RECV_START) {
            recvInProgress = true;
            currPos = 0;
        }
    }
    return false;
}

void processInData() {
    parseError = deserializeJson(docIn, recvBuffer);

    if (parseError != NULL) {
        docOut["error"] = parseError.c_str();
    } else {

        if ( ! docIn["P1W"].isNull() ) {
            // Re-init the play as input/output, (and make sure we don't do it if it's already in the state)
            docIn["P1W"].as<bool>() ? MAKE_WRITE(p1_spot) : MAKE_READ(p1_spot);
            reset_as_player(&p1_spot);
        }
        if ( ! docIn["P2W"].isNull() ) {
            docIn["P2W"].as<bool>() ? MAKE_WRITE(p2_spot) : MAKE_READ(p2_spot);
            //reset_as_player(&p2_spot);
        }

        if (p1_spot.writing) {
            if (!(docIn["P1X"].isNull() || docIn["P1X"].isNull())) {
                tmp_x = docIn["P1X"];
                tmp_y = docIn["P1Y"];
                write_player_knobs(&p1_spot, tmp_x, tmp_y, 120); // todo - english
            } // else { we have insufficient data received }
        }
        if (p2_spot.writing) {
            if (!(docIn["P2X"].isNull() || docIn["P2X"].isNull())) {
                tmp_x = docIn["P2X"];
                tmp_y = docIn["P2Y"];
                write_player_knobs(&p2_spot, tmp_x, tmp_y, 120);
            } // else { we have insufficient data received }
        }
    }
}

void processOutData() {
    // Construct one piece of JSON message
    docOut["P1_IS_WRITING"] = p1_spot.writing;
    docOut["P2_IS_WRITING"] = p2_spot.writing;

    if ( ! p1_spot.writing ) {
        // Read mode
        read_player_position(&p1_spot, &tmp_x, &tmp_y);
        docOut["P1_X_READ"] = tmp_x;
        docOut["P1_Y_READ"] = tmp_y;
        docOut["P1_RESET_READ"] = read_player_reset(&p1_spot);
    }

    if ( ! p2_spot.writing ) {
        // Read mode
        read_player_position(&p2_spot, &tmp_x, &tmp_y);
        docOut["P2_X_READ"] = tmp_x;
        docOut["P2_Y_READ"] = tmp_y;
        docOut["P2_RESET_READ"] = read_player_reset(&p2_spot);
    }

    read_player_position(&ball_spot, &tmp_x, &tmp_y);
    docOut["BALL_X_READ"] = tmp_x;
    docOut["BALL_Y_READ"] = tmp_y;

    read_player_position(&wall_spot, &tmp_x, &tmp_y);
    docOut["WALL_X_READ"] = tmp_x;

    // Write json message to serial (->Unity3D)
    serializeJson(docOut, Serial);
    Serial.println();
}

void loop() {
    docOut.remove("error");

    // Serial buffer has only 64-bytes. Gotta read message by blocks
    if (recvSerialWithStartEnd()) {
        // We have JSON available in recv_buffer
        //        Serial.print("JSON available: ");
        //        Serial.write(recvBuffer, RECV_BUF_SIZE);
        //        Serial.println("");
        processInData();
    }

    processOutData();

    serve_audio();
    serve_english();

    // Rate limiting | TODO millis() overflow? (approx. 50 days)
    //    Serial.print("loop time (ms):");
    //    Serial.println(millis() - tsLastLoop);
    tsUsed = millis() - tsLastLoop;
    if (update_period > tsUsed) {
        // we still have some time remained
        delay(update_period - tsUsed);
    } else {
        Serial.print("too slow: remain_t (ms) =");
        Serial.println((long)update_period - tsUsed);
    }
    tsLastLoop = millis();

}

/// Audio functionality

#include <SD.h>
#define SD_ChipSelectPin 53   // Used by TMRpcm library

#include <TMRpcm.h>

// For outputting audio
TMRpcm tmrpcm;

/**
 * Audio initialization
 * @return true if initialization is success
 */
bool init_audio() {
    tmrpcm.speakerPin = 11;
    return SD.begin(SD_ChipSelectPin);
}

/**
 * Handle playing sound based on detections
 */
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
        || (!p2_spot.writing && read_player_reset(&p2_spot))) {
        crowbar_was_inactive = false;
    }

    if (crowbar && crowbar_was_inactive) {
        crowbar_was_inactive = false;
        tmrpcm.play("crowbar.wav");
    }
}
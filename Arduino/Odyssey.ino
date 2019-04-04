// Test sketch to verify basic arduino interface functionality

#include <SD.h>                      // need to include the SD library
#define SD_ChipSelectPin 53  //example uses hardware SS pin 53 on Mega2560
#include <TMRpcm.h>           //  also need to include this library...
#include <SPI.h>
//#include <SoftwareSerial.h>
//#include <SerialCommand.h>


//SerialCommand sCmd;

TMRpcm tmrpcm;   // create an object for use in this sketch

const int P1_X_PWM = 5;
const int P1_Y_PWM = 3;
const int P1_CROWBAR_PWM = 2;
// const int P2_X_PWM = 2; traded for crowbar
const int P2_Y_PWM = 8; // Disconnected in stage unit

// Spot positions
const int P2_X_READ = A3; //A10;
const int P2_Y_READ = A10;//A3;
const int P3_X_READ = A4;
const int P3_Y_READ = A5;
const int P4_X_READ = A6;
const int P4_Y_READ = A7;
const int BALL_X_READ = A9;
const int BALL_Y_READ = A8;
const int WALL_X_READ = A11;

const int NOT_CROWBAR_READ = A12; // Crowbar state

const int P1_ENGLISH_RELAY = 49;

// States of flip flops
const int NOT_ENGLISH_WEAK_Q_READ = A13;
const int NOT_BALL_WEAK_Q_READ = A14;

// To allow reading the odyssey's supply voltage
const int ODYSSEY_HALF_VCC = A15;

// P1 reset pins
const int P1_RESET = 12; // Set to output and low in order to reset the Odyssey, else set to input
const int P1_CROWBAR_RESET = 13; // Set to output and low normally, set high to reset the crowbar circuit

// make global so update from serial
//int x_counts;
//int y_counts;

void p1_reset() {
  pinMode(P1_RESET, OUTPUT);
  digitalWrite(P1_RESET, LOW);
  digitalWrite(P1_CROWBAR_RESET, HIGH);
  delay(2); // Wait 2ms
  pinMode(P1_RESET, INPUT);
  digitalWrite(P1_CROWBAR_RESET, LOW);
}

void setup() {
  // Increase PWM frequency for timer 3 and 4 controlled pins
  // https://forum.arduino.cc/index.php?topic=72092.0
  TCCR3B &= 0b11111000;
  TCCR3B |= 0b00000001;
  TCCR4B &= 0b11111000;
  TCCR4B |= 0b00000001;

  pinMode(P1_X_PWM, OUTPUT);
  pinMode(P1_Y_PWM, OUTPUT);
  pinMode(P1_CROWBAR_PWM, OUTPUT);
  pinMode(P1_ENGLISH_RELAY, OUTPUT);
  pinMode(P1_CROWBAR_RESET, OUTPUT);

  // Note when P2 has control of the ball P1_CROWBAR_PWM must be set to an input
  analogWrite( P1_CROWBAR_PWM, 255 * 3 / 6); // Set P1 english to middle of screen

  Serial.begin(115200);
  //sCmd.addCommand("setP1", p1Handler);
  //sCmd.addCommand("p1Reset", p1_reset);

  tmrpcm.speakerPin = 11;

  if (!SD.begin(SD_ChipSelectPin)) {  // see if the card is present and can be initialized:
   // Serial.println("SD fail");  
  } else {
    //Serial.println();
    //Serial.println("SD SUCCESS!!!");
  }
}

/*void p1Handler() {
    x_counts = atoi(sCmd.next());    // Get the next argument from the SerialCommand object buffer
    y_counts = atoi(sCmd.next());
}*/

// Move the player in a circle
void loop() {
  // Volt => position is inverted compared to how pixels work
  // e.g right positions have the highest X voltage
  // and the top of the screen is the highest Y voltage
  float offset_x_volts = 2.5;
  float offset_y_volts = 2.5;
  
  float radius_volts = 0.75;
  // 255 pwm counts over 5 volts
  float pwm_scale = 255.0 / 5.0;

  // Speed to rotate
  float rad_s = 180.0 * (PI / 180.0);
  float update_period = 1.0 / 60.0;

  float theta = 0.0;
  while(true) {
    theta += rad_s * update_period;
    if (theta > 2 * PI) {
      theta -= 2 * PI;
    }

    int x_counts = (int)((cos(theta) * radius_volts + offset_x_volts) * pwm_scale);
    int y_counts = (int)((sin(theta) * radius_volts + offset_y_volts) * pwm_scale);
    //x_counts = (int)((offset_x_volts) * pwm_scale);
    //y_counts = (int)((offset_y_volts) * pwm_scale);
    //y_counts = 75;
    Serial.print("{ ");
    Serial.print("\"P1_X_READ\": ");
    Serial.print(x_counts);
    Serial.print(", ");
    Serial.print("\"P1_Y_READ\": ");
    Serial.print(y_counts);
    Serial.print(",");
    analogWrite(P1_X_PWM, x_counts);
    analogWrite(P1_Y_PWM, y_counts);

    #define READ_ANALOG_PRINT(x) Serial.print("  \"" #x "\":"); Serial.print(analogRead(x)); Serial.print(",");
    #define READ_DIGITAL_PRINT(x) Serial.print("  \"" #x "\":"); Serial.print(digitalRead(x)); Serial.print(",");

    #define READ_ANALOG_FROM_VOLT(x) Serial.print("  \"" #x "\":"); Serial.print(analogRead(x) / 4); Serial.print(",");
    
    //READ_ANALOG_PRINT(P2_X_READ);
    //READ_ANALOG_PRINT(P2_Y_READ);
    READ_ANALOG_FROM_VOLT(P2_X_READ);
    READ_ANALOG_FROM_VOLT(P2_Y_READ);
    READ_ANALOG_PRINT(P3_X_READ);
    READ_ANALOG_PRINT(P3_Y_READ);
    READ_ANALOG_PRINT(P4_X_READ);
    READ_ANALOG_PRINT(P4_Y_READ);
    READ_ANALOG_FROM_VOLT(BALL_X_READ);
    READ_ANALOG_FROM_VOLT(BALL_Y_READ);
    READ_ANALOG_FROM_VOLT(WALL_X_READ);
    READ_DIGITAL_PRINT(NOT_ENGLISH_WEAK_Q_READ);
    READ_DIGITAL_PRINT(NOT_BALL_WEAK_Q_READ);
    READ_DIGITAL_PRINT(NOT_CROWBAR_READ);
    Serial.print("}");
    Serial.println();

    // Turn on the ENGLISH disconnect relay if P2 has the ball
    // Prevents P1 from writing the english position
    bool english = digitalRead(NOT_ENGLISH_WEAK_Q_READ);
    static bool last_english = false;

    if (english != last_english) {
          digitalWrite(P1_ENGLISH_RELAY, english);
          tmrpcm.play("bounce.wav");
          last_english = english;
    }

    static bool last_crowbar_high = false;
    bool crowbar = digitalRead(NOT_CROWBAR_READ);

    if (!last_crowbar_high && crowbar) {
       last_crowbar_high = true;
    }

    if (last_crowbar_high && !crowbar) {
      last_crowbar_high = false;
      tmrpcm.play("crowbar.wav");
    }

    if(Serial.available()){    
      if(Serial.read() == 'r'){ // send r over the monitor to reset
        p1_reset();
      }
    }

    delay(update_period * 1000);
  }
}

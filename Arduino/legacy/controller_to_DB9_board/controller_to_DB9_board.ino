//include these libraries for the encoder:
//https://github.com/buxtronix/arduino/tree/master/libraries/Rotary
//https://github.com/GreyGnome/EnableInterrupt

//encoder stuff taken from:
//http://www.buxtronix.net/2011/10/rotary-encoders-done-properly.html

#include <EnableInterrupt.h>
#include <Rotary.h>

//rotary encoder vars
Rotary r2 = Rotary(6, 7);
int counter = 0;

//digital pins
const int resetPin1 = 2;  
const int resetPin2 = 3;
const int selectPin = 9;
const int enterPin = 10;
const int crowbarPin = 11;
const int crowbarResetPin = 12;

//button states
int buttonState1 = 0;
int lastButtonState1 = 0;
int buttonState2 = 0;
int lastButtonState2 = 0;
int buttonState3 = 0;
int lastButtonState3 = 0;
int buttonState4 = 0;
int lastButtonState4 = 0;
int buttonState5 = 0;
int lastButtonState5 = 0;
int buttonState6 = 0;
int lastButtonState6 = 0;

//button status
int resetStatus1 = 0;
int resetStatus2 = 0;
int selectStatus = 0;
int enterStatus = 0;
int crowbarStatus = 0;
int crowbarResetStatus = 0;

//run when pins 6 and 7 (connected to encoder) output voltage, inc/dec counter
void interrupt(void) {
  
    char result2 = r2.process();
    if (result2) {
    //Serial.println(result2 == DIR_CW ? "2Right" : "2Left");
    
    if(result2 == DIR_CW) {
      counter++;
      //Serial.println(counter);
    } else if (result2 == DIR_CCW) {
      counter--;
      //Serial.println(counter);
    }
  }
}

void setup() {
  pinMode (6, INPUT_PULLUP);
  pinMode (7, INPUT_PULLUP);
  enableInterrupt(6, interrupt, CHANGE);
  enableInterrupt(7, interrupt, CHANGE);

  Serial.begin(115200);
  pinMode(resetPin1, INPUT_PULLUP);
  pinMode(resetPin2, INPUT_PULLUP);
  pinMode(selectPin, INPUT_PULLUP);
  pinMode(enterPin, INPUT_PULLUP);
  pinMode(crowbarPin, INPUT_PULLUP);
  pinMode(crowbarResetPin, INPUT_PULLUP);
}

void loop() {

  int J1_horz = analogRead(A2);
  int J1_vert = analogRead(A1);
  int J1_eng = analogRead(A0);

  int J2_horz = analogRead(A5);
  int J2_vert = analogRead(A4);
  int J2_eng = analogRead(A3);

  int ball_speed = analogRead(A6)/4;


  //reads voltages from DB9s and buttons and prints values
  Serial.print("{");
  Serial.print("\"P1_X_READ\": ");
  Serial.print(J1_horz);
  Serial.print(", ");

  Serial.print("\"P1_Y_READ\": ");
  Serial.print(J1_vert);
  Serial.print(", ");

  Serial.print("\"P1_ENG_READ\": ");
  Serial.print(J1_eng);
  Serial.print(", ");

  Serial.print("\"P1_RESET_READ\": ");
  Serial.print(resetStatus1);
  Serial.print(", ");

  Serial.print("\"P2_X_READ\": ");
  Serial.print(J2_horz);
  Serial.print(", ");
  
  Serial.print("\"P2_Y_READ\": ");
  Serial.print(J2_vert);
  Serial.print(", ");

  Serial.print("\"P2_ENG_READ\": ");
  Serial.print(J2_eng);
  Serial.print(", ");

  Serial.print("\"P2_RESET_READ\": ");
  Serial.print(resetStatus2);
  Serial.print(", ");

  Serial.print("\"BALL_SPEED_READ\": ");
  Serial.print(ball_speed);
  Serial.print(", ");

  Serial.print("\"SELECT_READ\": ");
  Serial.print(selectStatus);
  Serial.print(", ");

  Serial.print("\"ENTER_READ\": ");
  Serial.print(enterStatus);
  Serial.print(", ");

  Serial.print("\"CROWBAR_READ\": ");
  Serial.print(crowbarStatus);
  Serial.print(", ");

  Serial.print("\"CROWBAR_RESET_READ\": ");
  Serial.print(crowbarResetStatus);
  Serial.print(", ");

  Serial.print("\"ENCODER_READ\": ");
  Serial.print(counter);

  Serial.print("}");
  Serial.println();

  //delay(1000); //remove later
  
  
  //edge detection for controller 1 reset
  buttonState1 = digitalRead(resetPin1);

  if(buttonState1 != lastButtonState1) {
    if(buttonState1 == LOW) {
      //Serial.println("reset button 1 pressed");
      resetStatus1 = 0;
    } else {
      //Serial.println("reset button 1 released");
      resetStatus1 = 1;
    }
    delay(50);
  }
  lastButtonState1 = buttonState1;

  //edge detection for controller 2 reset
  buttonState2 = digitalRead(resetPin2);

  if(buttonState2 != lastButtonState2) {
    if(buttonState2 == LOW) {
      //Serial.println("reset button 2 pressed");
      resetStatus2 = 0;
    } else {
      //Serial.println("reset button 2 released");
      resetStatus2 = 1;
    }
    delay(50);
  }
  lastButtonState2 = buttonState2;

  //edge detection for select
  buttonState3 = digitalRead(selectPin);

  if(buttonState3 != lastButtonState3) {
    if(buttonState3 == LOW) {
      //Serial.println("select button pressed");
      selectStatus = 1;
    } else {
      //Serial.println("select button released");
      selectStatus = 0;
    }
    delay(50);
  }
  lastButtonState3 = buttonState3;

  //edge detection for enter
  buttonState4 = digitalRead(enterPin);

  if(buttonState4 != lastButtonState4) {
    if(buttonState4 == LOW) {
      //Serial.println("enter button pressed");
      enterStatus = 1;
    } else {
      //Serial.println("enter button released");
      enterStatus = 0;
    }
    delay(50);
  }
  lastButtonState4 = buttonState4;

  //edge detection for crowbar
  buttonState5 = digitalRead(crowbarPin);

  if(buttonState5 != lastButtonState5) {
    if(buttonState5 == LOW) {
      //Serial.println("crowbar button pressed");
      crowbarStatus = 1;
    } else {
      //Serial.println("crowbar button released");
      crowbarStatus = 0;
    }
    delay(50);
  }
  lastButtonState5 = buttonState5;

  //edge detection for crowbarReset
  buttonState6 = digitalRead(crowbarResetPin);

  if(buttonState6 != lastButtonState6) {
    if(buttonState6 == LOW) {
      //Serial.println("crowbar reset button pressed");
      crowbarResetStatus = 1;
    } else {
      //Serial.println("crowbar reset button released");
      crowbarResetStatus = 0;
    }
    delay(50);
  }
  lastButtonState6 = buttonState6;
}

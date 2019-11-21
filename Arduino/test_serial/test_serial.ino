void setup() {
  // put your setup code here, to run once:
  Serial.begin(115200);
  Serial.println("ready.");
}

void loop() {
  // put your main code here, to run repeatedly:
  if (Serial.available()) {
    Serial.println(Serial.available());
    while(Serial.available()) {
      Serial.print((char)Serial.read());
    }
    Serial.println("");
    Serial.flush();
  }

  delay(1000);
  
}

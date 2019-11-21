# Player spot Read/Write

The digital potentiometers are enabled or disabled 
 - when disabled, you can read from that player spot
 - when enabled, that player spot is in write mode

there are 2 potentiometers for each player spot: one for horizontal position and one for vertical position.

(Levi)

[to disable the potentiometers] Disconnect the ground of the digital potentiometer and leave it floating. By disconnecting their ground and +5V sides from ground and +5V.

I do this using the GPIO pins of the arduino. When the pins are set to output, they provide ground or power to GND+VCC of the digital pot and GND of the Capacitor. But when the arduino pins are set to input, they go into a high impedance state essentially disconnection GND+VCC of the digital pot and GND of the capacitor, as if their wires were sticking up in the air. So then the digital pot and capacitor are connected to the Odyssey only on one end, there is no loop for current to flow through.

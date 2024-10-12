"""
main.py

Description:
    This file handles the following:
    1- connection process to the control box
    2- Reading/writing to the control box
    3- Passing in the messages to the MSG_Handler.py file


Author:
    Mohamed Abdelmoneim

Date:
    09/07/2024

Dependencies:
    - pyserial: For serial communication
    - MSG_Handler: Custom module for handling messages

Configuration:
    - DEBUG_ENABLE: Set to 1 to simulate input commands. Set to 0 for actual serial communication.
Notes:
    - N/A
"""
import MSG_Glue
import stepperMotor
import servo
FW_REV = 1.0
REGUlAR_ANGLES = []
REGULAR_ANGLES_ENCODED = [0,0,0,90,0,180,1,7,1,104]
# Configure the serial connection


def main ():

   MSG_Glue.runMsgGlue()
   stepperMotor.runStepperLogic(REGUlAR_ANGLES)
   servo.runServoLogic()
    #Run Solenoid logic
    #Run elcometer logic
    #Run Servo logic




def setCanWeSendTheStartingACK(x):
    global canWeSendTheStartingACK  # Declare that we're using the global variable
    canWeSendTheStartingACK = x

if __name__ == "__main__":
    main()

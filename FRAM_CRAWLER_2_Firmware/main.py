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
REGUlAR_ANGLES = [0,90,180,270,360]
# Configure the serial connection


def main ():

   MSG_Glue.runMsgGlue()
   if ((stepperMotor.runStepperLogic(REGUlAR_ANGLES)) or (not servo.runServoLogic())):
       MSG_Glue.setCanWeRunTransmit(True)
    #Run Solenoid logic
    #Run elcometer logic


if __name__ == "__main__":
    main()

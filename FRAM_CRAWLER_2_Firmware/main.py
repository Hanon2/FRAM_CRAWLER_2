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
import serial
import MSG_Handler
import binascii
import stepperMotor
FW_REV = 1.0
DEBUG_ENABLE = 1
REGUlAR_ANGLES = []
REGULAR_ANGLES_ENCODED = [0,0,0,90,0,180,1,7,1,104]
# Configure the serial connection


def main ():
    initial_crc = 0xFFFF
    data = []
    global canWeSendTheStartingACK
    canWeSendTheStartingACK = False
    print("DFT inspection system")
    print("FW Rev: ", FW_REV)
    if not DEBUG_ENABLE:
        ser = serial.Serial(
            port='COM3',  # Replace with the correct COM port
            baudrate=115200,
            timeout=None,  # We need to wait here until we get the first Ack which is 0xAA
        )
    while True:
        if DEBUG_ENABLE:
            try:
                data[0] = (int(input ("Enter the command to simulate"
                       "\n1-Take a measurement"
                       "\n2- Get the current position of the roboclaw\n>>>")))
            except Exception as e:
                print(e)
                data[0] = False
            if data[0]!= 1 and data[0] != 2:
                data[0] = False
                print ("Wrong input Enter either 1 or 2")
            else:
                if 1 == data[0]:
                    data[0] = MSG_Handler.totalBytes[data[0]]

                    data.append(REGULAR_ANGLES_ENCODED)
                    data.append(binascii.crc_hqx(data[1:len(data) - 2], initial_crc)) #Simulate the CRC
        else:
            data[0] = ser.read(1)
        if data[0]:
            if not DEBUG_ENABLE:
                ser.timeout = 3
            MSG_Handler.parseMessages(data)
        if canWeSendTheStartingACK and not DEBUG_ENABLE:
            ser.write(MSG_Handler.starting_ACK)
            canWeSendTheStartingACK = False
            data = ser.readline()
            if data:
                MSG_Handler.parseMessages(data)
            else:
                print("Connection timed out")
                break
        stepperMotor.runStepperLogic(REGUlAR_ANGLES)
        #Run Solenoid logic
        #Run elcometer logic
        #Run Servo logic




def setCanWeSendTheStartingACK(x):
    global canWeSendTheStartingACK  # Declare that we're using the global variable
    canWeSendTheStartingACK = x

if __name__ == "__main__":
    main()
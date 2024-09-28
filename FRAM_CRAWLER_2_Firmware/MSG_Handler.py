"""
MSG_Handler.py

Description:
    This file handles the following:
    1- Parses the messages and does the required logic for each messages

Author:
    Mohamed Abdelmoneim

Date:
    09/07/2024

Dependencies:
    - main: Custom module for handling messages (assumed to be implemented)

Configuration:
    - N/A
Notes:
    - This file is only a logic/Software file so it shouldn't have anything related to the HW in it.
"""

import main
import binascii
# Constants
requestToConnect_CMD = 0xAA
takeAMeasurement_CMD = 0x55
get_position_CMD = 0x2A
starting_ACK = 0x0B  # This is the ACK that the Pi will send after receiving the RQTC.
intermediate_ACK = 0x0C  # This is the ACK that the Pi will be waiting for after sending the measurements.
Advanced_ACK = 0xD5  # This is the ACK that the Pi will be waiting for after sending the PNG file.

# Combine the commands and ACKs into a 1D array (list)
totalBytes = [
    requestToConnect_CMD,
    takeAMeasurement_CMD,
    get_position_CMD,
    starting_ACK,
    intermediate_ACK,
    Advanced_ACK
]
def parseMessages(message):
    # Initial value for CRC computation
    initial_crc = 0xFFFF

    if requestToConnect_CMD == message[0]:
        main.setCanWeSendTheStartingACK(True)
    elif takeAMeasurement_CMD == message[0]:
        crc_value = binascii.crc_hqx(message[1:len(message)-2], initial_crc)
        if (message[len(message)-2]<<8 | message[len(message)-1]) == crc_value:
            parseAngles(message[1:message[8]]) #The eight index includes how many angles we have
        else:
            print("The CRC values don't match there is a miscommunication error")
    elif get_position_CMD ==message[0]:
        print("Send the position")

#We are using UART, so we need to provide
def parseAngles(rawAngleData):
    global REGUlAR_ANGLES
    for i in range(rawAngleData):
        try:
            REGUlAR_ANGLES[i] = (rawAngleData[i] >> 8 | rawAngleData[i + 1])
        except:
            REGUlAR_ANGLES.append(rawAngleData[i] >> 8 | rawAngleData[i + 1])
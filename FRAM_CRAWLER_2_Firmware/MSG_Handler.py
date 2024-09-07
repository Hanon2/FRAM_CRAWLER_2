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
    if requestToConnect_CMD == message:
        main.setCanWeSendTheStartingACK(True)
    elif takeAMeasurement_CMD == message:
        print("Do the measurement logic")
    elif get_position_CMD ==message:
        print("Send the position")
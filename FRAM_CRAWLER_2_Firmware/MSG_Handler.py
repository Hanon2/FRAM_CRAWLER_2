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
import stepperMotor
import MSG_Glue
# Commands coming from the control box
requestToConnect_CMD    = 0xAA
takeAMeasurement_CMD    = 0x55
move_camera_CMD         = 0xAC
stop_camera_CMD         = 0xCC

#Messages sent to the control box
requestToConnectReceived_ACK    = 0xBB
measurementsReceived_ACK        = 0x66
cameraIsNowMoving_ACK           = 0xBC
cameraIsStopped_ACK             = 0xDC
cameraReachedTheMaxDegree       = 0xEC


def parseMessages(data):
    if requestToConnect_CMD == data:
        return requestToConnectReceived_ACK
    elif takeAMeasurement_CMD == data[0]:
        anglesParser(data[1:])
        if (data[len(data)-1] | data[len(data)-2])  == (binascii.crc_hqx(data, 0xFFFF)):
            stepperMotor.setMotorDir("forward")
            MSG_Glue.setCanWeRunReceival(False)
            return measurementsReceived_ACK




def anglesParser(data):
    global REGUlAR_ANGLES
    for i in range(0, len(data), 2):
        try:
            REGUlAR_ANGLES[i // 2] = (data[i] << 8) | (data[i + 1] & 0xff)
        except IndexError:
            REGUlAR_ANGLES.append((data[i] << 8) | (data[i + 1] & 0xff))
        if i != 0 and data[i] == 0 and data[i + 1] == 0:
            break


import serial

import MSG_Handler

ser = serial.Serial(
    port='COMx',  # e.g., 'COM3'
    baudrate=115200,  # Baud rate
    bytesize=serial.EIGHTBITS,  # 8 data bits
    parity=serial.PARITY_NONE,  # No parity
    stopbits=serial.STOPBITS_ONE,  # 1 stop bit
    timeout=None  # Timeout for read/write operations
)
canWeRunReceival = True
def runMsgGlue():
    while (canWeRunReceival):
        data = ser.readline()
        if data:
            dataToBeSent = MSG_Handler.parseMessages(data)
            if dataToBeSent:
                ser.write(dataToBeSent)


def setCanWeRunReceival(x):
    global canWeRunReceival
    canWeRunReceival = x

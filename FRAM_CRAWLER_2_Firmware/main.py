from datetime import datetime
import serial
import binascii as b
import MSG_Handler
FW_REV = 1.0
# Get the current date and time
FW_Release_date = datetime.now().strftime("%Y-%m-%d %H:%M:%S")
def main ():
    print("DFT inspection system")
    print("FW Rev: ", FW_REV)
    print("Release FW date and time: " ,FW_Release_date)
    # Configure the serial connection
    ser = serial.Serial(
        port='COM3',  # Replace with the correct COM port
        baudrate=115200,
        timeout=None  # We need to wait here until we get the first Ack which is 0xAA
    )
    while True:
        data = ser.read(1)
        if data:
            MSG_Handler.parseMessages(b.hexlify(data))
if __name__ == "__main__":
    main()
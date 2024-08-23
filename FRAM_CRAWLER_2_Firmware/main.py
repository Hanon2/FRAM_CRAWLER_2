import serial
import MSG_Handler
FW_REV = 1.0
def main ():
    print("DFT inspection system")
    print("FW Rev: ", FW_REV)
    # Configure the serial connection
    ser = serial.Serial(
        port='COM3',  # Replace with the correct COM port
        baudrate=115200,
        timeout=None  # We need to wait here until we get the first Ack which is 0xAA
    )
    while True:
        data = ser.read(1)
        if data:
            MSG_Handler.parseMessages(data)
if __name__ == "__main__":
    main()
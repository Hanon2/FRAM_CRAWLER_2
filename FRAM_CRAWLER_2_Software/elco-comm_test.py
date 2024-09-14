import serial
import time
import pythonnet

# Replace '/dev/ttyS4' with the actual device name for COM5 on your Linux system.
PORT = '/dev/ttyS4'
BAUD_RATE = 9600  # Set the baud rate according to the device specifications.

def get_version_info():
    try:
        # Open the serial port
        with serial.Serial(PORT, baudrate=BAUD_RATE, timeout=1) as ser:
            # Sending the command to the gauge for version information
            # Assuming that the Elcometer 456 gauge expects a specific command,
            # this is just an example, replace 'GET_VERSION_COMMAND' with the actual command.
            command = b'GET_VERSION_COMMAND\r\n'
            ser.write(command)
            time.sleep(0.5)  # Wait for the device to respond

            # Reading the response
            if ser.in_waiting > 0:
                response = ser.read(ser.in_waiting)
                return response.decode('utf-8')
            else:
                return "No response from the device."
    except serial.SerialException as e:
        return f"Serial error: {e}"


if __name__ == '__main__':
    version_info = get_version_info()
    print("Elcometer 456 Version Info:", version_info)

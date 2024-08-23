#This file is for handling the messages
# It will take the data that the Pi recieved from the Control box
# It will parse these messages, and then tell the program what to do
#It will also handle sending the messages to the control box

requestToConnect_CMD = 0xAA
takeAMeasurement_CMD = 0x55
photoReceived_ACK = 0xD5
positionRecieved_ACK = 0x2A
def parseMessages(message):
    if message == requestToConnect_CMD:
        print ("Handle the Request to connect")
    elif message == takeAMeasurement_CMD:
        print("Handle the take a measurement")
    elif message == photoReceived_ACK:
        print("Photo received")
    else:
        print("Unknown data received")

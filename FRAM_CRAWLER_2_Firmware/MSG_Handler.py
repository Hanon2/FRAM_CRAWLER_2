def parseMessages(message):
    if message == '0xAA':
        print ("Handle the Request to connect")
    elif message == '0x55':
        print("Handle the take a measurement")
    elif message == '0xD5':
        print("Photo recieved")
    else:
        print("Unkown data recieved")
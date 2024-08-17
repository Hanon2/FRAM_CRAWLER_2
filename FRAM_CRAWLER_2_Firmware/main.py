from datetime import datetime
FW_REV = 1.0
# Get the current date and time
FW_Release_date = datetime.now().strftime("%Y-%m-%d %H:%M:%S")
def main ():
    print("DFT inspection system")
    print ("FW Rev: ", FW_REV)
    print("Release FW data and time: " ,FW_Release_date)
if __name__ == "__main__":
    main()
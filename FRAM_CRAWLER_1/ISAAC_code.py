import os
import sys
import time
import minimalmodbus
import serial.tools.list_ports
from PyQt5 import QtWidgets
from PyQt5.QtCore import QTimer
from PyQt5.QtCore import Qt
from PyQt5.QtGui import *
from PyQt5.QtMultimedia import *
from PyQt5.QtMultimediaWidgets import *
from PyQt5.QtWidgets import *
from roboclaw_3 import Roboclaw

ports = serial.tools.list_ports.comports()
portList = []
get_com = 'NULL'

for onePort in ports:
    portList.append(str(onePort))


class MyWindow(QMainWindow):
    def __init__(self):

        global camera_selector
        global comport_selector
        global portList
        global get_com
        global windowSize

        super().__init__()
        global sensor
        global roboclaw
        global enc_popup
        global sc
        global ths_com

        ################################################################################################################
        # Resizing all widgets relative to the size of the window to fit any size screen this application is displayed.
        ################################################################################################################
        self.resizeEvent = lambda event: resizeWidgets()  # calls resizeWidgets when the window size is changed
        self.setMinimumSize(600, 350)  # set minimum size for the window

        def resizeWidgets():
            # the ratios shown are based off of the original size of the widget, that will be specified individually in
            # comments for each widget, and the width and height of the control box screen of (1920, 1180)
            windowSize = self.size()  # stores the width and height of the window in this variable

            fontSize15 = round(windowSize.height() * (3 / 236))  # size 15 font variable to calculate the new font size
            fontSize12 = round(windowSize.height() * (3 / 295))  # size 12 font variable to calculate the new font size

            viewfinderSize = int(windowSize.width() * (145 / 192)), int(
                windowSize.height() * (50 / 59))  # original size (1450, 1000)
            self.viewfinder.setFixedSize(*viewfinderSize)  # camera display size variable and update

            #  button 1 / Stop button
            b1Size = int(windowSize.width() * (11 / 96)), int(windowSize.height() * (9 / 59))  # original size (220, 180)
            b1Loc = int(windowSize.width() * (169 / 192)), int(windowSize.height() * (17 / 236))  # original location (1690, 85)
            self.b1.setFont(QFont('Arial', fontSize12))
            self.b1.resize(*b1Size)
            self.b1.move(*b1Loc)
            # Button 2 / forward button
            b2Size = int(windowSize.width() * (11 / 96)), int(windowSize.height() * (9 / 59))  # original size (220, 180)
            b2Loc = int(windowSize.width() * (169 / 192)), int(windowSize.height() * (299 / 1180))  # original location (1690, 299)
            self.b2.setFont(QFont('Arial', fontSize12))
            self.b2.resize(*b2Size)
            self.b2.move(*b2Loc)
            # Button 3 / reverse button
            b3Size = int(windowSize.width() * (11 / 96)), int(windowSize.height() * (9 / 59))  # original size (220, 180)
            b3Loc = int(windowSize.width() * (169 / 192)), int(windowSize.height() * (128 / 295))  # original location (1690, 512)
            self.b3.setFont(QFont('Arial', fontSize12))
            self.b3.resize(*b3Size)
            self.b3.move(*b3Loc)
            # Button 4 / power up button
            b4Size = int(windowSize.width() * (11 / 96)), int(windowSize.height() * (9 / 59))  # original size (220, 180)
            b4Loc = int(windowSize.width() * (169 / 192)), int(windowSize.height() * (145 / 236))  # original location (1690, 725)
            self.b4.setFont(QFont('Arial', fontSize12))
            self.b4.resize(*b4Size)
            self.b4.move(*b4Loc)
            # Button 5 / power down button
            b5Size = int(windowSize.width() * (11 / 96)), int(windowSize.height() * (9 / 59))  # original size (220, 180)
            b5Loc = int(windowSize.width() * (169 / 192)), int(windowSize.height() * (469 / 590))  # original location (1690, 938)
            self.b5.setFont(QFont('Arial', fontSize12))
            self.b5.resize(*b5Size)
            self.b5.move(*b5Loc)
            # Button 6 / 24 ft/min button 
            b6Size = int(windowSize.width() * (11 / 96)), int(windowSize.height() * (9 / 59))  # original size (220, 180)
            b6Loc = int(windowSize.width() * (13 / 1920)), int(windowSize.height() * (17 / 236))  # original location (13, 85)
            self.b6.setFont(QFont('Arial', fontSize15))
            self.b6.resize(*b6Size)
            self.b6.move(*b6Loc)
            # Button 7 / 20 ft/min button 
            b7Size = int(windowSize.width() * (11 / 96)), int(windowSize.height() * (9 / 59))  # original size (220, 180)
            b7Loc = int(windowSize.width() * (13 / 1920)), int(windowSize.height() * (64 / 295))  # original location (13, 256)
            self.b7.setFont(QFont('Arial', fontSize15))
            self.b7.resize(*b7Size)
            self.b7.move(*b7Loc)
            # Button 8 / 16 ft/min button
            b8Size = int(windowSize.width() * (11 / 96)), int(windowSize.height() * (9 / 59))  # original size (220, 180)
            b8Loc = int(windowSize.width() * (13 / 1920)), int(windowSize.height() * (427 / 1180))  # original location (13, 427
            self.b8.setFont(QFont('Arial', fontSize15))
            self.b8.resize(*b8Size)
            self.b8.move(*b8Loc)
            # Button 9 / 12 ft/min button
            b9Size = int(windowSize.width() * (11 / 96)), int(windowSize.height() * (9 / 59))  # original size (220, 180)
            b9Loc = int(windowSize.width() * (13 / 1920)), int(windowSize.height() * (597 / 1180))  # original location (13, 597)
            self.b9.setFont(QFont('Arial', fontSize15))
            self.b9.resize(*b9Size)
            self.b9.move(*b9Loc)
            # Button 10 / 8 ft/min button
            b10Size = int(windowSize.width() * (11 / 96)), int(windowSize.height() * (9 / 59))  # original size (220, 180)
            b10Loc = int(windowSize.width() * (13 / 1920)), int(windowSize.height() * (192 / 295))  # original location (13, 768)
            self.b10.setFont(QFont('Arial', fontSize15))
            self.b10.resize(*b10Size)
            self.b10.move(*b10Loc)
            # Button 11 / 4 ft/min button
            b11Size = int(windowSize.width() * (11 / 96)), int(windowSize.height() * (9 / 59))  # original size (220, 180)
            b11Loc = int(windowSize.width() * (13 / 1920)), int(windowSize.height() * (469 / 590))  # original location (13, 938)
            self.b11.setFont(QFont('Arial', fontSize15))
            self.b11.resize(*b11Size)
            self.b11.move(*b11Loc)
            # Button 12 / Encoder button
            b12Size = int(windowSize.width() * (25 / 192)), int(windowSize.height() * (5 / 59))  # original size (250, 100)
            b12Loc = int(windowSize.width() * (79 / 640)), int(windowSize.height() * (17 / 236))  # original location (237, 85)
            self.b12.setFont(QFont('Arial', fontSize15))
            self.b12.resize(*b12Size)
            self.b12.move(*b12Loc)

            # roboclaw0 connected indicator label
            r0indLoc = int(windowSize.width() * (1 / 8)), int(windowSize.height() * (101 / 118))  # original location (240, 1010)
            self.r0ind.setFont(QFont('Arial', fontSize15))
            self.r0ind.move(*r0indLoc)
            # roboclaw1 connected indicator label
            r1indLoc = int(windowSize.width() * (7 / 48)), int(windowSize.height() * (101 / 118))  # original location (280, 1010)
            self.r1ind.setFont(QFont('Arial', fontSize15))
            self.r1ind.move(*r1indLoc)
            # roboclaw2 connected indicator label
            r2indLoc = int(windowSize.width() * (1 / 6)), int(windowSize.height() * (101 / 118))  # original location (320, 1010)
            self.r2ind.setFont(QFont('Arial', fontSize15))
            self.r2ind.move(*r2indLoc)
            # roboclaw3 connected indicator label
            r3indLoc = int(windowSize.width() * (3 / 16)), int(windowSize.height() * (101 / 118))  # original location (360, 1010)
            self.r3ind.setFont(QFont('Arial', fontSize15))
            self.r3ind.move(*r3indLoc)
            # roboclaw4 connected indicator label
            r4indLoc = int(windowSize.width() * (5 / 24)), int(windowSize.height() * (101 / 118))  # original location (400, 1010)
            self.r4ind.setFont(QFont('Arial', fontSize15))
            self.r4ind.move(*r4indLoc)
            # roboclaw5 connected indicator label
            r5indLoc = int(windowSize.width() * (11 / 48)), int(windowSize.height() * (101 / 118))  # original location (440, 1010)
            self.r5ind.setFont(QFont('Arial', fontSize15))
            self.r5ind.move(*r5indLoc)
            # roboclaw6 connected indicator label
            r6indLoc = int(windowSize.width() * (1 / 4)), int(windowSize.height() * (101 / 118))  # original location (480, 1010)
            self.r6ind.setFont(QFont('Arial', fontSize15))
            self.r6ind.move(*r6indLoc)
            # roboclaw7 connected indicator label
            r7indLoc = int(windowSize.width() * (13 / 48)), int(windowSize.height() * (101 / 118))  # original location (520, 1010)
            self.r7ind.setFont(QFont('Arial', fontSize15))
            self.r7ind.move(*r7indLoc)
            # sensor readout for air temperature label
            atSize = int(windowSize.width() * (19 / 64)), int(windowSize.height() * (5 / 118))  # original size (360, 50)
            atLoc = int(windowSize.width() * (1 / 8)), int(windowSize.height() * (947 / 1180))  # original location (240, 947)
            self.air_temp_label.setFont(QFont('Arial', fontSize15))
            self.air_temp_label.resize(*atSize)
            self.air_temp_label.move(*atLoc)
            # sensor readout for humidity label
            humSize = int(windowSize.width() * (19 / 64)), int(windowSize.height() * (5 / 118))  # original size (360, 50)
            humLoc = int(windowSize.width() * (1 / 8)), int(windowSize.height() * (228 / 295))  # original location (240, 912)
            self.humidity_label.setFont(QFont('Arial', fontSize15))
            self.humidity_label.resize(*humSize)
            self.humidity_label.move(*humLoc)
            # target speed label
            speedSize = int(windowSize.width() * (49 / 192)), int(windowSize.height() * (5 / 118))  # original size (490, 50)
            speedLoc = int(windowSize.width() * (47 / 192)), int(windowSize.height() * (273 / 295))  # original location (470, 1092)
            self.speed_label.setFont(QFont('Arial', fontSize15))
            self.speed_label.resize(*speedSize)
            self.speed_label.move(*speedLoc)
            # direction label
            dir_labelSize = int(windowSize.width() * (49 / 192)), int(windowSize.height() * (5 / 118))  # original size (490, 50)
            dir_labelLoc = int(windowSize.width() * (193 / 384)), int(windowSize.height() * (273 / 295))  # original location (965, 1092)
            self.dir_label.setFont(QFont('Arial', fontSize15))
            self.dir_label.resize(*dir_labelSize)
            self.dir_label.move(*dir_labelLoc)
            # toolbar and toolbar button fonts
            toolbar.setFixedSize(int(windowSize.width()), int(windowSize.height() * (3 / 59)))
            menu.setFont(QFont('Times', fontSize15))
            click_action.setFont(QFont('Times', fontSize15))
            change_folder_action.setFont(QFont('Times', fontSize15))
            click_refresh.setFont(QFont('Times', fontSize15))
            camera_selector.setFont(QFont('Times', fontSize15))
            # roboclaw readout labels
            rSize = int(windowSize.width() * 1500 / 1920), int(windowSize.height() * 5 / 118)  # original size (750, 50)
            self.R0_label.resize(*rSize)
            self.R1_label.resize(*rSize)
            self.R2_label.resize(*rSize)
            self.R3_label.resize(*rSize)
            self.R4_label.resize(*rSize)
            self.R5_label.resize(*rSize)
            self.R6_label.resize(*rSize)
            self.R7_label.resize(*rSize)
            self.R0_label.setFont(QFont('Arial', fontSize15))
            self.R1_label.setFont(QFont('Arial', fontSize15))
            self.R2_label.setFont(QFont('Arial', fontSize15))
            self.R3_label.setFont(QFont('Arial', fontSize15))
            self.R4_label.setFont(QFont('Arial', fontSize15))
            self.R5_label.setFont(QFont('Arial', fontSize15))
            self.R6_label.setFont(QFont('Arial', fontSize15))
            self.R7_label.setFont(QFont('Arial', fontSize15))

            c = 0  # this is a counter variable for the roboclaw readouts to make sure the stack below each other no
            # matter the order of the roboclaws that are connected
            h = 205  # height position of the first
            try:

                if r0[0] == 1:  # if roboclaw 0 is detected, roboclaw sends a [1,x] (x = version string) if the device
                    # is connected and a [0,0] is not.
                    rLoc = int(windowSize.width() * 1 / 8), int(windowSize.height() * ((h + (c * 36)) / 1180))
                    self.R0_label.move(*rLoc)
                    c = c + 1  # add to the counter so the program knows to place any subsequent roboclaw labels underneath this one
                if r1[0] == 1:
                    rLoc = int(windowSize.width() * 1 / 8), int(windowSize.height() * ((h + (c * 36)) / 1180))
                    self.R1_label.move(*rLoc)
                    c = c + 1
                if r2[0] == 1:
                    rLoc = int(windowSize.width() * 1 / 8), int(windowSize.height() * ((h + (c * 36)) / 1180))
                    self.R2_label.move(*rLoc)
                    c = c + 1
                if r3[0] == 1:
                    rLoc = int(windowSize.width() * 1 / 8), int(windowSize.height() * ((h + (c * 36)) / 1180))
                    self.R3_label.move(*rLoc)
                    c = c + 1
                if r4[0] == 1:
                    rLoc = int(windowSize.width() * 1 / 8), int(windowSize.height() * ((h + (c * 36)) / 1180))
                    self.R4_label.move(*rLoc)
                    c = c + 1
                if r5[0] == 1:
                    rLoc = int(windowSize.width() * 1 / 8), int(windowSize.height() * ((h + (c * 36)) / 1180))
                    self.R5_label.move(*rLoc)
                    c = c + 1
                if r6[0] == 1:
                    rLoc = int(windowSize.width() * 1 / 8), int(windowSize.height() * ((h + (c * 36)) / 1180))
                    self.R6_label.move(*rLoc)
                    c = c + 1
                if r7[0] == 1:
                    rLoc = int(windowSize.width() * 1 / 8), int(windowSize.height() * ((h + (c * 36)) / 1180))
                    self.R7_label.move(*rLoc)
            except NameError:  # if no variable (r0,r1,...) exists then pass over the function and do nothing
                pass

        ################################################################################################################
        ################################################################################################################

        centralWidget = QWidget(self)  # creates a central widget for the camera viewfinder
        self.setCentralWidget(centralWidget)  # adds the widget to the main window
        central_layout = QVBoxLayout(centralWidget)  # creates a layout for the central widget and the viewfinder
        central_layout.setAlignment(Qt.AlignCenter)  # aligning the viewfinder in the center of the viewfinder layout
        self.viewfinder = QCameraViewfinder(self)  # sets the camera viewfinder on the main window
        central_layout.addWidget(self.viewfinder)  # adding the viewfinder layout to the central layout

        # getting available cameras
        self.available_cameras = QCameraInfo.availableCameras()

        # if no camera found
        if not self.available_cameras:
            # exit the code
            sys.exit()

        # creating a status bar
        self.status = QStatusBar(self)

        # setting style sheet to the status bar
        self.status.setStyleSheet("background : white;")

        # adding status bar to the main window
        self.setStatusBar(self.status)

        # path to save
        self.save_path = ""

        # Set the default camera.
        self.select_camera(0)

        # creating a tool bar
        toolbar = QToolBar("Camera Tool Bar", self)
        self.addToolBar(toolbar)

        menu = QAction("MENU", self)
        menu.setFont(QFont('Times', 15))
        menu.setStatusTip("MENU")
        menu.setToolTip("MENU")
        toolbar.addAction(menu)

        # creating a photo action to take photo
        click_action = QAction("Click photo", self)
        click_action.setFont(QFont('Times', 15))
        # adding status tip to the photo action
        click_action.setStatusTip("This will capture picture")

        # adding tool tip
        click_action.setToolTip("Capture picture")

        # adding action to it
        # calling take_photo method
        click_action.triggered.connect(self.click_photo)

        # adding this to the tool bar
        toolbar.addAction(click_action)

        # similarly creating action for changing save folder
        change_folder_action = QAction("Change save location",
                                       self)
        change_folder_action.setFont(QFont('Times', 15))
        # adding status tip
        change_folder_action.setStatusTip("Change folder where picture will be saved saved.")

        # adding tool tip to it
        change_folder_action.setToolTip("Change save location")

        # setting calling method to the change folder action
        # when triggered signal is emitted
        change_folder_action.triggered.connect(self.change_folder)

        # adding this to the tool bar
        toolbar.addAction(change_folder_action)

        click_refresh = QAction("Refresh", self)  # adds a Refresh button to the toolbar
        click_refreshshortcut = QShortcut(QKeySequence("Ctrl+Shift+="), self)
        click_refresh.setFont(QFont('Times', 15))
        click_refresh.setStatusTip("Refresh Connected Roboclaws")  #
        click_refresh.setToolTip("Refresh")  # tool tip for the refresh button when hovering over it
        click_refresh.triggered.connect(self.rcRefresh)  # refresh button functionality, calls "rcRefresh" when clicked
        click_refreshshortcut.activated.connect(self.rcRefresh)
        toolbar.addAction(click_refresh)  # adds the button to the toolbar

        # creating a combo box for selecting camera
        camera_selector = QComboBox()
        camera_selector.setFont(QFont('Times', 15))

        # adding status tip to it
        camera_selector.setStatusTip("Choose camera to take pictures")

        # adding tool tip to it
        camera_selector.setToolTip("Select Camera")
        camera_selector.setToolTipDuration(2500)

        # adding items to the combo box
        camera_selector.addItems([camera.description()
                                  for camera in self.available_cameras])

        # calling the select camera method
        camera_selector.currentIndexChanged.connect(self.select_camera)

        # adding this to tool bar
        toolbar.addWidget(camera_selector)

        # setting tool bar stylesheet
        toolbar.setStyleSheet("background : white;")
        # setting window title
        self.setWindowTitle("IAS Control System")

        # setting the timer for the roboclaw_readout updates, just add more "self.qTimer.timeout.connect(self.____)"
        # to add other components to the timer
        self.qTimer = QTimer()  # creating a timer for roboclaw readouts, used to update values shown on OSD
        self.qTimer.setInterval(250)  # quarter second intervals
        self.qTimer.timeout.connect(self.roboclaw_readout)  # connecting the speed readout object

        self.thTimer = QTimer()  # Timer for the air temperature and humidity readout
        self.thTimer.setInterval(2000)  # 2 second intervals.

        self.vCheckTimer = QTimer()  # Timer for the version check for the roboclaws
        self.vCheckTimer.setInterval(1000)  # Checks the version every 1 second
        self.vCheckTimer.timeout.connect(
            self.version_check)  # connect the function "version_check" every 1 second

        ################################################################################################################
        # creating and designing the buttons and OSD display labels
        ################################################################################################################
        # Air temp widget for OSD
        self.air_temp_label = QtWidgets.QLabel(self)
        self.air_temp_label.setFont(QFont('Arial', 15))  # text font and size
        self.air_temp_label.setStyleSheet('background : transparent;' 'color : white;')

        # Humidity widget for OSD
        self.humidity_label = QtWidgets.QLabel(self)
        self.humidity_label.setFont(QFont('Arial', 15))  # text font and size
        self.humidity_label.setStyleSheet('background : transparent;' 'color : white;')

        self.R0_label = QtWidgets.QLabel(self)
        self.R0_label.setFont(QFont('Arial', 15))
        self.R0_label.setStyleSheet('background : transparent;' 'color : white;')
        self.R0_label.move(240, 205)

        self.R1_label = QtWidgets.QLabel(self)
        self.R1_label.setFont(QFont('Arial', 15))
        self.R1_label.setStyleSheet('background : transparent;' 'color : white;')
        self.R1_label.move(240, 205)

        self.R2_label = QtWidgets.QLabel(self)
        self.R2_label.setFont(QFont('Arial', 15))
        self.R2_label.setStyleSheet('background : transparent;' 'color : white;')
        self.R2_label.move(240, 205)

        self.R3_label = QtWidgets.QLabel(self)
        self.R3_label.setFont(QFont('Arial', 15))
        self.R3_label.setStyleSheet('background : transparent;' 'color : white;')
        self.R3_label.move(240, 205)

        self.R4_label = QtWidgets.QLabel(self)
        self.R4_label.setFont(QFont('Arial', 15))
        self.R4_label.setStyleSheet('background : transparent;' 'color : white;')
        self.R4_label.move(240, 205)

        self.R5_label = QtWidgets.QLabel(self)
        self.R5_label.setFont(QFont('Arial', 15))
        self.R5_label.setStyleSheet('background : transparent;' 'color : white;')
        self.R5_label.move(240, 205)

        self.R6_label = QtWidgets.QLabel(self)
        self.R6_label.setFont(QFont('Arial', 15))
        self.R6_label.setStyleSheet('background : transparent;' 'color : white;')
        self.R6_label.move(240, 205)

        self.R7_label = QtWidgets.QLabel(self)
        self.R7_label.setFont(QFont('Arial', 15))
        self.R7_label.setStyleSheet('background : transparent;' 'color : white;')
        self.R7_label.move(240, 205)

        # target speed widget
        self.speed_label = QtWidgets.QLabel(self)
        self.speed_label.setText("SET V:00 ft/min")
        self.speed_label.setFont(QFont('Arial', 15))
        self.speed_label.setStyleSheet("background : white;")

        # direction widget
        self.dir_label = QtWidgets.QLabel(self)
        self.dir_label.setText("DIR: N")
        self.dir_label.setFont(QFont('Arial', 15))
        self.dir_label.setStyleSheet("background : white;")

        # push button set up and design
        # button 1 / stop button
        self.b1 = QtWidgets.QPushButton(self)
        self.b1shortcut = QShortcut(QKeySequence("Ctrl+Shift+S"), self) # setting up the Keyboard shortcut for the stop button
        self.b1.setText("STOP 🛑")
        self.b1.setFont(QFont('Arial', 12))
        self.b1.setStyleSheet("background-color : red")
        self.b1.clicked.connect(self.button1_clicked)
        self.b1shortcut.activated.connect(self.button1_clicked)  # when the shortcut is used run the button 1 function
        # button 2 / Forward button
        self.b2 = QtWidgets.QPushButton(self)
        self.b2shortcut = QShortcut(QKeySequence("Ctrl+Shift+F"), self)
        self.b2.setText("FORWARD ⏩")
        self.b2.setFont(QFont('Arial', 12))
        self.b2.clicked.connect(self.button2_clicked)
        self.b2shortcut.activated.connect(self.button2_clicked)
        # button 3 / Reverse Button
        self.b3 = QtWidgets.QPushButton(self)
        self.b3shortcut = QShortcut(QKeySequence("Ctrl+Shift+R"), self)
        self.b3.setText("REVERSE ⏪")
        self.b3.setFont(QFont('Arial', 12))
        self.b3.clicked.connect(self.button3_clicked)
        self.b3shortcut.activated.connect(self.button3_clicked)
        # button 4 / Power up button
        self.b4 = QtWidgets.QPushButton(self)
        self.b4shortcut = QShortcut(QKeySequence("Ctrl+Shift+U"), self)
        self.b4.setText("POWER ⬆️")
        self.b4.setFont(QFont('Arial', 12))
        self.b4.clicked.connect(self.button4_clicked)
        self.b4shortcut.activated.connect(self.button4_clicked)
        # button 5 / Power Down button
        self.b5 = QtWidgets.QPushButton(self)
        self.b5shortcut = QShortcut(QKeySequence("Ctrl+Shift+D"), self)
        self.b5.setText("POWER ⬇️")
        self.b5.setFont(QFont('Arial', 12))
        self.b5.clicked.connect(self.button5_clicked)
        self.b5shortcut.activated.connect(self.button5_clicked)
        # button 6 / 24 ft/min
        self.b6 = QtWidgets.QPushButton(self)
        self.b6shortcut = QShortcut(QKeySequence("Ctrl+Shift+6"), self)
        self.b6.setText("24 ft/min")
        self.b6.setFont(QFont('Arial', 12))
        self.b6.clicked.connect(self.button6_clicked)
        self.b6shortcut.activated.connect(self.button6_clicked)
        # button 7 / 20 ft/min
        self.b7 = QtWidgets.QPushButton(self)
        self.b7shortcut = QShortcut(QKeySequence("Ctrl+Shift+5"), self)
        self.b7.setText("20 ft/min")
        self.b7.setFont(QFont('Arial', 15))
        self.b7.clicked.connect(self.button7_clicked)
        self.b7shortcut.activated.connect(self.button7_clicked)
        # button 8 / 16 ft/min
        self.b8 = QtWidgets.QPushButton(self)
        self.b8shortcut = QShortcut(QKeySequence("Ctrl+Shift+4"), self)
        self.b8.setText("16 ft/min")
        self.b8.setFont(QFont('Arial', 15))
        self.b8.clicked.connect(self.button8_clicked)
        self.b8shortcut.activated.connect(self.button8_clicked)
        # button 9 / 12 ft/min
        self.b9 = QtWidgets.QPushButton(self)
        self.b9shortcut = QShortcut(QKeySequence("Ctrl+Shift+3"), self)
        self.b9.setText("12 ft/min")
        self.b9.setFont(QFont('Arial', 15))
        self.b9.clicked.connect(self.button9_clicked)
        self.b9shortcut.activated.connect(self.button9_clicked)
        # button 10 / 8 ft/min
        self.b10 = QtWidgets.QPushButton(self)
        self.b10shortcut = QShortcut(QKeySequence("Ctrl+Shift+2"), self)
        self.b10.setText("8 ft/min")
        self.b10.setFont(QFont('Arial', 15))
        self.b10.clicked.connect(self.button10_clicked)
        self.b10shortcut.activated.connect(self.button10_clicked)
        # button 11 / 4 ft/min
        self.b11 = QtWidgets.QPushButton(self)
        self.b11shortcut = QShortcut(QKeySequence("Ctrl+Shift+1"), self)
        self.b11.setText("4 ft/min")
        self.b11.setFont(QFont('Arial', 15))
        self.b11.clicked.connect(self.button11_clicked)
        self.b11shortcut.activated.connect(self.button11_clicked)
        # button 12 / Encoder reset
        self.b12 = QtWidgets.QPushButton(self)
        self.b12.setText("Encoder Reset")
        self.b12.setFont(QFont('Arial', 15))
        self.b12.clicked.connect(self.button12_clicked)

        self.r0ind = QtWidgets.QLabel(self)
        self.r0ind.setText("r0")
        self.r0ind.setFont(QFont('Arial', 15))
        self.r0ind.setStyleSheet('background : transparent;' 'color : red;')
        self.r0ind.resize(50, 50)

        self.r1ind = QtWidgets.QLabel(self)
        self.r1ind.setText("r1")
        self.r1ind.setFont(QFont('Arial', 15))
        self.r1ind.setStyleSheet('background : transparent;' 'color : red;')
        self.r1ind.resize(50, 50)

        self.r2ind = QtWidgets.QLabel(self)
        self.r2ind.setText("r2")
        self.r2ind.setFont(QFont('Arial', 15))
        self.r2ind.setStyleSheet('background : transparent;' 'color : red;')
        self.r2ind.resize(50, 50)

        self.r3ind = QtWidgets.QLabel(self)
        self.r3ind.setText("r3")
        self.r3ind.setFont(QFont('Arial', 15))
        self.r3ind.setStyleSheet('background : transparent;' 'color : red;')
        self.r3ind.resize(50, 50)

        self.r4ind = QtWidgets.QLabel(self)
        self.r4ind.setText("r4")
        self.r4ind.setFont(QFont('Arial', 15))
        self.r4ind.setStyleSheet('background : transparent;' 'color : red;')
        self.r4ind.resize(50, 50)

        self.r5ind = QtWidgets.QLabel(self)
        self.r5ind.setText("r5")
        self.r5ind.setFont(QFont('Arial', 15))
        self.r5ind.setStyleSheet('background : transparent;' 'color : red;')
        self.r5ind.resize(50, 50)

        self.r6ind = QtWidgets.QLabel(self)
        self.r6ind.setText("r6")
        self.r6ind.setFont(QFont('Arial', 15))
        self.r6ind.setStyleSheet('background : transparent;' 'color : red;')
        self.r6ind.resize(50, 50)

        self.r7ind = QtWidgets.QLabel(self)
        self.r7ind.setText("r7")
        self.r7ind.setFont(QFont('Arial', 15))
        self.r7ind.setStyleSheet('background : transparent;' 'color : red;')
        self.r7ind.resize(50, 50)

        self.show()
        ################################################################################################################
        ################################################################################################################

        enc_popup = QMessageBox(self)  # popup that is placed on the window to make sure the user wants to reset the encoders
        enc_popup.setIcon(QMessageBox.Question)  # icon for the popup window
        enc_popup.setText(
            "This button resets the encoder position, Are you sure you want to reset?")  # text on the popup window
        enc_popup.setStandardButtons(QMessageBox.Yes | QMessageBox.No)  # given options on the popup window
        enc_popup.setWindowTitle("Warning")
        enc_popup.buttonClicked.connect(self.erp_button_clicked)  # connects the erp_button_clicked function to read what button is clicked

        ################################################################################################################
        # This is where the code searches through available COM ports and tests whether the temp and humidity sensor are
        # connected.
        ################################################################################################################
        #
        p = 0  # placeholder value that allows the while loop to run
        num = 0  # this is the COM number that the while loop starts with
        while p == 0:
            try:
                if num >= 0 and num <= 256:
                    # below are the sensors desired settings to read values
                    ####################################################################################################
                    sensor = minimalmodbus.Instrument('COM'f'{num}', 1, mode=minimalmodbus.MODE_RTU)
                    sensor.serial.baudrate = 38400  # Baud
                    sensor.serial.bytesize = 8
                    sensor.serial.parity = minimalmodbus.serial.PARITY_NONE
                    sensor.serial.stopbits = 1
                    sensor.serial.timeout = .2
                    ####################################################################################################
                    sensor.read_register(0)  # if a valid COM port is read then send a byte to register 0 and see if
                    # any value comes back if not then it moves to next COM port
                    ths_com = 'COM'f'{num}'
                    print('The sensors COM port is  ' + ths_com)
                    p = 1  # when p = 1 the while loop terminates and the program can move on to the next action
                else:
                    p = 1  # if the while loop gets to 256 without finding an open COM port then stop the loop

            except serial.serialutil.SerialException:  # if the specified  port is not open then add to the number
                # count and close the serial port
                num = num + 1
                try:
                    sensor.serial.close()  # close the serial communication with the port that was checked if the port is
                    # open but the sensor is not connected to that port
                except NameError:
                    pass
            except minimalmodbus.NoResponseError:  # if the port is open but the device does not send data back to
                # the computer then add to the number count and close the serial port
                num = num + 1
                try:
                    sensor.serial.close()
                except NameError:
                    pass
            except minimalmodbus.InvalidResponseError:  # if the computer receives a byte with a bad checksum add 1 to
                # the counter and try closing the serial communication on that port
                num = num + 1
                try:
                    sensor.serial.close()
                except NameError:
                    pass

        ################################################################################################################
        ################################################################################################################

        ################################################################################################################
        # This is where the code searches through available COM ports and tests what COM port the Roboclaws are
        # connected to.
        ################################################################################################################
        # global variables/functions for this section
        global roboclaw
        global detectpopup
        global rc_num
        global rc
        global r0
        global r1
        global r2
        global r3
        global r4
        global r5
        global r6
        global r7
        windowSize = self.size()
        if not portList:  # if there are no ports available...
            self.com_error()  # show com_error popup window
            rc = []  # rc = empty array
            r0 = [0, 0]  # r0, r1, r2, etc. are the status of each of the 8 Roboclaws that can be connected
            r1 = [0, 0]  # if x = 0 in the matrix [x, 0] then there is no Roboclaw connected
            r2 = [0, 0]  # so these are a placeholder for each of the Roboclaws statuses.
            r3 = [0, 0]
            r4 = [0, 0]
            r5 = [0, 0]
            r6 = [0, 0]
            r7 = [0, 0]
        else:  # if there are ports available...
            r0 = [0, 0]  # r0, r1, r2, etc. are the status of each of the 8 Roboclaws that can be connected
            r1 = [0, 0]  # if x = 0 in the matrix [x, 0] then there is no Roboclaw connected
            r2 = [0, 0]  # so these are a placeholder for each of the Roboclaws statuses.
            r3 = [0, 0]
            r4 = [0, 0]
            r5 = [0, 0]
            r6 = [0, 0]
            r7 = [0, 0]

            k = 0  # a placeholder value that indicates whether the while loop below runs or not
            rc_num = 0  # the roboclaw COM port number the loop starts on
            while k == 0:  # place holder value
                if 0 <= rc_num <= 256:
                    try:
                        roboclaw = Roboclaw('COM'f'{rc_num}', 38400)
                        roboclaw.Open()
                        test_rc = roboclaw.ReadVersion(
                            0x80)  # version check to see if a roboclaw is connected to the port
                        if test_rc[0] == 0:  # if there is no roboclaw add the counter and move on to the next port
                            rc_num = rc_num + 1
                        else:  # if there is a roboclaw, save the COM port as rc, print the COM port, and end the loop
                            rc = 'COM'f'{rc_num}'
                            print('rc COM is ' + rc)
                            k = 1
                    except AttributeError:  # if the port cannot be opened then add to the counter nad move on to the
                        # next COM port
                        rc_num = rc_num + 1
                    except serial.serialutil.SerialException:
                        rc_num = rc_num + 1
                else:
                    rc = []  # this allows rc to be specified later in the code when the refresh button is clicked
                    self.com_error()  # if there is no open COM ports then open the com_error window and stop the loop
                    k = 1
            ############################################################################################################
            ############################################################################################################

            is_detecting = 1  # variable for the status of the while loop
            while is_detecting == 1:
                detectpopup = QDialog(self)  # setting up the "searching for Roboclaws" popup.
                detectpopup.resize(450, 1)
                detectpopup.setWindowTitle('Detecting Roboclaws')
                detectpopup.open()

                try:  # idicates whether a roboclaw is connected and stores the values in r0, r1, etc.
                    r0 = roboclaw.ReadVersion(0x80)
                    r1 = roboclaw.ReadVersion(0x81)
                    r2 = roboclaw.ReadVersion(0x82)
                    r3 = roboclaw.ReadVersion(0x83)
                    r4 = roboclaw.ReadVersion(0x84)
                    r5 = roboclaw.ReadVersion(0x85)
                    r6 = roboclaw.ReadVersion(0x86)
                    r7 = roboclaw.ReadVersion(0x87)
                except AttributeError:  # if no port is available then pass over the try function.
                    pass

                if r0[0] == 1:  # if a roboclaw is connected change the r0 indicator color to green.
                    self.r0ind.setStyleSheet('background : transparent;' 'color : green;')
                if r1[0] == 1:
                    self.r1ind.setStyleSheet('background : transparent;' 'color : green;')
                if r2[0] == 1:
                    self.r2ind.setStyleSheet('background : transparent;' 'color : green;')
                if r3[0] == 1:
                    self.r3ind.setStyleSheet('background : transparent;' 'color : green;')
                if r4[0] == 1:
                    self.r4ind.setStyleSheet('background : transparent;' 'color : green;')
                if r5[0] == 1:
                    self.r5ind.setStyleSheet('background : transparent;' 'color : green;')
                if r6[0] == 1:
                    self.r6ind.setStyleSheet('background : transparent;' 'color : green;')
                if r7[0] == 1:
                    self.r7ind.setStyleSheet('background : transparent;' 'color : green;')

                is_detecting = 0  # stop detecting for roboclaws
            else:  # after detection of Roboclaws is complete then the popup closes
                detectpopup.close()

            self.vCheckTimer.start()
            self.qTimer.start()

            status = sensor.serial.isOpen()  # checking if the serial port is open for the sensor
            if status == True:
                self.thTimer.timeout.connect(self.air_temp_readout)  # connecting the temp readout function
                self.thTimer.timeout.connect(self.humid_readout)  # connecting the humidity readout function
                self.thTimer.start()

            else:  # if serial port is not open do nothing
                pass

    def com_error(self):  # error message that pops up when the com_error method is called in the code.

        com_popup = QMessageBox(self)
        com_popup.setIcon(QMessageBox.Warning)
        com_popup.setText(
            "Roboclaws are not connected to a COM port, please connected the Roboclaws and click the Refresh button")
        com_popup.setStandardButtons(QMessageBox.Ok)
        com_popup.setWindowTitle("COM Port")
        com_popup.exec_()

    def select_camera(self, i):

        # getting the selected camera
        self.camera = QCamera(self.available_cameras[i])

        # setting view finder to the camera
        self.camera.setViewfinder(self.viewfinder)
        # setting capture mode to the camera
        self.camera.setCaptureMode(QCamera.CaptureStillImage)

        # if any error occur show the alert
        self.camera.error.connect(lambda: self.alert(self.camera.errorString()))
        # start the camera
        self.camera.start()

        # creating a QCameraImageCapture object
        self.capture = QCameraImageCapture(self.camera)

        # showing alert if error occur
        self.capture.error.connect(lambda error_msg, error,
                                          msg: self.alert(msg))

        # when image captured showing message
        self.capture.imageCaptured.connect(lambda d,
                                                  i: self.status.showMessage("Image captured : "
                                                                             + str(self.save_seq)))

        # getting current camera name
        self.current_camera_name = self.available_cameras[i].description()

        # initial save sequence
        self.save_seq = 0

    # method to take photo
    def click_photo(self):

        # time stamp
        timestamp = time.strftime("%d-%b-%Y-%H_%M_%S")

        # capture the image and save it on the save path
        self.capture.capture(os.path.join(self.save_path,
                                          "%s-%04d-%s.jpg" % (
                                              self.current_camera_name,
                                              self.save_seq,
                                              timestamp
                                          )))

        # increment the sequence
        self.save_seq += 1

    # change folder method
    def change_folder(self):

        # open the dialog to select path
        path = QFileDialog.getExistingDirectory(self,
                                                "Picture Location", "")

        # if path is selected
        if path:
            # update the path
            self.save_path = path
            # update the sequence
            self.save_seq = 0

    # method for alerts
    def alert(self, msg):

        # error message
        error = QErrorMessage(self)

        # setting text to the error message
        error.showMessage(msg)

    def rcRefresh(self):  # the function rcRefresh makes the code go through the "start-up" action again, ie searching
        # for COM Ports, indicating what Roboclaws are currently connected to the system, etc.
        global r0
        global r1
        global r2
        global r3
        global r4
        global r5
        global r6
        global r7
        global sensor
        global onePort
        global rc
        global roboclaw
        global rc_num
        global detectpopup
        global com_err
        global is_detecting
        p = 0
        num = 0
        windowSize = self.size()
        while p == 0:
            try:
                if num >= 0 and num <= 256 and num != rc:
                    sensor = minimalmodbus.Instrument('COM'f'{num}', 1, mode=minimalmodbus.MODE_RTU)
                    sensor.serial.baudrate = 38400  # Baud
                    sensor.serial.bytesize = 8
                    sensor.serial.parity = minimalmodbus.serial.PARITY_NONE
                    sensor.serial.stopbits = 1
                    sensor.serial.timeout = .2
                    sensor.read_register(0)

                    status = sensor.serial.isOpen()
                    if status == True:
                        self.thTimer.timeout.connect(self.air_temp_readout)  # connecting the temp readout object
                        self.thTimer.timeout.connect(self.humid_readout)
                        self.thTimer.start()
                    ths_com = 'COM'f'{num}'
                    print('sensor com port is  ' + ths_com)
                    p = 1
                elif num == rc:
                    pass
                else:
                    self.vCheckTimer.start()
                    self.qTimer.start()
                    p = 1
                    try:
                        sensor.serial.close()
                    except NameError:
                        pass
                    self.air_temp_label.setText(" ")
                    self.humidity_label.setText(" ")
            except serial.serialutil.SerialException:
                num = num + 1
                try:
                    sensor.serial.close()
                except NameError:
                    pass
            except minimalmodbus.NoResponseError:
                num = num + 1
                try:
                    sensor.serial.close()
                except NameError:
                    pass
            except minimalmodbus.InvalidResponseError:
                num = num + 1
                try:
                    sensor.serial.close()
                except NameError:
                    pass

        k = 0
        rc_num = 0
        while k == 0:
            if 0 <= rc_num <= 256:
                try:
                    roboclaw = Roboclaw('COM'f'{rc_num}', 38400)
                    roboclaw.Open()
                    test_rc = roboclaw.ReadVersion(0x80)
                    if test_rc[0] == 0:
                        rc_num = rc_num + 1
                    else:
                        rc = 'COM'f'{rc_num}'
                        print('rc COM is ' + rc)
                        com_err = False
                        self.vCheckTimer.start()
                        self.qTimer.start()
                        k = 1

                except AttributeError:
                    rc_num = rc_num + 1
            else:
                self.com_error()
                com_err = True
                k = 1

        if com_err == True:
            is_detecting = 0
        else:
            is_detecting = 1

        while is_detecting == 1:
            detectpopup = QDialog(self)
            detectpopup.resize(450, 1)
            detectpopup.setWindowTitle('Detecting Roboclaws')
            detectpopup.open()
            try:
                r0 = roboclaw.ReadVersion(0x80)
                r1 = roboclaw.ReadVersion(0x81)
                r2 = roboclaw.ReadVersion(0x82)
                r3 = roboclaw.ReadVersion(0x83)
                r4 = roboclaw.ReadVersion(0x84)
                r5 = roboclaw.ReadVersion(0x85)
                r6 = roboclaw.ReadVersion(0x86)
                r7 = roboclaw.ReadVersion(0x87)
            except AttributeError:
                pass

            h = 205
            c = 0
            if r0[0] == 1:  # if a roboclaw is connected change the r0 indicator color to green.
                self.r0ind.setStyleSheet('background : transparent;' 'color : green;')
                rLoc = int(windowSize.width() * 1 / 8), int(windowSize.height() * ((h + (c * 36)) / 1180))
                self.R0_label.move(*rLoc)
                c = c + 1
            if r1[0] == 1:
                self.r1ind.setStyleSheet('background : transparent;' 'color : green;')
                rLoc = int(windowSize.width() * 1 / 8), int(windowSize.height() * ((h + (c * 36)) / 1180))
                self.R1_label.move(*rLoc)
                c = c + 1
            if r2[0] == 1:
                self.r2ind.setStyleSheet('background : transparent;' 'color : green;')
                rLoc = int(windowSize.width() * 1 / 8), int(windowSize.height() * ((h + (c * 36)) / 1180))
                self.R2_label.move(*rLoc)
                c = c + 1
            if r3[0] == 1:
                self.r3ind.setStyleSheet('background : transparent;' 'color : green;')
                rLoc = int(windowSize.width() * 1 / 8), int(windowSize.height() * ((h + (c * 36)) / 1180))
                self.R3_label.move(*rLoc)
                c = c + 1
            if r4[0] == 1:
                self.r4ind.setStyleSheet('background : transparent;' 'color : green;')
                rLoc = int(windowSize.width() * 1 / 8), int(windowSize.height() * ((h + (c * 36)) / 1180))
                self.R4_label.move(*rLoc)
                c = c + 1
            if r5[0] == 1:
                self.r5ind.setStyleSheet('background : transparent;' 'color : green;')
                rLoc = int(windowSize.width() * 1 / 8), int(windowSize.height() * ((h + (c * 36)) / 1180))
                self.R5_label.move(*rLoc)
                c = c + 1
            if r6[0] == 1:
                self.r6ind.setStyleSheet('background : transparent;' 'color : green;')
                rLoc = int(windowSize.width() * 1 / 8), int(windowSize.height() * ((h + (c * 36)) / 1180))
                self.R5_label.move(*rLoc)
                c = c + 1
            if r7[0] == 1:
                self.r7ind.setStyleSheet('background : transparent;' 'color : green;')
                rLoc = int(windowSize.width() * 1 / 8), int(windowSize.height() * ((h + (c * 36)) / 1180))
                self.R7_label.move(*rLoc)

            is_detecting = 0
        else:
            try:
                detectpopup.close()
            except NameError:
                pass

    def version_check(self):  # constantly check whether the connected roboclaws are still connected to the system to
        # reduce disconnection lag in the GUI, and to streamline reconnection of the roboclaws
        global r0
        global r1
        global r2
        global r3
        global r4
        global r5
        global r6
        global r7
        global r0c
        global r1c
        global r2c
        global r3c
        global r4c
        global r5c
        global r6c
        global r7c
        try:
            if r0[0] == 1:  # checks if the specified roblaw is still connected, if it is not it passes over the version check
                r0c = roboclaw.ReadVersion(0x80)  # checking the version of the roboclaw
                if r0c[0] == 0:  # if the first position in the version check array is zero then the device is disconnected
                    self.r0ind.setStyleSheet('background : transparent;' 'color : red;')
                    self.R0_label.setText('r0 Error - Device Serial Disconnected')  # indicated that the roboclaw is disconnected
                    r0 = [0, 1]  # creating a dummy array to show that the roboclaw WAS connected but is now not connected
            if r1[0] == 1:  # same concept as the previous and going forwards up until "if r7[0] == 0:"
                r1c = roboclaw.ReadVersion(0x81)
                if r1c[0] == 0:
                    self.r1ind.setStyleSheet('background : transparent;' 'color : red;')
                    self.R1_label.setText('r1 Error - Device Serial Disconnected')
                    r1 = [0, 1]
            if r2[0] == 1:
                r2c = roboclaw.ReadVersion(0x82)
                if r2c[0] == 0:
                    self.r2ind.setStyleSheet('background : transparent;' 'color : red;')
                    self.R2_label.setText('r2 Error - Device Serial Disconnected')
                    r2 = [0, 1]
            if r3[0] == 1:
                r3c = roboclaw.ReadVersion(0x83)
                if r3c[0] == 0:
                    self.r3ind.setStyleSheet('background : transparent;' 'color : red;')
                    self.R3_label.setText('r3 Error - Device Serial Disconnected')
                    r3 = [0, 1]
            if r4[0] == 1:
                r4c = roboclaw.ReadVersion(0x84)
                if r4c[0] == 0:
                    self.r4ind.setStyleSheet('background : transparent;' 'color : red;')
                    self.R4_label.setText('r4 Error - Device Serial Disconnected')
                    r4 = [0, 1]
            if r5[0] == 1:
                r5c = roboclaw.ReadVersion(0x85)
                if r5c[0] == 0:
                    self.r5ind.setStyleSheet('background : transparent;' 'color : red;')
                    self.R5_label.setText('r5 Error - Device Serial Disconnected')
                    r5 = [0, 1]
            if r6[0] == 1:
                r6c = roboclaw.ReadVersion(0x86)
                if r6c[0] == 0:
                    self.r6ind.setStyleSheet('background : transparent;' 'color : red;')
                    self.R6_label.setText('r6 Error - Device Serial Disconnected')
                    r6 = [0, 1]
            if r7[0] == 1:
                r7c = roboclaw.ReadVersion(0x87)
                if r7c[0] == 0:
                    self.r7ind.setStyleSheet('background : transparent;' 'color : red;')
                    self.R7_label.setText('r7 Error - Device Serial Disconnected')
                    r7 = [0, 1]
            # this is how the roboclaws automatically know that it has been reconnected
            if r0[1] == 1:  # checks to see if the 2nd position in the r0 array is 1
                r0c = roboclaw.ReadVersion(0x80)  # reads the version of the roboclaw
                if r0c[0] == 1:  # if the roboclaw is connected indicate that the roboclaw is reconnected
                    self.r0ind.setStyleSheet('background : transparent;' 'color : green;')
                    r0 = r0c  # inputting the array found in r0c into r0
            if r1[1] == 1:  # same concept all the way down to r7
                r1c = roboclaw.ReadVersion(0x81)
                if r1c[0] == 1:
                    self.r1ind.setStyleSheet('background : transparent;' 'color : green;')
                    r1 = r1c
            if r2[1] == 1:
                r2c = roboclaw.ReadVersion(0x82)
                if r2c[0] == 1:
                    self.r2ind.setStyleSheet('background : transparent;' 'color : green;')
                    r2 = r2c
            if r3[1] == 1:
                r3c = roboclaw.ReadVersion(0x83)
                if r3c[0] == 1:
                    self.r3ind.setStyleSheet('background : transparent;' 'color : green;')
                    r3 = r3c
            if r4[1] == 1:
                r4c = roboclaw.ReadVersion(0x84)
                if r4c[0] == 1:
                    self.r4ind.setStyleSheet('background : transparent;' 'color : green;')
                    r4 = r4c
            if r5[1] == 1:
                r5c = roboclaw.ReadVersion(0x85)
                if r5c[0] == 1:
                    self.r5ind.setStyleSheet('background : transparent;' 'color : green;')
                    r5 = r5c
            if r6[1] == 1:
                r6c = roboclaw.ReadVersion(0x86)
                if r6c[0] == 1:
                    self.r6ind.setStyleSheet('background : transparent;' 'color : green;')
                    r7 = r6c
            if r7[1] == 1:
                r7c = roboclaw.ReadVersion(0x87)
                if r7c[0] == 1:
                    self.r7ind.setStyleSheet('background : transparent;' 'color : green;')
                    r7 = r7c
        except serial.serialutil.SerialException:  # error handling
            pass
        except AttributeError:  # error handling
            pass

    def air_temp_readout(self):

        sensor.clear_buffers_before_each_transaction = True
        try:
            T = sensor.read_register(2)  # reading register 2 from the sensor
            self.air_temp_label.setText(str(round((T * .05), 1)) + "°F")  # sensor datasheet specified math to get a correct reading
        except minimalmodbus.NoResponseError:  # no sensor detected error handling
            self.air_temp_label.setText("error - no power to instrument")
        except serial.serialutil.SerialException:  # no serial connection with sensor error handling
            self.air_temp_label.setText("error - bad serial connection")
        except minimalmodbus.InvalidResponseError:  # sensor did not get a valid response, error handling
            self.air_temp_label.setText("error")

    def humid_readout(self):

        sensor.clear_buffers_before_each_transaction = True
        try:
            H = sensor.read_register(0) # reading register 0 from the sensor
            self.humidity_label.setText(str(round((H * .01), 1)) + "%RH")  # sensor datasheet specified math to get an accurate reading
        except minimalmodbus.NoResponseError:   # no sensor detected error handling
            self.humidity_label.setText("error - no power to instrument")
        except serial.serialutil.SerialException:  # no serial connection with sensor error handling
            self.humidity_label.setText("error - bad serial connection")
        except minimalmodbus.InvalidResponseError:  # sensor did not get a valid response, error handling
            self.humidity_label.setText("error")

    def roboclaw_readout(self):

        d = 1.845  # wheel diameter used on the Crawler in inches
        C = 3.14 * d  # circumference of the wheel
        ppr = 200  # pulses per rev of the encoder
        # each if statement below is the same, except it is being done for each of the different roboclaw addresses so
        # the comments on roboclaw0 (r0) will be applicable for the other roboclaws
        try:
            # roboclaw 0 / Master roboclaw
            if r0[0] == 1 & r0c[0] == 1:
                sr0 = roboclaw.ReadSpeedM1(0x80)  # reading the speed of the roboclaw based on the encoder input
                tr0 = roboclaw.ReadTemp(0x80)  # reading the board temperature of the roboclaw
                bvr0 = roboclaw.ReadMainBatteryVoltage(0x80)  # Reading the battery voltage of the roboclaw
                cr0 = roboclaw.ReadCurrents(0x80)  # reading the current of the roboclaw
                pr0 = roboclaw.ReadEncM1(0x80)  # reading the encoder count
                error0 = roboclaw.ReadError(0x80)  # read the status of the roboclaw
                try:  # using a try to allow for any invalid reading to be passed
                    sr0_v = str(round(.0432 * sr0[1], 1))  # setting a variable to hold the value of the speed reading
                    # and converting from pulses per second to ft/min
                    tr0_v = str(round((1.8 * (.1 * tr0[1]) + 32), 1))  # setting a variable to hold the value of the
                    # temprature reading and converting the value from a tenth of a degree in Celsius to Fahrenheit
                    bvr0_v = str(round(.1 * bvr0[1], 1))  # setting a variable to hold the battery voltage  reading
                    cr0_v = str(round(.01 * cr0[1], 1))  # setting a variable to hold the current reading
                    pr0_v = str(round((1 / (((4 * ppr) / C) * 12)) * pr0[1], 2))  # setting a variable to read the
                    # encoder count of the roboclaw and to convert that value to feet

                    # below are all possible error messages that the roboclaw can send, this is according to the roboclaw
                    # user manual labeled function 90 - Read Status
                    if error0[1] == 0:  # 0x000000 roboclaw is working normally
                        error0_v = "Normal Op."
                    elif error0[1] == 1:  # 0x000001 value in hexadecimal for reference in the user manual
                        error0_v = "E-Stop"
                    elif error0[1] == 2:  # 0x000002
                        error0_v = "Temperature Error"
                    elif error0[1] == 8:  # 0x000008
                        error0_v = "Main Voltage High Error"
                    elif error0[1] == 16:  # 0x000010
                        error0_v = "Logic Voltage High Error"
                    elif error0[1] == 32:  # 0x000020
                        error0_v = "Logic Voltage Low Error"
                    elif error0[1] == 64:  # 0x000040
                        error0_v = "Driver Fault Error"
                    elif error0[1] == 256:  # 0x000100
                        error0_v = "Speed Error"
                    elif error0[1] == 1024:  # 0x000400
                        error0_v = "Position Error"
                    elif error0[1] == 4096:  # 0x001000
                        error0_v = "Current Error"
                    elif error0[1] == 65536:  # 0x010000
                        error0_v = "Over Current Warning"
                    elif error0[1] == 262144:  # 0x040000
                        error0_v = "Main Voltage High Warning"
                    elif error0[1] == 524288:  # 0x080000
                        error0_v = "Main Voltage Low Warning "
                    elif error0[1] == 1048576:  # 0x100000
                        error0_v = "Temperature Warning"
                    else:
                        error0_v = " "

                except UnboundLocalError:  # pass over these variables if they cannot be defined
                    pass

                try:  # setting the readout label to display the variables previously described above
                    self.R0_label.setText(
                        'r0 - ' + bvr0_v + 'V | ' + sr0_v + 'ft/min | ' + tr0_v + '°F | ' + cr0_v + 'A | ' + pr0_v + 'ft | ' + error0_v)
                except UnboundLocalError:
                    pass

        except serial.serialutil.SerialException:  # if the above items cannot be performed and an error is output then
            # update the roboclaw label accurately (if the roboclaw is disconnected from power or serial communication
            # is disconnected
            self.R0_label.setText('r0 Error - Device Serial Disconnected')
        except AttributeError:
            pass
        except NameError:
            pass
        # roboclaw 1
        try:
            if r1[0] == 1 & r1c[0] == 1:
                sr1 = roboclaw.ReadSpeedM1(0x81)
                tr1 = roboclaw.ReadTemp(0x81)
                bvr1 = roboclaw.ReadMainBatteryVoltage(0x81)
                cr1 = roboclaw.ReadCurrents(0x81)
                pr1 = roboclaw.ReadEncM1(0x81)
                error1 = roboclaw.ReadError(0x81)
                try:
                    sr1_v = str(round(.0432 * sr1[1], 1))
                    tr1_v = str(round((1.8 * (.1 * tr1[1]) + 32), 1))
                    bvr1_v = str(round(.1 * bvr1[1], 1))
                    cr1_v = str(round(.01 * cr1[1], 1))
                    pr1_v = str(round((1 / (((4 * ppr) / C) * 12)) * pr1[1], 2))

                    if error1[1] == 0:
                        error1_v = "Normal Op."
                    elif error1[1] == 1:
                        error1_v = "E-Stop"
                    elif error1[1] == 2:
                        error1_v = "Temperature Error"
                    elif error1[1] == 8:
                        error1_v = "Main Voltage High Error"
                    elif error1[1] == 16:
                        error1_v = "Logic Voltage High Error"
                    elif error1[1] == 32:
                        error1_v = "Logic Voltage Low Error"
                    elif error1[1] == 64:
                        error1_v = "Driver Fault Error"
                    elif error1[1] == 256:
                        error1_v = "Speed Error"
                    elif error1[1] == 1024:
                        error1_v = "Position Error"
                    elif error1[1] == 4096:
                        error1_v = "Current Error"
                    elif error1[1] == 65536:
                        error1_v = "Over Current Warning"
                    elif error1[1] == 262144:
                        error1_v = "Main Voltage High Warning"
                    elif error1[1] == 524288:
                        error1_v = "Main Voltage Low Warning "
                    elif error1[1] == 1048576:
                        error1_v = "Temperature Warning"
                    else:
                        error1_v = " "

                except UnboundLocalError:
                    pass

                try:
                    self.R1_label.setText(
                        'r1 - ' + bvr1_v + 'V | ' + sr1_v + 'ft/min | ' + tr1_v + '°F | ' + cr1_v + 'A | ' + pr1_v + 'ft | ' + error1_v)
                except UnboundLocalError:
                    pass

        except serial.serialutil.SerialException:
            self.R1_label.setText('r1 Error - Device Serial Disconnected')
        except AttributeError:
            pass
        except NameError:
            pass
        # roboclaw 2
        try:
            if r2[0] == 1 & r2c[0] == 1:
                sr2 = roboclaw.ReadSpeedM1(0x82)
                tr2 = roboclaw.ReadTemp(0x82)
                bvr2 = roboclaw.ReadMainBatteryVoltage(0x82)
                cr2 = roboclaw.ReadCurrents(0x82)
                pr2 = roboclaw.ReadEncM1(0x82)
                error2 = roboclaw.ReadError(0x82)
                try:
                    sr2_v = str(round(.0432 * sr2[1], 1))
                    tr2_v = str(round((1.8 * (.1 * tr2[1]) + 32), 1))
                    bvr2_v = str(round(.1 * bvr2[1], 1))
                    cr2_v = str(round(.01 * cr2[1], 1))
                    pr2_v = str(round((1 / (((4 * ppr) / C) * 12)) * pr2[1], 2))

                    if error2[1] == 0:
                        error2_v = "Normal Op."
                    elif error2[1] == 1:
                        error2_v = "E-Stop"
                    elif error2[1] == 2:
                        error2_v = "Temperature Error"
                    elif error2[1] == 8:
                        error2_v = "Main Voltage High Error"
                    elif error2[1] == 16:
                        error2_v = "Logic Voltage High Error"
                    elif error2[1] == 32:
                        error2_v = "Logic Voltage Low Error"
                    elif error2[1] == 64:
                        error2_v = "Driver Fault Error"
                    elif error2[1] == 256:
                        error2_v = "Speed Error"
                    elif error2[1] == 1024:
                        error2_v = "Position Error"
                    elif error2[1] == 4096:
                        error2_v = "Current Error"
                    elif error2[1] == 65536:
                        error2_v = "Over Current Warning"
                    elif error2[1] == 262144:
                        error2_v = "Main Voltage High Warning"
                    elif error2[1] == 524288:
                        error2_v = "Main Voltage Low Warning "
                    elif error2[1] == 1048576:
                        error2_v = "Temperature Warning"
                    else:
                        error2_v = " "

                except UnboundLocalError:
                    pass

                try:
                    self.R2_label.setText(
                        'r2 - ' + bvr2_v + 'V | ' + sr2_v + 'ft/min | ' + tr2_v + '°F | ' + cr2_v + 'A | ' + pr2_v + 'ft | ' + error2_v)
                except UnboundLocalError:
                    pass

        except serial.serialutil.SerialException:
            self.R2_label.setText('r2 Error - Device Serial Disconnected')
        except AttributeError:
            pass
        except NameError:
            pass
        # roboclaw 3
        try:
            if r3[0] == 1 & r3c[0] == 1:
                sr3 = roboclaw.ReadSpeedM1(0x83)
                tr3 = roboclaw.ReadTemp(0x83)
                bvr3 = roboclaw.ReadMainBatteryVoltage(0x83)
                cr3 = roboclaw.ReadCurrents(0x83)
                pr3 = roboclaw.ReadEncM1(0x83)
                error3 = roboclaw.ReadError(0x83)
                try:
                    sr3_v = str(round(.0432 * sr3[1], 1))
                    tr3_v = str(round((1.8 * (.1 * tr3[1]) + 32), 1))
                    bvr3_v = str(round(.1 * bvr3[1], 1))
                    cr3_v = str(round(.01 * cr3[1], 1))
                    pr3_v = str(round((1 / (((4 * ppr) / C) * 12)) * pr3[1], 2))

                    if error3[1] == 0:
                        error3_v = "Normal Op."
                    elif error3[1] == 1:
                        error3_v = "E-Stop"
                    elif error3[1] == 2:
                        error3_v = "Temperature Error"
                    elif error3[1] == 8:
                        error3_v = "Main Voltage High Error"
                    elif error3[1] == 16:
                        error3_v = "Logic Voltage High Error"
                    elif error3[1] == 32:
                        error3_v = "Logic Voltage Low Error"
                    elif error3[1] == 64:
                        error3_v = "Driver Fault Error"
                    elif error3[1] == 256:
                        error3_v = "Speed Error"
                    elif error3[1] == 1024:
                        error3_v = "Position Error"
                    elif error3[1] == 4096:
                        error3_v = "Current Error"
                    elif error3[1] == 65536:
                        error3_v = "Over Current Warning"
                    elif error3[1] == 262144:
                        error3_v = "Main Voltage High Warning"
                    elif error3[1] == 524288:
                        error3_v = "Main Voltage Low Warning "
                    elif error3[1] == 1048576:
                        error3_v = "Temperature Warning"
                    else:
                        error3_v = " "

                except UnboundLocalError:
                    pass

                try:
                    self.R3_label.setText(
                        'r3 - ' + bvr3_v + 'V | ' + sr3_v + 'ft/min | ' + tr3_v + '°F | ' + cr3_v + 'A | ' + pr3_v + 'ft | ' + error3_v)
                except UnboundLocalError:
                    pass

        except serial.serialutil.SerialException:
            self.R3_label.setText('r3 Error - Device Serial Disconnected')
        except AttributeError:
            pass
        except NameError:
            pass
        # roboclaw 4
        try:
            if r4[0] == 1 & r4c[0] == 1:
                sr4 = roboclaw.ReadSpeedM1(0x84)
                tr4 = roboclaw.ReadTemp(0x84)
                bvr4 = roboclaw.ReadMainBatteryVoltage(0x84)
                cr4 = roboclaw.ReadCurrents(0x84)
                pr4 = roboclaw.ReadEncM1(0x84)
                error4 = roboclaw.ReadError(0x84)
                try:
                    sr4_v = str(round(.0432 * sr4[1], 1))
                    tr4_v = str(round((1.8 * (.1 * tr4[1]) + 32), 1))
                    bvr4_v = str(round(.1 * bvr4[1], 1))
                    cr4_v = str(round(.01 * cr4[1], 1))
                    pr4_v = str(round((1 / (((4 * ppr) / C) * 12)) * pr4[1], 2))

                    if error4[1] == 0:
                        error4_v = "Normal Op."
                    elif error4[1] == 1:
                        error4_v = "E-Stop"
                    elif error4[1] == 2:
                        error4_v = "Temperature Error"
                    elif error4[1] == 8:
                        error4_v = "Main Voltage High Error"
                    elif error4[1] == 16:
                        error4_v = "Logic Voltage High Error"
                    elif error4[1] == 32:
                        error4_v = "Logic Voltage Low Error"
                    elif error4[1] == 64:
                        error4_v = "Driver Fault Error"
                    elif error4[1] == 256:
                        error4_v = "Speed Error"
                    elif error4[1] == 1024:
                        error4_v = "Position Error"
                    elif error4[1] == 4096:
                        error4_v = "Current Error"
                    elif error4[1] == 65536:
                        error4_v = "Over Current Warning"
                    elif error4[1] == 262144:
                        error4_v = "Main Voltage High Warning"
                    elif error4[1] == 524288:
                        error4_v = "Main Voltage Low Warning "
                    elif error4[1] == 1048576:
                        error4_v = "Temperature Warning"
                    else:
                        error4_v = " "

                except UnboundLocalError:
                    pass

                try:
                    self.R4_label.setText(
                        'r4 - ' + bvr4_v + 'V | ' + sr4_v + 'ft/min | ' + tr4_v + '°F | ' + cr4_v + 'A | ' + pr4_v + 'ft | ' + error4_v)
                except UnboundLocalError:
                    pass

        except serial.serialutil.SerialException:
            self.R4_label.setText('r4 Error - Device Serial Disconnected')
        except AttributeError:
            pass
        except NameError:
            pass
        # roboclaw 5
        try:
            if r5[0] == 1 & r5c[0] == 1:
                sr5 = roboclaw.ReadSpeedM1(0x85)
                tr5 = roboclaw.ReadTemp(0x85)
                bvr5 = roboclaw.ReadMainBatteryVoltage(0x85)
                cr5 = roboclaw.ReadCurrents(0x85)
                pr5 = roboclaw.ReadEncM1(0x85)
                error5 = roboclaw.ReadError(0x85)
                try:
                    sr5_v = str(round(.0432 * sr5[1], 1))
                    tr5_v = str(round((1.8 * (.1 * tr5[1]) + 32), 1))
                    bvr5_v = str(round(.1 * bvr5[1], 1))
                    cr5_v = str(round(.01 * cr5[1], 1))
                    pr5_v = str(round((1 / (((4 * ppr) / C) * 12)) * pr5[1], 2))

                    if error5[1] == 0:
                        error5_v = "Normal Op."
                    elif error5[1] == 1:
                        error5_v = "E-Stop"
                    elif error5[1] == 2:
                        error5_v = "Temperature Error"
                    elif error5[1] == 8:
                        error5_v = "Main Voltage High Error"
                    elif error5[1] == 16:
                        error5_v = "Logic Voltage High Error"
                    elif error5[1] == 32:
                        error5_v = "Logic Voltage Low Error"
                    elif error5[1] == 64:
                        error5_v = "Driver Fault Error"
                    elif error5[1] == 256:
                        error5_v = "Speed Error"
                    elif error5[1] == 1024:
                        error5_v = "Position Error"
                    elif error5[1] == 4096:
                        error5_v = "Current Error"
                    elif error5[1] == 65536:
                        error5_v = "Over Current Warning"
                    elif error5[1] == 262144:
                        error5_v = "Main Voltage High Warning"
                    elif error5[1] == 524288:
                        error5_v = "Main Voltage Low Warning "
                    elif error5[1] == 1048576:
                        error5_v = "Temperature Warning"
                    else:
                        error5_v = " "

                except UnboundLocalError:
                    pass

                try:
                    self.R5_label.setText(
                        'r5 - ' + bvr5_v + 'V | ' + sr5_v + 'ft/min | ' + tr5_v + '°F | ' + cr5_v + 'A | ' + pr5_v + 'ft | ' + error5_v)
                except UnboundLocalError:
                    pass

        except serial.serialutil.SerialException:
            self.R5_label.setText('r5 Error - Device Serial Disconnected')
        except AttributeError:
            pass
        except NameError:
            pass
        # roboclaw 6
        try:
            if r6[0] == 1 & r6c[0] == 1:
                sr6 = roboclaw.ReadSpeedM1(0x86)
                tr6 = roboclaw.ReadTemp(0x86)
                bvr6 = roboclaw.ReadMainBatteryVoltage(0x86)
                cr6 = roboclaw.ReadCurrents(0x86)
                pr6 = roboclaw.ReadEncM1(0x86)
                error6 = roboclaw.ReadError(0x86)
                try:
                    sr6_v = str(round(.0432 * sr6[1], 1))
                    tr6_v = str(round((1.8 * (.1 * tr6[1]) + 32), 1))
                    bvr6_v = str(round(.1 * bvr6[1], 1))
                    cr6_v = str(round(.01 * cr6[1], 1))
                    pr6_v = str(round((1 / (((4 * ppr) / C) * 12)) * pr6[1], 2))

                    if error6[1] == 0:
                        error6_v = "Normal Op."
                    elif error6[1] == 1:
                        error6_v = "E-Stop"
                    elif error6[1] == 2:
                        error6_v = "Temperature Error"
                    elif error6[1] == 8:
                        error6_v = "Main Voltage High Error"
                    elif error6[1] == 16:
                        error6_v = "Logic Voltage High Error"
                    elif error6[1] == 32:
                        error6_v = "Logic Voltage Low Error"
                    elif error6[1] == 64:
                        error6_v = "Driver Fault Error"
                    elif error6[1] == 256:
                        error6_v = "Speed Error"
                    elif error6[1] == 1024:
                        error6_v = "Position Error"
                    elif error6[1] == 4096:
                        error6_v = "Current Error"
                    elif error6[1] == 65536:
                        error6_v = "Over Current Warning"
                    elif error6[1] == 262144:
                        error6_v = "Main Voltage High Warning"
                    elif error6[1] == 524288:
                        error6_v = "Main Voltage Low Warning "
                    elif error6[1] == 1048576:
                        error6_v = "Temperature Warning"
                    else:
                        error6_v = " "

                except UnboundLocalError:
                    pass

                try:
                    self.R6_label.setText(
                        'r6 - ' + bvr6_v + 'V | ' + sr6_v + 'ft/min | ' + tr6_v + '°F | ' + cr6_v + 'A | ' + pr6_v + 'ft | ' + error6_v)
                except UnboundLocalError:
                    pass

        except serial.serialutil.SerialException:
            self.R6_label.setText('r6 Error - Device Serial Disconnected')
        except AttributeError:
            pass
        except NameError:
            pass
        # roboclaw 7
        try:
            if r7[0] == 1 & r7c[0] == 1:
                sr7 = roboclaw.ReadSpeedM1(0x87)
                tr7 = roboclaw.ReadTemp(0x87)
                bvr7 = roboclaw.ReadMainBatteryVoltage(0x87)
                cr7 = roboclaw.ReadCurrents(0x87)
                pr7 = roboclaw.ReadEncM1(0x87)
                error7 = roboclaw.ReadError(0x87)
                try:
                    sr7_v = str(round(.0432 * sr7[1], 1))
                    tr7_v = str(round((1.8 * (.1 * tr7[1]) + 32), 1))
                    bvr7_v = str(round(.1 * bvr7[1], 1))
                    cr7_v = str(round(.01 * cr7[1], 1))
                    pr7_v = str(round((1 / (((4 * ppr) / C) * 12)) * pr7[1], 2))

                    if error7[1] == 0:
                        error7_v = "Normal Op."
                    elif error7[1] == 1:
                        error7_v = "E-Stop"
                    elif error7[1] == 2:
                        error7_v = "Temperature Error"
                    elif error7[1] == 8:
                        error7_v = "Main Voltage High Error"
                    elif error7[1] == 16:
                        error7_v = "Logic Voltage High Error"
                    elif error7[1] == 32:
                        error7_v = "Logic Voltage Low Error"
                    elif error7[1] == 64:
                        error7_v = "Driver Fault Error"
                    elif error7[1] == 256:
                        error7_v = "Speed Error"
                    elif error7[1] == 1024:
                        error7_v = "Position Error"
                    elif error7[1] == 4096:
                        error7_v = "Current Error"
                    elif error7[1] == 65536:
                        error7_v = "Over Current Warning"
                    elif error7[1] == 262144:
                        error7_v = "Main Voltage High Warning"
                    elif error7[1] == 524288:
                        error7_v = "Main Voltage Low Warning "
                    elif error7[1] == 1048576:
                        error7_v = "Temperature Warning"
                    else:
                        error7_v = " "

                except UnboundLocalError:
                    pass

                try:
                    self.R7_label.setText(
                        'r7 - ' + bvr7_v + 'V | ' + sr7_v + 'ft/min | ' + tr7_v + '°F | ' + cr7_v + 'A | ' + pr7_v + 'ft | ' + error7_v)
                except UnboundLocalError:
                    pass

        except serial.serialutil.SerialException:
            self.R7_label.setText('r7 Error - Device Serial Disconnected')
        except AttributeError:
            pass
        except NameError:
            pass

    # stop button
    def button1_clicked(self):
        # setting all roboclaws that are connected speed to zero
        if r0[0] == 1:
            roboclaw.ForwardM1(0x80, 0)
            self.speed_label.setText("SET V: 00 ft/min")  # sets the target speed label
            self.dir_label.setText("DIR: N")  # sets the Direction label to N (None)
            self.b2.setStyleSheet("background-color : white")  # changes b2 color back to white
            self.b3.setStyleSheet("background-color : white")  # changes b3 color back to white
        # set all other roboclaws that are connected to have a speed of 0
        if r1[0] == 1:
            roboclaw.ForwardM1(0x81, 0)
        if r2[0] == 1:
            roboclaw.ForwardM1(0x82, 0)
        if r3[0] == 1:
            roboclaw.ForwardM1(0x83, 0)
        if r4[0] == 1:
            roboclaw.ForwardM1(0x84, 0)
        if r5[0] == 1:
            roboclaw.ForwardM1(0x85, 0)
        if r6[0] == 1:
            roboclaw.ForwardM1(0x86, 0)
        if r7[0] == 1:
            roboclaw.ForwardM1(0x87, 0)

    # forward button
    def button2_clicked(self):

        b = self.dir_label.text()  # variable to hold the current text of the direction label
        if r0[0] == 1 and b[5] != "R":  # if r0 is connected and the 6 character of the direction label is not R (Reverse)
            self.speed_label.setText("SET V: 0 ft/min")  # updating the target speed indicator
            self.b2.setStyleSheet("background-color : lightblue")  # changes b2 color to light blue to indicate its active
            self.dir_label.setText("DIR: F")  # updating the direction indicator

        if r0[0] == 1 and b[5] == "R":  # if the 6th character in the direction label is N, the run the stop button function
            # this reduces the chance for current spikes, causing the error light indicator on the roboclaw to light up
            self.button1_clicked()
            self.b3.setStyleSheet("background-color : white")  # changes b3 color to white

    # reverse button
    def button3_clicked(self):

        b = self.dir_label.text()  # variable that holds the current text string of the direction label
        if r0[0] == 1 and b[5] != "F":   # if r0 is connected and the 6th character of the direction label is not F (Forward)
            self.b3.setStyleSheet("background-color : lightblue")  # changes b3 color to light blue to indicate its active
            self.speed_label.setText("SET V: 0 ft/min")  # update the target speed indicator
            self.dir_label.setText("DIR: R")  # update the direction label

        if r0[0] == 1 and b[5] == "F":  # if r0 is connected and the 6th character in text string b is F then run the
            # stop button function
            self.button1_clicked()
            self.b2.setStyleSheet("background-color : white")  # changes b2 color to white

    def button4_clicked(self):

        b = self.dir_label.text()   # variable that holds the current text string of the direction label
        f = self.speed_label.text()  # variable that holds the current text string of the target speed label
        f_int = int(f[7:9])  # stores the number value of the target speed label
        if (f_int < 24) and not (b[5] == "N"):  # if the number value is less than 24 (ft/min) and the roboclaw has a direction
            # to go, then add 1 ft/min to the speed of the motor
            f_int = f_int + 1  # adding 1 to the f_int value
            p = 5.25 * f_int  # holds the power value of the roboclaw based off of the target speed reading
            power = int(p)  # converting into a integer value
            self.speed_label.setText("SET V: " + str(f_int) + " ft/min")  # updating the speed label
        if (f_int < 24) and (b[5] == "F"):  # only allows the counter to count up to 24 ft/min and is for when the motors
            # are going forward

            # if any amount of roboclaws are connected, then set the speed of the motors connected to each roboclaw to
            # that power value in the forward direction, irrespective of the encoder speed.
            if r0[0] == 1:
                roboclaw.ForwardM1(0x80, power)
            if r1[0] == 1:
                roboclaw.ForwardM1(0x81, power)
            if r2[0] == 1:
                roboclaw.ForwardM1(0x82, power)
            if r3[0] == 1:
                roboclaw.ForwardM1(0x83, power)
            if r4[0] == 1:
                roboclaw.ForwardM1(0x84, power)
            if r5[0] == 1:
                roboclaw.ForwardM1(0x85, power)
            if r6[0] == 1:
                roboclaw.ForwardM1(0x86, power)
            if r7[0] == 1:
                roboclaw.ForwardM1(0x87, power)
        if (f_int < 24) and (b[5] == "R"):  # if the direction is reverse than set the reverse speed of the roboclaws
            if r0[0] == 1:
                roboclaw.BackwardM1(0x80, power)
            if r1[0] == 1:
                roboclaw.BackwardM1(0x81, power)
            if r2[0] == 1:
                roboclaw.BackwardM1(0x82, power)
            if r3[0] == 1:
                roboclaw.BackwardM1(0x83, power)
            if r4[0] == 1:
                roboclaw.BackwardM1(0x84, power)
            if r5[0] == 1:
                roboclaw.BackwardM1(0x85, power)
            if r6[0] == 1:
                roboclaw.BackwardM1(0x86, power)
            if r7[0] == 1:
                roboclaw.BackwardM1(0x87, power)

    def button5_clicked(self):

        b = self.dir_label.text()  # variable that holds the current text string of the direction label
        f = self.speed_label.text()  # variable that holds the current text string of the target speed label
        f_int = int(f[7:9])  # stores the number value of the target speed label
        if (f_int > 0) and not (b[5] == "N"):  # if number value of the target speed is more than 0 and the direction is
            # not NONE then subtract from f_int
            f_int = f_int - 1
            p = 5.25 * f_int  # scales and hold the power value to get to that target speed
            power = int(p)  # changes the power value to an integer
            self.speed_label.setText("SET V: " + str(f_int) + " ft/min")  # changing the target speed label

        if (f_int > 0) and (b[5] == "F"):  # counter cannot be less than zero and if the direction label is F (Forward)
            if r0[0] == 1:
                roboclaw.ForwardM1(0x80, power)
            if r1[0] == 1:
                roboclaw.ForwardM1(0x81, power)
            if r2[0] == 1:
                roboclaw.ForwardM1(0x82, power)
            if r3[0] == 1:
                roboclaw.ForwardM1(0x83, power)
            if r4[0] == 1:
                roboclaw.ForwardM1(0x84, power)
            if r5[0] == 1:
                roboclaw.ForwardM1(0x85, power)
            if r6[0] == 1:
                roboclaw.ForwardM1(0x86, power)
            if r7[0] == 1:
                roboclaw.ForwardM1(0x87, power)
        if (f_int > 0) and (b[5] == "R"):  # if the direction is R (Reverse)
            if r0[0] == 1:
                roboclaw.BackwardM1(0x80, power)
            if r1[0] == 1:
                roboclaw.BackwardM1(0x81, power)
            if r2[0] == 1:
                roboclaw.BackwardM1(0x82, power)
            if r3[0] == 1:
                roboclaw.BackwardM1(0x83, power)
            if r4[0] == 1:
                roboclaw.BackwardM1(0x84, power)
            if r5[0] == 1:
                roboclaw.BackwardM1(0x85, power)
            if r6[0] == 1:
                roboclaw.BackwardM1(0x86, power)
            if r7[0] == 1:
                roboclaw.BackwardM1(0x87, power)

    def button6_clicked(self):

        b = self.dir_label.text()  # holds the text string of the direction label
        if not (b[5] == "N"):  # if the 6th character in b is not N then update the speed label 
            self.speed_label.setText("SET V: 24 ft/min")
        if b[5] == "F":  # if the 6th character in b is F (Forward), then set all roboclaws connected speed value to 550,
            # respective of the encoder speed
            if r0[0] == 1:
                roboclaw.SpeedM1(0x80, 550)
            if r1[0] == 1:
                roboclaw.SpeedM1(0x81, 550)
            if r2[0] == 1:
                roboclaw.SpeedM1(0x82, 550)
            if r3[0] == 1:
                roboclaw.SpeedM1(0x83, 550)
            if r4[0] == 1:
                roboclaw.SpeedM1(0x84, 550)
            if r5[0] == 1:
                roboclaw.SpeedM1(0x85, 550)
            if r6[0] == 1:
                roboclaw.SpeedM1(0x86, 550)
            if r7[0] == 1:
                roboclaw.SpeedM1(0x87, 550)
        if b[5] == "R":  # if R is the 6th character in B then set all roboclaws connected to a speed value of -550
            if r0[0] == 1:
                roboclaw.SpeedM1(0x80, -550)
            if r1[0] == 1:
                roboclaw.SpeedM1(0x81, -550)
            if r2[0] == 1:
                roboclaw.SpeedM1(0x82, -550)
            if r3[0] == 1:
                roboclaw.SpeedM1(0x83, -550)
            if r4[0] == 1:
                roboclaw.SpeedM1(0x84, -550)
            if r5[0] == 1:
                roboclaw.SpeedM1(0x85, -550)
            if r6[0] == 1:
                roboclaw.SpeedM1(0x86, -550)
            if r7[0] == 1:
                roboclaw.SpeedM1(0x87, -550)

    def button7_clicked(self):

        b = self.dir_label.text()  # holds the text string of the direction label
        if not (b[5] == "N"):  # if the 6th character in b is not N then update the speed label
            self.speed_label.setText("SET V: 20 ft/min")
        if b[5] == "F":  # if the 6th character in b is F (Forward), then set all roboclaws connected speed value to 462,
            # respective of the encoder speed
            if r0[0] == 1:
                roboclaw.SpeedM1(0x80, 462)
            if r1[0] == 1:
                roboclaw.SpeedM1(0x81, 462)
            if r2[0] == 1:
                roboclaw.SpeedM1(0x82, 462)
            if r3[0] == 1:
                roboclaw.SpeedM1(0x83, 462)
            if r4[0] == 1:
                roboclaw.SpeedM1(0x84, 462)
            if r5[0] == 1:
                roboclaw.SpeedM1(0x85, 462)
            if r6[0] == 1:
                roboclaw.SpeedM1(0x86, 462)
            if r7[0] == 1:
                roboclaw.SpeedM1(0x87, 462)
        if b[5] == "R":  # if R is the 6th character in B then set all roboclaws connected to a speed value of -462
            if r0[0] == 1:
                roboclaw.SpeedM1(0x80, -462)
            if r1[0] == 1:
                roboclaw.SpeedM1(0x81, -462)
            if r2[0] == 1:
                roboclaw.SpeedM1(0x82, -462)
            if r3[0] == 1:
                roboclaw.SpeedM1(0x83, -462)
            if r4[0] == 1:
                roboclaw.SpeedM1(0x84, -462)
            if r5[0] == 1:
                roboclaw.SpeedM1(0x85, -462)
            if r6[0] == 1:
                roboclaw.SpeedM1(0x86, -462)
            if r7[0] == 1:
                roboclaw.SpeedM1(0x87, -462)

    def button8_clicked(self):

        b = self.dir_label.text()  # holds the text string of the direction label
        if not (b[5] == "N"):  # if the 6th character in b is not N then update the speed label
            self.speed_label.setText("SET V: 16 ft/min")
        if b[5] == "F":  # if the 6th character in b is F (Forward), then set all roboclaws connected speed value to 370,
            # respective of the encoder speed
            if r0[0] == 1:
                roboclaw.SpeedM1(0x80, 370)
            if r1[0] == 1:
                roboclaw.SpeedM1(0x81, 370)
            if r2[0] == 1:
                roboclaw.SpeedM1(0x82, 370)
            if r3[0] == 1:
                roboclaw.SpeedM1(0x83, 370)
            if r4[0] == 1:
                roboclaw.SpeedM1(0x84, 370)
            if r5[0] == 1:
                roboclaw.SpeedM1(0x85, 370)
            if r6[0] == 1:
                roboclaw.SpeedM1(0x86, 370)
            if r7[0] == 1:
                roboclaw.SpeedM1(0x87, 370)
        if b[5] == "R":  # if R is the 6th character in B then set all roboclaws connected to a speed value of -370
            if r0[0] == 1:
                roboclaw.SpeedM1(0x80, -370)
            if r1[0] == 1:
                roboclaw.SpeedM1(0x81, -370)
            if r2[0] == 1:
                roboclaw.SpeedM1(0x82, -370)
            if r3[0] == 1:
                roboclaw.SpeedM1(0x83, -370)
            if r4[0] == 1:
                roboclaw.SpeedM1(0x84, -370)
            if r5[0] == 1:
                roboclaw.SpeedM1(0x85, -370)
            if r6[0] == 1:
                roboclaw.SpeedM1(0x86, -370)
            if r7[0] == 1:
                roboclaw.SpeedM1(0x87, -370)

    def button9_clicked(self):

        b = self.dir_label.text()  # holds the text string of the direction label
        if not (b[5] == "N"):  # if the 6th character in b is not N then update the speed label
            self.speed_label.setText("SET V: 12 ft/min")

        if b[5] == "F":  # if the 6th character in b is F (Forward), then set all roboclaws connected speed value to 277,
            # respective of the encoder speed
            if r0[0] == 1:
                roboclaw.SpeedM1(0x80, 277)
            if r1[0] == 1:
                roboclaw.SpeedM1(0x81, 277)
            if r2[0] == 1:
                roboclaw.SpeedM1(0x82, 277)
            if r3[0] == 1:
                roboclaw.SpeedM1(0x83, 277)
            if r4[0] == 1:
                roboclaw.SpeedM1(0x84, 277)
            if r5[0] == 1:
                roboclaw.SpeedM1(0x85, 277)
            if r6[0] == 1:
                roboclaw.SpeedM1(0x86, 277)
            if r7[0] == 1:
                roboclaw.SpeedM1(0x87, 277)
        if b[5] == "R":  # if R is the 6th character in B then set all roboclaws connected to a speed value of -277
            if r0[0] == 1:
                roboclaw.SpeedM1(0x80, -277)
            if r1[0] == 1:
                roboclaw.SpeedM1(0x81, -277)
            if r2[0] == 1:
                roboclaw.SpeedM1(0x82, -277)
            if r3[0] == 1:
                roboclaw.SpeedM1(0x83, -277)
            if r4[0] == 1:
                roboclaw.SpeedM1(0x84, -277)
            if r5[0] == 1:
                roboclaw.SpeedM1(0x85, -277)
            if r6[0] == 1:
                roboclaw.SpeedM1(0x86, -277)
            if r7[0] == 1:
                roboclaw.SpeedM1(0x87, -277)

    def button10_clicked(self):

        b = self.dir_label.text()  # holds the text string of the direction label
        if not (b[5] == "N"):  # if the 6th character in b is not N then update the speed label
            self.speed_label.setText("SET V: 8 ft/min")

        if b[5] == "F":  # if the 6th character in b is F (Forward), then set all roboclaws connected speed value to 185,
            # respective of the encoder speed
            if r0[0] == 1:
                roboclaw.SpeedM1(0x80, 185)
            if r1[0] == 1:
                roboclaw.SpeedM1(0x81, 185)
            if r2[0] == 1:
                roboclaw.SpeedM1(0x82, 185)
            if r3[0] == 1:
                roboclaw.SpeedM1(0x83, 185)
            if r4[0] == 1:
                roboclaw.SpeedM1(0x84, 185)
            if r5[0] == 1:
                roboclaw.SpeedM1(0x85, 185)
            if r6[0] == 1:
                roboclaw.SpeedM1(0x86, 185)
            if r7[0] == 1:
                roboclaw.SpeedM1(0x87, 185)

        if b[5] == "R":  # if R is the 6th character in B then set all roboclaws connected to a speed value of -185
            if r0[0] == 1:
                roboclaw.SpeedM1(0x80, -185)
            if r1[0] == 1:
                roboclaw.SpeedM1(0x81, -185)
            if r2[0] == 1:
                roboclaw.SpeedM1(0x82, -185)
            if r3[0] == 1:
                roboclaw.SpeedM1(0x83, -185)
            if r4[0] == 1:
                roboclaw.SpeedM1(0x84, -185)
            if r5[0] == 1:
                roboclaw.SpeedM1(0x85, -185)
            if r6[0] == 1:
                roboclaw.SpeedM1(0x86, -185)
            if r7[0] == 1:
                roboclaw.SpeedM1(0x87, -185)

    def button11_clicked(self):

        b = self.dir_label.text()  # holds the text string of the direction label
        if not (b[5] == "N"):  # if the 6th character in b is not N then update the speed label
            self.speed_label.setText("SET V: 4 ft/min")

        if b[5] == "F":  # if the 6th character in b is F (Forward), then set all roboclaws connected speed value to 90,
            # respective of the encoder speed
            if r0[0] == 1:
                roboclaw.SpeedM1(0x80, 90)
            if r1[0] == 1:
                roboclaw.SpeedM1(0x81, 90)
            if r2[0] == 1:
                roboclaw.SpeedM1(0x82, 90)
            if r3[0] == 1:
                roboclaw.SpeedM1(0x83, 90)
            if r4[0] == 1:
                roboclaw.SpeedM1(0x84, 90)
            if r5[0] == 1:
                roboclaw.SpeedM1(0x85, 90)
            if r6[0] == 1:
                roboclaw.SpeedM1(0x86, 90)
            if r7[0] == 1:
                roboclaw.SpeedM1(0x87, 90)
        if b[5] == "R":  # if R is the 6th character in B then set all roboclaws connected to a speed value of -90
            if r0[0] == 1:
                roboclaw.SpeedM1(0x80, -90)
            if r1[0] == 1:
                roboclaw.SpeedM1(0x81, -90)
            if r2[0] == 1:
                roboclaw.SpeedM1(0x82, -90)
            if r3[0] == 1:
                roboclaw.SpeedM1(0x83, -90)
            if r4[0] == 1:
                roboclaw.SpeedM1(0x84, -90)
            if r5[0] == 1:
                roboclaw.SpeedM1(0x85, -90)
            if r6[0] == 1:
                roboclaw.SpeedM1(0x86, -90)
            if r7[0] == 1:
                roboclaw.SpeedM1(0x87, -90)

    def button12_clicked(self):  # Encoder reset
        try:
            self.encoder_reset_popup()
        except serial.serialutil.SerialException:
            pass
        except NameError:
            pass

    def encoder_reset_popup(self):
        enc_popup.exec_()  # executes the enc_popup window

    def erp_button_clicked(self, i):  # method used to indicate what button was clicked on the enc_popup
        choice = i.text()  # reads what was selected on the encoder reset pop-up box, Yes or No
        if choice == '&Yes':  # if it reads &Yes then reset the encoder count
            if r0[0] == 1:
                roboclaw.ResetEncoders(
                    0x80)  # resetting the encoder count for roboclaw zero if the roboclaw is connected
            if r1[0] == 1:
                roboclaw.ResetEncoders(0x81)
            if r2[0] == 1:
                roboclaw.ResetEncoders(0x82)
            if r3[0] == 1:
                roboclaw.ResetEncoders(0x83)
            if r4[0] == 1:
                roboclaw.ResetEncoders(0x84)
            if r5[0] == 1:
                roboclaw.ResetEncoders(0x85)
            if r6[0] == 1:
                roboclaw.ResetEncoders(0x86)
            if r7[0] == 1:
                roboclaw.ResetEncoders(0x87)
        else:
            pass

if __name__ == '__main__':
    app = QApplication(sys.argv)
    win = MyWindow()
    win.show()
    sys.exit(app.exec_())

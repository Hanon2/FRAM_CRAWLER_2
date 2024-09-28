import RPi.GPIO as GPIO
import time
from enum import Enum
# GPIO pin configuration
STEP_PIN = 17
DIR_PIN = 27
ENABLE_PIN = 22  # Optionally use this for enabling/disabling
DELAY_BETWEEN_STEPS = 0.001
class motorDirections(Enum):
    forward = GPIO.HIGH
    reverse = GPIO.LOW
    noOperation = "stop the motor"

motorDirection = motorDirections.noOperation

def setMotorDir(x):
    global motorDirection
    if x == "forward":
        motorDirection = motorDirections.forward
    elif x == "reverse":
        motorDirection = motorDirections.reverse
    else:
        motorDirection = motorDirections.noOperation

def initGPIOForMotors():

    # Setup GPIO
    GPIO.setmode(GPIO.BCM)
    GPIO.setup(STEP_PIN, GPIO.OUT)
    GPIO.setup(DIR_PIN, GPIO.OUT)
    GPIO.setup(ENABLE_PIN, GPIO.OUT)
    # Set enable pin to low to enable the driver
    GPIO.output(ENABLE_PIN, GPIO.LOW)


def runStepperLogic(angle):
    global motorDirection
    if motorDirection != motorDirections.noOperation:
        GPIO.output(DIR_PIN, motorDirection)  # Set the direction
        #Declare static variables that will retain their values each time the function gets called

        if not hasattr(runStepperLogic, "totalStepsToBeTravelled",):
            runStepperLogic.totalStepsToBeTravelled = 0  # Initialize the static variable
        if not hasattr(runStepperLogic, "stepsTravelled",):
            runStepperLogic.stepsTravelled = 0  # Initialize the static variable

        runStepperLogic.totalStepsToBeTravelled = angle[0]

        if runStepperLogic.stepsTravelled < runStepperLogic.totalStepsToBeTravelled:

            runStepperLogic.stepsTravelled+=1
            GPIO.output(STEP_PIN, GPIO.HIGH)
            time.sleep(DELAY_BETWEEN_STEPS)
            GPIO.output(STEP_PIN, GPIO.LOW)
            time.sleep(DELAY_BETWEEN_STEPS)
            return False
        else:
            try:
                # Keep popping angles from the angles array, if we can't do it anymore then we finished everything
                angle.pop(0)
            except Exception:
                global motorDirection
                motorDirection = motorDirections.noOperation
            return True


import RPi.GPIO as GPIO
import time

# Setup
GPIO.setmode(GPIO.BCM)  # Use BCM pin numbering
servo_pin = 18          # GPIO pin for the servo
GPIO.setup(servo_pin, GPIO.OUT)

# Create a PWM instance
pwm = GPIO.PWM(servo_pin, 50)  # 50Hz frequency
pwm.start(0)  # Initialize with a duty cycle of 0
MAX_ANGLE = 45

shouldWeRunServo = False
def runServoLogic():
    if not hasattr(runServoLogic, "anglesTravelled", ):
        runServoLogic.anglesTravelled = 0  # Initialize the static variable
    runServoLogic.anglesTravelled+=1
    if runServoLogic.anglesTravelled >= MAX_ANGLE:
        return False
    if shouldWeRunServo:
        set_angle(runServoLogic.anglesTravelled)
        pwm.stop()

def setShouldWeRunServo(x):
    global shouldWeRunServo
    shouldWeRunServo = x
def set_angle(angle):
    duty_cycle = (angle / 18) + 2  # Convert angle to duty cycle
    pwm.ChangeDutyCycle(duty_cycle)  # Set the duty cycle
    time.sleep(1)  # Allow time for the servo to move




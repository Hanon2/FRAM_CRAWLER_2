import RPi.GPIO as GPIO
import time

# Setup
GPIO.setmode(GPIO.BCM)  # Use BCM pin numbering
servo_pin = 18          # GPIO pin for the servo
GPIO.setup(servo_pin, GPIO.OUT)

# Create a PWM instance
pwm = GPIO.PWM(servo_pin, 50)  # 50Hz frequency
pwm.start(0)  # Initialize with a duty cycle of 0
def runServoLogic(angle):
    for i in range(angle):
        set_angle(angle[i])
    pwm.stop()         # Stop PWM
def set_angle(angle):
    duty_cycle = (angle / 18) + 2  # Convert angle to duty cycle
    pwm.ChangeDutyCycle(duty_cycle)  # Set the duty cycle
    time.sleep(1)  # Allow time for the servo to move

    while True:
        set_angle(0)    # Rotate to 0 degrees
        time.sleep(2)   # Wait for 2 seconds
        set_angle(90)   # Rotate to 90 degrees
        time.sleep(2)   # Wait for 2 seconds
        set_angle(180)  # Rotate to 180 degrees
        time.sleep(2)   # Wait for 2 seconds



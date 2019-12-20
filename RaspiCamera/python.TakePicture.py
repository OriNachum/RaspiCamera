from picamera import PiCamera
from time import sleep

camera.start_preview()
sleep(5)
camera.capture('/home/pi/PythonPicture.jpg')
camera.stop_preview()

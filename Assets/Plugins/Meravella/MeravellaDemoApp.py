# (c) Meta Platforms, Inc. and affiliates. Confidential and proprietary.

import ctypes
import sys

import time
from ctypes import CDLL

from frame import clearFrame, darkFrame

# This test only works on Windows
# Note: This is just an example of how you can send a frame. The code is not optimized/multithreaded.


# Send a frame to the Meravella Dimming Driver
def SendFrame(meravellaDimmingDriver: object, frame: object):
    for i in range(0, 40):
        pointerToData = frame[i].ctypes.data_as(ctypes.POINTER(ctypes.c_uint8))
        meravellaDimmingDriver.MV_SendDataLeft(pointerToData, 48)
        meravellaDimmingDriver.MV_SendDataRight(pointerToData, 48)


print("Meravella Demo App")

args = sys.argv[1:]
numArgs = len(args)

if numArgs != 1:
    print("Usage: python MeravellaDemoApp.py <path_to_dll>")
    exit()

print("Loading DLL from path: ", args[0])

meravellaDimmingDriver = object()  # CDLL object

# Try loading the dll
try:
    meravellaDimmingDriver = CDLL(args[0])
    print("DLL Successfully loaded ", meravellaDimmingDriver)
except Exception as e:
    print(e)
    exit()

# TODO(sangarg): The driverType should be exported as a python variable (equivalent to a C++ enum)
# Add this when we create a SDK script
meravellaDimmingDriver.MV_CreateDimmingDrivers(0)

# Send alternating dark and clear frames
for _ in range(0, 3):
    print("Sent Dark Frame")
    SendFrame(meravellaDimmingDriver, darkFrame)
    time.sleep(1)

    print("Sent Clear Frame")
    SendFrame(meravellaDimmingDriver, clearFrame)
    time.sleep(1)

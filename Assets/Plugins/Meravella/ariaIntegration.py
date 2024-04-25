# (c) Meta Platforms, Inc. and affiliates. Confidential and proprietary.

import sys
import time
from ctypes import c_char, c_float, cast, CDLL, POINTER

import numpy

print("Meravella Aria integration demo App")

args = sys.argv[1:]
numArgs = len(args)

if numArgs != 1:
    print("Usage: python MeravellaDemoApp.py <path_to_dll>")
    exit()

print("Loading DLL from path: ", args[0])

aria_integration = object()  # CDLL object

# Try loading the dll
try:
    aria_integration = CDLL(args[0])
    print("DLL Successfully loaded ", aria_integration)
except Exception as e:
    print(e)
    exit()

aria_integration.MV_Aria_Initialize()
# aria_integration.MV_Aria_Authenticate()
# print(
#     "Will wait for 3 seconds for you to approve the authentication request on the Aria app..."
# )
# time.sleep(3)

print("Connect and start streaming")
aria_integration.MV_Aria_Connect()
time.sleep(1)
aria_integration.MV_Aria_StartStreaming()

# headPose = numpy.zeros(7, dtype=numpy.float32)
# headpose_float_ptr = headPose.ctypes.data_as(POINTER(c_float))

# for _ in range(0, 5):
#     aria_integration.MV_Aria_GetHeadPose(headpose_float_ptr, headPose.size)
#     print("Head pose ", headPose)
#     time.sleep(1)


class AriaImageMetadata:
    def __init__(self):
        self.frameNumber = 0
        self.imageBufferSize = 0
        self.imageHeight = 0
        self.imageWidth = 0
        self.imageFormat = 0


data = bytearray(400000)  # Note that we use bytearray instead of byte here
ariaImage = AriaImageMetadata()
ariaImage_address = id(ariaImage)
ariaImagePtr = cast(ariaImage_address, POINTER(c_char))

for _ in range(0, 5):
    aria_integration.MV_Aria_GetRgbImage(ariaImagePtr, data)
    print(
        f"ImageInfo: frameNumber:{ariaImage.frameNumber}, imageBufferSize:{ariaImage.imageBufferSize}, imageHeight:{ariaImage.imageHeight}, imageWidth:{ariaImage.imageWidth}, imageFormat:{ariaImage.imageFormat}, data size:{len(data)}"
    )
    time.sleep(1)

print("Will stop streaming and disconnect")
aria_integration.MV_Aria_StopStreaming()
aria_integration.MV_Aria_Disconnect()
aria_integration.MV_Aria_Shutdown()

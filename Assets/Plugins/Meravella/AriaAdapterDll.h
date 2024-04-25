// (c) Meta Platforms, Inc. and affiliates. Confidential and proprietary.

#pragma once

// TODO(sangarg): Figure out the correct exports since for aria, we are using the clang compiler
// even on windows
#ifndef MERAVELLA_EXPORT
#if defined(__GNUC__)
#define MERAVELLA_EXPORT __attribute__((visibility("default")))
#elif defined(_MSC_VER) || defined(__clang__)
#define MERAVELLA_EXPORT __declspec(dllexport)
#else
#error Unsupported compiler
#endif
#endif

extern "C" {

// TODO(sangarg): Cleanup the enums between aria integration dll and driver dll
/** \brief Error codes that can be returned by RLRAudio functions. */
typedef enum {
  MERAVELLA_Success = 0,
  MERAVELLA_Error_Exception = 1,
  MERAVELLA_Error_Unknown = 2, // An unknown error has occurred.
  MERAVELLA_Error_InvalidParameter = 3, // An invalid parameter was passed to a function.
  MERAVELLA_Error_FeatureDisabled = 4,
  MERAVELLA_Error_FileNotFound = 5,

  MERAVELLA_Error_AriaOperationFailed = 101,
  MERAVELLA_Error_AriaNotInitialized = 102,
  MERAVELLA_Error_AriaNotAuthenticated = 103,
  MERAVELLA_Error_AriaNotConnected = 104,
  MERAVELLA_Error_AriaNoDataAvailable = 105,
} MERAVELLA_Error;

/**
 * Initialize the aria initialize.
 * authentication.
 * @param
 * @return MERAVELLA_Success if successful
 * @return MERAVELLA_Error_FileNotFound if the config file is missing
 * @return MERAVELLA_Error_AriaOperationFailed if initialization failed which could be related to
 *          calibration capture vrs files being missing
 */
MERAVELLA_EXPORT MERAVELLA_Error MV_Aria_Initialize(const char* folderPath);

/**
 * Get the rgb image buffer size which is set in the json config
 * @param
 * @return MERAVELLA_Success if successful, rgbImageBufferSize will be set to the required size. The
 *          image buffer sent to the MV_Aria_GetRgbImage function should be of this size.
 * @return MERAVELLA_Error_AriaNotInitialized if MV_Aria_Initialize was not called
 * @return MERAVELLA_Error_InvalidParameter if the rgbImageBufferSize is an invalid ptr
 */
MERAVELLA_EXPORT MERAVELLA_Error MV_Aria_GetRgbImageBufferSize(uint32_t* rgbImageBufferSize);

/**
 * Cleanup all memory related to aria integration
 * @param
 * @return MERAVELLA_Success if successful
 * @return MERAVELLA_Error_AriaOperationFailed if authentication failed
 */
MERAVELLA_EXPORT void MV_Aria_Shutdown();

/**
 * Authenticate Aria. This will create a notification in the aria app to approve the device
 * authentication.
 * @param
 * @return MERAVELLA_Success if successful
 * @return MERAVELLA_Error_AriaOperationFailed if authentication failed
 */
MERAVELLA_EXPORT MERAVELLA_Error MV_Aria_Authenticate();

/**
 * Connect to aria glasses. The glasses should have already been authenticated
 * @param
 * @return MERAVELLA_Success if successful
 * @return MERAVELLA_Error_AriaNotInitialized if Authenticate was not called
 * @return MERAVELLA_Error_AriaOperationFailed if connection failed
 */
MERAVELLA_EXPORT MERAVELLA_Error MV_Aria_Connect();

/**
 * Start streaming data from the aria glasses. The glasses should have already been authenticated
 * and connected.
 * @param
 * @return MERAVELLA_Success if successful
 * @return MERAVELLA_Error_AriaNotInitialized if Authenticate was not called
 * @return MERAVELLA_Error_AriaNotConnected if Connect was not called
 * @return MERAVELLA_Error_AriaOperationFailed if connection failed
 */
MERAVELLA_EXPORT MERAVELLA_Error MV_Aria_StartStreaming();

/**
 * Convenience api to connect to aria glasses and start streaming
 * @param
 * @return MERAVELLA_Success if successful
 * @return MERAVELLA_Error_AriaNotInitialized if Authenticate was not called
 * @return MERAVELLA_Error_AriaOperationFailed if connection or startStreaming failes
 */
MERAVELLA_EXPORT MERAVELLA_Error MV_Aria_ConnectAndStartStreaming();

/**
 * Check and return if aria glasses are connected or not
 * @param
 * @return true if it is connected
 * @return false if it is not connected
 */
MERAVELLA_EXPORT bool MV_Aria_IsConnected();

/**
 * Check and return if aria glasses are streaming or not
 * @param
 * @return true if it is streaming
 * @return false if it is not streaming
 */
MERAVELLA_EXPORT bool MV_Aria_IsStreaming();

/**
 * Stop streaming data from aria glasses
 * @param
 * @return MERAVELLA_Success if successful
 * @return MERAVELLA_Error_AriaNotInitialized if Authenticate was not called
 * @return MERAVELLA_Error_AriaNotConnected if Connect was not called
 * @return MERAVELLA_Error_AriaOperationFailed if stopStreaming call failed
 */
MERAVELLA_EXPORT MERAVELLA_Error MV_Aria_StopStreaming();

/**
 * Disconnect aria glasses
 * @param
 * @return MERAVELLA_Success if successful
 * @return MERAVELLA_Error_AriaNotInitialized if Authenticate was not called
 * @return MERAVELLA_Error_AriaOperationFailed if disconnect called failed
 */
MERAVELLA_EXPORT MERAVELLA_Error MV_Aria_Disconnect();

/**
 * Get the latest head pose from the aria glasses and fill the input buffer. First 3 floats will
 be
 * for head translation, next 4 for head quaternion.
 * @param headPose [out] Head pose ptr. Make sure the array has at least
 * 7 float or memory allocated. First 3 floats are for translation and last 4 floats are for the
 * quaternion.
 * @param headPoseArraySize Input head pose array size. This SHOULD be 7.
 * @return MERAVELLA_Success if successful
 * @return MERAVELLA_Error_AriaNotInitialized if Authenticate was not called
 * @return MERAVELLA_Error_AriaNoDataAvailable if no data is available yet
 * @return MERAVELLA_Error_InvalidParameter if the headPose input is an invalid ptr or if the
 * size is not 7
 * @return MERAVELLA_Error_FeatureDisabled if the feature is disabled from the config json
 */
MERAVELLA_EXPORT MERAVELLA_Error MV_Aria_GetHeadPose(float* headPose, size_t headPoseArraySize);

MERAVELLA_EXPORT MERAVELLA_Error MV_Aria_GetRgbImage(uint8_t* ariaImageMetadata, uint8_t* data);
} // extern "C"

// (c) Meta Platforms, Inc. and affiliates. Confidential and proprietary.

#pragma once

#include <cstdint>

#ifndef MERAVELLA_EXPORT
#if defined(__GNUC__) || defined(__clang__)
#define MERAVELLA_EXPORT __attribute__((visibility("default")))
#elif defined(_MSC_VER)
#define MERAVELLA_EXPORT __declspec(dllexport)
#else
#error Unsupported compiler
#endif
#endif

extern "C" {

/** \brief Error codes that can be returned by RLRAudio functions. */
typedef enum {
  MERAVELLA_Success = 0,
  MERAVELLA_Error_Unknown = 1, // An unknown error has occurred.
  MERAVELLA_Error_InvalidParameter = 2, // An invalid parameter was passed to a function.
} MERAVELLA_Error;

typedef enum {
  MERAVELLA_DriverType_Usb = 0,
#ifdef MERAVELLA_SIMULATOR_SUPPORTED
  MERAVELLA_DriverType_Simulator = 1,
#endif // MERAVELLA_SIMULATOR_SUPPORTED
  MERAVELLA_DriverType_Count,
} MERAVELLA_DriverType;

/**
 * Create dimming drivers.
 * @param
 * @return MERAVELLA_Success if successful, otherwise an error code
 */
MERAVELLA_EXPORT MERAVELLA_Error MV_CreateDimmingDrivers(MERAVELLA_DriverType driverType);

/**
 * Send data to the left dimming panel
 * @param data uint8_t pointer to the data to send
 * @param length The number of bytes to send
 * @return The number of bytes sent or -1 on failure
 */
MERAVELLA_EXPORT uint32_t MV_SendDataLeft(const uint8_t* data, size_t length);

/**
 * Send data to the right dimming panel
 * @param data uint8_t pointer to the data to send
 * @param length The number of bytes to send
 * @return The number of bytes sent or -1 on failure
 */
MERAVELLA_EXPORT uint32_t MV_SendDataRight(const uint8_t* data, size_t length);
}

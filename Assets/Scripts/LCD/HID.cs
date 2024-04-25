#region Assembly LQ001_Class, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Users\jare0530\Projects\080723_MinimalDisplay\Mozaic_GUI_V12_source_code\Mozaic_GUI_V12\LQ001_Class.dll
// Decompiled with ICSharpCode.Decompiler 7.1.0.6543
#endregion

using System;
using System.Text;

namespace SLAB_HID_DEVICE
{
    public class Hid
    {
        protected IntPtr m_hid;

        public const byte HID_DEVICE_SUCCESS = 0;

        public const byte HID_DEVICE_NOT_FOUND = 1;

        public const byte HID_DEVICE_NOT_OPENED = 2;

        public const byte HID_DEVICE_ALREADY_OPENED = 3;

        public const byte HID_DEVICE_TRANSFER_TIMEOUT = 4;

        public const byte HID_DEVICE_TRANSFER_FAILED = 5;

        public const byte HID_DEVICE_CANNOT_GET_HID_INFO = 6;

        public const byte HID_DEVICE_HANDLE_ERROR = 7;

        public const byte HID_DEVICE_INVALID_BUFFER_SIZE = 8;

        public const byte HID_DEVICE_SYSTEM_CODE = 9;

        public const byte HID_DEVICE_UNSUPPORTED_FUNCTION = 10;

        public const byte HID_DEVICE_UNKNOWN_ERROR = byte.MaxValue;

        public const byte MAX_USB_DEVICES = 64;

        public const uint MAX_REPORT_REQUEST_XP = 512u;

        public const uint MAX_REPORT_REQUEST_2K = 200u;

        public const uint DEFAULT_REPORT_INPUT_BUFFERS = 0u;

        public const byte HID_VID_STRING = 1;

        public const byte HID_PID_STRING = 2;

        public const byte HID_PATH_STRING = 3;

        public const byte HID_SERIAL_STRING = 4;

        public const byte HID_MANUFACTURER_STRING = 5;

        public const byte HID_PRODUCT_STRING = 6;

        public const uint MAX_VID_LENGTH = 5u;

        public const uint MAX_PID_LENGTH = 5u;

        public const uint MAX_PATH_LENGTH = 260u;

        public const uint MAX_SERIAL_STRING_LENGTH = 256u;

        public const uint MAX_MANUFACTURER_STRING_LENGTH = 256u;

        public const uint MAX_PRODUCT_STRING_LENGTH = 256u;

        public const uint MAX_INDEXED_STRING_LENGTH = 256u;

        public const uint MAX_STRING_LENGTH = 260u;

        ~Hid()
        {
        }

        public static uint GetNumHidDevices(ushort vid, ushort pid)
        {
            return SLABHIDDevice_DLL.HidDevice_GetNumHidDevices(vid, pid);
        }

        public static byte GetHidString(uint deviceIndex, ushort vid, ushort pid, byte hidStringType, StringBuilder deviceString, uint deviceStringLength)
        {
            return SLABHIDDevice_DLL.HidDevice_GetHidString(deviceIndex, vid, pid, hidStringType, deviceString, deviceStringLength);
        }

        public static byte GetHidIndexedString(uint deviceIndex, ushort vid, ushort pid, uint stringIndex, StringBuilder deviceString, uint deviceStringLength)
        {
            return SLABHIDDevice_DLL.HidDevice_GetHidIndexedString(deviceIndex, vid, pid, stringIndex, deviceString, deviceStringLength);
        }

        public static byte GetHidAttributes(uint deviceIndex, ushort vid, ushort pid, ref ushort deviceVid, ref ushort devicePid, ref ushort deviceReleaseNumber)
        {
            return SLABHIDDevice_DLL.HidDevice_GetHidAttributes(deviceIndex, vid, pid, ref deviceVid, ref devicePid, ref deviceReleaseNumber);
        }

        public static void GetHidGuid(ref Guid hidGuid)
        {
            SLABHIDDevice_DLL.HidDevice_GetHidGuid(ref hidGuid);
        }

        public static byte GetHidLibraryVersion(ref byte major, ref byte minor, ref int release)
        {
            return SLABHIDDevice_DLL.HidDevice_GetHidLibraryVersion(ref major, ref minor, ref release);
        }

        public static bool GetDeviceIndex(ushort vid, ushort pid, string serial, ref uint deviceIndex)
        {
            uint num = 0u;
            bool result = false;
            for (uint num2 = 0u; num2 < GetNumHidDevices(vid, pid); num2++)
            {
                StringBuilder stringBuilder = new StringBuilder(256);
                if (GetHidString(num2, vid, pid, 4, stringBuilder, 256u) == 0 && serial == stringBuilder.ToString())
                {
                    num = num2;
                    result = true;
                    break;
                }
            }

            deviceIndex = num;
            return result;
        }

        public bool Connect(ushort vid, ushort pid, string serial, uint getReportTimeout, uint setReportTimeout)
        {
            bool result = false;
            uint deviceIndex = 0u;
            bool flag = false;
            if (GetDeviceIndex(vid, pid, serial, ref deviceIndex) && Open(deviceIndex, vid, pid, 512u) == 0)
            {
                SetTimeouts(getReportTimeout, setReportTimeout);
                result = true;
            }

            return result;
        }

        public byte Open(uint deviceIndex, ushort vid, ushort pid, uint numInputBuffers)
        {
            return SLABHIDDevice_DLL.HidDevice_Open(ref m_hid, deviceIndex, vid, pid, numInputBuffers);
        }

        public int IsOpened()
        {
            return SLABHIDDevice_DLL.HidDevice_IsOpened(m_hid);
        }

        public uint GetHandle()
        {
            return SLABHIDDevice_DLL.HidDevice_GetHandle(m_hid);
        }

        public byte GetString(byte hidStringType, StringBuilder deviceString, uint deviceStringLength)
        {
            return SLABHIDDevice_DLL.HidDevice_GetString(m_hid, hidStringType, deviceString, deviceStringLength);
        }

        public byte GetIndexedString(uint stringIndex, StringBuilder deviceString, uint deviceStringLength)
        {
            return SLABHIDDevice_DLL.HidDevice_GetIndexedString(m_hid, stringIndex, deviceString, deviceStringLength);
        }

        public byte GetAttributes(ref ushort deviceVid, ref ushort devicePid, ref ushort deviceReleaseNumber)
        {
            return SLABHIDDevice_DLL.HidDevice_GetAttributes(m_hid, ref deviceVid, ref devicePid, ref deviceReleaseNumber);
        }

        public byte SetFeatureReport_Control(byte[] buffer, uint bufferSize)
        {
            return SLABHIDDevice_DLL.HidDevice_SetFeatureReport_Control(m_hid, buffer, bufferSize);
        }

        public byte GetFeatureReport_Control(byte[] buffer, uint bufferSize)
        {
            return SLABHIDDevice_DLL.HidDevice_GetFeatureReport_Control(m_hid, buffer, bufferSize);
        }

        public byte SetOutputReport_Interrupt(byte[] buffer, uint bufferSize)
        {
            return SLABHIDDevice_DLL.HidDevice_SetOutputReport_Interrupt(m_hid, buffer, bufferSize);
        }

        public byte GetInputReport_Interrupt(byte[] buffer, uint bufferSize, uint numReports, ref uint bytesReturned)
        {
            return SLABHIDDevice_DLL.HidDevice_GetInputReport_Interrupt(m_hid, buffer, bufferSize, numReports, ref bytesReturned);
        }

        public byte SetOutputReport_Control(byte[] buffer, uint bufferSize)
        {
            return SLABHIDDevice_DLL.HidDevice_SetOutputReport_Control(m_hid, buffer, bufferSize);
        }

        public byte GetInputReport_Control(byte[] buffer, uint bufferSize)
        {
            return SLABHIDDevice_DLL.HidDevice_GetInputReport_Control(m_hid, buffer, bufferSize);
        }

        public ushort GetInputReportBufferLength()
        {
            return SLABHIDDevice_DLL.HidDevice_GetInputReportBufferLength(m_hid);
        }

        public ushort GetOutputReportBufferLength()
        {
            return SLABHIDDevice_DLL.HidDevice_GetOutputReportBufferLength(m_hid);
        }

        public ushort GetFeatureReportBufferLength()
        {
            return SLABHIDDevice_DLL.HidDevice_GetFeatureReportBufferLength(m_hid);
        }

        public uint GetMaxReportRequest()
        {
            return SLABHIDDevice_DLL.HidDevice_GetMaxReportRequest(m_hid);
        }

        public int FlushBuffers()
        {
            return SLABHIDDevice_DLL.HidDevice_FlushBuffers(m_hid);
        }

        public int CancelIo()
        {
            return SLABHIDDevice_DLL.HidDevice_CancelIo(m_hid);
        }

        public void GetTimeouts(ref uint getReportTimeout, ref uint setReportTimeout)
        {
            SLABHIDDevice_DLL.HidDevice_GetTimeouts(m_hid, ref getReportTimeout, ref setReportTimeout);
        }

        public void SetTimeouts(uint getReportTimeout, uint setReportTimeout)
        {
            SLABHIDDevice_DLL.HidDevice_SetTimeouts(m_hid, getReportTimeout, setReportTimeout);
        }

        public byte Close()
        {
            return SLABHIDDevice_DLL.HidDevice_Close(m_hid);
        }
    }
}
#if false // Decompilation log
'13' items in cache
------------------
Resolve: 'mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Found single assembly: 'mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Load from: 'C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.8\mscorlib.dll'
#endif

#region Assembly LQ001_Class, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Users\jare0530\Projects\080723_MinimalDisplay\Mozaic_GUI_V12_source_code\Mozaic_GUI_V12\LQ001_Class.dll
// Decompiled with ICSharpCode.Decompiler 7.1.0.6543
#endregion

using System;
using System.Runtime.InteropServices;
using System.Text;

namespace SLAB_HID_DEVICE
{
    public class SLABHIDDevice_DLL
    {
        [DllImport("SLABHIDDevice.dll")]
        public static extern uint HidDevice_GetNumHidDevices(ushort vid, ushort pid);

        [DllImport("SLABHIDDevice.dll")]
        public static extern byte HidDevice_GetHidString(uint deviceIndex, ushort vid, ushort pid, byte hidStringType, StringBuilder deviceString, uint deviceStringLength);

        [DllImport("SLABHIDDevice.dll")]
        public static extern byte HidDevice_GetHidIndexedString(uint deviceIndex, ushort vid, ushort pid, uint stringIndex, StringBuilder deviceString, uint deviceStringLength);

        [DllImport("SLABHIDDevice.dll")]
        public static extern byte HidDevice_GetHidAttributes(uint deviceIndex, ushort vid, ushort pid, ref ushort deviceVid, ref ushort devicePid, ref ushort deviceReleaseNumber);

        [DllImport("SLABHIDDevice.dll")]
        public static extern void HidDevice_GetHidGuid(ref Guid hidGuid);

        [DllImport("SLABHIDDevice.dll")]
        public static extern byte HidDevice_GetHidLibraryVersion(ref byte major, ref byte minor, ref int release);

        [DllImport("SLABHIDDevice.dll")]
        public static extern byte HidDevice_Open(ref IntPtr device, uint deviceIndex, ushort vid, ushort pid, uint numInputBuffers);

        [DllImport("SLABHIDDevice.dll")]
        public static extern int HidDevice_IsOpened(IntPtr device);

        [DllImport("SLABHIDDevice.dll")]
        public static extern uint HidDevice_GetHandle(IntPtr device);

        [DllImport("SLABHIDDevice.dll")]
        public static extern byte HidDevice_GetString(IntPtr device, byte hidStringType, StringBuilder deviceString, uint deviceStringLength);

        [DllImport("SLABHIDDevice.dll")]
        public static extern byte HidDevice_GetIndexedString(IntPtr device, uint stringIndex, StringBuilder deviceString, uint deviceStringLength);

        [DllImport("SLABHIDDevice.dll")]
        public static extern byte HidDevice_GetAttributes(IntPtr device, ref ushort deviceVid, ref ushort devicePid, ref ushort deviceReleaseNumber);

        [DllImport("SLABHIDDevice.dll")]
        public static extern byte HidDevice_SetFeatureReport_Control(IntPtr device, byte[] buffer, uint bufferSize);

        [DllImport("SLABHIDDevice.dll")]
        public static extern byte HidDevice_GetFeatureReport_Control(IntPtr device, byte[] buffer, uint bufferSize);

        [DllImport("SLABHIDDevice.dll")]
        public static extern byte HidDevice_SetOutputReport_Interrupt(IntPtr device, byte[] buffer, uint bufferSize);

        [DllImport("SLABHIDDevice.dll")]
        public static extern byte HidDevice_GetInputReport_Interrupt(IntPtr device, byte[] buffer, uint bufferSize, uint numReports, ref uint bytesReturned);

        [DllImport("SLABHIDDevice.dll")]
        public static extern byte HidDevice_SetOutputReport_Control(IntPtr device, byte[] buffer, uint bufferSize);

        [DllImport("SLABHIDDevice.dll")]
        public static extern byte HidDevice_GetInputReport_Control(IntPtr device, byte[] buffer, uint bufferSize);

        [DllImport("SLABHIDDevice.dll")]
        public static extern ushort HidDevice_GetInputReportBufferLength(IntPtr device);

        [DllImport("SLABHIDDevice.dll")]
        public static extern ushort HidDevice_GetOutputReportBufferLength(IntPtr device);

        [DllImport("SLABHIDDevice.dll")]
        public static extern ushort HidDevice_GetFeatureReportBufferLength(IntPtr device);

        [DllImport("SLABHIDDevice.dll")]
        public static extern uint HidDevice_GetMaxReportRequest(IntPtr device);

        [DllImport("SLABHIDDevice.dll")]
        public static extern int HidDevice_FlushBuffers(IntPtr device);

        [DllImport("SLABHIDDevice.dll")]
        public static extern int HidDevice_CancelIo(IntPtr device);

        [DllImport("SLABHIDDevice.dll")]
        public static extern void HidDevice_GetTimeouts(IntPtr device, ref uint getReportTimeout, ref uint setReportTimeout);

        [DllImport("SLABHIDDevice.dll")]
        public static extern void HidDevice_SetTimeouts(IntPtr device, uint getReportTimeout, uint setReportTimeout);

        [DllImport("SLABHIDDevice.dll")]
        public static extern byte HidDevice_Close(IntPtr device);
    }
}
#if false // Decompilation log
'13' items in cache
------------------
Resolve: 'mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Found single assembly: 'mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Load from: 'C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.8\mscorlib.dll'
#endif

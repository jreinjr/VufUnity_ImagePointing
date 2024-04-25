#region Assembly LQ001_Class, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Users\jare0530\Projects\080723_MinimalDisplay\Mozaic_GUI_V12_source_code\Mozaic_GUI_V12\LQ001_Class.dll
// Decompiled with ICSharpCode.Decompiler 7.1.0.6543
#endregion

using System;
using System.Security.Cryptography;
using System.Threading;
using SLAB_HID_DEVICE;
using UnityEngine;

namespace LQ001_Class
{
    public class DimmerPanelInterface
    { 
        public Hid SLABs = new Hid();


        protected const byte BRIDGE_MAX_RW_LENGTH = 65;
        protected byte[] bridgeArray = new byte[BRIDGE_MAX_RW_LENGTH];


        public virtual byte USBConnect(int ConnectFlag, ushort _connectedVID, ushort _connectedPID)
        {
            uint num = 0u;
            byte b = 1;
            try
            {
                num = Hid.GetNumHidDevices(_connectedVID, _connectedPID);
                if (num == 1 && ConnectFlag == 0)
                {
                    return SLABs.Open(0u, _connectedVID, _connectedPID, 512u);
                }

                if (num == 1 && ConnectFlag == 1)
                {
                    return b = 0;
                }

                return 1;
            }
            catch (DllNotFoundException)
            {
                return 1;
            }
        }

        public virtual byte USBDisconnect(int ConnectFlag, ushort _connectedVID, ushort _connectedPID)
        {
            if (Hid.GetNumHidDevices(_connectedVID, _connectedPID) == 0)
            {
                return SLABs.Close();
            }

            if (ConnectFlag == 1)
            {
                return SLABs.Close();
            }

            return byte.MaxValue;
        }
        public virtual byte MCURegWrite(byte bridgeCase, byte addr, int length, byte[] writeData)
        {
            // Set the first four bytes of bridgeArray directly
            bridgeArray[1] = 1;
            bridgeArray[2] = bridgeCase;
            bridgeArray[3] = addr;
            bridgeArray[4] = (byte)length;

            // Copy writeData directly to bridgeArray starting from the 5th position
            try
            {
                Array.Copy(writeData, 0, bridgeArray, 5, length);
            }
            catch (ArgumentException)
            {
                return 8;
            }
            return SLABs.SetOutputReport_Interrupt(bridgeArray, SLABs.GetOutputReportBufferLength());
        }
    }
}

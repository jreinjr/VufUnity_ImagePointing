using System;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;

using System.Linq;

public class LCDController : MonoBehaviour
{
    // This is what will actually generate greyscale values
    // Could be procedural, using texture sequence, render texture, etc
    public LCDDataProviderBase lcdDataProvider_L;
    public LCDDataProviderBase lcdDataProvider_R;

    public float sendRateHz = 10; // How many times per second to send data to the LCD
    protected int sendDelayMs;
    public bool sendOnStart = true; // Should we immediately start sending data values to the LCD

    protected Thread sendDataThread;
    protected bool shouldSendData = true;

    //------ defined new Dimming ------//
    protected LQ001_Class.DimmerPanelInterface DimmerPanel_L = new LQ001_Class.DimmerPanelInterface();
    protected LQ001_Class.DimmerPanelInterface DimmerPanel_R = new LQ001_Class.DimmerPanelInterface();

    //--- USB packages setting ---//
    protected const byte bridge_max_wr_length = 48; //Including first byte for report ID

    // Full frames
    protected byte[] sendData_L;
    protected byte[] sendData_R;

    // Single row
    protected byte[] writeData_L;
    protected byte[] writeData_R;

    //--- defined the VID & PID
    protected ushort _connectedVID_L = 0x0103;
    protected ushort _connectedPID_L = 0x8500;

    protected ushort _connectedVID_R = 0x0103;
    protected ushort _connectedPID_R = 0x8501;

    protected int ConnectFlag_L = 0;
    protected int ConnectFlag_R = 0;
    protected int EnableFlag_L = 0;
    protected int EnableFlag_R = 0;

    protected byte result;


    protected bool HWshowFlag = false;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        sendDelayMs = (int)((1f / sendRateHz) * 1000);
        sendDataThread = new Thread(SendDataLoopThreaded);

        sendData_L = new byte[1920];
        writeData_L = new byte[bridge_max_wr_length];

        sendData_R = new byte[1920];
        writeData_R = new byte[bridge_max_wr_length];



        ConnectPanels();

        if (sendOnStart)
        {
            StartSendingData();
        }
    }

    protected virtual void OnDestroy()
    {
        StopSendingData();

        DisconnectPanels();
    }

    [ContextMenu("Start")]
    public virtual void StartSendingData()
    {
        sendDataThread.Start();
    }

    public virtual void StopSendingData()
    {
        shouldSendData = false;
        sendDataThread.Join();
    }

    protected virtual void ConnectPanels()
    {
        byte result;

        result = DimmerPanel_L.USBConnect(ConnectFlag_L, _connectedVID_L, _connectedPID_L);
        ConnectFlag_L = (result == 0) ? 1 : 0;

        result = DimmerPanel_R.USBConnect(ConnectFlag_R, _connectedVID_R, _connectedPID_R);
        ConnectFlag_R = (result == 0) ? 1 : 0;

        EnableFlag_L = (ConnectFlag_L == 1 && EnableFlag_L == 0) ? 1 : 0;

        EnableFlag_R = (ConnectFlag_R == 1 && EnableFlag_R == 0) ? 1 : 0;
        Debug.LogFormat("ConnectL {0} ConnectR {1} EnableL {2} EnableR {3}", ConnectFlag_L, ConnectFlag_R, EnableFlag_L, EnableFlag_R);
    }

    protected virtual void DisconnectPanels()
    {
        DimmerPanel_L.USBDisconnect(1, _connectedVID_L, _connectedPID_L);
        DimmerPanel_R.USBDisconnect(1, _connectedVID_R, _connectedPID_R);
    }
    protected virtual void SendDataLoopThreaded()
    {
        while (shouldSendData)
        {
            Array.Copy(lcdDataProvider_L.ProvideLCDData(), sendData_L, 1920); // Ensure this doesn't access Unity's main thread
            Array.Copy(lcdDataProvider_R.ProvideLCDData(), sendData_R, 1920); // Ensure this doesn't access Unity's main thread
            SendData(sendData_L, sendData_R);
            Thread.Sleep(sendDelayMs); // Replace 'wait' with an actual time in milliseconds
        }
    }


    protected virtual void SendData(byte[] data_L, byte[] data_R)
    {
        for (int m = 0; m < 40; m++)
        {
            WriteRowWithBlockCopy(data_L, m, ref writeData_L);
            WriteRowWithBlockCopy(data_R, m, ref writeData_R);

            Task[] tasks = new Task[2];

            tasks[0] = Task.Run(() =>
            {
                result = DimmerPanel_L.MCURegWrite(0x07, 0xD8, 48, writeData_L);
            });

            tasks[1] = Task.Run(() =>
            {
                writeData_R = writeData_R.Reverse().ToArray();
                result = DimmerPanel_R.MCURegWrite(0x07, 0xD8, 48, writeData_R);
            });

            Task.WaitAll(tasks.Where(t => t != null).ToArray());
        }
    }

    protected unsafe virtual void WriteRowWithBlockCopy(byte[] data, int rowIndex, ref byte[] buffer)
    {
        fixed (byte* pData = data)
        {
            byte* pRowStart = pData + rowIndex * 48;

            fixed (byte* pTarget = buffer)
            {
                Buffer.MemoryCopy(pRowStart, pTarget, 48, 48);
            }
        }
    }
}

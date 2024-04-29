using System;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;

using System.Linq;

public class FerreroLCDController : MonoBehaviour
{
    // This is what will actually generate greyscale values
    // Could be procedural, using texture sequence, render texture, etc
    public LCDDataProviderBase lcdDataProvider_L;
    public LCDDataProviderBase lcdDataProvider_R;


    [SerializeField] FerreroSocketClient ferreroSocketClient;
    public float sendRateHz = 144; // How many times per second to send data to the LCD
    protected int sendDelayMs;
    public bool sendOnStart = true; // Should we immediately start sending data values to the LCD

    protected Thread sendDataThread;
    protected bool shouldSendData = true;

    //--- USB packages setting ---//
    protected const byte bridge_max_wr_length = 48; //Including first byte for report ID

    // Full frames
    protected byte[] sendData_SBS;

    protected byte[] buffer_L;
    protected byte[] buffer_R;

    protected byte result;


    protected bool HWshowFlag = false;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        buffer_L = new byte[1920];
        buffer_R = new byte[1920];
        sendDelayMs = (int)((1f / sendRateHz) * 1000);
        sendDataThread = new Thread(SendDataLoopThreaded);

        sendData_SBS = new byte[3840];


        if (sendOnStart)
        {
            StartSendingData();
        }
    }

    protected virtual void OnDestroy()
    {
        StopSendingData();
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

    protected virtual void SendDataLoopThreaded()
    {
        while (shouldSendData)
        {
            buffer_L = lcdDataProvider_L.ProvideLCDData();
            buffer_R = lcdDataProvider_R.ProvideLCDData();

            Array.Copy(buffer_R, 0, sendData_SBS, 0, 1920); // Ensure this doesn't access Unity's main thread
            Array.Copy(buffer_L, 0, sendData_SBS, 1920, 1920); // Ensure this doesn't access Unity's main thread

            //sendData_SBS = lcdDataProvider_SBS.ProvideLCDData();

            ferreroSocketClient.SendData(sendData_SBS);
            //Array.Copy(lcdDataProvider_SBS.ProvideLCDData(), sendData_SBS, 3840); // Ensure this doesn't access Unity's main thread
            ////Array.Copy(lcdDataProvider_R.ProvideLCDData(), sendData_R, 1920); // Ensure this doesn't access Unity's main thread
            //SendData(sendData_SBS);
            Thread.Sleep(sendDelayMs); // Replace 'wait' with an actual time in milliseconds
        }
    }


    protected virtual void SendData(byte[] data_SBS)
    {
        //result = DimmerPanel_L.MCURegWrite(0x07, 0xD8, 48, writeData_L);
        ferreroSocketClient.SendData(data_SBS);
    }

}

using System;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;
using Vuforia;

public class LoadVufUnityDriver : MonoBehaviour 
{
    public RenderTexture vuforiaTexture;
    private Texture2D tempTexture;


    void Start()
    {
        tempTexture = new Texture2D(vuforiaTexture.width, vuforiaTexture.height, TextureFormat.RGB24, false);
        

        var driverName = "VufUnity.dll";

        VuforiaApplication.Instance.OnVuforiaStarted += OnVuforiaStarted;

        VuforiaApplication.Instance.Initialize(driverName, IntPtr.Zero);
    }

    void OnVuforiaStarted()
    {
        //SendCameraFrame();
    }

    void FixedUpdate()
    {
        RenderTexture.active = vuforiaTexture;
        tempTexture.ReadPixels(new Rect(0, 0, vuforiaTexture.width, vuforiaTexture.height), 0, 0);
        tempTexture.Apply();
        RenderTexture.active = null;

        byte[] rgbData = tempTexture.GetRawTextureData();
        SendCameraFrame(rgbData);
    }

    public void SendCameraFrame(byte[] rgbData)
    {
        SetCameraFrame(rgbData, rgbData.Length);
    }



    [DllImport("VufUnity", CallingConvention = CallingConvention.Cdecl)]
    private static extern void SetCameraFrame(byte[] frameData, int length);
}


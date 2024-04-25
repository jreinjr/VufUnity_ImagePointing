// (c) Meta Platforms, Inc. and affiliates. Confidential and proprietary.

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;
using System;
using System.Linq;
using System.Threading;

public class MeravellaAriaAdapter : MonoBehaviour
{
    //public Transform[] headPoses;
    // DLL imports
    public enum MERAVELLA_Error
    {
        MERAVELLA_Success = 0,
        MERAVELLA_Error_Exception = 1,
        MERAVELLA_Error_Unknown = 2,
        MERAVELLA_Error_InvalidParameter = 3,
        MERAVELLA_Error_FeatureDisabled = 4,
        MERAVELLA_Error_FileNotFound = 5,

        MERAVELLA_Error_AriaOperationFailed = 101,
        MERAVELLA_Error_AriaNotInitialized = 102,
        MERAVELLA_Error_AriaNotAuthenticated = 103,
        MERAVELLA_Error_AriaNotConnected = 104,
        MERAVELLA_Error_AriaNoDataAvailable = 105,
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct AriaImageMetadata {
        public uint frameNumber;
        public uint imageBufferSize;
        public uint imageHeight;
        public uint imageWidth;
        public uint imageFormat;
    }

    public const string kMeravellaAriaIntegrationDllPath = "meravella_aria_integration_shared_lib.dll";
    [DllImport(kMeravellaAriaIntegrationDllPath)]
    public static extern MERAVELLA_Error MV_Aria_Initialize(string configFolderpath);
    [DllImport(kMeravellaAriaIntegrationDllPath)]
    public static extern MERAVELLA_Error MV_Aria_GetRgbImageBufferSize(ref uint rgbImageBufferSize);
    [DllImport(kMeravellaAriaIntegrationDllPath)]
    public static extern MERAVELLA_Error MV_Aria_Shutdown();
    [DllImport(kMeravellaAriaIntegrationDllPath)]
    public static extern MERAVELLA_Error MV_Aria_Authenticate();
    [DllImport(kMeravellaAriaIntegrationDllPath)]
    public static extern MERAVELLA_Error MV_Aria_Connect();
    [DllImport(kMeravellaAriaIntegrationDllPath)]
    public static extern MERAVELLA_Error MV_Aria_StartStreaming();
    [DllImport(kMeravellaAriaIntegrationDllPath)]
    public static extern MERAVELLA_Error MV_Aria_ConnectAndStartStreaming();
    [DllImport(kMeravellaAriaIntegrationDllPath)]
    public static extern bool MV_Aria_IsConnected();
    [DllImport(kMeravellaAriaIntegrationDllPath)]
    public static extern bool MV_Aria_IsStreaming();
    [DllImport(kMeravellaAriaIntegrationDllPath)]
    public static extern MERAVELLA_Error MV_Aria_StopStreaming();
    [DllImport(kMeravellaAriaIntegrationDllPath)]
    public static extern MERAVELLA_Error MV_Aria_Disconnect();
    [DllImport(kMeravellaAriaIntegrationDllPath)]
    public static extern MERAVELLA_Error MV_Aria_GetHeadPose(ref float headPose, uint headPoseArraySize);
    [DllImport(kMeravellaAriaIntegrationDllPath)]
    public static extern MERAVELLA_Error MV_Aria_GetRgbImage(ref AriaImageMetadata ariaImage, ref byte data);

    // Data

    // public
    public RenderTexture renderTexture;

    // private
    private const uint kHeadPoseArraySize = 7;
    private float[] headPose_;
    private uint rgbImageBufferSize_;
    private AriaImageMetadata ariaImage_;
    private byte[] data_;
    private bool isConnected_ = false;
    private bool isStreaming_ = false;
    private Texture2D texture2D_;

    // Start is called before the first frame update
    void Start()
    {
        // Set the path to the meravella folder
        string configFolderPath = Application.dataPath + "/Plugins/Meravella";

        try
        {
            MERAVELLA_Error error = MV_Aria_Initialize(configFolderPath);
            if (error != MERAVELLA_Error.MERAVELLA_Success) {
                Debug.LogError(
                    string.Format(
                        "Unable to initialize Meravella Aria integration. configFolderPath:{0}, ErrorCode:{1}",
                        configFolderPath,
                        error.ToString()));
                return;
            }

            //// NOTE : Disabling authentication for now since it only needs to be done once.
            //// Only need to authenticate once. This can be moved to a button click
            // MV_Aria_Authenticate();
            //// // Debug.Log("Authentication initiated, will wait for 3 seconds for you to approve the request");
            // Thread.Sleep(10000);

            Debug.Log("Connection and streaming initiated");
            MV_Aria_Connect();
            MV_Aria_StartStreaming();
            headPose_ = new float[kHeadPoseArraySize];
            MV_Aria_GetRgbImageBufferSize(ref rgbImageBufferSize_);
            Debug.Log("rgbImageBufferSize: " + rgbImageBufferSize_.ToString());
            data_ = new byte[rgbImageBufferSize_];

            RenderTexture.active = renderTexture;
        }
        catch
        {
            Debug.Log("An error occurred.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            if (!isConnected_) {
                isConnected_ = MV_Aria_IsConnected();
                Debug.Log("IsConnected: " + isConnected_);
                return;
            }
            if (!isStreaming_) {
                isStreaming_ = MV_Aria_IsStreaming();
                Debug.Log("IsStreaming: " + isStreaming_);
                return;
            }

            getHeadPose();
            getRgbImage();
        }
        catch
        {
            Debug.Log("Update exception");
        }
    }

    void OnDestroy()
    {
        try
        {
            Debug.Log("Stop stream and disconnect");
            MERAVELLA_Error error = MV_Aria_StopStreaming();
            if (error != MERAVELLA_Error.MERAVELLA_Success) {
                Debug.LogError("Failed MV_Aria_StopStreaming");
            }
            error = MV_Aria_Disconnect();
            if (error != MERAVELLA_Error.MERAVELLA_Success) {
                Debug.LogError("Failed MV_Aria_StopStreaming");
            }
            MV_Aria_Shutdown();
        }
        catch
        {
            Debug.Log("destroy exception");
        }
    }

    private void getHeadPose()
    {
        // Get the head pose from Aria
        MERAVELLA_Error errorCode = MV_Aria_GetHeadPose(ref headPose_[0], kHeadPoseArraySize);
        if (errorCode != MERAVELLA_Error.MERAVELLA_Success)
        {
            Debug.LogError(string.Format("Unable to get head pose data. ErrorCode:{0}", errorCode.ToString()));
        }
        else
        {
            //foreach (var headPose in headPoses)
            //{
            //    headPose.localPosition = new Vector3(headPose_[0], headPose_[1], headPose_[2]);
            //    headPose.localRotation = new Quaternion(headPose_[3], headPose_[4], headPose_[5], headPose_[6]);
            //}
            // <Add code here to process the head pose data>
        }
    }

    private void getRgbImage()
    {
        // Get the rgb image from Aria
        MERAVELLA_Error errorCode = MV_Aria_GetRgbImage(ref ariaImage_, ref data_[0]);
        if (errorCode != MERAVELLA_Error.MERAVELLA_Success)
        {
            Debug.LogError(string.Format("Unable to get rgb image. ErrorCode:{0}", errorCode.ToString()));
        }
        else
        {
            // Create a new Texture2D object
            texture2D_ = new Texture2D((int)ariaImage_.imageWidth, (int)ariaImage_.imageHeight);
            // Load the image data into the texture
            texture2D_.LoadImage(data_);
            Graphics.Blit(texture2D_, renderTexture);
            Destroy(texture2D_);
            texture2D_ = null;
        }
    }
}

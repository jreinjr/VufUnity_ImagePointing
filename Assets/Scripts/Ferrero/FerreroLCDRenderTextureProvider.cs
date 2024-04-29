using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class FerreroLCDRenderTextureProvider : FerreroLCDDataProviderBase
{
    bool isRequesting = false;
    public RenderTexture renTex;
    protected NativeArray<byte> data;
    private AsyncGPUReadbackRequest readbackRequest;
    private bool isQuitting = false;


    protected override void Initialize()
    {
        data = new NativeArray<byte>(48 * 40 * 2, Allocator.Persistent);

        base.Initialize();
    }

    protected override void Cleanup()
    {
        isQuitting = true;
        readbackRequest.WaitForCompletion();
        DisposeNativeArray();
        base.Cleanup();

    }
    private void Update()
    {
        if (!isRequesting && !isQuitting)
        {
            AsyncReadbackIntoNativeArray();
        }
    }
    void DisposeNativeArray()
    {
        if (data.IsCreated)
        {
            data.Dispose();
        }
    }

    public override byte[] ProvideLCDData()
    {
        if (!data.IsCreated)
        {
            throw new InvalidOperationException("NativeArray not initialized!");
        }

        return byteData;
    }

    // Reads the data from a render texture into NativeArray data (provided by ProvideLCDData in base class)
    void AsyncReadbackIntoNativeArray()
    {
        if (renTex == null) return;

        isRequesting = true;
        readbackRequest = AsyncGPUReadback.RequestIntoNativeArray(ref data, renTex, 0, OnReadbackComplete);
        
    }

    void OnReadbackComplete(AsyncGPUReadbackRequest request)
    {
        if (request.hasError)
        {
            Debug.Log("Failed to read GPU data.");
            isRequesting = false;
            return;
        }
        data.CopyTo(byteData);


        isRequesting = false;
    }
}

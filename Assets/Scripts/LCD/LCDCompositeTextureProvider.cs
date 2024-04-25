using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class LCDCompositeTextureProvider : LCDDataProviderBase
{
    [Header("Textures")]
    public Texture2D[] regularTextures;
    public RenderTexture[] renderTextures;
    public RenderTexture outTex;

    NativeArray<byte> data;

    protected override void Initialize()
    {
        data = new NativeArray<byte>(48 * 40, Allocator.Persistent);

        base.Initialize();
    }

    protected override void Cleanup()
    {
        DisposeNativeArray();
        base.Cleanup();

    }
    private void Update()
    {

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


}

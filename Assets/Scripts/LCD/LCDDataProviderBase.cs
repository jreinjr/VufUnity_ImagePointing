using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.Collections;
using UnityEngine;

public abstract class LCDDataProviderBase : MonoBehaviour
{

    protected bool initialized = false;
    protected byte[] byteData;
    public abstract byte[] ProvideLCDData();

    protected virtual void Awake()
    {
        Initialize();
    }


    protected virtual void OnDestroy()
    {
        Cleanup();
    }

    protected virtual void Initialize()
    {
        if (!initialized)
        {
            byteData = new byte[48 * 40];
        }
        initialized = true;
    }

    protected virtual void Cleanup()
    {
        initialized = false;
    }
}
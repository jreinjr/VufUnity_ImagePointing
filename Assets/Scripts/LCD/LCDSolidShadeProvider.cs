using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LCDSolidShadeProvider : LCDDataProviderBase
{
    [Range(0, 255)] public float shade;
    public override byte[] ProvideLCDData()
    {
        for (int i = 0; i < byteData.Length; i++)
        {
            byteData[i] = (byte)shade;
        }
        return byteData;
    }
}

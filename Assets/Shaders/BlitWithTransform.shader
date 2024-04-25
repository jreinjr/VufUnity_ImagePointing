Shader"Hidden/BlitWithTransform"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

#include "UnityCG.cginc"

struct appdata
{
    float4 vertex : POSITION;
    float2 texcoord : TEXCOORD0;
};

struct v2f
{
    float4 position : SV_POSITION;
    float2 texcoord : TEXCOORD0;
};

uniform sampler2D _MainTex;
uniform int _Rotation; // 0: None, 1: CW90, -1: CCW90, 2: CW180
uniform int _FlipH;
uniform int _FlipV;
uniform float2 _Scale;
uniform float2 _Offset;

v2f vert(appdata v)
{
    v2f o;
    o.position = UnityObjectToClipPos(v.vertex);
    o.texcoord = v.texcoord;

                // Apply flip
    if (_FlipH == 1)
    {
        o.texcoord.x = 1 - o.texcoord.x;
    }
    if (_FlipV == 1)
    {
        o.texcoord.y = 1 - o.texcoord.y;
    }

                // Apply rotation
    if (_Rotation == 1) // CW90
    {
        o.texcoord = float2(o.texcoord.y, 1 - o.texcoord.x);
    }
    else if (_Rotation == -1) // CCW90
    {
        o.texcoord = float2(1 - o.texcoord.y, o.texcoord.x);
    }
    else if (_Rotation == 2) // CW180
    {
        o.texcoord = float2(1 - o.texcoord.x, 1 - o.texcoord.y);
    }

                // Apply scaling and offset
    o.texcoord = _Scale * (o.texcoord - 0.5f) + 0.5f + _Offset;

    return o;
}

float4 frag(v2f i) : SV_Target
{
    return tex2D(_MainTex, i.texcoord);
}

            ENDCG
        }
    }
}

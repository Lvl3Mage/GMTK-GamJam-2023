Shader "Unlit/OceanShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (0,0,0,0)
        _Scale ("Scale", Float) = 1
        _OffsetSize ("Scale", Float) = 0.1
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        LOD 100

        Pass
        {   
            Blend SrcAlpha OneMinusSrcAlpha


            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct MeshData
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Interpolators
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            float _Scale;
            float _OffsetSize;

            Interpolators vert (MeshData v)
            {
                Interpolators o;
                float3 pos = v.vertex.xyz;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (Interpolators i) : SV_Target
            {
                // sample the texture
                // return sin(i.uv.x*10)/2 + 0.5;
                i.uv *= _Scale;
                float2 MaskCoords = i.uv.xy * 25;
                float2 OffsetCoords = i.uv.xy * 10;
                float mask = saturate(sin(MaskCoords.x/10 + _Time.y*0.3)+sin(MaskCoords.y/15 + _Time.y*0.2)+sin(MaskCoords.y/25 + _Time.y*0.2 )*(sin(MaskCoords.x/15 + _Time.y)));//*;
                float topRight = cos(OffsetCoords.y*5 + OffsetCoords.x*15 + _Time.z*3)/2 + 0.5;
                float topLeft = cos(OffsetCoords.y*7- OffsetCoords.x*10 + _Time.z*2)/2 + 0.5;
                // float topLeft = sin(coords.y*coords.x+_Time.y)*sin(coords.y*-coords.x+_Time.y) * cos(coords.y*10 + coords.x*10 + _Time.z);
                float offsetx = lerp(topRight, topLeft, mask) + mask;
                i.uv.x += offsetx*_OffsetSize;
                fixed4 col = tex2D(_MainTex, i.uv);
                // return mask;
                return col * _Color;
            }
            ENDCG
        }
    }
}

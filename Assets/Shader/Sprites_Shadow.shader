Shader "Sprites/Shadow"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 200

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            float4x4 unity_ObjectToWorld;
            float4x4 unity_MatrixVP;
            float4 _MainTex_ST;

            struct VertexInput
            {
                float4 pos : POSITION;
                float2 uv  : TEXCOORD0;
            };

            struct VertexOutput
            {
                float2 uv  : TEXCOORD0;
                float4 pos : SV_POSITION;
            };

            VertexOutput vert(VertexInput input)
            {
                VertexOutput output;
                output.uv = (input.uv.xy * _MainTex_ST.xy) + _MainTex_ST.zw;
                output.pos = mul(unity_MatrixVP, mul(unity_ObjectToWorld, input.pos));
                return output;
            }

            Texture2D<float4> _MainTex;
            SamplerState sampler_MainTex;
            float4 _Color;

            struct FragmentInput
            {
                float2 uv : TEXCOORD0;
            };

            float4 frag(FragmentInput input) : SV_Target
            {
                float4 tex = _MainTex.Sample(sampler_MainTex, input.uv);
                return float4(tex.rgb * _Color.rgb, tex.a * _Color.a);
            }
            ENDHLSL
        }
    }

    Fallback "Sprites/Default"
}

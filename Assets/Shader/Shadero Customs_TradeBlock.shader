Shader "Shadero Customs/TradeBlock" {
	Properties {
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		PixelXYUV_SizeX_1 ("PixelXYUV_SizeX_1", Range(1, 128)) = 32
		PixelXYUV_SizeY_1 ("PixelXYUV_SizeY_1", Range(1, 128)) = 32
		_Burn_Value_1 ("_Burn_Value_1", Range(0, 1)) = 0.5878265
		_Burn_Speed_1 ("_Burn_Speed_1", Range(-8, 8)) = 0.5
		_Brightness_Fade_1 ("_Brightness_Fade_1", Range(0, 1)) = 0.5
		_Hologram_Value_1 ("_Hologram_Value_1", Range(-1, 1)) = 0.002574431
		_Hologram_Speed_1 ("_Hologram_Speed_1", Range(0, 4)) = 0.2872002
		_Add_Fade_1 ("_Add_Fade_1", Range(0, 4)) = 1
		_SpriteFade ("SpriteFade", Range(0, 1)) = 1
		[HideInInspector] _StencilComp ("Stencil Comparison", Float) = 8
		[HideInInspector] _Stencil ("Stencil ID", Float) = 0
		[HideInInspector] _StencilOp ("Stencil Operation", Float) = 0
		[HideInInspector] _StencilWriteMask ("Stencil Write Mask", Float) = 255
		[HideInInspector] _StencilReadMask ("Stencil Read Mask", Float) = 255
		[HideInInspector] _ColorMask ("Color Mask", Float) = 15
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200

		Pass
		{
			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			float4x4 unity_ObjectToWorld;
			float4x4 unity_MatrixVP;
			float4 _MainTex_ST;

			struct Vertex_Stage_Input
			{
				float4 pos : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct Vertex_Stage_Output
			{
				float2 uv : TEXCOORD0;
				float4 pos : SV_POSITION;
			};

			Vertex_Stage_Output vert(Vertex_Stage_Input input)
			{
				Vertex_Stage_Output output;
				output.uv = (input.uv.xy * _MainTex_ST.xy) + _MainTex_ST.zw;
				output.pos = mul(unity_MatrixVP, mul(unity_ObjectToWorld, input.pos));
				return output;
			}

			Texture2D<float4> _MainTex;
			SamplerState sampler_MainTex;

			struct Fragment_Stage_Input
			{
				float2 uv : TEXCOORD0;
			};

			float4 frag(Fragment_Stage_Input input) : SV_TARGET
			{
				return _MainTex.Sample(sampler_MainTex, input.uv.xy);
			}

			ENDHLSL
		}
	}
	Fallback "Sprites/Default"
}
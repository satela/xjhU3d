Shader "Custom/IcFog" {
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	SubShader
	{
		Pass
		{
			ZTest Always
			Cull Off
			ZWrite Off
			Fog { Mode off }
					
CGPROGRAM
#pragma vertex vert_img
#pragma fragment frag vertex:vert
#pragma fragmentoption ARB_precision_hint_fastest 
#include "UnityCG.cginc"

sampler2D _MainTex;
sampler2D _CameraDepthTexture;

uniform float4x4 _InverseMVP;
uniform float4 _CamPos;

//---------------------------------
//uniform float _GlobalDensity;
//uniform float4 _FogColor;
//uniform float4 _StartDistance;
//
//// for fast world space reconstruction
//
//uniform float4x4 _FrustumCornersWS;
//uniform float4 _MainTex_TexelSize;
//-----------------------------------

struct Input
{
	float4 position : POSITION;
	float2 uv : TEXCOORD0;
	//float2 uv_depth : TEXCOORD1;
	//float4 interpolatedRay : TEXCOORD2;
};

void vert (inout appdata_full v, out Input o)
{
	o.position = mul(UNITY_MATRIX_MVP, v.vertex);
	o.uv = v.texcoord.xy;

//	half index = v.vertex.z;
//	v.vertex.z = 0.1;
//	o.position = mul(UNITY_MATRIX_MVP, v.vertex);
//	o.uv = v.texcoord.xy;			
//	o.uv_depth = v.texcoord.xy;
//	
//	#if UNITY_UV_STARTS_AT_TOP
//	if (_MainTex_TexelSize.y < 0)
//		o.uv.y = 1-o.uv.y;
//	#endif	
//	
//	o.interpolatedRay = _FrustumCornersWS[(int)index];
//	o.interpolatedRay.w = index;
}

float3 CamToWorld (in float2 uv, in float depth)
{
	float4 pos = float4(uv.x, uv.y, depth, 1.0);
	pos.xyz = pos.xyz * 2.0 - 1.0;
	pos = mul(_InverseMVP, pos);
	return pos.xyz / pos.w;
}

fixed4 frag (Input i) : COLOR
{
	half4 original = tex2D(_MainTex, i.uv);
	
	#if SHADER_API_D3D9
	float2 depthUV = i.uv;
	depthUV.y = lerp(depthUV.y, 1.0 - depthUV.y, _CamPos.w);
	float depth = UNITY_SAMPLE_DEPTH(tex2D(_CameraDepthTexture, depthUV));
	float3 pos = CamToWorld(depthUV, depth);
#else
	float depth = UNITY_SAMPLE_DEPTH(tex2D(_CameraDepthTexture, i.uv));
	float3 pos = CamToWorld(i.uv, depth);
#endif


//#if SHADER_API_D3D9
//	float2 depthUV = i.uv_depth;
//	depthUV.y = lerp(depthUV.y, 1.0 - depthUV.y, _CamPos.w);
//	float depth = UNITY_SAMPLE_DEPTH(tex2D(_CameraDepthTexture, depthUV));
//	float3 pos = CamToWorld(depthUV, depth);
//#else
//	float depth = UNITY_SAMPLE_DEPTH(tex2D(_CameraDepthTexture, i.uv_depth));
//	float3 pos = CamToWorld(i.uv_depth, depth);
//#endif

////-----------------------------------------------
//	float dpth = Linear01Depth(depth);		
//	float4 camDir = ( /*_CameraWS  + */ dpth * i.interpolatedRay);
//	float fogInt = saturate(length( camDir ) * _StartDistance.x - 1.0) * _StartDistance.y;	
//	float4 tempColor = lerp(_FogColor, original, exp(-_GlobalDensity*fogInt));		
////-----------------------------------------------
//	
	float factor = clamp(pos.y / 10.0, 0, 1.0);
	
	float factor2 = 0;
	float temp2 = 1 - (abs(i.uv.x - 0.5) + abs(i.uv.y - 0.5));
	if(temp2 < 0.2)
	{
	//0.2  --> [0,1]
		factor2 = (1.0-temp2 * 5.0);
	}

	float4 fogColor = float4(0,0,0,1.0);
	//return lerp(tempColor, fogColor, factor);
	return lerp(original, fogColor, factor + factor2);
}
ENDCG
		}
	}
	Fallback off
}

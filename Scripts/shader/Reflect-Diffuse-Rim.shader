Shader "Custom/Reflective/Diffuse-Rim" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_ReflectColor ("Reflection Color", Color) = (1,1,1,0.5)
	_MainTex ("Base (RGB) RefStrength (A)", 2D) = "white" {} 
    _RimColor ("Rim Color", Color) = (0.26,0.19,0.16,0.0)
    _RimPower ("Rim Power", Range(0.5,8.0)) = 3.0
	_Cube ("Reflection Cubemap", Cube) = "_Skybox" { TexGen CubeReflect }
}
SubShader {
	LOD 200
	Tags { "RenderType"="Opaque" }
	
CGPROGRAM
#pragma surface surf Lambert

sampler2D _MainTex;
samplerCUBE _Cube;
float4 _RimColor;
float _RimPower;
fixed4 _Color;
fixed4 _ReflectColor;

struct Input {
	float2 uv_MainTex;
	float3 worldRefl;
    float3 viewDir;
};

void surf (Input IN, inout SurfaceOutput o) {
	fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
	fixed4 c = tex * _Color;
	o.Albedo = c.rgb;
	
	fixed4 reflcol = texCUBE (_Cube, IN.worldRefl);
	reflcol *= tex.a;
	o.Alpha = reflcol.a * _ReflectColor.a;

	//o.Normal = UnpackNormal (tex2D (_BumpMap, IN.uv_BumpMap));
    half rim = 1.0 - saturate(dot (normalize(IN.viewDir), o.Normal));
    o.Emission = reflcol.rgb * _ReflectColor.rgb * 0.6 + _RimColor.rgb * pow (rim, _RimPower) *0.4;
}
ENDCG
}
	
FallBack "Reflective/VertexLit"
} 

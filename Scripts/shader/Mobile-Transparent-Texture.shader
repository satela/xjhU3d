Shader "Custom/Mobile/Transparent/Texture" {
	Properties {
	    _MainTex ("Texture (A = Transparency)", 2D) = ""
	}
	 
	SubShader {
	    Tags {Queue = Transparent}
	    ZWrite Off
	    Lighting Off
	    Blend SrcAlpha OneMinusSrcAlpha
	    Pass {SetTexture[_MainTex]}
	}
}

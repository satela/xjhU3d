Shader "Custom/Line" {
	Properties {
		_Color ("Main Color", Color) = (1,1,1,1)
	}

	SubShader {
		Tags { "Queue"="Overlay" }

		Pass{
				Color [_Color]
				ZTest Always    
			}
	}
}

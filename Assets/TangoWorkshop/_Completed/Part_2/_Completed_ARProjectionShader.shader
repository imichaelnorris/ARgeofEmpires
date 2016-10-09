// original work credited to Deniz Cetinalp: https://github.com/DenizTC/YorkUResearch

Shader "Tango Workshop/_Completed/AR Projection" {
	Properties {
		_MainTex ("Video RGB Texture", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf ShadowOnly fullforwardshadows
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex : TEXCOORD0;
			float4 screenPos;
		};

		inline fixed4 LightingShadowOnly(SurfaceOutput s, half3 lightDir, half atten) {
			fixed4 c;
			c.rgb = s.Albedo * atten * _LightColor0.rgb;
			c.a = s.Alpha;
			return c;
		}

		void surf (Input IN, inout SurfaceOutput o) {
			o.Albedo = tex2D(_MainTex, IN.screenPos.xy / IN.screenPos.w);
			o.Alpha = 1.0f;
		}
		ENDCG
	}
	FallBack "Diffuse"
}

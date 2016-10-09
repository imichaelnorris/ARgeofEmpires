Shader "Custom/NewSurfaceShader" {
	Properties {
		_MainTex ("Video RGB Texture", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf ShadowOnly fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
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
			// Albedo comes from a texture tinted by color
			o.Albedo = tex2D(_MainTex, IN.screenPos.xy / IN.screenPos.w);
			o.Alpha = 1.0f;
		}
		ENDCG
	}
	FallBack "Diffuse"
}

Shader "Custom/SimpleDissolve" {
	 Properties {
      _MainTex ("Texture (RGB)", 2D) = "white" {}
      _NormalMap ("NormalMap", 2D) = "bump" {}
      _NormalIntensity ("Intensity", Range(0.0, 5.0)) = 1.0
      _SliceGuide ("Slice Guide (RGB)", 2D) = "white" {}
      _SliceAmount ("Slice Amount", Range(0.0, 1.0)) = 0.5
      
    }
    SubShader {
      Tags { "RenderType" = "Opaque" }
      Cull Off
      CGPROGRAM
      //if you're not planning on using shadows, remove "addshadow" for better performance
      #pragma surface surf Lambert addshadow
      struct Input {
          float2 uv_MainTex;
          float2 uv_SliceGuide;
          float2 uv_NormalMap;
          float _SliceAmount;
      };
      sampler2D _MainTex;
      sampler2D _SliceGuide;
      sampler2D _NormalMap;
      float _SliceAmount;
      void surf (Input IN, inout SurfaceOutput o) {
          clip(tex2D (_SliceGuide, IN.uv_SliceGuide).rgb - _SliceAmount);
          o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb;
          o.Normal = UnpackNormal(tex2D (_NormalMap, IN.uv_NormalMap));
      }
      ENDCG
    } 
    Fallback "Diffuse"
}

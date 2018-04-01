Shader "Custom/DissolveRimLight" {
Properties {

    

      _Color ("Main Color", Color) = (1,1,1,1)

      _MainTex ("Texture (RGB)", 2D) = "white" {}

      _MainBumpMap ("Normalmap", 2D) = "bump" {}

      _InnerTex("Inner Texture (RGB)", 2D) = "white" {}

      _InnerBumpMap ("Normalmap", 2D) = "bump" {}

      _SliceGuide ("Slice Guide (RGB)", 2D) = "white" {}

      _SliceAmount ("Slice Amount", Range(0.0, 1.0)) = 0.5

 

 

    }

 

    SubShader {

 

   Tags {"Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent"}

 

      LOD 200

      

      

      CGPROGRAM

#pragma surface surf Lambert alpha

 

sampler2D _InnerTex;

sampler2D _InnerBumpMap;

fixed4 _Color;

 

struct Input {

    float2 uv_InnerTex;

    float2 uv_InnerBumpMap;

};

 

void surf (Input IN, inout SurfaceOutput o) {

    fixed4 c = tex2D(_InnerTex, IN.uv_InnerTex) * _Color;

    o.Albedo = c.rgba;

    o.Alpha = c.a;

    o.Normal = UnpackNormal(tex2D(_InnerBumpMap, IN.uv_InnerBumpMap));

}

ENDCG

 

      CGPROGRAM

      #pragma surface surf Lambert alpha

      

      

      struct Input {

          float2 uv_MainTex;

          float2 uv_MainBumpMap;

          float2 uv_SliceGuide;

          float _SliceAmount;

 

      };

      sampler2D _MainTex;

      sampler2D _SliceGuide;

      sampler2D _MainBumpMap;

      float _SliceAmount;

      

               

      

      void surf (Input IN, inout SurfaceOutput o) {

   

          clip(tex2D (_SliceGuide, IN.uv_SliceGuide).rgb - _SliceAmount);

          fixed4 c = tex2D(_MainTex, IN.uv_MainTex);

          o.Albedo = c.rgb;

          o.Alpha = c.a;

          o.Normal = UnpackNormal(tex2D(_MainBumpMap, IN.uv_MainBumpMap));

                      

      }

      ENDCG

    }

    Fallback "Diffuse"

 


}

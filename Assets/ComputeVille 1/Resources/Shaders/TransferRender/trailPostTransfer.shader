  Shader "Example/Normal Extrusion" {
    Properties {
      _MainTex ("Texture", 2D) = "white" {}
      _Amount ("Extrusion Amount", Range(-1,1)) = 0.5
    }
    SubShader {
      Tags { "RenderType" = "Opaque" }
      CGPROGRAM
      #pragma surface surf Lambert vertex:vert

      #pragma target 4.5
#include "UnityCG.cginc"
 struct appdata{
            float4 vertex : POSITION;
            float3 normal : NORMAL;
            float4 texcoord : TEXCOORD0;
            float4 texcoord1 : TEXCOORD1;
            float4 texcoord2 : TEXCOORD2;
 
            uint id : SV_VertexID;
            uint inst : SV_InstanceID;
         };
 

 




      #ifdef SHADER_API_METAL
           struct Transfer {
        float3 vertex;
        float3 normal;
        float2 uv;
      };
      StructuredBuffer<Transfer> _transferBuffer;
#endif


      struct Input {
          float2 uv_MainTex;
      };
      float _Amount;


      void vert (inout appdata v) {

     
 

      #ifdef SHADER_API_METAL

                   float3 fPos = _transferBuffer[v.id].vertex;
        float3 fNor = _transferBuffer[v.id].normal;
        float2 fUV = _transferBuffer[v.id].uv;
            v.vertex = mul(UNITY_MATRIX_VP, float4(fPos,1));// float4(v.vertex.xyz,1.0f);
            v.normal = fNor; //float4(normalize(points[id].normal), 1.0f);
            v.texcoord = float4(fUV,1,1);
            #endif
 
         }
 
      sampler2D _MainTex;
      void surf (Input IN, inout SurfaceOutput o) {
          o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb;
      }
      ENDCG
    } 
    Fallback "Diffuse"
  }

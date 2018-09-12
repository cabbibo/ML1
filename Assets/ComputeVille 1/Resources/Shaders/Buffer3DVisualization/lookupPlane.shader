Shader "Buffer3D_VIZ/lookupPlane" {


  Properties {

  }

  SubShader {


    Pass {

      CGPROGRAM


      #pragma target 4.5
      #pragma vertex vert
      #pragma fragment frag

      #include "UnityCG.cginc"
     #include "../Chunks/noise.cginc"
    struct SDF {
  float  dist;
  float3 nor;
  float  ao;
  float  pn;
  float2 debug;
};


      uniform sampler3D _MainTex;


uniform float3 _Dimensions;
uniform float3 _Extents;
uniform float3 _Center;

uniform float4x4 _SDFTransform;


StructuredBuffer<SDF> _volumeBuffer;

      struct VertexIn{
         float4 position  : POSITION;
         float3 normal    : NORMAL;
         float4 texcoord  : TEXCOORD0;
         float4 tangent   : TANGENT;
      };


      struct VertexOut {
          float4 pos    	: POSITION;
          float3 normal 	: NORMAL;
          float4 uv     	: TEXCOORD0;
          float3 ro     	: TEXCOORD1;
          float3 rd     	: TEXCOORD2;
      };




      VertexOut vert(VertexIn v) {

        VertexOut o;

        o.normal = v.normal;

        o.uv = v.texcoord;



        float3 fPos = v.position;
        // Getting the position for actual position
        o.pos = UnityObjectToClipPos(  fPos );



        float3 mPos = mul( unity_ObjectToWorld , float4(fPos,1) ).xyz;

        // The ray origin will be right where the position is of the surface
        o.ro = mPos;//fPos;


        float3 camPos = mul( unity_WorldToObject , float4( _WorldSpaceCameraPos , 1. )).xyz;

        // the ray direction will use the position of the camera in local space, and
        // draw a ray from the camera to the position shooting a ray through that point
        o.rd = normalize(mPos - _WorldSpaceCameraPos);//normalize( fPos.xyz - camPos );

        return o;

      }





      // Fragment Shader
      fixed4 frag(VertexOut v) : COLOR {

				// Ray origin
        float3 ro 			= v.ro;

        // Ray direction
        float3 rd 			= normalize(v.ro - _WorldSpaceCameraPos);

        // Our color starts off at zero,
        float3 col = float3( 0.0 , 0.0 , 0.0 );



        float3 p;
        float depth = 0;
        float dist = 0;

        float hit = 0;

        float3 tmpPos = mul( _SDFTransform ,float4(ro,1));


        float3 fV = (((tmpPos-_Center) / _Extents) + 1)/2 ;
        float4 info = tex3D(_MainTex,  fV );


        //col = float3(sin( id / 100),0,0);
        col = info.yzw* .5 + .5;//*100;

        col = clamp(sin(info.x * 100) / ( info.x * info.x * 30),0,1);

 if( fV.x >= 1 || fV.y >= 1 || fV.z >= 1 || fV.x <= 0 || fV.y <= 0 || fV.z <= 0 ){
          col = float3(0,0,0);
        }

       // if( length(col ) < 0.1){ discard;}
		    fixed4 color;
        color = fixed4( col , 1. );
        return color;
      }

      ENDCG
    }
  }
  FallBack "Diffuse"
}

Shader "Custom/proceduralPostTransfer" {
  Properties {

    _Tex("", 2D) = "white" {}

    _CubeMap( "Cube Map" , Cube )  = "defaulttexture" {}



  }

	SubShader {
		// COLOR PASS
		Pass {
			Tags{ "LightMode" = "ForwardBase" }
			Cull Off

			CGPROGRAM
			#pragma target 4.5
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			#include "AutoLight.cginc"
			#include "../Chunks/hsv.cginc"
			#include "../Chunks/noise.cginc"

			sampler2D _Tex;

            uniform samplerCUBE _CubeMap;
			struct Transfer {
        float3 pos;
        float3 normal;
        float3 tangent;
        float2 uv;
        float debug;
      };

			StructuredBuffer<Transfer> _transferBuffer;
			StructuredBuffer<int> _triBuffer;

			struct varyings {
				float4 pos 		: SV_POSITION;
				float3 nor 		: TEXCOORD0;
				float2 uv  		: TEXCOORD1;
				float3 eye      : TEXCOORD5;
				float3 worldPos : TEXCOORD6;
				float3 debug    : TEXCOORD7;
				float3 closest    : TEXCOORD8;
				UNITY_SHADOW_COORDS(2)
			};

			 varyings vert (uint id : SV_VertexID){

			 	int fID = _triBuffer[id];
				float3 fPos = _transferBuffer[fID].pos;
				float3 fNor = _transferBuffer[fID].normal;
				float2 fUV = _transferBuffer[fID].uv;

				//fPos = fPos;float3( sin(id*10) , fPos.y, sin( id * 3));
				varyings o;

				o.pos = mul(UNITY_MATRIX_VP, float4(fPos,1));
				o.worldPos = fPos;
				o.eye = _WorldSpaceCameraPos - fPos;
				o.nor = fNor;
				o.uv =  fUV;
				float fb = dot( UNITY_MATRIX_V[2].xyz , fNor );
				o.debug = float3(fb,1,0);


				return o;
			}

			float4 frag(varyings v) : COLOR {

				float3 col;
				float3 cCol =  texCUBE( _CubeMap , v.nor );
				col = 4*hsv(length(cCol)*.3+v.uv.x+_Time.y*.3,.8,1) * cCol;


				if( v.debug.x < 0 ){ col = float3(1,1,1);}
				col = float3(1,1,1);
				//if( sin( v.uv.x * 300  +3* noise(v.worldPos*100)) < -.7 ){ discard;}


				//col = v.uv.x;//float3( 1,1,0);
				return float4( col , 1.);
			}

			ENDCG
		}
	}

}

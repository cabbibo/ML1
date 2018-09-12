Shader "Custom/nonSurface" {
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
			#pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight

			#include "UnityCG.cginc"
			#include "AutoLight.cginc"
			#include "../Chunks/hsv.cginc"

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

			varyings vert(uint id : SV_VertexID) {

				float3 fPos = _transferBuffer[id].pos;
				float3 fNor = _transferBuffer[id].normal;
				float2 fUV = _transferBuffer[id].uv;



				//fPos = fPos;float3( sin(id*10) , fPos.y, sin( id * 3));
				varyings o;

				UNITY_INITIALIZE_OUTPUT(varyings, o);

				o.pos = mul(UNITY_MATRIX_VP, float4(fPos,1));
				o.worldPos = fPos;
				o.eye = _WorldSpaceCameraPos - fPos;
				o.nor = fNor;
				o.uv =  fUV;
				float fb = dot( UNITY_MATRIX_V[2].xyz , fNor );
				o.debug = float3(fb,1,0);

				UNITY_TRANSFER_SHADOW(o,o.worldPos);

				return o;
			}

			float4 frag(varyings v) : COLOR {

				float3 col;
				float3 cCol =  texCUBE( _CubeMap , v.nor );
				col = hsv(length(cCol)*.3+.5,1,1) * cCol;


				if( v.debug.x < 0 ){ col = 1-5*col;}
				if( sin( v.uv.x * 300 ) < -.5 ){ discard;}

				fixed shadow = UNITY_SHADOW_ATTENUATION(v,v.worldPos) * .9 + .1 ;

			//	float3 col = float3( 1,1,0);
				return float4( col , 1.);
			}

			ENDCG
		}

		// SHADOW PASS

		Pass
		{
			Tags{ "LightMode" = "ShadowCaster" }

			Fog{ Mode Off }
			ZWrite On
			ZTest LEqual
			Cull Back
			Offset 1, 1

			CGPROGRAM
			#pragma target 4.5
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_shadowcaster
			#pragma fragmentoption ARB_precision_hint_fastest
			#include "UnityCG.cginc"

			struct Transfer {
			  float3 vertex;
			  float3 normal;
			  float2 uv;
			};

			struct v2f {
				V2F_SHADOW_CASTER;
			};

			StructuredBuffer<Transfer> _transferBuffer;

			v2f vert(appdata_base v, uint id : SV_VertexID)
			{
				v2f o;
				o.pos = mul(UNITY_MATRIX_VP, float4(_transferBuffer[id].vertex, 1));
				return o;
			}

			float4 frag(v2f i) : COLOR
			{
				SHADOW_CASTER_FRAGMENT(i)
			}
			ENDCG
		}
	}
}

Shader "Procedural/TrailParticleMat" {
	Properties {
		 _Color( "Color", Color ) = (1,1,1,1)
		}


  SubShader{
//        Tags { "RenderType"="Transparent" "Queue" = "Transparent" }
    Cull Off
    Pass{

      //Blend SrcAlpha OneMinusSrcAlpha // Alpha blending

      CGPROGRAM
      #pragma target 4.5

      #pragma vertex vert
      #pragma fragment frag

      #include "UnityCG.cginc"
      #include "../Chunks/noise.cginc"
      #include "../Chunks/hsv.cginc"

		  uniform int _Count;
      uniform float3 _Color;

struct Vert{
  float3 pos;
  float id;
};



      StructuredBuffer<Vert> _vertBuffer;


      //uniform float4x4 worldMat;

      //A simple input struct for our pixel shader step containing a position.
      struct varyings {
          float4 pos      : SV_POSITION;
          float3 nor      : TEXCOORD0;
          float3 worldPos : TEXCOORD1;
          float3 eye      : TEXCOORD2;
          float3 debug    : TEXCOORD3;
          float2 uv       : TEXCOORD4;
          float  noiseVal : TEXCOORD5;
      };


      //Our vertex function simply fetches a point from the buffer corresponding to the vertex index
      //which we transform with the view-projection matrix before passing to the pixel program.
      varyings vert (uint id : SV_VertexID){

        varyings o;

        int base = id / 6;
        int alternate = id %6;
        if( base < _Count ){

        	float3 extra = float3(0,0,0);

          float3 l = UNITY_MATRIX_V[0].xyz;
          float3 u = UNITY_MATRIX_V[1].xyz;
          float2 uv = float2(0,0);
          if( alternate == 0 ){ extra = -l - u; uv = float2(0,0); }
          if( alternate == 1 ){ extra =  l - u; uv = float2(1,0); }
          if( alternate == 2 ){ extra =  l + u; uv = float2(1,1); }
          if( alternate == 3 ){ extra = -l - u; uv = float2(0,0); }
          if( alternate == 4 ){ extra =  l + u; uv = float2(1,1); }
          if( alternate == 5 ){ extra = -l + u; uv = float2(0,1); }


        	Vert v = _vertBuffer[base];

       		o.worldPos = v.pos + extra * .01;
	        o.eye = _WorldSpaceCameraPos - o.worldPos;

          o.uv = uv;

	        o.pos = mul (UNITY_MATRIX_VP, float4(o.worldPos,1.0f));

       	}

        return o;

      }




      //Pixel function returns a solid color for each point.
      float4 frag (varyings v) : COLOR {
      		float3 col = _Color;//normalize(v.nor) * .5 + .5;

          if( length(v.uv-float2(.5,.5)) > .5 ){
            discard;
          }
          return float4( col, 1 );

      }

      ENDCG

    }
  }

  Fallback Off


}

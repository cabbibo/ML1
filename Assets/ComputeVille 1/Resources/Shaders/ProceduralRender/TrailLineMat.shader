Shader "Procedural/TrailLineMat" {
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
      uniform int _VertsPerParticle;

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
        int pID = base / (_VertsPerParticle-1);
        int bID = base % (_VertsPerParticle-1);


        int alternate = id%6;

        if( pID < _Count ){

        	float3 extra = float3(0,0,0);

           int baseID = pID * _VertsPerParticle;

          float3 l = UNITY_MATRIX_V[0].xyz;
          float3 u = UNITY_MATRIX_V[1].xyz;

          Vert vU = _vertBuffer[ baseID + bID + 0 ];
          Vert vD = _vertBuffer[ baseID + bID + 1 ];

          float3 dir = vU.pos - vD.pos;
          float3 up = u;

          float size = .004;

          float3 p1 = vU.pos - up * size;
          float3 p2 = vD.pos - up * size;
          float3 p3 = vU.pos + up * size;
          float3 p4 = vD.pos + up * size;



          float3 pos = float3( 0,0,0);
          float2 uv = float2(0,0);

          if( alternate == 0 ){ pos = p1; uv = float2(0,0); }
          if( alternate == 1 ){ pos = p2; uv = float2(1,0); }
          if( alternate == 2 ){ pos = p4; uv = float2(1,1); }
          if( alternate == 3 ){ pos = p1; uv = float2(0,0); }
          if( alternate == 4 ){ pos = p4; uv = float2(1,1); }
          if( alternate == 5 ){ pos = p3; uv = float2(0,1); }



       		o.worldPos = pos;
	        o.eye = _WorldSpaceCameraPos - o.worldPos;

          	o.uv = uv;

	        o.pos = mul (UNITY_MATRIX_VP, float4(o.worldPos,1.0f));

       	}

        return o;

      }




      //Pixel function returns a solid color for each point.
      float4 frag (varyings v) : COLOR {
      		float3 col = _Color;//normalize(v.nor) * .5 + .5;
          return float4( col, 1 );

      }

      ENDCG

    }
  }

  Fallback Off


}

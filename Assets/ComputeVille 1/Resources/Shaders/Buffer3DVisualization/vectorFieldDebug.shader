Shader "Custom/vectorFieldDebug" {
	Properties {
        _NormalMap( "Normal Map" , 2D ) = "white" {}
        _CubeMap( "Cube Map" , Cube ) = "white" {}
        _TexMap( "Tex Map" , 2D ) = "white" {}
        _SizeMultiplier( "Size Multiplier" , float ) = 1
    }
  SubShader{




    Cull off
    Pass{


      CGPROGRAM
      #pragma target 4.5

      #pragma vertex vert
      #pragma fragment frag

      #include "UnityCG.cginc"

struct F{
  float len;
  float3 nor;
};

uniform float3 _Center;
uniform float3 _Extents;

			uniform int _VolDim;
      RWStructuredBuffer<F>  _buffer;

      //A simple input struct for our pixel shader step containing a position.
      struct varyings {
          float4 pos 			: SV_POSITION;
          float3 nor 			: TEXCOORD0;
          float2 uv  			: TEXCOORD1;
          float3 eye      : TEXCOORD5;
          //float3 worldPos : TEXCOORD6;
          float3 debug    : TEXCOORD7;

      };


      varyings vert (uint id : SV_VertexID){

        varyings o;


        int pID = floor( (float(id) / 2));

        int inOut = id %2;



       //	if( particleID < _RibbonsIn ){


				if( pID < _VolDim * _VolDim * _VolDim ){

					int xID = pID % _VolDim;
    			int yID = (pID / (_VolDim)) % _VolDim;
    			int zID = pID / (_VolDim * _VolDim);
			
    			float x = (float(xID)+.5) / float(_VolDim);
    			float y = (float(yID)+.5) / float(_VolDim);
    			float z = (float(zID)+.5) / float(_VolDim);



				   F v1  = _buffer[pID];

				   float3 fPos = ((float3(x,y,z)-float3(.5,.5,.5)) * 2 *_Extents);

				   if( inOut == 1){

            float l = v1.len;
            if( v1.len < 0 ){
              l = 1;
            }else{
               l = .1/ ( .1 + v1.len * 20);
            }
				   	fPos += .1 * v1.nor * l;
				   }

           fPos += _Center;

				   o.debug = (normalize(v1.nor)*.5 + .5);

				   o.uv = float2(0,0);
				   o.nor = v1.nor;
				   o.eye = v1.nor;


						o.pos = mul (UNITY_MATRIX_VP, float4(fPos,1.0f));

				}


        return o;


      }
      //Pixel function returns a solid color for each point.
      float4 frag (varyings v) : COLOR {

        float3 col = v.debug;// v.nor * .5 + .5;//float3( .5,.3,.1);
        return float4( col , 1. );

      }

      ENDCG

    }
  }

  Fallback Off

}
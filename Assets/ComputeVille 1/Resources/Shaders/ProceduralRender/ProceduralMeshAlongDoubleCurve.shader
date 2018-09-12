Shader "ProceduralRender/MeshAlongDoubleCurve" {
	Properties {
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

		  uniform int _CurveCount;
		  uniform int _VertCount;
      uniform float3 _Color;
      uniform float _ModelLength;

struct Vert{
  float3 pos;
  float3 nor;
  float2 uv;
};


struct Curve{
  float3 pos;
  float3 oPos;
  float3 nor;
  float idDown;
  float idUp;
  float2 uv;
  float3 debug;
};



      StructuredBuffer<Vert> _vertBuffer;
      StructuredBuffer<Curve> _curveBuffer1;
      StructuredBuffer<Curve> _curveBuffer2;
      StructuredBuffer<int> _triBuffer;


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



float3 cubicCurve( float t , float3  c0 , float3 c1 , float3 c2 , float3 c3 ){
  
  float s  = 1. - t; 

  float3 v1 = c0 * ( s * s * s );
  float3 v2 = 3. * c1 * ( s * s ) * t;
  float3 v3 = 3. * c2 * s * ( t * t );
  float3 v4 = c3 * ( t * t * t );

  float3 value = v1 + v2 + v3 + v4;

  return value;

}


float3 cubicFromValue1( in float val , out float3 upPos , out float3 doPos ){

  //float3 upPos;
  //float3 doPos;


  float3 p0 = float3( 0. , 0. , 0. );
  float3 v0 = float3( 0. , 0. , 0. );
  float3 p1 = float3( 0. , 0. , 0. );
  float3 v1 = float3( 0. , 0. , 0. );

  float3 p2 = float3( 0. , 0. , 0. );


  float base = val * (float(_CurveCount)-1.);
  float baseUp   = floor( base );
  float baseDown = ceil( base );
  float amount = base - baseUp;


  if( baseUp == 0. ){

    p0 = _curveBuffer1[ int( baseUp )         ].pos;
    p1 = _curveBuffer1[ int( baseDown )       ].pos;
    p2 = _curveBuffer1[ int( baseDown + 1. )  ].pos;


    v1 = .5 * ( p2 - p0 );

  }else if( baseDown == float(_CurveCount-1) ){

    p0 = _curveBuffer1[ int( baseUp )       ].pos;
    p1 = _curveBuffer1[ int( baseDown )     ].pos;
    p2 = _curveBuffer1[ int( baseUp - 1. )  ].pos;

    v0 = .5 * ( p1 - p2 );

  }else{

    p0 = _curveBuffer1[ int( baseUp )    ].pos;
    p1 = _curveBuffer1[ int( baseDown )  ].pos;


    float3 pMinus;

    pMinus = _curveBuffer1[ int( baseUp - 1. )    ].pos;
    p2 =     _curveBuffer1[ int( baseDown + 1. )  ].pos;

    v1 = .5 * ( p2 - p0 );
    v0 = .5 * ( p1 - pMinus );

  }


  float3 c0 = p0;
  float3 c1 = p0 + v0/3.;
  float3 c2 = p1 - v1/3.;
  float3 c3 = p1;

  float3 pos = cubicCurve( amount , c0 , c1 , c2 , c3 );

  upPos = cubicCurve( amount  + .01 , c0 , c1 , c2 , c3 );
  doPos = cubicCurve( amount  - .01 , c0 , c1 , c2 , c3 );

  return pos;


}



float3 cubicFromValue2( in float val , out float3 upPos , out float3 doPos ){

  //float3 upPos;
  //float3 doPos;


  float3 p0 = float3( 0. , 0. , 0. );
  float3 v0 = float3( 0. , 0. , 0. );
  float3 p1 = float3( 0. , 0. , 0. );
  float3 v1 = float3( 0. , 0. , 0. );

  float3 p2 = float3( 0. , 0. , 0. );


  float base = val * (float(_CurveCount)-1.);
  float baseUp   = floor( base );
  float baseDown = ceil( base );
  float amount = base - baseUp;


  if( baseUp == 0. ){

    p0 = _curveBuffer2[ int( baseUp )         ].pos;
    p1 = _curveBuffer2[ int( baseDown )       ].pos;
    p2 = _curveBuffer2[ int( baseDown + 1. )  ].pos;


    v1 = .5 * ( p2 - p0 );

  }else if( baseDown == float(_CurveCount-1) ){

    p0 = _curveBuffer2[ int( baseUp )       ].pos;
    p1 = _curveBuffer2[ int( baseDown )     ].pos;
    p2 = _curveBuffer2[ int( baseUp - 1. )  ].pos;

    v0 = .5 * ( p1 - p2 );

  }else{

    p0 = _curveBuffer2[ int( baseUp )    ].pos;
    p1 = _curveBuffer2[ int( baseDown )  ].pos;


    float3 pMinus;

    pMinus = _curveBuffer2[ int( baseUp - 1. )    ].pos;
    p2 =     _curveBuffer2[ int( baseDown + 1. )  ].pos;

    v1 = .5 * ( p2 - p0 );
    v0 = .5 * ( p1 - pMinus );

  }


  float3 c0 = p0;
  float3 c1 = p0 + v0/3.;
  float3 c2 = p1 - v1/3.;
  float3 c3 = p1;

  float3 pos = cubicCurve( amount , c0 , c1 , c2 , c3 );

  upPos = cubicCurve( amount  + .01 , c0 , c1 , c2 , c3 );
  doPos = cubicCurve( amount  - .01 , c0 , c1 , c2 , c3 );

  return pos;


}


      //Our vertex function simply fetches a point from the buffer corresponding to the vertex index
      //which we transform with the view-projection matrix before passing to the pixel program.
      varyings vert (uint id : SV_VertexID){

        varyings o;

        int triID = _triBuffer[id];

        if( triID < _VertCount ){



        	Vert v = _vertBuffer[triID];

        	float valAlong = clamp(v.pos.z / _ModelLength,0,1);

        	float3 up1; float3 down1;
        	float3 up2; float3 down2;
        	float3 extraPos1 = cubicFromValue1(valAlong,up1,down1);
        	float3 extraPos2 = cubicFromValue2(valAlong,up2,down2);
        	
        	float x = v.pos.x;
        	float y = v.pos.y;

        	float3 zDir = normalize(up1-extraPos1);

        	float3 xDir = normalize(extraPos1-extraPos2);//cross(upDir,downDir));


        	
       
        	//if(dot(upDir,xDir)*1000<=0&&dot(downDir,xDir)*1000>0){xDir *= -1;}
        	//if(dot(upDir,xDir)*1000>0&&dot(downDir,xDir)*1000<=0){xDir *= -1;}
        	//if(dot(downDir,xDir)*1000<0){xDir *= -1;}

	
      

        	//float3 zDir =normalize(upDir);// normalize( up - extraPos);

        //	xDir = normalize(cross( 10000*zDir , 1000*float3(1,0,0)));

        	float3 yDir = normalize(cross( xDir , zDir ));

        	float3 c1 = float3( xDir.x , yDir.x , zDir.x );
        	float3 c2 = float3( xDir.y , yDir.y , zDir.y );
        	float3 c3 = float3( xDir.z , yDir.z , zDir.z );
        	float3x3 rotMat = float3x3(c1,c2,c3);

        //	xDir =  normalize(cross(zDir,xDir));
        //	if( dot(1000* yDir , 1000*upDir) < 0 ){ yDir *= -1;}
        
  	//
  				//if( d1 != d2 ){ zDir *= -1;}
  				//if( d1 != d2 ){ yDir *= -1;}
  				//if( d1 != d2 ){ xDir *= -1;}
  				//if( d1 == d2 ){ xDir *= -1;}

        //	if(dot(upDir,xDir)*1000>0&&dot(downDir,xDir)*1000>0){xDir *= -1;}
        //	if(dot(upDir,xDir)*1000<=0&&dot(downDir,xDir)*1000<=0){xDir *= -1;}

        	//if( dot( xDir , float3(1,0,0)) < 0 ){ xDir *= -1;}
        	//if( dot( zDir , float3(0,0,1)) < 0 ){ zDir *= -1;}
          //if( dot( yDir , float3(0,1,0)) < 0 ){ yDir *= -1;}



					float3 fPos = extraPos1  +mul(rotMat, float3(v.pos.x,v.pos.y,0));//
        	//float3 fPos = extraPos1  + x  * xDir + y * yDir;


       		o.worldPos = fPos;///* .001/(.1+length(v.debug));//*(1/(.1+max(length(v.debug),0)));//mul( worldMat , float4( v.pos , 1.) ).xyz;
	       // o.debug =float3(float(id)/1000000,1,0);
	        o.eye = _WorldSpaceCameraPos - o.worldPos;
          o.nor =normalize( mul(rotMat,v.nor));//fPos;
          o.uv = v.uv;

	        o.pos = mul (UNITY_MATRIX_VP, float4(o.worldPos,1.0f));

       	}

        return o;

      }




      //Pixel function returns a solid color for each point.
      float4 frag (varyings v) : COLOR {
      		float3 col = normalize(v.nor) * .5 + .5;


          col =  col;//float3(v.uv.x,0,0);//sin(v.uv.x*100) + sin(v.uv.y*100);
          return float4( col, 1 );

      }

      ENDCG

    }
  }

  Fallback Off


}

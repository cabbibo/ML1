Shader "Custom/fromLookPoint3" {
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			float3 _LookPoint;
      float _MainTime;

			struct appdata
			{
				float4 vertex : POSITION;
				float4 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
				float3 nor : TEXCOORD1;
				float3 pos : TEXCOORD2;
				float3 worldPos : TEXCOORD3;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.nor = v.normal;
				o.pos = v.vertex.xyz;
				o.worldPos = mul( unity_ObjectToWorld , v.vertex ).xyz;
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}


#ifndef __noise_hlsl_
#define __noise_hlsl_
 
// hash based 3d value noise
// function taken from [url]https://www.shadertoy.com/view/XslGRr[/url]
// Created by inigo quilez - iq/2013
// License Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License.
 
// ported from GLSL to HLSL
 
float hash( float n )
{
    return frac(sin(n)*43758.5453);
}
 
float noise( float3 x )
{
    // The noise function returns a value in the range -1.0f -> 1.0f
 
    float3 p = floor(x);
    float3 f = frac(x);
 
    f       = f*f*(3.0-2.0*f);
    float n = p.x + p.y*57.0 + 113.0*p.z;
 
    return lerp(lerp(lerp( hash(n+0.0), hash(n+1.0),f.x),
                   lerp( hash(n+57.0), hash(n+58.0),f.x),f.y),
               lerp(lerp( hash(n+113.0), hash(n+114.0),f.x),
                   lerp( hash(n+170.0), hash(n+171.0),f.x),f.y),f.z);
}
 
#endif 


			float3 hsv(float h, float s, float v)
{
  return lerp( float3( 1.0 , 1, 1 ) , clamp( ( abs( frac(
    h + float3( 3.0, 2.0, 1.0 ) / 3.0 ) * 6.0 - 3.0 ) - 1.0 ), 0.0, 1.0 ), s ) * v;
}

float3 bandColor( float3 pos , float3 cCol ,  float3 bandCol, float bandSize ,float band ){

	return lerp( cCol , bandCol , clamp( (bandSize-(.2*noise(pos*10))-band) / bandSize,0,1));

}
			fixed4 frag (v2f v) : SV_Target
			{


				            // Blending factor of triplanar mapping
            float3 bf = normalize(abs(v.nor));
            bf /= dot(bf, (float3)1);

            float3 dif = v.worldPos - _LookPoint;
            float d = length( v.worldPos - _LookPoint);
                    // Triplanar mapping
            float2 tx = v.pos.yz;//* sin(d*4);
            float2 ty = v.pos.zx;//* sin(d*4);
            float2 tz = v.pos.xy;//* sin(d*4);

            // Base color
            half4 cx = tex2D(_MainTex, tx) * bf.x;
            half4 cy = tex2D(_MainTex, ty) * bf.y;
            half4 cz = tex2D(_MainTex, tz) * bf.z;
            half4 color = (cx + cy + cz);

            float3 col =hsv( sin(length(color.xyz)*10 + _Time.y) ,1,sin(length(color.xyz)*10 +_Time.y *.8));



            //col *= sin(20*d+ noise(v.worldPos*100)-_Time.y);//float3(1,1,1) * .1 / d;
            col = .1 * clamp(col, float3(0,0,0),float3(1,1,1)) / d;

          
          //) * clamp(1-.01*( _Time.y-_Collision1.w ) /d,0,1)/(200*d*d) - .2*( _Time.y-_Collision1.w );

				// sample the texture
			//x	fixed4 col = color;//float4( sin( 10000 * i.uv.x )  ,sin( 100 * i.uv.y ),0,1) +  1*float4(i.nor * .5 + .5);// * tex2D(_MainTex, i.uv);
				// apply fog
				//UNITY_APPLY_FOG(i.fogCoord, col);
				return fixed4(col,1);
			}
			ENDCG
		}
	}
}


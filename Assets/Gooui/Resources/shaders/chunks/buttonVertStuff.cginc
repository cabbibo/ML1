    void vert (inout appdata_full v) {

      //o.worldPos =  mul( unity_ObjectToWorld , v.vertex );
      float3 dL = mul( unity_WorldToObject , float4( _Entered ,1 )).xyz;

      float lL = length( dL.xz - v.vertex.xz );

      float zL = abs( dL.y - v.vertex.y );

      float fL =lL;
      v.color = float4( fL + zL * .1 , 0, 1 , 1);
//      o.closest = fL + min( zL , zR ) * .1;

      float4 fPos = v.vertex;

      fPos.y = lerp( (1-_TriggerVal) * fPos.y , fPos.y , min(pow(fL,.5),1));

      float3 up = v.vertex + float4( 0.003 , 0 , 0 , 0 );
      float3 down = v.vertex + float4( 0 , 0 , 0.003 , 0 );


      lL = length( dL.xz - up.xz );

      fL = lL ;

      up.y = lerp( (1-_TriggerVal) * up.y , up.y , min(pow(fL,.5),1));

      lL = length( dL.xz - down.xz );
      fL = lL;


      down.y = lerp( (1-_TriggerVal) * down.y , down.y , min(pow(fL,.5),1));


      float3 d = normalize(fPos.xyz - down);
      float3 u = normalize(fPos.xyz - up);

      float3 n = normalize(cross( u  , d ));

      v.vertex.xyz = fPos;
      v.normal = -n;
      
    }
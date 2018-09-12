using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ComputeVille{
public class RopeVertBuffer : VerletVertBuffer {

  public override void SetOriginalValues(){

    int index = 20;
    for( int i = 0; i < count; i++ ){

      // positions
      values[ index++ ] = 0;
      values[ index++ ] = 0;
      values[ index++ ] = 0;

      // positions
      values[ index++ ] = 0;
      values[ index++ ] = 0;
      values[ index++ ] = 0;

      // normals
      values[ index++ ] = 1;
      values[ index++ ] = 0;
      values[ index++ ] = 0;

      // normals
      values[ index++ ] = 0;
      values[ index++ ] = 1;
      values[ index++ ] = 0;

      // normals
      values[ index++ ] = 0;
      values[ index++ ] = 0;
      values[ index++ ] = 1;
      

      int idDown = i-1;
      if( i == 0 ){ idDown = -100; }
      // ID Down
      values[ index++ ] = idDown+1;

      int idUp = i+1;
      if( i >= count-2 ){ idUp = -100; }
      // ID Up
      values[ index++ ] = idUp+1;

      // uvs
      values[ index++ ] = ((float)i+1)/((float)count);
      values[ index++ ] = i+1;

      // debug
      values[ index++ ] = 0;
   
    }

    SetData();

  }

}
}

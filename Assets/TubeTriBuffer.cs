using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ComputeVille{
public class TubeTriBuffer :  TransferTriBuffer {

  public int width;
  public int length;

  public override void SetCount(){ count = width * (length-1) * 3 * 2; }

  // This function you will override to assign which triangles point to which verts
  public override void MakeMesh(){

    int index = 0;


    for( int j = 0; j < length-1; j++){
      for(int i = 0; i < width; i++){
        values[index++] = j * width + i;
        values[index++] = j * width + (i+1)%width;
        values[index++] = (j+1) * width + (i+1)%width;
        values[index++] = j * width + i;
        values[index++] = (j+1) * width + (i+1)%width;
        values[index++] = (j+1) * width + i;
      }
    }


  }


}
}

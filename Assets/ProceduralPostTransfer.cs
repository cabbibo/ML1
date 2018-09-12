using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ComputeVille;

public class ProceduralPostTransfer :  ProceduralRender{

  public TubeVertBuffer transferBuffer;
  public TubeTriBuffer triBuffer;
	 public override void Render(){
    material.SetPass(0);
    material.SetBuffer("_transferBuffer", transferBuffer._buffer);
    material.SetBuffer("_triBuffer", triBuffer._buffer);
    material.SetInt("_Count",buffer.count);
    Graphics.DrawProcedural(MeshTopology.Triangles, triBuffer.count );
  }
	
}

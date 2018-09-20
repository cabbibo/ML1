using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace ComputeVille{
[RequireComponent(typeof(RopeVertBuffer))]
public class RopePhysics: VerletPhysics{

  public float ropeLength;
  public GameObject startPoint;
  public GameObject endPoint;
  public Vector3 sp;
  public Vector3 ep;
  public override void GetBuffer(){ buffer = GetComponent<RopeVertBuffer>(); }
  public override void SetShaderValues(){

    shader.SetVector("_EndPosition", startPoint.transform.position );
    shader.SetVector("_StartPosition", endPoint.transform.position );
    shader.SetFloat("_Length" , ropeLength );
    shader.SetFloat("_Count" , buffer.count );
    shader.SetFloat("_SpringDistance", ropeLength / (float)buffer.count );

  }

}
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ComputeVille{
[RequireComponent(typeof(TransferVertBuffer))]
[RequireComponent(typeof(TransferTriBuffer))]
[RequireComponent(typeof(RopeVertBuffer))]
public class CubicTubeAlongRope : MeshFromTransferBuffers {

  public int tubeWidth;
  public int tubeLength;
  public RopeVertBuffer vBuffer;


 public override void GetBuffer(){
    if( buffer == null ){ buffer = GetComponent<TransferVertBuffer>(); }
    if( triBuffer == null ){ triBuffer = GetComponent<TransferTriBuffer>(); }
    if( particleBuffer == null ){ particleBuffer = GetComponent<RopeVertBuffer>(); }
    vBuffer = (RopeVertBuffer)particleBuffer;

   // material = new Material(material);
   // material.SetBuffer("_transferBuffer", buffer._buffer);
  }

  public override void Dispatch(){
    shader.SetInt("_Count", buffer.count );
    shader.SetVector("_CameraRight", Camera.main.gameObject.transform.right );
    shader.SetVector("_CameraUp", Camera.main.gameObject.transform.up );
    shader.SetInt("_TubeWidth", tubeWidth );
    shader.SetInt("_TubeLength", tubeLength );
    shader.SetInt("_RopeVerts", vBuffer.count);
    shader.SetBuffer(kernel, "transferBuffer" , buffer._buffer );
    shader.SetBuffer(kernel, "vertBuffer" , vBuffer._buffer );
    shader.Dispatch(kernel,numGroups,1,1 );
    //mr.material.SetBuffer("_transferBuffer", buffer._buffer);
  }



}}

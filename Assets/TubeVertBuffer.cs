using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ComputeVille{
public class TubeVertBuffer :  TransferVertBuffer {

  public int width;
  public int length;


  struct Vert{
    public Vector3 pos;
    public Vector3 nor;
    public Vector3 tan;
    public Vector2 uv;
    public float debug;
  }

  public override void SetCount(){ count = width * length; }
  public override void SetStructSize(){ structSize = 12; }


}
}

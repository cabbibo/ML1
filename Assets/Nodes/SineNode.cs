using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineNode : Node {

  public override void UpdateSignal(){
    output = Mathf.Sin( input );
  }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplyNode : Node {

  public float multiplier;

	// Update is called once per frame
	public override void UpdateSignal() {
		output = input * multiplier;
	}

}

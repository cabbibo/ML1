using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeNode : Node {
	
	// Update is called once per frame
	public override void UpdateSignal () {
		  output = Time.time;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {

  public float input;
  public float output;
  
	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void Update () {
		UpdateSignal();
	}

  public virtual void UpdateSignal(){
  
  }


}

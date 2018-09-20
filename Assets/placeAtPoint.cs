using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class placeAtPoint : MonoBehaviour {
  public InterfaceMaker placer;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = placer.point;
	}
}

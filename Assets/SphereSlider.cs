using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereSlider : MonoBehaviour {

  public GameObject knob;

  public GameObject start;
  public GameObject end;
  public float value;
  public Vector3 dif;
  public bool sliding;



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

      dif = end.transform.position-start.transform.position ;
      knob.transform.position = start.transform.position + dif * value;
		
	}
}

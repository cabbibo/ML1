using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;

public class PlaceSlider : MonoBehaviour {

  public GameObject SliderPrefab;

public string pose; 
public string oPose;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
// Finger
    // L
  oPose = pose;
  pose = MLHands.Right.KeyPose.ToString();

  if( oPose != pose ){
    if( pose == "Fist"){
      GameObject go = Instantiate( SliderPrefab );
      go.transform.position = MLHands.Right.Center;
    }

  }

	}
}

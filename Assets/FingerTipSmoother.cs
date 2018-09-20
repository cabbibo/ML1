using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;

public class FingerTipSmoother : MonoBehaviour {

  public Transform tipObject;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
      // Index

    tipObject.position = Vector3.Lerp(  tipObject.position , MLHands.Right.Index.KeyPoints[0].Position  , .1f );
           
		
	}
}

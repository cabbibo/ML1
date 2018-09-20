using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderConnector : MonoBehaviour {

  public SphereSlider slider;
  public GameObject lineConnector;
  public float value;
  public float minVal;
  public float maxVal;

	// Use this for initialization
	void Start () {

    value = minVal;
		
	}
	
	// Update is called once per frame
	void Update () {

    if( slider != null ){
      value = Mathf.Lerp( minVal , maxVal , slider.value);
    }
		
	}
}

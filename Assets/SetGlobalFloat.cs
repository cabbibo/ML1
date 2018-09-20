using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetGlobalFloat : MonoBehaviour {

  public string name;

  public PressButton pb;
	// Use this for initialization
	void Start () {
    pb = GetComponent<PressButton>();
		
	}
	
	// Update is called once per frame
	void Update () {
    Shader.SetGlobalFloat(name, pb.SliderValue);
	}
  
}

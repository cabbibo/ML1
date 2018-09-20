using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderConnectors : MonoBehaviour {
  public SliderConnector HueConnector;
  public SliderConnector DepthConnector;
  public SliderConnector CutoffConnector;

private MeshRenderer mr;
	// Use this for initialization
	void Start () {
    mr= GetComponent<MeshRenderer>();
		
	}
	
	// Update is called once per frame
	void Update () {

    mr.material.SetFloat("_HueSize", HueConnector.value );
    mr.material.SetFloat("_BaseHue", DepthConnector.value );
    mr.material.SetFloat("_NoiseSize", CutoffConnector.value );


		
	}
}

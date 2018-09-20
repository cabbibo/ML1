using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetBallPos : MonoBehaviour {

  public GameObject ball;
	// Use this for initialization
	void Start () {
		
	}

  public void Reset(){
    ball.GetComponent<Rigidbody>().position = transform.position + transform.up * .3f;
    ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
  }
	
	// Update is called once per frame
	void Update () {
		
	}
}

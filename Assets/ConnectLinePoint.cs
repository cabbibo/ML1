using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectLinePoint : MonoBehaviour {
  public Transform o;
  private LineRenderer lr;
	// Use this for initialization
	void Start () {
    lr = GetComponent<LineRenderer>();
		
	}
	
	// Update is called once per frame
	void Update () {
		if( o != null ){
      lr.SetPosition(0 , transform.position);
      lr.SetPosition(1 , o.position);
    }else{
      lr.SetPosition(0 , transform.position);
    lr.SetPosition(1 , transform.position);
    }
	}
}

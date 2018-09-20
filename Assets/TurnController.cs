using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnController : MonoBehaviour {

  public GameObject ship;

  private Rigidbody rb;
	// Use this for initialization
	void Start () {

    rb = ship.GetComponent<Rigidbody>();
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  public void UpdateFromTouchpad( Vector3 val ){
    print("yaaaa : " + val );
    rb.AddTorque( -ship.transform.up * val.x  * val.z );
  }

  public void UpdateFromTrigger( float val ){
    rb.AddForce( ship.transform.forward * val  * 40 );
  }
}

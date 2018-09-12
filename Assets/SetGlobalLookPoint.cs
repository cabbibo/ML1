using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetGlobalLookPoint : MonoBehaviour {

    Vector3 point;
    Vector3 fPoint;
    float distance;
	// Use this for initialization
	void Start () {
		
	}
	
  // Update is called once per frame
  void LateUpdate () {
      RaycastHit hit;

        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
          point  = hit.point;
          distance = hit.distance;
        }
        else
        {
            point = Camera.main.transform.position +  Camera.main.transform.TransformDirection(Vector3.forward) * distance;
        }


        fPoint = Vector3.Lerp( point , fPoint, .01f);
        transform.position = fPoint;
    Shader.SetGlobalVector( "_LookPoint" , fPoint );
  }

}

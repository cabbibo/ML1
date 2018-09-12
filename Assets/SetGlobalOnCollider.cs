using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetGlobalOnCollider : MonoBehaviour {

  public Vector3 previousVector;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void LateUpdate () {
		
    Shader.SetGlobalFloat( "_MainTime" , Time.time);
	}

  void OnCollisionEnter( Collision c ){

    Shader.SetGlobalVector( "_Collision8" ,Shader.GetGlobalVector( "_Collision7" ));
    Shader.SetGlobalVector( "_Collision7" ,Shader.GetGlobalVector( "_Collision6" ));
    Shader.SetGlobalVector( "_Collision6" ,Shader.GetGlobalVector( "_Collision5" ));
    Shader.SetGlobalVector( "_Collision5" ,Shader.GetGlobalVector( "_Collision4" ));
    Shader.SetGlobalVector( "_Collision4" ,Shader.GetGlobalVector( "_Collision3" ));
    Shader.SetGlobalVector( "_Collision3" ,Shader.GetGlobalVector( "_Collision2" ));
    Shader.SetGlobalVector( "_Collision2" ,Shader.GetGlobalVector( "_Collision1" ));
    Shader.SetGlobalVector( "_Collision1" , new Vector4(transform.position.x,transform.position.y,transform.position.z,Time.time));
  }

}

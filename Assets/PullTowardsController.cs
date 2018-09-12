using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullTowardsController : MonoBehaviour {
 public  MagicLeap.ControllerTransform  Follow;
  public float offsetUp;

  public AudioPlayer audio;
  public string state;


  public float lobPullForce;
  public float lobMass;
  public float lobDrag;


  public float whipPullForce;
  public float whipDistPullForce;
  public float whipMass;
  public float whipDrag;

  public float saberPullForce;
  public float saberMass;
  public float saberDrag;
  public float saberOffset;
  public float saberCutoff;


  Vector3 dif;
  float l;
  Vector3 f;
  Vector3 up;
  public Rigidbody rb;

  void FixedUpdate(){
    //print("T1 " + Follow.controller.TriggerValue);
    //print("T2 " + Follow.controller.Touch2Active);
    //print("T3 " + Follow.controller.Touch1Active);
/*
       up = new Vector3( 0, 0, offsetUp);
    transform.localScale =Vector3.one * .1f;
      dif = transform.position - Follow.transform.TransformPoint(up);//.position;

      l = dif.magnitude;
      dif = -dif.normalized;
  
      Vector3 lobForce  = dif * lobPullForce * (1-Follow.controller.TriggerValue); ///new Vector
      Vector3 whipForce = ((dif * whipPullForce) + (dif * l * whipDistPullForce)) * (Follow.controller.TriggerValue); ///new Vector
    //Vector3 whipForce = dif * lobPullForce; ///new Vector
    //Vector3 lobForce  = dif * lobPullForce; ///new Vector

      rb.mass = Mathf.Lerp( lobMass , whipMass , Follow.controller.TriggerValue);
      rb.drag = Mathf.Lerp( lobDrag , whipDrag , Follow.controller.TriggerValue);
      f = Vector3.Lerp( lobForce , whipForce , Follow.controller.TriggerValue);
      rb.AddForce( f );
    
      if( l > 1 ){
        rb.position = transform.position;
        rb.velocity = Vector3.zero;
      }*/

      if( Follow.controller.Touch1Active == true ){
          rb.position = transform.position;

      }else{

          dif = rb.position - transform.position;//.position;
          rb.AddForce( -( .3f + 30 *Follow.controller.TriggerValue) *dif   );
    
      }
  }
}

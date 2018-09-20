using UnityEngine;
using System.Collections;

public class PlayOnCollide : MonoBehaviour {

  public AudioPlayer audio;
  public AudioClip clip;
  // Use this for initialization
  void Start () {
  
  }
  
  // Update is called once per frame
  void Update () {
  
  }

  void OnCollisionEnter(Collision c){

   // if( c.collider.tag == "Hand"){
    float rl = c.relativeVelocity.magnitude;
    float i = c.impulse.magnitude;
    float rV = GetComponent<Rigidbody>().velocity.magnitude;
    
      audio.Play( clip , rV * 30 );


    //}
  }
}

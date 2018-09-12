using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pull : MonoBehaviour {
/*
//  public ControllerInfo Follow;
  public float offsetUp;

  public AudioPlayer audio;
  public string state;

  public AudioClip saberCollideClip;
  public AudioClip whipCollideClip;
  public AudioClip lobCollideClip;

  public AudioClip saberWhileClip;
  public AudioClip whipWhileClip;
  public AudioClip lobWhileClip;


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


  public Color saberLightStartColor;
  public Color saberLightEndColor;
  public float saberLightTweenSpeed;
  public float saberLightRange;
  public float saberLightIntensity;


  public Color whipLightStartColor;
  public Color whipLightEndColor;
  public float whipLightTweenSpeed;
  public float whipLightRange;
  public float whipLightIntensity;

  public Color lobLightStartColor;
  public Color lobLightEndColor;
  public float lobLightTweenSpeed;
  public float lobLightRange;
  public float lobLightIntensity;

  private Rigidbody rb;
  private LineRenderer lr;
  private TrailRenderer tr;
  private ParticleSystem ps;
  private ParticleSystem.EmissionModule em;
//  private tweenOnCollide toc;

  private AudioClip whileClip;
  private AudioClip collideClip;


	void Awake() {
    rb = GetComponent<Rigidbody>();
    lr = GetComponent<LineRenderer>();
    tr = GetComponent<TrailRenderer>();
    ps = GetComponent<ParticleSystem>();
   // toc = GetComponent<tweenOnCollide>();
    em = ps.emission;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

    Vector3 up; Vector3 dif; Vector3 f; float l;



    if( Follow.middle > saberCutoff ){

      state = "saber";


      ////toc.speedTween = saberLightTweenSpeed;
      ////toc.light.intensity = saberLightIntensity;
      ////toc.light.range = saberLightRange;
      ////toc.startColor =saberLightStartColor;
      ////toc.endColor =saberLightEndColor;

      up = new Vector3( 0, .2f, saberOffset);

      rb.mass = saberMass;
      rb.drag = saberDrag;


      dif = transform.position - Follow.transform.TransformPoint(up);

      l = dif.magnitude;

      if( l > 1 ){
        transform.position = Follow.transform.TransformPoint(up);// + dif.normalized * .1f;
        rb.position = Follow.transform.TransformPoint(up);
        rb.velocity = Vector3.zero;
      }
      dif = -dif.normalized;
      f = l*dif * saberPullForce;
      rb.AddForce( f );

      lr.SetPosition(0,transform.position);
      lr.SetPosition(1,Follow.transform.position);
      tr.time = .2f;
      tr.widthMultiplier = .001f;
      tr.startColor = Color.red;


      //tr.time =  0;
      em.rate = 0;

      transform.localScale = Vector3.one * .01f;

       collideClip = saberCollideClip;
    }else{

      if( Follow.index > .5f ){ 
        state = "whip";
        //toc.speedTween = .6f;
        //toc.startColor = Color.red;
        
        //toc.speedTween = whipLightTweenSpeed;
        //toc.light.intensity = whipLightIntensity;
        //toc.light.range = whipLightRange;
        //toc.startColor =whipLightStartColor;
        //toc.endColor =whipLightEndColor;
        ////toc.startColor = Color.red;
      }else{
        state = "lob";
        //toc.speedTween = 10;

        //toc.startColor = Color.blue;

        //toc.speedTween = lobLightTweenSpeed;
        //toc.light.intensity = lobLightIntensity;
        //toc.light.range = lobLightRange;
        //toc.startColor =lobLightStartColor;
        //toc.endColor =lobLightEndColor;
      }

      up = new Vector3( 0, 0, offsetUp);
    transform.localScale =Vector3.one * .1f;
      dif = transform.position - Follow.transform.TransformPoint(up);//.position;

      l = dif.magnitude;
      dif = -dif.normalized;
  
      Vector3 lobForce  = dif * lobPullForce * (1-Follow.index); ///new Vector
      Vector3 whipForce = ((dif * whipPullForce) + (dif * l * whipDistPullForce)) * (Follow.index); ///new Vector
    //Vector3 whipForce = dif * lobPullForce; ///new Vector
    //Vector3 lobForce  = dif * lobPullForce; ///new Vector

      rb.mass = Mathf.Lerp( lobMass , whipMass , Follow.index );
      rb.drag = Mathf.Lerp( lobDrag , whipDrag , Follow.index );
      f = Vector3.Lerp( lobForce , whipForce , Follow.index );
      rb.AddForce( f );

      lr.SetPosition(0,transform.position);
      lr.SetPosition(1,transform.position);
      

      tr.time =  Follow.index *.8f;
      
      tr.widthMultiplier = .01f + .1f *Follow.index;
      tr.startColor = Color.white;




      em.rate = (1-Follow.index) * 100;
      collideClip = lobCollideClip;

 

    }
	}

  void OnCollisionEnter(Collision c){

    audio.Play( collideClip , Random.Range(.6f,1));

  }
*/

}

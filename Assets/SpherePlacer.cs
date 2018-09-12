using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ComputeVille;
using UnityEngine.UI;

public class SpherePlacer : MonoBehaviour {
  
  public GameObject SpherePrefab;

  private LineRenderer lr;


  public Vector3 point;
  public float distance;
  public bool triggerDown;
  public float lastTime;
  public Vector3 lastPoint;
  public float placeTime;
  public GameObject lastGO;
  public List<RopePhysics> ropes;
  public List<GameObject> startPoints;
  public List<GameObject> endPoints;
  public List<GameObject> spheres;

  public int totalPoints;
  public int maxPoints;


  public Text text;

  // Use this for initialization
  void Start () {

    lr = GetComponent<LineRenderer>();

    for( int i = 0; i < maxPoints; i++ ){

        string rp = "_RopePoint" + i;
        Shader.SetGlobalVector(rp , Vector3.one *100000 );
      }
  }
	
	// Update is called once per frame
	void Update () {
    CastRay();
    lr.SetPosition(0,transform.position);
    lr.SetPosition(1,point);
    if( triggerDown == true ){
      float d = Time.time-lastTime;
      if( d > placeTime ){
       
      }
    }


      int i = 0;
     string s = "Start Points:" + "\n";
     foreach(GameObject p in spheres)
     {

        string rp = "_RopePoint" + i;
        Shader.SetGlobalVector(rp , p.transform.position );
        i++;
     }

     i = 0;
     foreach(GameObject sp in startPoints)
     {
        ropes[i].startPoint = sp;
        i++;
     }

      i = 0;
     foreach(GameObject ep in endPoints)
     {
        ropes[i].endPoint = ep;
        i++;
     }


     //print (s);

     text.text = s;
		
	}


  void Place(){
      lastTime = Time.time;

      if(totalPoints < maxPoints ){
    
        CastRay();

        totalPoints += 1;

        GameObject go = Instantiate( SpherePrefab , point , Quaternion.identity );
        go.transform.position = point;

        spheres.Add(go);


        if( lastGO == null ){
          go.GetComponent<RopeVertBuffer>().enabled = false;
          go.GetComponent<RopePhysics>().enabled = false;
          go.GetComponent<ProceduralLineRender>().enabled = false;
          go.GetComponent<CubicTubeAlongRope>().enabled = false;
          go.GetComponent<TubeVertBuffer>().enabled = false;
          go.GetComponent<TubeTriBuffer>().enabled = false;
        }else{

          go.GetComponent<RopePhysics>().startPoint = go;
          go.GetComponent<RopePhysics>().endPoint = lastGO;//lastPoint;
          startPoints.Add(go);
          endPoints.Add(lastGO);
          ropes.Add(go.GetComponent<RopePhysics>());
          go.GetComponent<RopePhysics>().Reset();
        }

        lastPoint = point;
        lastGO = go;

      }

  }
  public void OnTriggerDown(){
    Place();
    triggerDown = true;
  }

  public void OnTriggerUp(){
    triggerDown = false;
  }

  void CastRay(){

       RaycastHit hit;

        // Does the ray intersect any objects excluding the player layer
        if (UnityEngine.Physics.Raycast( transform.position,  transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
          point  = hit.point;
          distance = hit.distance;
        }
        else
        {
            point =  transform.position +   transform.TransformDirection(Vector3.forward) * distance;
        }

  }
}

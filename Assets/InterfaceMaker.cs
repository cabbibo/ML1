using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ComputeVille;
using UnityEngine.UI;

public class InterfaceMaker : MonoBehaviour {
  
  public GameObject SpherePrefab;
  public GameObject LinePrefab;
  public GameObject ConnectorPrefab;
  public GameObject bumperPrefab;

  public GameObject Artifact;

  private LineRenderer lr;


  public Vector3 point;
  public float distance;

  public Vector3 lastPoint;
  public float lastTime;
  public GameObject lastGO;

  public int totalPoints;
  public int maxPoints;
  public List<GameObject> spheres;
  public bool triggerDown;

  public Text text;

  public GameObject lineBeingPlaced;
  public GameObject collidedGO;
  public GameObject lineConnector;

  public bool sliding;
  public SphereSlider sphereSlider;

  public Vector3[] bumperPoints;
  public int bumperPoint = 0;
  public GameObject[] bumperRepresenters;

  public SphereSlider tempSlider;




  // Use this for initialization
  void Start () {

    lr = GetComponent<LineRenderer>();
    bumperPoints = new Vector3[4];
    bumperRepresenters = new GameObject[4];
    for( int i =0; i < 4; i++){
      bumperRepresenters[i] = Instantiate( bumperPrefab );
    }

  /*  Mesh m = Artifact.GetComponent<MeshFilter>().mesh;
    print("h");
    print(m.triangles.Length);
    print(m.vertices.Length);*/
  }
  
  // Update is called once per frame
  void Update () {
    CastRay();
    lr.SetPosition(0,transform.position);
    lr.SetPosition(1,point);
    

    if( triggerDown == true && Time.time - lastTime >.3f && lineBeingPlaced == null && sphereSlider == null && lineConnector == null ){
      PlaceLine();
    }

    if( lineBeingPlaced != null ){
      UpdateLine();
    }
    if( lineConnector != null ){
      lineConnector.transform.position = point;
    }

    if( sphereSlider != null ){

        float v =  Vector3.Project( point - sphereSlider.start.transform.position , sphereSlider.dif ).magnitude / sphereSlider.dif.magnitude;
        v = Mathf.Clamp( v , 0 , 1 );
        sphereSlider.value = v;


    }
  }

  void PlaceLine(){
    GameObject go = Instantiate( LinePrefab , point, Quaternion.identity );
    go.GetComponent<SphereSlider>().start = lastGO;
    lastGO.GetComponent<SliderRef>().slider = go.GetComponent<SphereSlider>();
    go.GetComponent<SphereSlider>().knob.GetComponent<ConnectLinePoint>().o = lastGO.transform;
    go.GetComponent<ConnectLinePoint>().o = go.GetComponent<SphereSlider>().knob.transform;
    lineBeingPlaced = go;
  }

  void UpdateLine(){
    lineBeingPlaced.transform.position = point;
  }

  void Place(){

      lastTime = Time.time;

      totalPoints += 1;

      GameObject go = Instantiate( SpherePrefab , point , Quaternion.identity );
      go.transform.position = point;

      spheres.Add(go);

      lastPoint = point;
      lastGO = go;


  }
  public void OnTriggerDown(){
    CastRay();
    if( tag == "Mesh" ){
      Place();
    }else if( tag == "Knob"){

      print("HI");
      sphereSlider =collidedGO.transform.parent.gameObject.GetComponent<SphereSlider>();
      sphereSlider.sliding = true;
 

    }else if( tag == "PlacedSphere" ){
      print("WHOA");

      if( collidedGO.GetComponent<SliderRef>().slider != null ){

      lineConnector = Instantiate( ConnectorPrefab , point , Quaternion.identity );
      lineConnector.GetComponent<RopePhysics>().startPoint = collidedGO;
      lineConnector.GetComponent<RopePhysics>().endPoint = lineConnector;
      lineConnector.GetComponent<RopePhysics>().Reset();
      tempSlider =  collidedGO.GetComponent<SliderRef>().slider;

      }

    }


    triggerDown = true;
  }

  public void OnTriggerUp(){

    triggerDown = false;
    CastRay();
    lineBeingPlaced = null;
    if( sphereSlider != null){
      sphereSlider.sliding = false;
      sphereSlider = null;
    }

    if( lineConnector != null ){
      LetGoLineConnector();
    }
    if( tempSlider != null ){
      tempSlider = null;
    }
  }

  void LetGoLineConnector(){
    print(tag);
    if( tag == "SliderConnector"){
      print("hello");
      print( tempSlider);

      if( collidedGO.GetComponent<SliderConnector>().lineConnector != null ){
        Destroy( collidedGO.GetComponent<SliderConnector>().lineConnector );
      }
      lineConnector.transform.position = collidedGO.transform.position;
      collidedGO.GetComponent<SliderConnector>().slider = tempSlider;
      collidedGO.GetComponent<SliderConnector>().lineConnector = lineConnector;
    }else{
      Destroy( lineConnector );
    }
    lineConnector = null;
  }



  public void OnBumperDown(){

    bumperPoints[bumperPoint] = point;
    bumperRepresenters[bumperPoint].transform.position = point;
    bumperPoint += 1;

    if( bumperPoint == 4 ){

      GameObject go = Instantiate( Artifact );
      go.transform.position = Vector3.zero;
      go.transform.localScale = Vector3.one;

     Vector3[] p = new Vector3[4];

      p[0] = bumperPoints[0];
      p[1] = bumperPoints[1];
      p[2] = bumperPoints[2];
      p[3] = bumperPoints[3];

      int[] t = new int[6];
      
      t[0] = 0;
      t[1] = 1;
      t[2] = 3;
      t[3] = 2;
      t[4] = 3;
      t[5] = 1;

      go.GetComponent<MeshFilter>().mesh.vertices = p;
      go.GetComponent<MeshFilter>().mesh.triangles = t;
      go.GetComponent<MeshFilter>().mesh.RecalculateNormals();

      Vector3[] normals = go.GetComponent<MeshFilter>().mesh.normals;
      for( int i =0; i < 4; i++ ){
        p[i] += normals[i] * .1f;
      }


      go.GetComponent<MeshFilter>().mesh.vertices = p;
      go.GetComponent<MeshFilter>().mesh.RecalculateBounds();


      ShaderConnectors sc = go.GetComponent<ShaderConnectors>();

      Vector3 dif = p[2]-p[1];
      Vector3 dif2 = p[1]-p[0];

      sc.HueConnector.transform.position    = normals[1] * .05f + dif2*.05f + dif * .1f + p[1];
      sc.DepthConnector.transform.position  = normals[1] * .05f + dif2*.05f + dif * .3f + p[1];
      sc.CutoffConnector.transform.position = normals[1] * .05f + dif2*.05f + dif * .5f + p[1];

      sc.HueConnector.transform.LookAt(sc.HueConnector.transform.position-normals[1] );
      sc.DepthConnector.transform.LookAt(sc.DepthConnector.transform.position-normals[1] );
      sc.CutoffConnector.transform.LookAt(sc.CutoffConnector.transform.position-normals[1] );


     /*() avePoint /= 4;
      go.transform.localScale =  new Vector3( (bumperPoints[0] - bumperPoints[1]).magnitude , (bumperPoints[0] - bumperPoints[3]).magnitude ,1);
      go.transform.position = avePoint;
*/

      bumperPoint = 0;

    }

  }

  public void OnBumperUp(){

  }




  void CastRay(){

       RaycastHit hit;

      // Does the ray intersect any objects excluding the player layer
        if (UnityEngine.Physics.Raycast( transform.position,  transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity) && triggerDown == false)
        {
          point  = hit.point;
          distance = hit.distance;
          tag = hit.collider.tag;
          collidedGO = hit.collider.gameObject;
        }
        else
        {
            point =  transform.position +   transform.TransformDirection(Vector3.forward) * distance;
            tag = "NONE";
            collidedGO = null;
        }

  }
}

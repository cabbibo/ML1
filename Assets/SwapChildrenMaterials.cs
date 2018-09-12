using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapChildrenMaterials : MonoBehaviour {

  public Material[] materials;
  public int currentMaterial = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		SetMaterials();
	}

  public void Up(){
    currentMaterial += 1;
    currentMaterial %= materials.Length;
    SetMaterials();
  }

  public void Down(){
    currentMaterial -= 1;
    currentMaterial %= materials.Length;
    SetMaterials();
  }

  void SetMaterials(){
    Renderer[] renderers = GetComponentsInChildren<Renderer>();
    foreach (var r in renderers)
    {
       r.material = materials[currentMaterial];
    }

  }
}

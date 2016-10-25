using UnityEngine;
using System.Collections;
using HoloToolkit.Unity;

public class BeginScan : MonoBehaviour {

    SpatialMappingManager spatialMappingManager; 

	// Use this for initialization
	void Start () {

        spatialMappingManager = SpatialMappingManager.Instance;
        spatialMappingManager.StartObserver();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

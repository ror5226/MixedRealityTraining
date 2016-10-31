using UnityEngine;
using System.Collections;
using HoloToolkit.Unity;

public class BeginScan : Singleton<BeginScan> {

    SpatialMappingManager spatialMappingManager; 

	// Use this for initialization
	void Start () {

        spatialMappingManager = SpatialMappingManager.Instance;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnSelect() {
        this.gameObject.SetActive(false);
        spatialMappingManager.StartObserver();
    }
}

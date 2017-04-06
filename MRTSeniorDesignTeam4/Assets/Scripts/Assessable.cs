using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;
using HoloToolkit.Unity.SpatialMapping;

// Placement types
public enum PlacementPosition
{
    Floor = 1,
    HighWall = 2,
    MidWall = 3,
    Ceiling = 4,
    WallFloor = 5
}

// Module types
public enum ModuleType
{
    Kitchen = 1,
    LivingRoom = 2,
    Both = 3
}

 
public class Assessable : MonoBehaviour {

    // Number of questions 
    public int questionVal = 1;

    // Public variables for outside scripts to access
    public SurfacePlane plane = null;
    [Tooltip("Type of plane that the object has been classified as.")]
    public PlacementPosition placement = PlacementPosition.MidWall;
    public ModuleType module = ModuleType.Kitchen;

    // Store plane that obj is placed on
    public void setPlane(SurfacePlane surfacePlane)
    {
        plane = surfacePlane;
    }

    // Return plane
    public SurfacePlane getPlane()
    {
        return plane; 
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

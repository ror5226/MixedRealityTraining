using UnityEngine;
using System.Collections;
using HoloToolkit.Unity;
using UnityEngine.VR.WSA.Input;
using System.Collections.Generic;
using System;


public class SpaceUnderstanding : Singleton<SpaceUnderstanding> {

    public int minWalls = 2;
    public int minFloors = 2;

    private SpatialMappingManager spatialMappingManager;
    private SurfaceMeshesToPlanes surfaceMeshesToPlanes;
    private GestureRecognizer gestureRecognizer;
    RemoveSurfaceVertices removeVerts;

    // Use this for initialization
    private void Start () {

        // Get Instance of SpatialMappingManager
        spatialMappingManager = SpatialMappingManager.Instance;

        surfaceMeshesToPlanes = SurfaceMeshesToPlanes.Instance;

        // Start to recognize gestures 
        gestureRecognizer = new GestureRecognizer();
        gestureRecognizer.SetRecognizableGestures(GestureSettings.Tap);

        removeVerts = RemoveSurfaceVertices.Instance;
    }
	
	// Update is called once per frame
	private void Update () {
	
	}

    public void ShowScan()
    {
       if (spatialMappingManager != null || gestureRecognizer != null)
       {
            // Begin spatial scanning
            spatialMappingManager.drawVisualMeshes = true;

            gestureRecognizer.StartCapturingGestures();

            // Listen for an airtap to create planes
            gestureRecognizer.TappedEvent += Create_Planes;
       }
      else
      {
           Debug.Log("Gesture Recognizer and SpatialMappingManager must be instantiated");
      }
    }

    private void Create_Planes(InteractionSourceKind source, int tapCount, Ray headRay)
    {
        Create_Planes();
    }

    public void Create_Planes()
    {
        // Quit observer and complete planes
        spatialMappingManager.StopObserver();
        surfaceMeshesToPlanes.MakePlanes();

        surfaceMeshesToPlanes.MakePlanesComplete += Remove_Verts;
        gestureRecognizer.TappedEvent -= Create_Planes;
    }

    private void Remove_Verts(object source, EventArgs args)
    {
        List<GameObject> horizontal = new List<GameObject>();
        List<GameObject> vertical = new List<GameObject>();

        //
        horizontal = surfaceMeshesToPlanes.GetActivePlanes(PlaneTypes.Table | PlaneTypes.Floor);
        vertical = surfaceMeshesToPlanes.GetActivePlanes(PlaneTypes.Wall);

        // Ensure that enough of the room has been scanned 
        if (horizontal.Count >= minFloors && vertical.Count >= minWalls)
        {
            if (removeVerts != null && removeVerts.enabled)
            {
                removeVerts.RemoveSurfaceVerticesWithinBounds(surfaceMeshesToPlanes.ActivePlanes);
            }
            else
            {
                Debug.Log("RemoveVerts must be enabled");
            }
        }
        else
        {
            Debug.Log("Not enough walls or floors");
        }
    }
}

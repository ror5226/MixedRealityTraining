﻿using UnityEngine;
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

       }
      else
      {
           Debug.Log("Gesture Recognizer and SpatialMappingManager must be instantiated");
      }
    }

    public void Create_Planes()
    {
        // Quit observer and complete planes
        spatialMappingManager.StopObserver();
        surfaceMeshesToPlanes.MakePlanes();

        surfaceMeshesToPlanes.MakePlanesComplete += Remove_Verts;
    }

    private void Remove_Verts(object source, EventArgs args)
    {
        // Display the planes if in the editor
        foreach (GameObject plane in surfaceMeshesToPlanes.ActivePlanes)
        {
#if UNITY_EDITOR
            plane.SetActive(true);
#else
            plane.SetActive(false);
#endif
        }

        List<GameObject> horizontal = new List<GameObject>();
        List<GameObject> vertical = new List<GameObject>();

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

        // Eventually should add in code to only enable scan button once enough walls are found or something like that 
        else
        {
            Debug.Log("Not enough walls or floors");
        }

        RoomAssetManager.Instance.GenerateItemsInWorld(horizontal, vertical);

    }

    private void OnDestroy()
    {
        if (SurfaceMeshesToPlanes.Instance != null)
        {
            SurfaceMeshesToPlanes.Instance.MakePlanesComplete -= Remove_Verts;
        }
    }
}

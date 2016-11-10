using UnityEngine;
using System.Collections;
using HoloToolkit.Unity;
using UnityEngine.VR.WSA.Input;
using System;

public class MainMenu : Singleton<MainMenu> {

    private SpatialMappingManager spatialMappingManager;
    private SurfaceMeshesToPlanes surfaceMeshesToPlanes;
    private GestureRecognizer gestureRecognizer;

	// Use this for initialization
	public void Start ()
    {
        // Start mapping room
        spatialMappingManager = SpatialMappingManager.Instance;
        spatialMappingManager.DrawVisualMeshes = false;
        spatialMappingManager.StartObserver();

        surfaceMeshesToPlanes = SurfaceMeshesToPlanes.Instance;

        // Start to recognize gestures 
        gestureRecognizer = new GestureRecognizer();
        gestureRecognizer.SetRecognizableGestures(GestureSettings.Tap);
        gestureRecognizer.StartCapturingGestures();
    }

    // Update is called once per frame
    private void Update()
    {

    }

    public void OnSelect()
    {
        if (gestureRecognizer != null && spatialMappingManager != null )
        {
            this.gameObject.SetActive(false);
            spatialMappingManager.drawVisualMeshes = true;
            gestureRecognizer.TappedEvent += Planes_Finished;
        }
        else
        {
            //Console.WriteLine("Gesture Recognizer and SpatialMappingManager must be instantiated");
        }
    }

    private void Planes_Finished(InteractionSourceKind source, int tapCount, Ray headRay)
    {
        surfaceMeshesToPlanes.MakePlanes();
        gestureRecognizer.TappedEvent -= Planes_Finished;
    }
}

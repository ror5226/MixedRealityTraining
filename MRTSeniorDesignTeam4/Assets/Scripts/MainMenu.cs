using UnityEngine;
using System.Collections;
using HoloToolkit.Unity;
using UnityEngine.VR.WSA.Input;
using System.Collections.Generic;
using System;

public class MainMenu : Singleton<MainMenu> {

    private GestureRecognizer gestureRecognizer;
    private SpaceUnderstanding spaceUnderstanding;
    private SpatialMappingManager spatialMappingManager;

    // Use this for initialization
    public void Start()
    {
        spaceUnderstanding = SpaceUnderstanding.Instance;

        // Start mapping room, hide mesh 
        spatialMappingManager = SpatialMappingManager.Instance;
        spatialMappingManager.DrawVisualMeshes = false;
        spatialMappingManager.StartObserver();

        // Start to recognize gestures 
        gestureRecognizer = new GestureRecognizer();
        gestureRecognizer.SetRecognizableGestures(GestureSettings.Tap);
        gestureRecognizer.StartCapturingGestures();

        // Listen for Tap to place menu 
        gestureRecognizer.TappedEvent += Place_Menu;

    }


    /*
    Vector3 normal;
    private void menuMove()
    {
        RaycastHit hitInfo;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitInfo, 10, 31))
        {
            this.transform.position =  hitInfo.point
            // 2.a: Assign property Normal to be the hitInfo normal.
            l = hitInfo.normal;
        }
        else
        {
            Vector3 Position = Camera.main.transform.position + (Camera.main.transform.forward * 5);
            Vector3 Normal = Camera.main.transform.forward;
        }
    */

// Update is called once per frame
private void Update()
    {
    //    menuMove();
    }

    // First tap detected **NEED TO ADD PLACEMENT**
    private void Place_Menu(InteractionSourceKind source, int tapCount, Ray headRay)
    {
        if (gestureRecognizer != null)
        {
            gestureRecognizer.TappedEvent -= Place_Menu;
       
            // Change Text Box
            GameObject textBox = GameObject.Find("StartText");
            TextMesh text = textBox.GetComponent<TextMesh>();
            text.text = "Look Around the Room to Scan";

            spaceUnderstanding.StartScan();
        }
        else
        {
            Debug.Log("Gesture Recognizer must be instantiated");
        }
    }
 }

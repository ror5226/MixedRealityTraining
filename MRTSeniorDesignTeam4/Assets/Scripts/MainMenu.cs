using UnityEngine;
using System.Collections;
using HoloToolkit.Unity;
using UnityEngine.VR.WSA.Input;
using System.Collections.Generic;
using System;

public class MainMenu : Singleton<MainMenu> {

    private GestureRecognizer gestureRecognizer;
    private SpaceUnderstanding spaceUnderstanding;

    // Use this for initialization
    public void Start ()
    {

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
        }
    }

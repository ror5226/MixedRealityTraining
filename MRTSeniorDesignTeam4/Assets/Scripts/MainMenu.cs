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
        spaceUnderstanding = SpaceUnderstanding.Instance;

        // Start to recognize gestures 
        gestureRecognizer = new GestureRecognizer();
        gestureRecognizer.SetRecognizableGestures(GestureSettings.Tap);
        gestureRecognizer.StartCapturingGestures();

        // Listen for Tap to place menu 
        gestureRecognizer.TappedEvent += Place_Menu;
    }

    // Update is called once per frame
    private void Update()
    {

    }

    // First tap detected **NEED TO ADD PLACEMENT**
    private void Place_Menu(InteractionSourceKind source, int tapCount, Ray headRay)
    {
        if (gestureRecognizer != null)
        {
            gestureRecognizer.TappedEvent -= Place_Menu;
       
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

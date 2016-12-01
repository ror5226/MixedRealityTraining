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
    private bool mobileMenu = false; 

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

    }

    private void menuMove()
    {
        RaycastHit hitInfo;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitInfo, 10, 1 << 31))
        {
            this.transform.position = hitInfo.point + GazeManager.Instance.Normal * .5f;
            this.transform.forward = Camera.main.transform.forward;
           // gameObject.transform. = hitInfo.normal;

        }
        else
        {
            Vector3 Position = Camera.main.transform.position;
            Vector3 Normal = Camera.main.transform.forward;
        }
    }

// Update is called once per frame
private void Update()
    {
        if (mobileMenu)
        {
            menuMove();
        }
    }

    // First tap detected **NEED TO ADD PLACEMENT**
    private void Place_Menu()
    {
        if (gestureRecognizer != null)
        {
         //   gestureRecognizer.TappedEvent -= Place_Menu;
       
            // Change Text Box
          //  GameObject textBox = GameObject.Find("StartText");
         //   TextMesh text = textBox.GetComponent<TextMesh>();
         //   text.text = "Look Around the Room to Scan";

            spaceUnderstanding.StartScan();
        }
        else
        {
            Debug.Log("Gesture Recognizer must be instantiated");
        }
    }
    public void Place_Menu_Button()
    {
        mobileMenu = true; 

        GameObject menuPanel = this.transform.FindChild("AssessmentPanel").gameObject;
        GameObject startCanvas = this.transform.FindChild("FirstMenu").gameObject;

        //
        //  First set the assessmentPanel to be visable
        //  Then set the assessmentPanel's position to be where the damage info's position.
        //  then hide damageInfoPanel.
        //

        menuPanel.SetActive(true);
        menuPanel.transform.position = startCanvas.transform.position;
        startCanvas.SetActive(false);

        gestureRecognizer.TappedEvent += Stop_Menu_Moving;
  
    }

    private void Stop_Menu_Moving(InteractionSourceKind source, int tapCount, Ray headRay)
    {
        mobileMenu = false;

        gestureRecognizer.TappedEvent -= Stop_Menu_Moving;

        Place_Menu();
    }
}

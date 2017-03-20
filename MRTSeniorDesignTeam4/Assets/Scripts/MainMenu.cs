using UnityEngine;
using System.Collections;
using HoloToolkit.Unity.SpatialMapping;
using UnityEngine.VR.WSA.Input;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;


public class MainMenu : Singleton<MainMenu> {

    private GestureRecognizer gestureRecognizer;
    private SpaceUnderstanding spaceUnderstanding;
    private SpatialMappingManager spatialMappingManager;
    private bool mobileMenu = false;
    private bool billboard = true;
    private int mainMenu_ButtonCount = 0;
    private int tapNum = 0;

    // Use this for initialization
    public void Start()
    {
       
        spaceUnderstanding = SpaceUnderstanding.Instance;

        // Start mapping room, hide mesh 
        if (SpatialMappingManager.Instance == null)
        {
            Debug.Log("SpatialMappingManager is null");
        }
        else
        {
            spatialMappingManager = SpatialMappingManager.Instance;
        }

#if UNITY_EDITOR
        spatialMappingManager.DrawVisualMeshes = true;
#else
        spatialMappingManager.DrawVisualMeshes = false;
        spatialMappingManager.StartObserver();
#endif

        // Start to recognize gestures 
        gestureRecognizer = new GestureRecognizer();
        gestureRecognizer.SetRecognizableGestures(GestureSettings.Tap);
        gestureRecognizer.StartCapturingGestures();

    }

    //Move menu with user gaze against wall
    private void menuMove()
    {
        billboard = false;

        RaycastHit hitInfo;

        //Hit everything but UI layer
        int raymask = 1 << 5;
        raymask = ~raymask;

        //Raycast against wall to place menu 
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitInfo, 10, raymask))
        {
            this.transform.position = hitInfo.point - GazeManager.Instance.GazeNormal * .05f;
            this.transform.forward = -hitInfo.normal;

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
        //Place menu
        if (mobileMenu)
        {
            menuMove();
        }

        //Menu faces user
        if (billboard)
        {
            this.transform.forward = Camera.main.transform.forward;
        }

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.M))
        {
            mobileMenu = false;
            GameObject menuPanel = this.transform.FindChild("RoomSelection").gameObject;
            GameObject startCanvas = this.transform.FindChild("FirstMenu").gameObject;

            menuPanel.SetActive(true);
            menuPanel.transform.position = startCanvas.transform.position;
            startCanvas.SetActive(false);

            // Generate Planes from scan
            spaceUnderstanding.Create_Planes();

        }
#endif

    }

    public void Scan_Room_Button_Clicked()
    {

        // Check to make sure Gesture Recognizer is instantiated 
        if (gestureRecognizer != null)
        {
            if(mainMenu_ButtonCount == 0)
            {
                // Show mesh on walls
                spaceUnderstanding.ShowScan();

                // Update UI text
                GameObject startPanel = GameObject.FindGameObjectWithTag("Start_Panel_Text");
                Text panelText = startPanel.GetComponent<Text>();
                panelText.text = "Please look around the room\nwhere training items can be placed";

                GameObject panelButton = GameObject.FindGameObjectWithTag("Scan_Button_Text"); 
                Text buttonText = panelButton.GetComponent<Text>();
                buttonText.text = "Stop Scan";

                mainMenu_ButtonCount++;
            }
            else
            {

#if UNITY_EDITOR
                spatialMappingManager.DrawVisualMeshes = true;
                // Generate Planes from scan
                spaceUnderstanding.Create_Planes();
#else
                spatialMappingManager.DrawVisualMeshes = false;
#endif
                // Permenently plane menu on the wall
                Place_Menu();
            }
        }
        else
        {
            Debug.Log("Gesture Recognizer must be instantiated");
        }
    }

    public void Place_Menu()
    {
        mobileMenu = true;

        // Update UI text
        GameObject startPanel = GameObject.FindGameObjectWithTag("Start_Panel_Text");
        Text panelText = startPanel.GetComponent<Text>();
        panelText.text = "Look around the room to place menu\n airtap to place";

        // Remove Button on UI
        GameObject scanButton = GameObject.FindGameObjectWithTag("Scan_Button");
        scanButton.SetActive(false);

        gestureRecognizer.TappedEvent += Stop_Menu_Moving;

            mainMenu_ButtonCount++;
    }

    private void Stop_Menu_Moving(InteractionSourceKind source, int tapCount, Ray headRay)
    {
        if (tapNum > 0)
        {
            mobileMenu = false;

            GameObject menuPanel = this.transform.FindChild("RoomSelection").gameObject;
            GameObject startCanvas = this.transform.FindChild("FirstMenu").gameObject;

            menuPanel.SetActive(true);
            menuPanel.transform.position = startCanvas.transform.position;
            startCanvas.SetActive(false);

            // Generate Planes from scan
            spaceUnderstanding.Create_Planes();

            gestureRecognizer.TappedEvent -= Stop_Menu_Moving;
        }
        else
        {
            tapNum++;
        }
    }
}

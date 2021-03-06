﻿
using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity.SpatialMapping;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;
using UnityEngine.UI;

namespace HoloToolkit.Examples.SpatialUnderstandingFeatureOverview
{
    public class QuerySetup : Singleton<QuerySetup>//, ISourceStateHandler, IInputClickHandler
    {
        // Consts
        float kMinAreaForStats = 100.0f;

        public void setMinArea(float fl) {
            kMinAreaForStats = fl;
        }

        float kMinAreaForComplete = 50.0f;
        //public float kMinHorizAreaForComplete = 25.0f;
        //public float kMinWallAreaForComplete = 10.0f;

        // Config
        public TextMesh DebugDisplay;
        public TextMesh DebugSubDisplay;
        public Transform Parent_Scene;
        public SpatialMappingObserver MappingObserver;
        public SpatialUnderstandingCursor AppCursor;

        Text areaText;
        Button scanButton;

        // Properties
        public string SpaceQueryDescription
        {
            get
            {
                return spaceQueryDescription;
            }
            set
            {
                spaceQueryDescription = value;
                objectPlacementDescription = "";
            }
        }

        public string ObjectPlacementDescription
        {
            get
            {
                return objectPlacementDescription;
            }
            set
            {
                objectPlacementDescription = value;
                spaceQueryDescription = "";
            }
        }

        public bool DoesScanMeetMinBarForCompletion
        {
            get
            {
                // Only allow this when we are actually scanning
                if ((SpatialUnderstanding.Instance.ScanState != SpatialUnderstanding.ScanStates.Scanning) ||
                    (!SpatialUnderstanding.Instance.AllowSpatialUnderstanding))
                {
                    return false;
                }

                // Query the current playspace stats
                IntPtr statsPtr = SpatialUnderstanding.Instance.UnderstandingDLL.GetStaticPlayspaceStatsPtr();
                if (SpatialUnderstandingDll.Imports.QueryPlayspaceStats(statsPtr) == 0)
                {
                    return false;
                }
                SpatialUnderstandingDll.Imports.PlayspaceStats stats = SpatialUnderstanding.Instance.UnderstandingDLL.GetStaticPlayspaceStats();

                // Check our preset requirements
                if (stats.TotalSurfaceArea > kMinAreaForComplete)
                {
                    return true;
                }
                return false;
            }
        }

        public string PrimaryText
        {
            get
            {
                // Display the space and object query results (has priority)
                if (!string.IsNullOrEmpty(SpaceQueryDescription))
                {
                    return SpaceQueryDescription;
                }
                else if (!string.IsNullOrEmpty(ObjectPlacementDescription))
                {
                    return ObjectPlacementDescription;
                }

                // Scan state
                if (SpatialUnderstanding.Instance.AllowSpatialUnderstanding)
                {
                    switch (SpatialUnderstanding.Instance.ScanState)
                    {
                        case SpatialUnderstanding.ScanStates.Scanning:
                            // Get the scan stats
                            IntPtr statsPtr = SpatialUnderstanding.Instance.UnderstandingDLL.GetStaticPlayspaceStatsPtr();
                            if (SpatialUnderstandingDll.Imports.QueryPlayspaceStats(statsPtr) == 0)
                            {
                                return "playspace stats query failed";
                            }

                            // The stats tell us if we could potentially finish
                            if (DoesScanMeetMinBarForCompletion)
                            {
                                return "When ready, air tap to finalize your playspace";
                            }
                            return "Walk around and scan in your playspace";
                        case SpatialUnderstanding.ScanStates.Finishing:
                            return "Finalizing scan (please wait)";
                        case SpatialUnderstanding.ScanStates.Done:
                            return "Scan complete - Use the menu to run queries";
                        default:
                            return "ScanState = " + SpatialUnderstanding.Instance.ScanState.ToString();
                    }
                }
                return "";
            }
        }

        public Color PrimaryColor
        {
            get
            {
                if (SpatialUnderstanding.Instance.ScanState == SpatialUnderstanding.ScanStates.Scanning)
                {
                    if (trackedHandsCount > 0)
                    {
                        return DoesScanMeetMinBarForCompletion ? Color.green : Color.red;
                    }
                    return DoesScanMeetMinBarForCompletion ? Color.yellow : Color.white;
                }

                // If we're looking at the menu, fade it out
                Vector3 hitPos, hitNormal;
                UnityEngine.UI.Button hitButton;
                float alpha = AppCursor.RayCastUI(out hitPos, out hitNormal, out hitButton) ? 0.15f : 1.0f;

                // Special case processing & 
                return (!string.IsNullOrEmpty(SpaceQueryDescription) || !string.IsNullOrEmpty(ObjectPlacementDescription)) ?
                    (PrimaryText.Contains("processing") ? new Color(1.0f, 0.0f, 0.0f, 1.0f) : new Color(1.0f, 0.7f, 0.1f, alpha)) :
                    new Color(1.0f, 1.0f, 1.0f, alpha);
            }
        }

        public string DetailsText
        {
            get
            {
                if (SpatialUnderstanding.Instance.ScanState == SpatialUnderstanding.ScanStates.None)
                {
                    return "";
                }

                // Scanning stats get second priority
                if ((SpatialUnderstanding.Instance.ScanState == SpatialUnderstanding.ScanStates.Scanning) &&
                    (SpatialUnderstanding.Instance.AllowSpatialUnderstanding))
                {
                    IntPtr statsPtr = SpatialUnderstanding.Instance.UnderstandingDLL.GetStaticPlayspaceStatsPtr();
                    if (SpatialUnderstandingDll.Imports.QueryPlayspaceStats(statsPtr) == 0)
                    {
                        return "Playspace stats query failed";
                    }
                    SpatialUnderstandingDll.Imports.PlayspaceStats stats = SpatialUnderstanding.Instance.UnderstandingDLL.GetStaticPlayspaceStats();

                    // Start showing the stats when they are no longer zero
                    if (stats.TotalSurfaceArea > kMinAreaForStats)
                    {
                        string subDisplayText = string.Format("Scanned area: {0:0.0}m", stats.TotalSurfaceArea);
                        subDisplayText += " of required area: " + string.Format("{0:0.0}m", kMinAreaForComplete);
                        //subDisplayText += string.Format("\nnumFloorCells={0}, numCeilingCells={1}, numPlatformCells={2}", stats.NumFloor, stats.NumCeiling, stats.NumPlatform);
                        //subDisplayText += string.Format("\npaintMode={0}, seenCells={1}, notSeen={2}", stats.CellCount_IsPaintMode, stats.CellCount_IsSeenQualtiy_Seen + stats.CellCount_IsSeenQualtiy_Good, stats.CellCount_IsSeenQualtiy_None);
                        return subDisplayText;
                    }
                    return "";
                }
                return "";
            }
        }

        // Privates
        private string spaceQueryDescription;
        private string objectPlacementDescription;
        private uint trackedHandsCount = 0;
        private KeywordRecognizer keywordRecognizer;

        // Functions
        private void Start()
        {/*
            // Default the scene & the HoloToolkit objects to the camera
            Vector3 sceneOrigin = Camera.main.transform.position;
            Parent_Scene.transform.position = sceneOrigin;
            MappingObserver.SetObserverOrigin(sceneOrigin);
            InputManager.Instance.AddGlobalListener(gameObject);


            var keywordsToActions = new Dictionary<string, Action>
            {
                { "Toggle Scanned Mesh", ToggleScannedMesh },
                { "Toggle Processed Mesh", ToggleProcessedMesh },
            };

            keywordRecognizer = new KeywordRecognizer(keywordsToActions.Keys.ToArray());
            keywordRecognizer.OnPhraseRecognized += args => keywordsToActions[args.text].Invoke();
            keywordRecognizer.Start();
            */

            //areaText = GameObject.FindGameObjectWithTag("Start_Panel_Area").GetComponent<Text>();
            areaText = GameObject.Find("FirstMenu").transform.FindChild("MenuPanel").FindChild("AreaScanned").GetComponent<Text>();
            //scanButton = GameObject.FindGameObjectWithTag("Scan_Button").GetComponent<Button>();
            scanButton = GameObject.Find("FirstMenu").transform.Find("ScanRoomButton").GetComponent<Button>();
            Debug.Log(scanButton.name);
            Debug.Log(areaText.name);
#if UNITY_EDITOR
#else
            if (SpatialUnderstanding.Instance != null)
            {
                  SpatialUnderstanding.Instance.UnderstandingCustomMesh.DrawProcessedMesh = false;
            }
            else{
                  Debug.Log("Spatial Understanding is null");
            }
#endif
        }

        protected override void OnDestroy()
        {
            InputManager.Instance.RemoveGlobalListener(gameObject);
        }

        private void Update_DebugDisplay(float deltaTime)
        {
            // Basic checks
            //if (DebugDisplay == null)
            //{
            //    return;
            //}

            // Update display text
            //areaText.text = PrimaryText;
            //areaText.color = PrimaryColor;
            areaText.text = DetailsText;
            if (DoesScanMeetMinBarForCompletion) {
                scanButton.interactable = true;
            }
        }

#if UNITY_EDITOR
        private void Update_KeyboardInput(float deltaTime)
        {
            // Toggle SurfaceMapping & CustomUnderstandingMesh visibility
            if (Input.GetKeyDown(KeyCode.BackQuote) &&
                (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift)))
            {
                ToggleScannedMesh();
            }
            else if (Input.GetKeyDown(KeyCode.BackQuote) &&
                     (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
            {
                ToggleProcessedMesh();
            }
        }

        private static void ToggleScannedMesh()
        {
            SpatialMappingManager.Instance.DrawVisualMeshes = !SpatialMappingManager.Instance.DrawVisualMeshes;
            Debug.Log("SpatialUnderstanding -> SpatialMappingManager.Instance.DrawVisualMeshes=" + SpatialMappingManager.Instance.DrawVisualMeshes);
        }

        private static void ToggleProcessedMesh()
        {
            SpatialUnderstanding.Instance.UnderstandingCustomMesh.DrawProcessedMesh = !SpatialUnderstanding.Instance.UnderstandingCustomMesh.DrawProcessedMesh;
            Debug.Log("SpatialUnderstanding -> SpatialUnderstanding.Instance.UnderstandingCustomMesh.DrawProcessedMesh=" + SpatialUnderstanding.Instance.UnderstandingCustomMesh.DrawProcessedMesh);
        }
#endif
        private void Update()
        {
            // Updates DIsplay
            Update_DebugDisplay(Time.deltaTime);

#if UNITY_EDITOR //Uses keyboard only in editor
            Update_KeyboardInput(Time.deltaTime);
#endif
        }

        /*
        public void OnSourceDetected(SourceStateEventData eventData)
        {
            // If the source has positional info and there is currently no visible source
            if (eventData.InputSource.SupportsInputInfo(eventData.SourceId, SupportedInputInfo.Position))
            {
                trackedHandsCount++;
            }
        }

        public void OnSourceLost(SourceStateEventData eventData)
        {
            if (eventData.InputSource.SupportsInputInfo(eventData.SourceId, SupportedInputInfo.Position))
            {
                trackedHandsCount--;
            }
        }

    //End Scan
        public void OnInputClicked(InputClickedEventData eventData)
        {
            if ((SpatialUnderstanding.Instance.ScanState == SpatialUnderstanding.ScanStates.Scanning) &&
                !SpatialUnderstanding.Instance.ScanStatsReportStillWorking)
            {
                SpatialUnderstanding.Instance.RequestFinishScan();
            }
        }

    */
    }
}

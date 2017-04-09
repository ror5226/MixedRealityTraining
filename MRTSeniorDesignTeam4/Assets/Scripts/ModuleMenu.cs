using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;


public class ModuleMenu : Singleton<ModuleMenu>{

    // Private variables only used in this script
    private GameObject currentPanel;
    private GameObject modulePanel;
    private RoomAssetManager roomAssetManager;

    // Use this for initialization
    void Start () {

        // Store Panels
        currentPanel = this.transform.FindChild("RoomSelection").gameObject;
        modulePanel = this.transform.FindChild("InModule").gameObject;
        modulePanel.SetActive(false);

        if(RoomAssetManager.Instance != null)
        {
            roomAssetManager = RoomAssetManager.Instance;
        }
        else
        {
            Debug.Log("Room Asset Manager is null");
        }
    }
	
    // Ran when end scene button is clicked
    public void EndScene()
    {
        //Remove all asset from scene
        for (int i = 0; i < roomAssetManager.instantiatedAssets.Count; i++)
        {
            roomAssetManager.instantiatedAssets[i].SetActive(false);
        }
        roomAssetManager.instantiatedAssets.Clear();

        // Swap menu
        EndModuleMenuSwap();

        // Clear all of the lists
        roomAssetManager.floorObjects.Clear();
        roomAssetManager.ceilingObjects.Clear();
        roomAssetManager.wallFloorObjects.Clear();
        roomAssetManager.highWallObjects.Clear();
        roomAssetManager.midWallObjects.Clear();

        if(ReadText.Instance != null)
        {
            ReadText.Instance.Say("Select a module to load.");
        }
    }

    // Ran when kitchen button is pressed
    public void RunKitchenScene()
    {
        ReadText.Instance.Say("Airtap on objects to interact with them.");

        if (SpaceUnderstanding.horizontal != null && SpaceUnderstanding.vertical != null)
        {
            // Load kitchen objects and swap menu
            StartModuleMenuSwap();
            RoomAssetManager.Instance.GenerateItemsInWorld(SpaceUnderstanding.horizontal, SpaceUnderstanding.vertical, ModuleType.Kitchen);
        }
    }

    // Ran when living room button is pressed
    public void RunLivingRoonScene()
    {
        ReadText.Instance.Say("Airtap on objects to interact with them.");

        if (SpaceUnderstanding.horizontal != null && SpaceUnderstanding.vertical != null)
        {
            // Load living room objects and swap menu
            StartModuleMenuSwap();
            RoomAssetManager.Instance.GenerateItemsInWorld(SpaceUnderstanding.horizontal, SpaceUnderstanding.vertical, ModuleType.LivingRoom);
        }
    }

    // Swap the current menu with the in module menu 
    public void StartModuleMenuSwap()
    {
        modulePanel.SetActive(true);
        AccessPanel.Instance.setCurrentScore();
        modulePanel.transform.position = currentPanel.transform.position;
        currentPanel.SetActive(false);
    }

    // Swap the current menu with the in module menu 
    public void EndModuleMenuSwap()
    {
        currentPanel.SetActive(true);
        currentPanel.transform.position = currentPanel.transform.position;
        AccessPanel.Instance.resetCorrectlyAnswered();
        modulePanel.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;


public class ModuleMenu : Singleton<ModuleMenu>{
    GameObject currentPanel;
    GameObject modulePanel;

    private RoomAssetManager roomAssetManager;

    // Use this for initialization
    void Start () {

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
	
    public void EndScene()
    {
        //Remove all asset from scene
        for (int i = 0; i < roomAssetManager.instantiatedAssets.Count; i++)
        {
            roomAssetManager.instantiatedAssets[i].SetActive(false);
        }
        roomAssetManager.instantiatedAssets.Clear();

        EndModuleMenuSwap();

        roomAssetManager.floorObjects.Clear();
        roomAssetManager.ceilingObjects.Clear();
        roomAssetManager.wallFloorObjects.Clear();
        roomAssetManager.highWallObjects.Clear();
        roomAssetManager.midWallObjects.Clear();




    }

    public void RunKitchenScene()
    {
        if(SpaceUnderstanding.horizontal != null && SpaceUnderstanding.vertical != null)
        {
            // Load kitchen objects and swap menu
            StartModuleMenuSwap();
            RoomAssetManager.Instance.GenerateItemsInWorld(SpaceUnderstanding.horizontal, SpaceUnderstanding.vertical, ModuleType.Kitchen);
        }
    }

    public void RunLivingRoonScene()
    {
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
        modulePanel.transform.position = currentPanel.transform.position;
        currentPanel.SetActive(false);
    }

    // Swap the current menu with the in module menu 
    public void EndModuleMenuSwap()
    {
        currentPanel.SetActive(true);
        currentPanel.transform.position = currentPanel.transform.position;
        modulePanel.SetActive(false);
    }
}

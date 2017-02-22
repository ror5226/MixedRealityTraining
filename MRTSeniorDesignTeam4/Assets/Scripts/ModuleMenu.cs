using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;


public class ModuleMenu : Singleton<ModuleMenu>{

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void RunKitchenScene()
    {
        if(SpaceUnderstanding.horizontal != null && SpaceUnderstanding.vertical != null)
        {
            RoomAssetManager.Instance.GenerateItemsInWorld(SpaceUnderstanding.horizontal, SpaceUnderstanding.vertical, ModuleType.Kitchen);
        }
    }

    public void RunLivingRoonScene()
    {
        if (SpaceUnderstanding.horizontal != null && SpaceUnderstanding.vertical != null)
        {
            RoomAssetManager.Instance.GenerateItemsInWorld(SpaceUnderstanding.horizontal, SpaceUnderstanding.vertical, ModuleType.LivingRoom);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomAssetManager : MonoBehaviour {


    [Tooltip("A collection of assessable room objects to generate in the world.")]
    public List<GameObject> spaceObjectPrefabs;

    public void GenerateItemsInWorld(List<GameObject> horizontalSurfaces, List<GameObject> verticalSurfaces)
    {
        List<GameObject> floorObjects = new List<GameObject>();
        List<GameObject> highWallObjects = new List<GameObject>();
        List<GameObject> midWallObjects = new List<GameObject>();
        List<GameObject> lowWallObjects = new List<GameObject>();
        List<GameObject> wallFloorObjects = new List<GameObject>();

        foreach (GameObject spacePrefab in spaceObjectPrefabs)
        {
            Assessable assessObject = spacePrefab.GetComponent<Assessable>();
            if (assessObject.placement == PlacementPosition.Floor)
            {
                floorObjects.Add(spacePrefab);
            }
            else if (assessObject.placement == PlacementPosition.LowWall)
            {
                lowWallObjects.Add(spacePrefab);
            }
            else
            {
                midWallObjects.Add(spacePrefab);
            }
        }
        /*
        if (horizontalObjects.Count > 0)
        {
            CreateSpaceObjects(horizontalObjects, horizontalSurfaces, PlacementSurfaces.Horizontal);
        }

        if (verticalObjects.Count > 0)
        {
            CreateSpaceObjects(verticalObjects, verticalSurfaces, PlacementSurfaces.Vertical);
        }
        */
    }

   
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

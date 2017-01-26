using System;
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
            else if (assessObject.placement == PlacementPosition.MidWall)
            {
                midWallObjects.Add(spacePrefab);
            }
            else if (assessObject.placement == PlacementPosition.HighWall)
            {
                highWallObjects.Add(spacePrefab);
            }
            else
            {
                wallFloorObjects.Add(spacePrefab);
            }
        }
        

        if (floorObjects.Count > 0)
        {
            //CreateSpaceObjects(horizontalObjects, horizontalSurfaces, PlacementSurfaces.Horizontal);
        }

        if (lowWallObjects.Count > 0)
        {
            // CreateSpaceObjects(verticalObjects, verticalSurfaces, PlacementSurfaces.Vertical);
        }

        if (midWallObjects.Count > 0)
        {
            CreateSpaceObjects(midWallObjects, verticalSurfaces, PlacementPosition.MidWall);
        }

        if (highWallObjects.Count > 0)
        {
            // CreateSpaceObjects(verticalObjects, verticalSurfaces, PlacementSurfaces.Vertical);
        }

        if (wallFloorObjects.Count > 0)
        {
           // CreateSpaceObjects(verticalObjects, verticalSurfaces, PlacementSurfaces.Vertical);
        }
        
    }

    private void CreateSpaceObjects(List<GameObject> spaceObjects, List<GameObject> surfaces, PlacementPosition placementType)
    {
        List<int> UsedPlanes = new List<int>();

        // Sort the planes by distance to user.
        surfaces.Sort((lhs, rhs) =>
        {
            Vector3 headPosition = Camera.main.transform.position;
            Collider rightCollider = rhs.GetComponent<Collider>();
            Collider leftCollider = lhs.GetComponent<Collider>();

            // This plane is big enough, now we will evaluate how far the plane is from the user's head.  
            // Since planes can be quite large, we should find the closest point on the plane's bounds to the 
            // user's head, rather than just taking the plane's center position.
            Vector3 rightSpot = rightCollider.ClosestPointOnBounds(headPosition);
            Vector3 leftSpot = leftCollider.ClosestPointOnBounds(headPosition);

            return Vector3.Distance(leftSpot, headPosition).CompareTo(Vector3.Distance(rightSpot, headPosition));
        });
    }


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

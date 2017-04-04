using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using HoloToolkit.Unity;
using UnityEngine;
using HoloToolkit.Unity.SpatialMapping;


public class RoomAssetManager : Singleton<RoomAssetManager> {


    [Tooltip("A collection of assessable room objects to generate in the world.")]
    public List<GameObject> spaceObjectPrefabs;
    public List<GameObject> instantiatedAssets;
    public int assetCount = 10; 


    private SurfacePlane mainFloor;
    private SurfacePlane roomCeiling;
    private QueryCalls queryCalls;
    private LevelSolver levelSolver;

    public void Start()
    {
        if(QueryCalls.Instance != null)
        {
            queryCalls = QueryCalls.Instance;
            queryCalls.InitializeSolver();
        }

        if (LevelSolver.Instance != null)
        {
               levelSolver = LevelSolver.Instance;
        }
    }

    public void GenerateItemsInWorld(List<GameObject> horizontalSurfaces, List<GameObject> verticalSurfaces, ModuleType moduleSelected)
    {
        // Types of items defined in assessable 
        List<GameObject> floorObjects = new List<GameObject>();
        List<GameObject> highWallObjects = new List<GameObject>();
        List<GameObject> midWallObjects = new List<GameObject>();
        List<GameObject> ceilingObjects = new List<GameObject>();
        List<GameObject> wallFloorObjects = new List<GameObject>();

        // Loop for all items to be placed in the scene
        foreach (GameObject spacePrefab in spaceObjectPrefabs)
        {
            
            Assessable assessObject = spacePrefab.GetComponent<Assessable>();
            if(assessObject.module == moduleSelected)
            {
                // Ensure items have the assessable script 
                if (assessObject == null)
                {
                    Debug.Log("Item needs to have Assable script attached to it");
                }

                // Sort items into their proper lists
                if (assessObject.placement == PlacementPosition.Floor)
                {
                    floorObjects.Add(spacePrefab);
                }
                else if (assessObject.placement == PlacementPosition.Ceiling)
                {
                    ceilingObjects.Add(spacePrefab);
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
           
        }

        float largestArea = 0.0f; 

        // Remove any tables/other raised objects
        foreach (GameObject horizontal in horizontalSurfaces)
        {
            SurfacePlane plane = null;
            if (horizontal != null)
            {
                plane = horizontal.GetComponent<SurfacePlane>();
            }

            if (plane != null)
            {
                if(plane.PlaneType != PlaneTypes.Floor && plane.PlaneType != PlaneTypes.Ceiling)
                {
                    Destroy(horizontal);
                }
                else if(plane.PlaneType == PlaneTypes.Ceiling)
                {
                    roomCeiling = plane;
                }
                else
                {
                    if(plane.Plane.Area > largestArea)
                    {
                        mainFloor = plane;
                    }
                    else //remove any smaller floors
                    {
                        Destroy(horizontal);
                    }
                }
            }
        }

        // Remove anything not considered a wall 
        foreach (GameObject vertical in verticalSurfaces)
        {
            SurfacePlane plane = vertical.GetComponent<SurfacePlane>();
            if (plane != null)
            {
                if (plane.PlaneType != PlaneTypes.Wall)
                {
                    Destroy(vertical);
                }
            }
        }

        // Find postions for objects based off their type
        if (floorObjects.Count > 0)
        {
           PlaceFloorObjects(floorObjects, horizontalSurfaces, PlacementPosition.Floor);
        }

        if (ceilingObjects.Count > 0)
        {
           PlaceCeilingObjects(ceilingObjects, verticalSurfaces, PlacementPosition.Ceiling);
        }

        if (midWallObjects.Count > 0)
        {
          //  CreateSpaceObjects(midWallObjects, verticalSurfaces, PlacementPosition.MidWall);
        }

        if (highWallObjects.Count > 0)
        {
           // CreateSpaceObjects(highWallObjects, verticalSurfaces, PlacementPosition.HighWall);
        }

        if (wallFloorObjects.Count > 0)
        {
             PlaceWallFloorObjects(wallFloorObjects, verticalSurfaces, PlacementPosition.WallFloor);
        }

        // Update UI text
        GameObject startPanel = GameObject.FindGameObjectWithTag("InModule_Text");
        Text panelText = startPanel.GetComponent<Text>();
        panelText.text = "Module Score: 0/100";

    }
    
    private void PlaceWallFloorObjects(List<GameObject> spaceObjects, List<GameObject> surfaces, PlacementPosition placementType)
    {
        #region Original Placement Technique
        
        List<int> UsedPlanes = new List<int>();
        // Sort the planes by distance to user.
        surfaces.Sort((lhs, rhs) =>
        {
            Vector3 headPosition = Camera.main.transform.position;
            Collider rightCollider = rhs.GetComponent<Collider>();
            Collider leftCollider = lhs.GetComponent<Collider>();

            // Order planes by distance to user
            Vector3 rightSpot = rightCollider.ClosestPointOnBounds(headPosition);
            Vector3 leftSpot = leftCollider.ClosestPointOnBounds(headPosition);

            return Vector3.Distance(leftSpot, headPosition).CompareTo(Vector3.Distance(rightSpot, headPosition));
        });

        foreach (GameObject item in spaceObjects)
        {
            int index = -1;

            BoxCollider collider = item.GetComponent<BoxCollider>();

            if(collider == null)
            {
                Debug.Log("Object needs BoxCollider");
            }

            index = FindNearestPlane(surfaces, item.transform.localScale, placementType, UsedPlanes);

            UsedPlanes.Add(index);

            Quaternion rotation = Quaternion.identity;
            Vector3 position;

            // If there is somewhere to put the object 
            if (index >= 0)
            {
                GameObject surface = surfaces[index];
                SurfacePlane plane = surface.GetComponent<SurfacePlane>();

                // Generate postion by taking middle point of plane and then offseting by the width of the asset
                position = surface.transform.position + ((plane.PlaneThickness + (.5f * Math.Abs(collider.size.z) * item.transform.localScale.z)) * plane.SurfaceNormal);

                if(placementType == PlacementPosition.WallFloor)
                {
                    position.y = mainFloor.Plane.Bounds.Center.y + collider.size.y * .5f * item.transform.localScale.y;
                }

                if (placementType == PlacementPosition.HighWall || placementType == PlacementPosition.MidWall || placementType == PlacementPosition.Ceiling)
                {
                    // Vertical objects should face out from the wall.
                    position.y = position.y + collider.size.y * .5f;
                    rotation = Quaternion.LookRotation(surface.transform.forward, Vector3.up);


                }
                else
                {
                    rotation = Quaternion.LookRotation(surface.transform.forward, Vector3.up);

                }

                //Vector3 finalPosition = AdjustPositionWithSpatialMap(position, placementType);
                GameObject spaceObject = Instantiate(item, position, rotation) as GameObject;

                // Add object to list for later removal of scene
                instantiatedAssets.Add(spaceObject);

                Assessable assessable = spaceObject.GetComponent<Assessable>();
                assessable.setPlane(plane);
            }
           
        }
        
        #endregion

        #region Spatial Understanding Technique

        /*
        List<LevelSolver.PlacementQuery> placementQuery = new List<LevelSolver.PlacementQuery>();

        foreach (GameObject obj in spaceObjects)
        {
            BoxCollider collider = obj.GetComponent<BoxCollider>();

            //Debug.Log("x: " + (Math.Abs(collider.size.x) * obj.transform.localScale.x));
            //Debug.Log("y: " + (Math.Abs(collider.size.y) * obj.transform.localScale.y));
            //Debug.Log("z: " + (Math.Abs(collider.size.z) * obj.transform.localScale.z));

            placementQuery.Add(levelSolver.Query_OnWall(((Math.Abs(collider.size.x) * obj.transform.localScale.x)) / 2.0f, ((Math.Abs(collider.size.y) * obj.transform.localScale.y)) / 2.0f, ((Math.Abs(collider.size.z) * obj.transform.localScale.z)) / 2.0f, 0.0f, Math.Abs(collider.size.y) * obj.transform.localScale.y + .5f));
        }
        LevelSolver.Instance.PlaceObjectAsync("OnWall", placementQuery);

        int returnedVal;

                do
                {
                    returnedVal = levelSolver.ProcessPlacementResults();
                }
                while (returnedVal == -1);
               
                if(returnedVal == -2)
                {
                    Debug.Log("Mapp is too small");
                }
                else
                {

            for (int i = 0; i < levelSolver.placementResults.Count; i++)
            {*/
        //Debug.Log(levelSolver.placementResults[i].Result.Position.x + " " + levelSolver.placementResults[i].Result.Position.y + " " + levelSolver.placementResults[i].Result.Position.z);
        /*
        float mindiff = 100.0f;

        GameObject minplane = new GameObject();
        foreach (GameObject s in surfaces)
        {
            float diff = Math.Abs((s.transform.position.x - levelSolver.result.Position.x) + (s.transform.position.z - levelSolver.result.Position.z));
            Debug.Log("diff " + diff);
            if (diff < mindiff)
            {
                diff = mindiff;
                minplane = s;
            }
        }
        Vector3 querypos = levelSolver.result.
        querypos = levelSolver.result.Position;// + ((.5f * Math.Abs(collider.size.z) * obj.transform.localScale.z)) * -minplane.GetComponent<SurfacePlane>().SurfaceNormal;
        querypos.y = mainFloor.Plane.Bounds.Center.y + collider.size.y * .5f * obj.transform.localScale.y;

        GameObject spaceObject = Instantiate(obj, querypos, Quaternion.LookRotation(-levelSolver.result.Forward, Vector3.up)) as GameObject;
        spaceObject.SetActive(true);
        }
        */

        //  }          
        // }
        #endregion
    }

    private void PlaceFloorObjects(List<GameObject> spaceObjects, List<GameObject> surfaces, PlacementPosition placementType)
    {
        
        List<LevelSolver.PlacementQuery> placementQuery = new List<LevelSolver.PlacementQuery>();
        BoxCollider collider;
        foreach (GameObject obj in spaceObjects)
        {
            collider = obj.GetComponent<BoxCollider>();

            placementQuery.Add(levelSolver.Query_OnFloor(.3f, .3f, .3f));
        }
            LevelSolver.Instance.PlaceObjectAsync("OnFloor", placementQuery);

            int returnedVal;

            do
            {
                returnedVal = levelSolver.ProcessPlacementResults();
            }
            while (returnedVal == -1);

            if (returnedVal == -2)
            {
                Debug.Log("Mapp is too small");
            }
        
                for (int i = 0; i < levelSolver.placementResults.Count; i++)
                {
                    Debug.Log(levelSolver.placementResults[i].Result.Position.x + " " + levelSolver.placementResults[i].Result.Position.y + " " + levelSolver.placementResults[i].Result.Position.z);

                    collider = spaceObjects[i].GetComponent<BoxCollider>();
                    GameObject minplane = new GameObject();

                    Vector3 querypos = levelSolver.placementResults[i].Result.Position;
                    querypos.y = mainFloor.Plane.Bounds.Center.y; // + collider.size.y * .5f * spaceObjects[i].transform.localScale.y;

                    GameObject spaceObject = Instantiate(spaceObjects[i], querypos, Quaternion.LookRotation(-levelSolver.placementResults[i].Result.Forward, Vector3.up)) as GameObject;
                    // spaceObject.transform.rotation = Quaternion.LookRotation(spaceObject.transform.localRotation., Vector3.up);
                    spaceObject.SetActive(true);
                
                    // Add object to list for later removal of scene

                     instantiatedAssets.Add(spaceObject);
                }
    }

    private void PlaceCeilingObjects(List<GameObject> spaceObjects, List<GameObject> surfaces, PlacementPosition placementType)
    {
        List<LevelSolver.PlacementQuery> placementQuery = new List<LevelSolver.PlacementQuery>();
        BoxCollider collider;
        foreach (GameObject obj in spaceObjects)
        {
            collider = obj.GetComponent<BoxCollider>();

            placementQuery.Add(levelSolver.Query_OnCeiling(.3f, .3f, .3f));
        }
        LevelSolver.Instance.PlaceObjectAsync("OnCeiling", placementQuery);

        int returnedVal;

        do
        {
            returnedVal = levelSolver.ProcessPlacementResults();
        }
        while (returnedVal == -1);

        if (returnedVal == -2)
        {
            Debug.Log("Mapp is too small");
        }

        for (int i = 0; i < levelSolver.placementResults.Count && i < spaceObjects.Count; i++)
        {
            Debug.Log(levelSolver.placementResults[i].Result.Position.x + " " + levelSolver.placementResults[i].Result.Position.y + " " + levelSolver.placementResults[i].Result.Position.z);

            collider = spaceObjects[i].GetComponent<BoxCollider>();
            GameObject minplane = new GameObject();

            Vector3 querypos = levelSolver.placementResults[i].Result.Position;

            querypos.y = roomCeiling.Plane.Bounds.Center.y;

            GameObject spaceObject = Instantiate(spaceObjects[i], querypos, Quaternion.LookRotation(levelSolver.placementResults[i].Result.Forward, Vector3.up)) as GameObject;

            spaceObject.SetActive(true);

            // Add object to list for later removal of scene
            instantiatedAssets.Add(spaceObject);
        }
    }

    private int FindNearestPlane(List<GameObject> planes, Vector3 minSize, PlacementPosition surface, List<int> usedPlanes)
    {
        int planeIndex = -1;

        for (int i = 0; i < planes.Count; i++)
        {

            if (usedPlanes.Contains(i))
            {
                continue;
            }

            //BoxCollider collider = planes[i].GetComponent<BoxCollider>();
            if (planes[i].transform.localScale.x < minSize.x || planes[i].transform.localScale.y < minSize.y)
            {
                // Plain is too small
                continue;
            }
            
            return i;
        }
        return planeIndex;
    }

    private Vector3 AdjustPositionWithSpatialMap(Vector3 position, Vector3 surfaceNormal)
    {
        Vector3 newPosition = position;
        RaycastHit hitInfo;
        float distance = 0.5f;

        // Check to see if there is a SpatialMapping mesh occluding the object at its current position.
        if (Physics.Raycast(position, surfaceNormal, out hitInfo, distance, SpatialMappingManager.Instance.LayerMask))
        {
            // If the object is occluded, reset its position.
            newPosition = hitInfo.point;
        }
        return newPosition;
    }
}

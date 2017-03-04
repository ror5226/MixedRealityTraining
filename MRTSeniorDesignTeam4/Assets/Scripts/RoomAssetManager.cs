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
    private SurfacePlane mainFloor;

    public void GenerateItemsInWorld(List<GameObject> horizontalSurfaces, List<GameObject> verticalSurfaces, ModuleType moduleSelected)
    {
        List<GameObject> floorObjects = new List<GameObject>();
        List<GameObject> highWallObjects = new List<GameObject>();
        List<GameObject> midWallObjects = new List<GameObject>();
        List<GameObject> lowWallObjects = new List<GameObject>();
        List<GameObject> wallFloorObjects = new List<GameObject>();

        foreach (GameObject spacePrefab in spaceObjectPrefabs)
        {
            Assessable assessObject = spacePrefab.GetComponent<Assessable>();

            if(assessObject == null)
            {
                Debug.Log("Item needs to have Assable script attached to it");
            }
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
                if(plane.PlaneType != PlaneTypes.Floor)
                {
                    Destroy(horizontal);
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

        if (floorObjects.Count > 0)
        {
           // CreateSpaceObjects(horizontalObjects, horizontalSurfaces, PlacementSurfaces.Horizontal);
        }

        if (lowWallObjects.Count > 0)
        {
            CreateSpaceObjects(lowWallObjects, verticalSurfaces, PlacementPosition.LowWall);
        }

        if (midWallObjects.Count > 0)
        {
            CreateSpaceObjects(midWallObjects, verticalSurfaces, PlacementPosition.MidWall);
        }

        if (highWallObjects.Count > 0)
        {
            CreateSpaceObjects(highWallObjects, verticalSurfaces, PlacementPosition.HighWall);
        }

        if (wallFloorObjects.Count > 0)
        {
             CreateSpaceObjects(wallFloorObjects, verticalSurfaces, PlacementPosition.WallFloor);
        }

        // Update UI text
        GameObject startPanel = GameObject.FindGameObjectWithTag("InModule_Text");
        Text panelText = startPanel.GetComponent<Text>();
        panelText.text = "Module Score: 0/100";

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

                //     position = AdjustPositionWithSpatialMap(position, plane.SurfaceNormal);
                //     position = AdjustPositionWithSpatialMap(position, mainFloor.SurfaceNormal);

                if (placementType == PlacementPosition.HighWall || placementType == PlacementPosition.MidWall || placementType == PlacementPosition.LowWall)
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

                Assessable assessable = spaceObject.GetComponent<Assessable>();
                assessable.setPlane(plane);

            }
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

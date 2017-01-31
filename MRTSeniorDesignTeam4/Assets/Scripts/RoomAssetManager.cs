using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;


public class RoomAssetManager : Singleton<RoomAssetManager> {


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

        // Remove any tables/other raised objects
        foreach (GameObject horizontal in horizontalSurfaces)
        {
            SurfacePlane plane = horizontal.GetComponent<SurfacePlane>();
            if (plane != null)
            {
                if(plane.PlaneType != PlaneTypes.Floor)
                {
                    Destroy(horizontal);
                }
                else
                {
                    Debug.Log("floor");
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
                else
                {
                    Debug.Log("wall");
                }
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

            // Order planes by distance to user
            Vector3 rightSpot = rightCollider.ClosestPointOnBounds(headPosition);
            Vector3 leftSpot = leftCollider.ClosestPointOnBounds(headPosition);

            return Vector3.Distance(leftSpot, headPosition).CompareTo(Vector3.Distance(rightSpot, headPosition));
        });

        foreach (GameObject item in spaceObjects)
        {
            int index = -1;
            Collider collider = item.GetComponent<Collider>();

            index = FindNearestPlane(surfaces, collider.bounds.size, placementType);

            Quaternion rotation = Quaternion.identity;
            Vector3 position;

            // If there is somewhere to put the object 
            if (index >= 0)
            {
                GameObject surface = surfaces[index];
                SurfacePlane plane = surface.GetComponent<SurfacePlane>();
                position = surface.transform.position + (plane.PlaneThickness * plane.SurfaceNormal);
                position = AdjustPositionWithSpatialMap(position, plane.SurfaceNormal);
                //rotation = Camera.main.transform.localRotation;

                if (placementType == PlacementPosition.HighWall || placementType == PlacementPosition.MidWall || placementType == PlacementPosition.LowWall|| placementType == PlacementPosition.WallFloor)
                {
                    // Vertical objects should face out from the wall.
                    rotation = Quaternion.LookRotation(surface.transform.forward, Vector3.up);
                }
                else
                {
                    // Horizontal objects should face the user.
                    rotation = Quaternion.LookRotation(Camera.main.transform.position);
                    rotation.x = 0f;
                    rotation.z = 0f;
                }

                //Vector3 finalPosition = AdjustPositionWithSpatialMap(position, placementType);
                GameObject spaceObject = Instantiate(item, position, rotation) as GameObject;
               // spaceObject.transform.up = -surface.transform.forward;
               // spaceObject.transform.up = surface.transform.up;
                //spaceObject.transform.parent = gameObject.transform;
            }
        }
    }
    private int FindNearestPlane(List<GameObject> planes, Vector3 minSize, PlacementPosition surface)
    {
        int planeIndex = -1;

        for (int i = 0; i < planes.Count; i++)
        {
           
            Collider collider = planes[i].GetComponent<Collider>();
            if (collider.bounds.size.x < minSize.x || collider.bounds.size.y < minSize.y)
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

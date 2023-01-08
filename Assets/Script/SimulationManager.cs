using System;
using System.Collections.Generic;
using UnityEngine;

public enum SimulationMode
{
    BRUTE_FORCE,
    OCTREE
}


public class SimulationManager : MonoBehaviour
{
    [SerializeField] private SimulationMode simulationMode;
    [SerializeField] private GameObject objectPrefab;
    [SerializeField] private int spawnObjCount;
    [Range(1,20)] [SerializeField] int nodeMinSize = 20;
    [SerializeField] private float worldSize = 10;
    public Bounds worldBounds;
    private  List<ObjectController> worldObjects = new List<ObjectController>();
    
    public Octree octree;

    public static SimulationManager instance;

    public float WorldSize => worldSize;

    void Start()
    {
        instance= this;
        worldBounds = new Bounds() { center = transform.position, extents = Vector3.one * worldSize };
        SpawnObjects(spawnObjCount);
    }

    private void SpawnObjects(int n)
    {
        for (int i = 0; i < n; i++)
        {
            ObjectController obj = Instantiate(objectPrefab).GetComponent<ObjectController>();
            worldObjects.Add(obj);
            obj.ID = worldObjects.Count;
        }
    }

    void FixedUpdate()
    {

        worldBounds.extents = Vector3.one*worldSize;
        if(simulationMode == SimulationMode.BRUTE_FORCE)
        {
            CollisionManager.ProcessCollision(worldObjects, worldObjects);
        }
        //octree update each 
        else if (simulationMode == SimulationMode.OCTREE)
        {
            octree = new Octree(worldObjects, nodeMinSize, worldBounds);

        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 0);
        Gizmos.DrawWireCube(worldBounds.center,worldBounds.center);
        //octree.rootNode.DrawBoundingBox();
        
    }


}

using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
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
    private Bounds worldBounds;
    private  List<ObjectController> worldObjects = new List<ObjectController>();
    
    public Octree octree;

    public static SimulationManager instance;

    public List<double> timeSteps = new List<double>();

    public float WorldSize => worldSize;

    private void Start()
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

    private void FixedUpdate()
    {

        worldBounds.extents = Vector3.one*worldSize;
       //force brute simulation
        if(simulationMode == SimulationMode.BRUTE_FORCE)
        {
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();
            CollisionManager.ProcessCollision(worldObjects, worldObjects);
            stopwatch.Stop();
            //UnityEngine.Debug.Log("Brute_Force millis:"+stopwatch.Elapsed.TotalMilliseconds.ToString());
            if (Time.fixedTime > 5) StorageOrShowMedia(stopwatch.Elapsed.TotalMilliseconds);
        }
        //octree update each fixedTime
        else if (simulationMode == SimulationMode.OCTREE)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            octree = new Octree(worldObjects, nodeMinSize, worldBounds);
            stopwatch.Stop();
            //UnityEngine.Debug.Log("Octree millis:"+stopwatch.Elapsed.TotalMilliseconds.ToString());
            if (Time.fixedTime > 5) StorageOrShowMedia(stopwatch.Elapsed.TotalMilliseconds);


        }

    }


    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 0);
        Gizmos.DrawWireCube(worldBounds.center, worldBounds.size);
        octree.rootNode.DrawBoundingBox();
        

    }

    private void StorageOrShowMedia(double timeMillis)
    {
        if(timeSteps.Count < 100) 
            timeSteps.Add(timeMillis); 
        else if (timeSteps.Count == 100)
            //show media at 100 times
            UnityEngine.Debug.Log("média: " + Media(timeSteps).ToString());

    }

    private double Media(List<double> timeMillis)
    {
        double count = 0f;
        for (int i = 0; i < 100; i++)
        {
            count+= timeMillis[i];
        }
        return count / (double)100.00000000f;
    }
}

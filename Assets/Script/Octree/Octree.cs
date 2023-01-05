using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class Octree
{
    public OctreeNode rootNode;
    public Bounds boundsD;

    public Octree(GameObject[] worldObjects, float minNodeSize)
    {
        Bounds bounds = new Bounds();

        //foreach (GameObject obj in worldObjects)
        //{
        //    bounds.Encapsulate(obj.GetComponent<Collider>().bounds);
        //}
        bounds.Expand(20);

        //float maxSize = Mathf.Max(new float[] { bounds.size.x, bounds.size.y, bounds.size.z });
        //Vector3 sizeVector = new Vector3(maxSize, maxSize, maxSize) * 0.5f;
        //bounds.SetMinMax(bounds.center - sizeVector, bounds.center + sizeVector);
        boundsD = bounds;
        rootNode = new OctreeNode(bounds, minNodeSize);
        AddObjects(worldObjects);
    }

    public void AddObjects(GameObject[] worldObjects)
    {
        foreach(GameObject go in worldObjects)
        {
            rootNode.AddObject(go);
        }
    }
}
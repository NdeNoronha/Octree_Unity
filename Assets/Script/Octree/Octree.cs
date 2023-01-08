using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Octree
{
    public OctreeNode rootNode;
    public Octree(List<ObjectController> worldObjects, float minNodeSize, Bounds worldBound)
    {
        rootNode = new OctreeNode(worldBound, minNodeSize);
        foreach(ObjectController obj in worldObjects)
        {
            rootNode.Subdivide(obj);
        }

        rootNode.CheckCollisions();

    }

}

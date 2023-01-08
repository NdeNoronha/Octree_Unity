using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OctreeNode 
{
    Bounds nodeBounds;
    public OctreeNode[] octreeNodeChild;
    float minSize;
    public List<ObjectController> objectControllers;
    public OctreeNode(Bounds nodeBounds, float minNodeSize)
    {
        objectControllers =   new List<ObjectController>();   
        this.nodeBounds = nodeBounds;
        minSize = minNodeSize;

    }

    public void Subdivide(ObjectController go)
    {
        //verify  stop condition is done : the nodeBounds size is less then minSize definition
        if (nodeBounds.size.y <= minSize)
        {
            objectControllers.Add(go);
            octreeNodeChild = null;
            return;
        }

        if (octreeNodeChild == null) octreeNodeChild = new OctreeNode[8];
        bool dividingOctreeNode = false;

        for (int i = 0; i < 8; i++)
        {
            if (octreeNodeChild[i] == null)
                octreeNodeChild[i] = new OctreeNode(OctantBound(i), minSize);

            if (octreeNodeChild[i].nodeBounds.Intersects(go.bound))
            {
                dividingOctreeNode = true;
                //sending ObjectController "go" to child when it is inside childBounds[i]
                    
                octreeNodeChild[i].Subdivide(go);
            }
        }
        if (dividingOctreeNode == false)
        {
            //whenn not inside childs
            objectControllers.Add(go);
            octreeNodeChild = null;
        }
        
        
        
    }

    //check all objects collisions at latest octrees subdivision or check octreeNodes collisions
    public void CheckCollisions() {

        if(octreeNodeChild == null)
        {
            CollisionManager.ProcessCollision(objectControllers, objectControllers);
        } else
        {
            foreach(OctreeNode node in octreeNodeChild)
            {
                node.CheckCollisions();
            }
        }

            
        

    }

    //Render octree BoundingBoxes
    public void DrawBoundingBox()
    {
        Gizmos.color = new Color(0, 1, 0);
        if(octreeNodeChild != null)
        {
            foreach (OctreeNode node in octreeNodeChild)
            {
                node.DrawBoundingBox();

            }

        }
    }

    private Bounds OctantBound(int index)
    {
        float quarter = nodeBounds.size.y / 4f;
        Vector3 childSize = Vector3.one * nodeBounds.size.y / 2f;

        if (index == 0) return new Bounds(nodeBounds.center + new Vector3(-quarter, quarter, -quarter), childSize);
        if (index == 1) return new Bounds(nodeBounds.center + new Vector3(-quarter, quarter, quarter), childSize);
        if (index == 2) return new Bounds(nodeBounds.center + new Vector3(quarter, quarter, -quarter), childSize);
        if (index == 3) return new Bounds(nodeBounds.center + new Vector3(quarter, quarter, quarter), childSize);
        if (index == 4) return new Bounds(nodeBounds.center + new Vector3(-quarter, -quarter, -quarter), childSize);
        if (index == 5) return new Bounds(nodeBounds.center + new Vector3(-quarter, -quarter, quarter), childSize);
        if (index == 6) return new Bounds(nodeBounds.center + new Vector3(quarter, -quarter, -quarter), childSize);
        return new Bounds(nodeBounds.center + new Vector3(quarter, -quarter, quarter), childSize);

    }


}




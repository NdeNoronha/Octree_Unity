using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CollisionManager
{

    public static void ProcessCollision(List<ObjectController> objControllers, List<ObjectController> otherObjControllers)
    {
        foreach(ObjectController obj in objControllers) {
            foreach (ObjectController otherObj in otherObjControllers)
            {

                if(obj != otherObj)
                {
                    float distance = DistanceBounds(obj.bound, otherObj.bound);
                    if(distance < obj.radius+otherObj.radius)
                    {
                        //Debug.Log("obj " + obj.ID + " contatcs obj " + otherObj.ID);
                        obj.OnCollide(otherObj.bound);
                    }
                }
            }
        }
    } 

    public static float DistanceBounds(Bounds boundsOne, Bounds boundsTwo)
    {
        float dx = Math.Abs(boundsOne.center.x - boundsTwo.center.x);
        float dy = Math.Abs(boundsOne.center.y - boundsTwo.center.y);
        float dz = Math.Abs(boundsOne.center.z - boundsTwo.center.z);

        float distance = (float)Math.Sqrt(dx * dx + dy * dy + dz * dz);
        return distance;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameData : MonoBehaviour
{
    // Transform to rectangular coordinate system
    public static Vector3 transformToRec(Vector3 Pos){
        return new Vector3(Pos.x + 2*Pos.y, Pos.x - 2*Pos.y, Pos.z);
    }

    public static Vector3 transformToRec_inverse(Vector3 Pos){
        return new Vector3((Pos.x + Pos.y)/2, (Pos.x - Pos.y)/4, Pos.z);
    }

    public static Vector3 roundVec3(Vector3 Pos){
        return new Vector3((float) Math.Round(Pos.x), (float) Math.Round(Pos.y), (float) Math.Round(Pos.z));
    }

    public static Vector3 getMousePos(){
        Vector3 cameraMousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane+2);
        return Camera.main.ScreenToWorldPoint(cameraMousePos);
    }

    // Get nearest vertex after transform to rectangular coordinate system
    public static Vector3 nearestVertex(Vector3 Pos){
        return transformToRec_inverse(roundVec3(transformToRec(Pos)));
    }

    public static float distanceRec(Vector3 Pos1, Vector3 Pos2){
        return Vector3.Distance(transformToRec(Pos1), transformToRec(Pos2));
    }

    // Get nearest object with tag after transform to rectangular coordinate system
    public static GameObject getNearestObjectWithTag(Vector3 selfPos, string Tag)
    {
        float minDistance = float.MaxValue;
        GameObject nearestObject = null;
        GameObject[] objects = GameObject.FindGameObjectsWithTag(Tag);

        foreach(GameObject o in objects)
        {
            float distance = distanceRec(selfPos, o.transform.position);
            if(distance < minDistance)
            {
                minDistance = distance;
                nearestObject = o;
            }
        }
        return nearestObject;
    }

    public static GameObject[] getInRangeObjectWithTag(Vector3 selfPos, string Tag, float Range){
        GameObject[] objects = GameObject.FindGameObjectsWithTag(Tag);
        return Array.FindAll(objects ,o => distanceRec(selfPos, o.transform.position) <= Range);
    }
}

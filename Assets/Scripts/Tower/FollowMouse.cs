using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class FollowMouse : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        transform.position = GameData.nearestVertex(GameData.getMousePos());
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class LineBehaviorHelper
{
    //static public Vector2 GetLineVelocity(Transform own, Transform start, Transform end, float deltaTime, float oneWayTime, ref float currentTime, Action goalCallback)
    //{
    //    Vector2 velocity = Vector2.zero;
    //    if (oneWayTime > 0)
    //    {
    //        Vector2 newPos = Vector3.Lerp(start.position, end.position, currentTime / oneWayTime);

    //        velocity = (newPos - (Vector2)own.position) / deltaTime;
    //    }
    //    else
    //    {
    //        velocity = Vector2.zero;
    //    }

    //    if (currentTime == oneWayTime)
    //    {
    //        goalCallback?.Invoke();
    //    }

    //    currentTime += deltaTime;

    //    if (currentTime > oneWayTime)
    //    {
    //        currentTime = oneWayTime;
    //    }

    //    return velocity;
    //}

    //static public Vector2 GetLineVelocity(Transform own, Transform start, Transform end, float progress)
    //{
    //    Vector2 velocity = Vector2.zero;
    //    Vector2 newPos = Vector3.Lerp(start.position, end.position, progress);

    //    velocity = (newPos - (Vector2)own.position) / Time.fixedDeltaTime;

    //    return velocity;
    //}

    static public Vector2 GetStrobe(Transform start,Transform end, float progress)
    {
        Vector2 pos = Vector3.Lerp(start.position, end.position, progress);

        return pos;
    }
}

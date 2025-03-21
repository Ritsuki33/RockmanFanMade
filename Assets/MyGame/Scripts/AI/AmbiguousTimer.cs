﻿using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AmbiguousTimer
{
    float time = 0f;
    float limit = -1f;

    bool done = false;
    public void Start(float fromTime, float toTime)
    {
        time = 0;
        limit = Random.Range(fromTime, toTime);
        done = false;
    }

    /// <summary>
    /// 時間を進める
    /// </summary>
    /// <param name="deltaTime"></param>
    /// <param name="callbackAfterTime"></param>
    public void MoveAheadTime(float deltaTime, System.Action callbackAfterTime,bool loop=false)
    {
        time += deltaTime;

        if ((loop||!done) && time > limit)
        {
            callbackAfterTime();
            done = true;
        }
    }
}

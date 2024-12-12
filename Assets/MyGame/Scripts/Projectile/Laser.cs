using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : ReusableObject
{
    [SerializeField] LaserBehavior laserBehavior;

    public void Launch(Transform start, float speed, Vector2 laserDir = default, float offsetSpeed = 0)
    {
        laserBehavior.Launch(start, speed, Delete, laserDir, offsetSpeed);

    }

    public void Cease()
    {
        laserBehavior.Cease();
    }

    public void Delete()
    {
        Pool.Release(this);
    }
}

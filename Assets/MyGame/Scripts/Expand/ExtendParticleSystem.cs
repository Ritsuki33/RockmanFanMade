using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class ExtendParticleSystem
{
    public static void SetSpeed(this ParticleSystem _particleSystem, float value)
    {
        var ma = _particleSystem.main;
        ma.startSpeedMultiplier = value;
    }

    public static float GetSpeed(this ParticleSystem _particleSystem)
    {
        return _particleSystem.main.startSpeedMultiplier;
    }
 
}

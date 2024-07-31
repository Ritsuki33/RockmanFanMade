using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeController : ReusableObject
{
    public void OnFinishedAnimation()
    {
        Pool.Release(this);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEnvetController : MonoBehaviour
{
    public Dictionary<int, Action> animationEvents = new Dictionary<int, Action>();

    public void OnEvent(int i)
    {
        animationEvents[i].Invoke();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// よく使われるものは共有
/// </summary>
static public class AnimationNameHash
{
    static public int Idle= Animator.StringToHash("Idle");
    static public int Run= Animator.StringToHash("Run");
    static public int Float= Animator.StringToHash("Float");
    static public int Pause = Animator.StringToHash("Pause");
    static public int Fire = Animator.StringToHash("Fire");
    static public int Shoot = Animator.StringToHash("Shoot");
    static public int Shooting = Animator.StringToHash("Shooting");
    static public int Repatriation = Animator.StringToHash("Repatriation");
    static public int Transfered = Animator.StringToHash("Transfered");
    static public int Damaged = Animator.StringToHash("Damaged");
}

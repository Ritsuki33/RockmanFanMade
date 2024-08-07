using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class ExtendAnimator
{
    static public bool IsPlayingCurrentAnimation(this Animator _animator)
    {
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

        return !(stateInfo.normalizedTime >= 1 && !_animator.IsInTransition(0));
    }
}

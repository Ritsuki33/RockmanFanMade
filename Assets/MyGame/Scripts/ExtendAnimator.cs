using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

static public class ExtendAnimator
{
    static public bool IsPlayingCurrentAnimation(this Animator _animator,int currentHash=-1)
    {
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        if (currentHash < 0)
        {
            return !(stateInfo.normalizedTime >= 1 && !_animator.IsInTransition(0));
        }
        else
        {
            // �n�b�V������v���Ă��Ȃ�(�A�j���[�V�������قȂ�)�ꍇ�͑J�ڒ��Ƃ���
            bool isTranstion = stateInfo.shortNameHash != currentHash;

            return isTranstion || stateInfo.normalizedTime < 1;
        }
    }
}

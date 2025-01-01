using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アニメーションを持つオブジェクト
/// </summary>
public class AnimObject : BaseObject
{
    [SerializeField] private ModelController m_model;

    public ModelController ModelController => m_model;

    protected override void OnPause(bool isPause) => m_model.OnPause(isPause);
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum KeyGuideType
{
    WASD,
    L,
    M,
    TAB,
    SPACE
}

/// <summary>
/// フッターUI
/// </summary>
public class FooterUI : MonoBehaviour
{
    [SerializeField] BaseTweemAnimator m_Animator;

    [SerializeField] KeyGuide wasdKeyGuide;
    [SerializeField] KeyGuide lGuide;
    [SerializeField] KeyGuide MGuide;
    [SerializeField] KeyGuide TabGuide;
    [SerializeField] KeyGuide SpaceGuide;



    public void Setup(params (KeyGuideType key, string value)[] pairs)
    {
        wasdKeyGuide.gameObject.SetActive(false);
        lGuide.gameObject.SetActive(false);
        MGuide.gameObject.SetActive(false);
        TabGuide.gameObject.SetActive(false);
        SpaceGuide.gameObject.SetActive(false);
        foreach (var pair in pairs)
        {
            switch (pair.key)
            {
                case KeyGuideType.WASD:
                    wasdKeyGuide.Setup(pair.value);
                    wasdKeyGuide.gameObject.SetActive(true);
                    break;
                case KeyGuideType.L:
                    lGuide.Setup(pair.value);
                    lGuide.gameObject.SetActive(true);
                    break;
                case KeyGuideType.M:
                    MGuide.Setup(pair.value);
                    MGuide.gameObject.SetActive(true);
                    break;
                case KeyGuideType.TAB:
                    TabGuide.Setup(pair.value);
                    TabGuide.gameObject.SetActive(true);
                    break;
                case KeyGuideType.SPACE:
                    SpaceGuide.Setup(pair.value);
                    SpaceGuide.gameObject.SetActive(true);
                    break;
            }
        }
    }

    public void Open(Action action = null)
    {
        m_Animator?.PlayOpen(action);

    }

    public void Close(Action action = null)
    {
        if (gameObject.activeSelf) m_Animator?.PlayClose(action);
    }
}

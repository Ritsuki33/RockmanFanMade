using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossSelectData", menuName = "ScriptableObject/CreateBossSelectData")]
public class BossSelectData : ScriptableObject
{
    [SerializeField] public List<PanelInfo> bossInfoList;

    [Serializable]
    public struct PanelInfo
    {
        [SerializeField] public int id;
        [SerializeField] public string panelName;
        [SerializeField] public string panelSprite;

        [SerializeField] public bool selectable;
    }

}

using System;
using UnityEngine;


public class BossSpawn : Spawn<StageBoss>
{
    [SerializeField] string path;

    protected override StageBoss OnGetResource()
    {
        return ObjectManager.Instance.OnLoad<StageBoss>(path);
    }
}

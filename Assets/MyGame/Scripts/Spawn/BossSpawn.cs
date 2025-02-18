using System;
using UnityEngine;


public class BossSpawn : Spawn<StageBoss>
{
    [SerializeField] string path;
    [SerializeField] int id;
    protected override StageBoss OnGetResource()
    {
        return ObjectManager.Instance.OnResouceLoad<StageBoss>(path, id);
    }
}

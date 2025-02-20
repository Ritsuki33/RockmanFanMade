using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawn : Spawn<BaseObject>
{
    [SerializeField] string path;
    [SerializeField] int id;
    protected override BaseObject OnGetResource()
    {
        return ObjectManager.Instance.OnAddressableLoad<BaseObject>(path, id);
    }
}

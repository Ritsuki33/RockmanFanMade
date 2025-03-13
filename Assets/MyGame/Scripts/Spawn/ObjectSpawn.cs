using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawn : Spawn<BaseObject>
{
    [Header("AddressablesロードでIDを指定してオブジェクトの生成")]
    [SerializeField] string path;
    [SerializeField] int id;
    protected override BaseObject OnGetResource()
    {
        return ObjectManager.Instance.OnAddressableLoad<BaseObject>(path, id);
    }
}

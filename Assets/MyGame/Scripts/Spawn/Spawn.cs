using UnityEngine;

public class Spawn : MonoBehaviour
{
    [SerializeField] BaseObject obj;
    IRegister _register = null;

    public BaseObject Obj => obj;

    public virtual void Init(IRegister register)
    {
        _register = register;
    }

    public virtual void Destroy()
    {
        _register = null;
    }

    public virtual void OnReset() { }
    public virtual void OnUpdate() { }

    /// <summary>
    /// Objectをスポーン
    /// </summary>
    public BaseObject SpawnObject()
    {
        if (_register == null)
        {
            Debug.Log("オブジェクト管理用インターフェイスが設定されていないため、スポーン出来ません");
            return null;
        }
        
        obj.transform.position = transform.position;
        obj.gameObject.SetActive(true);
        obj.Setup(() =>
        {
            _register.OnUnregist(obj);
            obj.gameObject.SetActive(false);
        });

        _register?.OnRegist(obj);

        return obj;
    }
}

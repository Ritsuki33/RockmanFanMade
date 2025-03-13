using UnityEngine;

public interface ISpawn
{
    void Initialize();
    void OnUpdate();
    void Terminate();
}

public abstract class Spawn : MonoBehaviour
{
    protected BaseObject obj;
    public BaseObject Obj => obj;
    public abstract void TrySpawnObject();
    public abstract void DeSpawnObject();
}

public abstract class Spawn<T> : Spawn where T : BaseObject
{
    public new T Obj => obj as T;

    public override void TrySpawnObject()
    {
        obj = OnGetResource();

        if (obj == null)
        {
            Debug.Log("リソースを取得できなかったため、スポーン出来ません");
            return;
        }

        InitializeObject();
    }
    public override void DeSpawnObject()
    {
        if (!obj) return;

        obj.Delete();
    }

    abstract protected T OnGetResource();

    virtual protected void InitializeObject()
    {
        obj.transform.position = transform.position;
    }
}

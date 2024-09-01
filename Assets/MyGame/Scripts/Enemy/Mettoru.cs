using UnityEngine;

public class Mettoru : EnemyObject
{
    [SerializeField] MettoruController mettoruController;

    /// <summary>
    /// 初期化
    /// </summary>
    public override void Init()
    {
        base.Init();
        mettoruController.Init();
    }
}

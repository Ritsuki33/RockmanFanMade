using UnityEngine;

public class Mettoru : EnemyObject
{
    [SerializeField] MettoruBehavior mettoruController;

    /// <summary>
    /// 初期化
    /// </summary>
    public override void Init()
    {
        base.Init();
        mettoruController.Init();
    }
}

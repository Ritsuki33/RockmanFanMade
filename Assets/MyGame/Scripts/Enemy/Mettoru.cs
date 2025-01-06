using UnityEngine;

public class Mettoru : EnemyObject
{
    [SerializeField] MettoruBehavior mettoruController;

    /// <summary>
    /// 初期化
    /// </summary>
    protected override void Init()
    {
        base.Init();
        //mettoruController.Init();
    }
}

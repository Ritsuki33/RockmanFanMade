using UnityEngine;

public class Batman : EnemyObject
{
    [SerializeField] BatmanController batmanController;

    public override void Init()
    {
        base.Init();
        batmanController.Init();
    }
}

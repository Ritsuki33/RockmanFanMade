using UnityEngine;

public class Batman : EnemyObject
{
    [SerializeField] BatmanBehavior batmanController;

    protected override void Init()
    {
        base.Init();
        //batmanController.Init();
    }
}

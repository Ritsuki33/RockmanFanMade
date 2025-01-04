using UnityEngine;

public class Batman : EnemyObject
{
    [SerializeField] BatmanBehavior batmanController;

    public override void Init()
    {
        base.Init();
        //batmanController.Init();
    }
}

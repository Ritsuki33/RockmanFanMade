using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : EnemyObject
{
    public override void OnAttacked(Collider2D collision)
    {
        base.OnAttacked(collision);
        UiManager.Instance.HpBar.SetParam((float)currentHp / maxHp);
    }
}

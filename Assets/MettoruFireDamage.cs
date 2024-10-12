using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MettoruFireDamage : DamageBase
{
    public override void TakeDamage(PlayerController player)
    {
        player.Damaged(3);
    }
}

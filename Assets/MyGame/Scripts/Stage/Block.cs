using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{

    //private ExplodePool ExplodePool => EffectManager.Instance.ExplodePool;
    RbCollide rbCollide=new RbCollide();

    private void Awake()
    {
        rbCollide.Init();

        rbCollide.onTriggerEnterRockBusterDamage += OnTriggerEnterRockBusterBase;
    }

    private void OnTriggerEnterRockBusterBase(RockBusterDamage damage)
    {
        Debug.Log("破壊！");
        this.gameObject.SetActive(false);
        var effect = ObjectManager.Instance.OnGet<PsObject>(PoolType.BlockBreakEffect);
        effect.transform.position = this.transform.position;

        damage.projectile.Delete();
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        rbCollide.OnTriggerEnter(collision);
    }
}

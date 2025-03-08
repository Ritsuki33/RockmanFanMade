using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour, IRbVisitor
{
    CachedCollide rbCollide = new CachedCollide();

    private void Awake()
    {
        rbCollide.CacheClear();
    }

    void IRbVisitor.OnTriggerEnter(RockBuster damage)
    {
        this.gameObject.SetActive(false);
        var effect = ObjectManager.Instance.OnGet<PsObject>(PoolType.BlockBreakEffect);
        effect.transform.position = this.transform.position;

        damage.Delete();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        rbCollide.OnTriggerEnter(this, collision);
    }
}

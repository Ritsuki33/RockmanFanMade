using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{

    private ExplodePool ExplodePool => EffectManager.Instance.ExplodePool;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("RockBuster"))
        {
            Destroy(this.gameObject);
            var explode = ExplodePool.Pool.Get();

            explode.transform.position = this.transform.position;
            var rockBuster = collision.gameObject.GetComponent<RockBuster>();
            rockBuster?.Erase();
        }
    }
}

using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PhysicalObject : AnimObject
{
    [SerializeField]protected Rigidbody2D rb;

    protected override void OnPause(bool isPause)
    {
        base.OnPause(isPause);

        rb.simulated = !isPause;
    }

    /// <summary>
    /// 挟まれ判定
    /// </summary>
    /// <param name="position"></param>
    /// <param name="size"></param>
    /// <param name="mask"></param>
    /// <param name="onCrashCallback"></param>
    public bool CrashCheck(Vector2 position, Vector2 size, LayerMask mask)
    {
        RaycastHit2D[] hits = Physics2D.BoxCastAll(position, size, 0, Vector2.down, 0, mask);

        if (hits.Length > 0)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                Vector2 a = hits[i].normal;
                for (int j = i + 1; j < hits.Length; j++)
                {
                    Vector2 b = hits[j].normal;

                    float check = Vector2.Dot(a, b);

                    if (check < 0)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }
}

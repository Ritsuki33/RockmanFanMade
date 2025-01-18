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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Projectile : ReusableObject
{
    [SerializeField] float speedRatio = 0.06f;
    private Vector2 direction = default;

    public void Init(Vector2 direction,Vector2 position,float speed=-1)
    {
        this.direction = direction;
        this.transform.position = position;

        if (speed > 0) speedRatio = speed;
    }

    private void Update()
    {
        this.transform.position += (Vector3)direction * speedRatio;

        if (GameManager.Instance.MainCameraControll.CheckOutOfView(this.gameObject))
        {
            Delete();
        }
    }

    public void Delete()
    {
        Pool.Release(this);
    }
}

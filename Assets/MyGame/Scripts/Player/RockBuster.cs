using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class RockBuster : MonoBehaviour
{
    [SerializeField] float speedRatio = 0.06f;

    private Vector2 direction = default;

    public IObjectPool<RockBuster> Pool { get; set; }

    public void Init(Vector2 direction,Vector2 position)
    {
        this.direction = direction;
        this.transform.position = position;
    }

    private void Update()
    {
        this.transform.position += (Vector3)direction * speedRatio;

        if (GameManager.Instance.MainCameraControll.CheckOutOfView(this.gameObject))
        {
            Pool.Release(this);
        }
    }
}

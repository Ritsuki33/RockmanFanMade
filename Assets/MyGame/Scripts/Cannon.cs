using System;
using UnityEngine;
public class Cannon : MonoBehaviour
{
    public CannonballController cannonballPrefab; // �C�e�̃v���n�u

    BaseObjectPool CannonPool => EffectManager.Instance.CannonPool;

    /// <summary>
    /// �C�e����
    /// </summary>
    /// <param name="targetPosition"></param>
    /// <param name="angle"></param>
    /// <param name="gravitySpeed"></param>
    /// <param name="failed"></param>
    public void FireCannonball(Vector3 targetPosition, float angle, float gravitySpeed, Action failed = null)
    {
        bool isRight = targetPosition.x > transform.position.x;

        // �e�틗���̎Z�o
        float length_x = Mathf.Abs(targetPosition.x - transform.position.x);
        float length_y = targetPosition.y - transform.position.y;

        // ���W�A���ϊ�
        float radian = angle * Mathf.Deg2Rad;

        // �d�͉����x
        float gravity = gravitySpeed / Time.fixedDeltaTime;

        // �p�x���瑬�x���v�Z
        float speed = Mathf.Sqrt(gravity * length_x * length_x / (2 * Mathf.Cos(radian) * Mathf.Cos(radian) * (length_x * Mathf.Tan(radian) - length_y)));

        if (float.IsNaN(speed))
        {
            failed?.Invoke();
            return;
        }

        var cannonball = CannonPool.Pool.Get();
        cannonball.transform.position = transform.position;
        cannonball.GetComponent<CannonballController>().Init(gravitySpeed);
        Rigidbody2D rb = cannonball.GetComponent<Rigidbody2D>();

        if (isRight)
        {
            Vector2 axis = Vector2.right.PositionRotate(angle);

            Vector2 vec = speed * axis;
            rb.velocity += vec;
        }
        else
        {
            Vector2 axis = Vector2.left.PositionRotate(-angle);

            Vector2 vec = speed * axis;
            rb.velocity += vec;
        }
    }
}
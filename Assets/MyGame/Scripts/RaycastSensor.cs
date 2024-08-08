using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;

public class RaycastSensor : MonoBehaviour
{
    [SerializeField, Header("��^�̊p�x")] public float coneAngle = 45f;
    [SerializeField, Header("��^�͈̔�")] public float coneRange = 10f;
    [SerializeField, Header("���˂��郌�C�̐�")] public int rayCount = 10;
    [SerializeField, Header("�^�[�Q�b�g���C���[")] public LayerMask targetmask;
    [SerializeField, Header("�^�[�Q�b�g�^�O��")] public string targetTag;

    RaycastHit2D hitted;

    public void SearchForTargetStay(Vector2 direction, Action<RaycastHit2D> onStayCallback)
    {
        SearchForTarget(direction,
            (hit) =>
            {
                if (hit)
                {
                    onStayCallback?.Invoke(hit);
                }
            });

        
    }


    public void SearchForTargetEnter(Vector2 direction, Action<RaycastHit2D> onEnterCallback)
    {
        SearchForTarget(direction, (hit) =>
        {
            if (hit)
            {
                if (hitted.collider == hit.collider) return;
                // �q�b�g�����ꍇ�̏���
                if (hit.collider.gameObject.CompareTag(targetTag))
                {
                    onEnterCallback?.Invoke(hit);
                }
            }
        });
    }

    public void SearchForTargetExit(Vector2 direction, Action<RaycastHit2D> onExitCallback)
    {
        SearchForTarget(direction,
            (hit) =>
            {
                if (hitted)
                {
                    if (!hit)
                    {
                        onExitCallback?.Invoke(hitted);
                    }
                }
            });

    }

    private void SearchForTarget(Vector2 direction, Action<RaycastHit2D> action)
    {
        float halfAngle = coneAngle / 2;
        RaycastHit2D hit = default;
        for (int i = 0; i < rayCount; i++)
        {
            // �e���C�̊p�x���v�Z
            float angle = -halfAngle + (i * (coneAngle / (rayCount - 1)));
            Vector3 separateDirection = Quaternion.Euler(0, 0, angle) * direction;
            Debug.DrawRay(transform.position, separateDirection * coneRange, Color.red);

            // �f�o�b�O�p�Ƀ��C��`��
            hit = Physics2D.Raycast(transform.position, separateDirection, coneRange, targetmask);

            if (hit) break;
        }

        action(hit);

        hitted = hit;
    }

}

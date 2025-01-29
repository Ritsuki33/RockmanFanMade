using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrashChecker
{
    List<ContactPoint2D> contacts = new List<ContactPoint2D>();

    public Action<ContactPoint2D, ContactPoint2D> onClashCallback;

    public void FixedUpdate()
    {
        if (contacts.Count > 0)
        {
            for (int i = 0; i < contacts.Count; i++)
            {
                Vector2 a = contacts[i].normal;
                for (int j = i + 1; j < contacts.Count; j++)
                {
                    Vector2 b = contacts[j].normal;

                    float check = Vector2.Dot(a, b);

                    if (check < 0)
                    {
                        onClashCallback?.Invoke(contacts[i], contacts[j]);
                    }
                }
            }
            contacts.Clear();
        }
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        // コンタクト情報
        foreach (var contact in collision.contacts)
        {
            contacts.Add(contact);
        }
    }

    public void OnDrawGizmos()
    {
        foreach (var contact in contacts)
        {
            Gizmos.DrawSphere(contact.point, 0.05f);
            Debug.Log(contact.normal);
        }
    }
}

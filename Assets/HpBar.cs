using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpBar : MonoBehaviour
{
    [SerializeField] Transform root;
    List<GameObject> list = new List<GameObject>();

    int maxHp => list.Count;
    int currentHp = 0;

    Coroutine coroutine = null;

    private void Awake()
    {
        // 子オブジェクトをすべて取得
        for (int i = 0; i < root.childCount; i++)
        {
            Transform child = root.GetChild(i);
            child.gameObject.SetActive(false);
            list.Add(child.gameObject);
        }

        currentHp = 0;
    }

    public void SetParam(int val)
    {

        int start = Mathf.Min(currentHp, val);  // 開始位置は小さい方
        int end = Mathf.Max(currentHp, val);    // 終了位置は大きい方
        bool isIncreasing = val > currentHp;    // 増減フラグ

        for (int i = start; i < end && i < maxHp; i++)
        {
            list[i].SetActive(isIncreasing);
        }

        currentHp = val;
    }

    public void ParamChangeAnimation(int val, Action fisnihCallback = null)
    {
        if (coroutine != null) { StopCoroutine(coroutine); }
        coroutine = StartCoroutine(CoParamChangeAnimation(val));

        IEnumerator CoParamChangeAnimation(int val)
        {
            int start = Mathf.Min(currentHp, val);  // 開始位置は小さい方
            int end = Mathf.Max(currentHp, val);    // 終了位置は大きい方
            bool isIncreasing = val > currentHp;    // 増減フラグ

            for (int i = start; i < end && i < maxHp; i++)
            {
                list[i].SetActive(isIncreasing);
                yield return new WaitForSeconds(0.1f);
            }

            currentHp = val;
            fisnihCallback?.Invoke();
            coroutine = null;
        }
    }
}

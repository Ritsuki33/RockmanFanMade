using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpBar : MonoBehaviour
{
    [SerializeField] Transform root;
    List<GameObject> list = new List<GameObject>();

    float currentHp = 0;

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

    /// <summary>
    /// パラメータの更新
    /// </summary>
    /// <param name="val">0～1の範囲</param>
    public void SetParam(float val)
    {

        int start = (int)(Mathf.Min(currentHp, val) * list.Count);  // 開始位置は小さい方
        int end = (int)(Mathf.Max(currentHp, val) * list.Count);    // 終了位置は大きい方
        bool isIncreasing = val > currentHp;    // 増減フラグ

        for (int i = start; i < end && i < list.Count; i++)
        {
            list[i].SetActive(isIncreasing);
        }

        currentHp = val;
    }

    /// <summary>
    /// パラメータの更新アニメーション
    /// </summary>
    /// <param name="val"></param>
    /// <param name="fisnihCallback"></param>
    public void ParamChangeAnimation(float val, Action fisnihCallback = null)
    {
        if (coroutine != null) { StopCoroutine(coroutine); }
        coroutine = StartCoroutine(CoParamChangeAnimation(val));

        IEnumerator CoParamChangeAnimation(float val)
        {
            int start = (int)(Mathf.Min(currentHp, val) * list.Count);  // 開始位置は小さい方
            int end = (int)(Mathf.Max(currentHp, val) * list.Count);    // 終了位置は大きい方
            bool isIncreasing = val > currentHp;    // 増減フラグ

            for (int i = start; i < end && i < list.Count; i++)
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

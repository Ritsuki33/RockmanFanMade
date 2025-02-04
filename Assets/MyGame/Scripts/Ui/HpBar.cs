using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpBar : MonoBehaviour
{
    [SerializeField] Transform root;
    List<GameObject> list = new List<GameObject>();

    float hpParam = 0;  // 現在表示中のHP
    float realHp = 0;   // 実質HP
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

        hpParam = 0;
        realHp = 0;
    }

    /// <summary>
    /// パラメータの更新(アニメーションなし)
    /// </summary>
    /// <param name="val">0～1の範囲</param>
    public void SetParam(float val)
    {

        int start = (int)Mathf.Ceil((Mathf.Min(hpParam, val) * list.Count));  // 開始位置は小さい方
        int end = (int)Mathf.Ceil((Mathf.Max(hpParam, val) * list.Count));    // 終了位置は大きい方
        bool isIncreasing = val > hpParam;    // 増減フラグ

        for (int i = start; i < end && i < list.Count; i++)
        {
            if (i >= 0) list[i].SetActive(isIncreasing);
        }
        realHp = val;
        hpParam = val;
    }


    /// <summary>
    /// パラメータの更新アニメーション
    /// </summary>
    /// <param name="val"></param>
    /// <param name="fisnihCallback"></param>
    public void ParamChangeAnimation(float val, Action fisnihCallback = null)
    {
        if (coroutine != null) { StopCoroutine(coroutine); }
        coroutine = StartCoroutine(CoParamChangeAnimation());

        IEnumerator CoParamChangeAnimation()
        {
            realHp = val;
            int start = (int)(Mathf.Min(hpParam, realHp) * list.Count);  // 開始位置は小さい方
            int end = (int)(Mathf.Max(hpParam, realHp) * list.Count);    // 終了位置は大きい方
            bool isIncreasing = realHp > hpParam;    // 増減フラグ

            for (int i = start; i < end && i < list.Count; i++)
            {
                list[i].SetActive(isIncreasing);
                yield return new WaitForSeconds(0.1f);
            }

            hpParam = val;
            fisnihCallback?.Invoke();
            coroutine = null;
        }
    }
}

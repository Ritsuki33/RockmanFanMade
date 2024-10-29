using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class LetterRevealText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    IEnumerator enumerator = null;

    public bool IsPlaying => enumerator != null && !isPause;
    bool isPause = false;

    public void Init()
    {
        text.maxVisibleCharacters = 0;
    }

    /// <summary>
    /// プレイ
    /// </summary>
    /// <param name="delay"></param>
    /// <param name="callback"></param>
    public void Play(float delay, Action callback = null)
    {
        enumerator = LetterRevealCo(delay, callback);
        StartCoroutine(enumerator);
        isPause = false;
    }

    /// <summary>
    /// 再開
    /// </summary>
    public void Resume()
    {
        if(enumerator!=null) StartCoroutine(enumerator);
        isPause = false;
    }

    /// <summary>
    /// ポーズ
    /// </summary>
    public void Pause()
    {
        if (enumerator != null) StopCoroutine(enumerator);
        isPause = true;
    }

    /// <summary>
    /// 停止
    /// </summary>
    public void Stop()
    {
        if (enumerator != null)
        {
            StopCoroutine(enumerator);
            enumerator = null;
        }

        isPause = false;
    }

    /// <summary>
    /// テキストタイピング
    /// </summary>
    /// <param name="delay"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    IEnumerator LetterRevealCo(float delay, Action callback = null)
    {
        for (int i = 0; i <= text.text.Length; i++)
        {
            text.maxVisibleCharacters = i;

            yield return new WaitForSeconds(delay);
        }

        callback?.Invoke();

        enumerator = null;
    }
}

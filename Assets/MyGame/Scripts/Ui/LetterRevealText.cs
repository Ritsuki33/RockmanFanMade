using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class LetterRevealText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    IEnumerator enumerator = null;

    bool IsPlaying => enumerator != null;
    bool isPause = false;
    /// <summary>
    /// プレイ
    /// </summary>
    /// <param name="delay"></param>
    /// <param name="callback"></param>
    public void Play(float delay, Action callback = null)
    {
        enumerator = LetterRevealCo(delay);
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

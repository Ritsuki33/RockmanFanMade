using System.Collections;
using UnityEngine;

public class ReadyUi : MonoBehaviour
{
    [SerializeField] CanvasGroup ready;
    int blinkingNum = 2;
    float showTime = 0.25f;
    float unshowTime = 0.15f;
    float finishTime = 1f;

    Coroutine coroutine = default;

    public bool IsBlinking => coroutine != null;

    private void Awake()
    {
        ready.alpha = 0;
    }

    public void Play()
    {
        coroutine = StartCoroutine(PlayCo());
    }

    private IEnumerator PlayCo()
    {
        ready.alpha = 0;

        int count = 0;
        while (count < blinkingNum)
        {
            ready.alpha = 1;
            yield return new WaitForSeconds(showTime);
            ready.alpha = 0;
            yield return new WaitForSeconds(unshowTime);
            count++;
        }

        ready.alpha = 1;

        yield return new WaitForSeconds(finishTime);

        ready.alpha = 0;

        coroutine = null;
    }
}

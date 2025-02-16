using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeParticleSystem : PsObject
{
    private Coroutine coroutine = null;

    /// <summary>
    /// 爆発パーティクルシステムの実行
    /// </summary>
    /// <param name="size">エリアサイズ</param>
    /// <param name="duration">再生時間</param>
    /// <param name="rateOverTime">1秒間に発射するエフェクト数</param>
    /// <param name="callback">再生終了後</param>
    public void Play(Vector2 size, float duration,float rateOverTime, Action callback)
    {
        coroutine = StartCoroutine(CoPlay());

        IEnumerator CoPlay()
        {
            // １回の発射に掛かる時間の計算
            float emitPerSecond = 1.0f / rateOverTime;
            float curTime = 0;

            // パーティクルシステムの領域変更
            var shape = ParticleSystem.shape;
            shape.scale = size;
            while (curTime < duration)
            {
                // 発射
                ParticleSystem.Emit(1);
                AudioManager.Instance.PlaySe(SECueIDs.explosion);

                yield return PauseManager.Instance.PausableWaitForSeconds(emitPerSecond);

                curTime += Time.deltaTime + emitPerSecond;
                Debug.Log(curTime);
            }

            callback?.Invoke();

            coroutine = null;
            Delete();
        }
    }

    protected override void Destroy()
    {
        if (coroutine != null) StopCoroutine(coroutine);
    }
}

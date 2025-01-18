using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : SingletonComponent<PauseManager>
{
    public class WaitForSeconds : CustomYieldInstruction
    {
        private float waitTime;
        PauseManager coroutineManager;
        float currentTime = 0;

        private WaitForSeconds(PauseManager manager, float seconds)
        {
            coroutineManager = manager;
            waitTime = seconds;
            currentTime = 0;
        }

        internal static WaitForSeconds Create(PauseManager manager, float seconds)
        {
            return new WaitForSeconds(manager, seconds);
        }

        public override bool keepWaiting
        {
            get
            {
                // ポーズ中はカウントしない
                if (!coroutineManager._isPause) currentTime += Time.deltaTime;
                return waitTime > currentTime;
            }
        }
    }

    bool _isPause = false;

    public WaitForSeconds PausableWaitForSeconds(float seconds)
    {
        return WaitForSeconds.Create(this, seconds);
    }

    public void OnPause(bool isPause)
    {
        _isPause = isPause;
    }

}


using CriWare;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioManager : SingletonComponent<AudioManager>
{
    [SerializeField] GameObject audioSources;
    [SerializeField, Header("SEのキューシート")] string seCueSheetName = "CueSheet_0";
    [SerializeField, Header("BGMのキューシート")] string bgmCueSheetName = "CueSheet_1";
    [SerializeField, Header("システムのキューシート")] string systemCueSheetName = "CueSheet_1";

    private CriAtomSource seSoundSource;
    private CriAtomSource bgmSoundSource;
    private CriAtomSource systemSoundSource;

    Dictionary<CriAtomExPlayback, Action> registeredFinishCallbacks = new Dictionary<CriAtomExPlayback, Action>();

    protected override void Awake()
    {
        base.Awake();

        seSoundSource = audioSources.AddComponent<CriAtomSource>();
        seSoundSource.cueSheet = seCueSheetName;
        bgmSoundSource = audioSources.AddComponent<CriAtomSource>();
        bgmSoundSource.cueSheet = bgmCueSheetName;
        systemSoundSource = audioSources.AddComponent<CriAtomSource>();
        systemSoundSource.cueSheet = systemCueSheetName;
    }

    public void Update()
    {
        // 登録された終了コールバックをスキャン
        for (int i = registeredFinishCallbacks.Count - 1; i >= 0; i--)
        {
            var playback = registeredFinishCallbacks.Keys.ElementAt(i);

            if (playback.status == CriAtomExPlayback.Status.Removed)
            {
                var callback = registeredFinishCallbacks[playback];
                callback?.Invoke();

                registeredFinishCallbacks.Remove(playback);
            }
        }
    }

    // SE再生
    public void PlaySe(string cue, Action finishCallback = null)
    {
        var playback = seSoundSource.Play(cue);

        if (finishCallback != null) registeredFinishCallbacks.Add(playback, finishCallback);
    }

    public void PlaySe(int id, Action finishCallback = null)
    {
        var playback = seSoundSource.Play(id);

        if (finishCallback != null) registeredFinishCallbacks.Add(playback, finishCallback);
    }

    // BGM再生
    public void PlayBgm(string cue, bool loop = true, Action finishCallback = null)
    {
        bgmSoundSource.loop = loop;
        var playback = bgmSoundSource.Play(cue);

        if (finishCallback != null) registeredFinishCallbacks.Add(playback, finishCallback);
    }

    public void PlayBgm(int id, bool loop = true, Action finishCallback = null)
    {
        bgmSoundSource.loop = loop;
        var playback = bgmSoundSource.Play(id);

        if (finishCallback != null) registeredFinishCallbacks.Add(playback, finishCallback);
    }

    // システム音再生
    public void PlaySystem(string cue, Action finishCallback = null)
    {
        var playback = systemSoundSource.Play(cue);

        if (finishCallback != null) registeredFinishCallbacks.Add(playback, finishCallback);
    }

    public void PlaySystem(int id, Action finishCallback = null)
    {
        // 再生中のものがあれば止める
        StopSystem();

        var playback = systemSoundSource.Play(id);

        if (finishCallback != null) registeredFinishCallbacks.Add(playback, finishCallback);
    }

    public void StopBGM() => bgmSoundSource.Stop();
    public void StopSystem() => systemSoundSource.Stop();

    /// <summary>
    /// SEとBGMを一時停止
    /// </summary>
    /// <param name="isPause"></param>
    public void OnPause(bool isPause)
    {
        seSoundSource.Pause(isPause);
        bgmSoundSource.Pause(isPause);
    }

    /// <summary>
    /// SEの一時停止
    /// </summary>
    /// <param name="isPause"></param>
    public void OnPauseSe(bool isPause)
    {
        seSoundSource.Pause(isPause);
    }
}

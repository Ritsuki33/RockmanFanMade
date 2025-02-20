using CriWare;
using CriWare.Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioManager : SingletonComponent<AudioManager>
{
    [SerializeField] GameObject audioSources;
    [SerializeField, Header("BGMのキューシート")] string bgmCueSheetName = "CueSheet_1";
    [SerializeField, Header("SEのキューシート")] string seCueSheetName = "CueSheet_0";
    [SerializeField, Header("システムのキューシート")] string systemCueSheetName = "CueSheet_1";

    //[SerializeField]private CriAtomSourceForAsset bgmSoundSource;
    //[SerializeField]private CriAtomSourceForAsset seSoundSource;
    //[SerializeField]private CriAtomSourceForAsset systemSoundSource;

    Dictionary<CriAtomExPlayback, Action> registeredFinishCallbacks = new Dictionary<CriAtomExPlayback, Action>();

    SoundPlayHandler bgmHandler;
    SoundPlayHandler seHandler;
    SoundPlayHandler systemHandler;

    public IEnumerator Configure()
    {
        bgmHandler = new SoundPlayHandler(false);
        seHandler = new SoundPlayHandler(false);
        systemHandler = new SoundPlayHandler(false);

        bgmHandler.LoadSoundSource(bgmCueSheetName);
        seHandler.LoadSoundSource(seCueSheetName);
        systemHandler.LoadSoundSource(systemCueSheetName);

        while (!bgmHandler.Loaded || !seHandler.Loaded|| !systemHandler.Loaded) yield return null;
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
    public CriAtomExPlayback PlaySe(string cue, Action finishCallback = null)
    {
        CriAtomExPlayback playback = seHandler.Play(cue);

        if (finishCallback != null) registeredFinishCallbacks.Add(playback, finishCallback);
        return playback;
    }

    public CriAtomExPlayback PlaySe(int id, Action finishCallback = null)
    {
        CriAtomExPlayback playback = seHandler.Play(id);
        if (finishCallback != null) registeredFinishCallbacks.Add(playback, finishCallback);
        return playback;
    }

    // BGM再生
    public CriAtomExPlayback PlayBgm(string cue, Action finishCallback = null)
    {
        var playback = bgmHandler.Play(cue);

        if (finishCallback != null) registeredFinishCallbacks.Add(playback, finishCallback);
        return playback;
    }

    public CriAtomExPlayback PlayBgm(int id, Action finishCallback = null)
    {
        var playback = bgmHandler.Play(id);

        if (finishCallback != null) registeredFinishCallbacks.Add(playback, finishCallback);
        return playback;
    }

    // システム音再生
    public CriAtomExPlayback PlaySystem(string cue, Action finishCallback = null)
    {
        var playback = systemHandler.Play(cue);

        if (finishCallback != null) registeredFinishCallbacks.Add(playback, finishCallback);
        return playback;
    }

    public CriAtomExPlayback PlaySystem(int id, Action finishCallback = null)
    {
        // 再生中のものがあれば止める
        StopSystem();

        var playback = systemHandler.Play(id);

        if (finishCallback != null) registeredFinishCallbacks.Add(playback, finishCallback);
        return playback;
    }

    public void StopSe() => seHandler.Stop();
    public void StopBGM() => bgmHandler.Stop();
    public void StopSystem() => systemHandler.Stop();

    /// <summary>
    /// SEとBGMを一時停止
    /// </summary>
    /// <param name="isPause"></param>
    public void OnPause(bool isPause)
    {
        seHandler.Pause(isPause);
        bgmHandler.Pause(isPause);
    }

    /// <summary>
    /// SEの一時停止
    /// </summary>
    /// <param name="isPause"></param>
    public void OnPauseSe(bool isPause)
    {
        seHandler.Pause(isPause);
    }
}

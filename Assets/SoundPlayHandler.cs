using CriWare;
using CriWare.Assets;
using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;

/// <summary>
/// サウンド再生用ハンドラー
/// </summary>
public class SoundPlayHandler
{
    private CriAtomExPlayer m_player;
    private CriAtomAcbAsset m_crtAtomAbcAsset;
    private CriAtomExPlayback m_playback;

    /// <summary>
    /// ロード中かどうか
    /// </summary>
    public bool Loaded => m_crtAtomAbcAsset != null && m_crtAtomAbcAsset.Loaded;

    public SoundPlayHandler(bool enableAudioSyncedTimer)
    {
        m_player = new CriAtomExPlayer(enableAudioSyncedTimer);
    }

    /// <summary>
    /// サウンドソースのロード
    /// </summary>
    /// <param name="_address"></param>
    public void LoadSoundSource(string _address)
    {
        ReleaseSoundSource();

        Addressables.LoadAssetAsync<CriAtomAcbAsset>(_address).Completed += (handle) =>
        {
            if (handle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
            {
                m_crtAtomAbcAsset = handle.Result;

                // 音源登録
                CriAtomAssetsLoader.AddCueSheet(m_crtAtomAbcAsset);
            }
            else
            {
                Debug.LogError("Failed To Get CriAtomAcbAsset Addressable Load!!");
            }
        };
    }

    /// <summary>
    /// サウンドのリリース
    /// </summary>
    public void ReleaseSoundSource()
    {
        if (m_crtAtomAbcAsset != null)
        {
            // 音源削除
            CriAtomAssetsLoader.ReleaseCueSheet(m_crtAtomAbcAsset);

            // リリース
            Addressables.Release(m_crtAtomAbcAsset);
        }

        m_crtAtomAbcAsset = null;
    }

    // サウンドの事前ロード
    public IEnumerator PreparePlay(int cueId)
    {
        // キューシートの読み込み待ち
        while (m_crtAtomAbcAsset == null || !m_crtAtomAbcAsset.Loaded)
        {
            yield return null;
        }

        m_player.SetCue(m_crtAtomAbcAsset.Handle, cueId);

        m_player.SetStartTime(0);

        m_playback = m_player.Prepare();

        while (m_playback.GetStatus() != CriAtomExPlayback.Status.Playing)
        {
            yield return null;
        }
    }

    public void Play(CriAtomEx.ResumeMode resumeMode)
    {
        m_player.SetStartTime(0);
        m_playback.Resume(resumeMode);
    }

    public CriAtomExPlayback Play(int cueId)
    {
        if (m_crtAtomAbcAsset.Handle == null)
        {
            Debug.LogError("CriAtomAcbAsset are not Load!!");
            return default;
        }

        m_player.SetCue(m_crtAtomAbcAsset.Handle, cueId);

        m_player.SetStartTime(0);
        return m_player.Start();
    }

    public CriAtomExPlayback Play(string cueName)
    {
        if (m_crtAtomAbcAsset.Handle == null)
        {
            Debug.LogError("CriAtomAcbAsset are not Load!!");
            return default;
        }

        m_player.SetCue(m_crtAtomAbcAsset.Handle, cueName);

        m_player.SetStartTime(0);
        return m_player.Start();
    }

    public void Stop()
    {
        m_player.Stop();
    }

    public void Pause(bool isPause)
    {
        m_player.Pause(isPause);
    }
}

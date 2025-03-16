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
    private CriAtomAcbAsset m_criAtomAbcAsset;
    private CriAtomExPlayback m_playback;

    public CriAtomExAcb Acb => m_criAtomAbcAsset ? m_criAtomAbcAsset.Handle : null;
    /// <summary>
    /// ロード中かどうか(ロードに失敗した場合は諦めてもらう)
    /// </summary>
    public bool Loaded => !isFailed ? m_criAtomAbcAsset != null && m_criAtomAbcAsset.Loaded : true;

    private bool isFailed = false;
    public SoundPlayHandler(bool enableAudioSyncedTimer)
    {
        m_player = new CriAtomExPlayer(enableAudioSyncedTimer);
        isFailed = false;
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
                m_criAtomAbcAsset = handle.Result;

                // 音源登録
                CriAtomAssetsLoader.AddCueSheet(m_criAtomAbcAsset);
            }
            else
            {
                Debug.LogError("Failed To Get CriAtomAcbAsset Addressable Load!!");
                isFailed = true;
            }
        };
    }

    /// <summary>
    /// サウンドのリリース
    /// </summary>
    public void ReleaseSoundSource()
    {
        if (m_criAtomAbcAsset != null)
        {
            // 音源削除
            CriAtomAssetsLoader.ReleaseCueSheet(m_criAtomAbcAsset);

            // リリース
            Addressables.Release(m_criAtomAbcAsset);
        }

        m_criAtomAbcAsset = null;
    }

    // サウンドの事前ロード
    public IEnumerator PreparePlay(int cueId)
    {
        // キューシートの読み込み待ち
        while (m_criAtomAbcAsset == null || !m_criAtomAbcAsset.Loaded)
        {
            yield return null;
        }

        m_player.SetCue(m_criAtomAbcAsset.Handle, cueId);

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
        if (m_criAtomAbcAsset == null || m_criAtomAbcAsset.Handle == null)
        {
            Debug.LogError("CriAtomAcbAsset are not Load!!");
            return default;
        }

        m_player.SetCue(m_criAtomAbcAsset.Handle, cueId);

        m_player.SetStartTime(0);
        return m_player.Start();
    }

    public CriAtomExPlayback Play(string cueName)
    {
        if (m_criAtomAbcAsset.Handle == null)
        {
            Debug.LogError("CriAtomAcbAsset are not Load!!");
            return default;
        }

        m_player.SetCue(m_criAtomAbcAsset.Handle, cueName);

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

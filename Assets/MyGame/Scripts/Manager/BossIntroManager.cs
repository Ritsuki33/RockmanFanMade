using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class BossIntroManager : MonoBehaviour
{
    readonly string path = "Prefabs/BossIntro";
    readonly string modelTrackName = "BossModel Track";
    readonly string modelAnimTrackName = "BossAnimation Track";
    readonly string pauseClipName = "Pause";

    [SerializeField] PlayableDirector director;
    [SerializeField] GameObject bossHolder;

    [SerializeField] TextMeshProUGUI text;
    Action _finishCallback;

    Animator modelData = default;
    Animator model = default;
    void Start()
    {
        // PlayableDirectorの終了時にOnPlayableDirectorStoppedを実行する
        director.stopped += OnPlayableDirectorStopped;
    }

    void OnDestroy()
    {
        // イベントの登録を解除（不要な参照を避けるため）
        director.stopped -= OnPlayableDirectorStopped;
    }

    public void Setup(Animator modelData)
    {
        this.modelData = modelData;
    }

    public void Play(Action finishCallback)
    {
        try
        {
            model = Instantiate(modelData, bossHolder.transform);

            var trackBindings = director.playableAsset.outputs;
            if (model == null)
            {
                throw new InvalidOperationException($"オブジェクトからAnimatorコンポーネントを取得できませんでした。(Object Name={model.name})");
            }

            foreach (var binding in trackBindings)
            {
                if (binding.streamName == modelTrackName)  // Timeline上のオブジェクト名
                {
                    director.SetGenericBinding(binding.sourceObject, model);
                }
                else if (binding.streamName.Equals(modelAnimTrackName))
                {
                    director.SetGenericBinding(binding.sourceObject, model);
                }
            }

            // Animator Controller から AnimationClip を取得
            AnimationClip[] clips = model.runtimeAnimatorController.animationClips;

            AnimationClip pauseClip = clips.Where(clip => clip.name.Equals(pauseClipName)).DefaultIfEmpty(null).First();

            if (pauseClip == null)
            {
                throw new InvalidOperationException($"指定したモデルからポーズクリップを取得できませんでした。(Clip Name={pauseClipName})");
            }

            // Timelineのアセットを取得
            TimelineAsset timeline = (TimelineAsset)director.playableAsset;

            foreach (var track in timeline.GetOutputTracks())
            {
                if (track is AnimationTrack animTrack && track.name.Equals(modelAnimTrackName))  // アニメーション用のトラックを探す
                {
                    // AnimationTrackのClipを差し替える
                    foreach (TimelineClip clip in animTrack.GetClips())
                    {
                        if (clip.asset is AnimationPlayableAsset animPlayable)
                        {
                            animPlayable.clip = pauseClip;  // 新しいクリップに差し替え
                        }
                    }
                }
            }
            this._finishCallback = finishCallback;
            director.Play();
        }
        catch (InvalidOperationException e)
        {
            finishCallback.Invoke();
            if (model != null) Destroy(model.gameObject);
            Debug.LogError(e.Message);
        }
    }

    public void Terminate()
    {
        if (model != null) Destroy(model.gameObject);
    }

    void OnPlayableDirectorStopped(PlayableDirector aDirector)
    {
        if (director == aDirector)
        {
            this._finishCallback?.Invoke();
        }
    }
}

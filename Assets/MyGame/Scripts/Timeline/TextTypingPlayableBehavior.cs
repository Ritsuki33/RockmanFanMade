using System;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

[Serializable]
public class TextTypingPlayableBehavior : PlayableBehaviour
{
    public ExposedReference<TextMeshProUGUI> targetText;

    private TextMeshProUGUI text;
    int length = 0;
    public override void OnGraphStart(Playable playable)
    {
        // `targetText`（ExposedReference<TextMeshProUGUI>）を解決して、
        // Playableグラフ内のリゾルバ（PlayableGraph.GetResolver）を使用して
        // 実際のTextMeshProUGUIインスタンスを取得します。
        // これにより、アニメーション中に対象のTextMeshProUGUIコンポーネントにアクセスできるようになる。
        if (text==null) text = targetText.Resolve(playable.GetGraph().GetResolver());
        text.maxVisibleCharacters = 0;
        length = text.text.Length;
    }

    public override void PrepareFrame(Playable playable, FrameData info)
    {
        double time = playable.GetTime();
        double duration = playable.GetDuration();
        float ratio = (float)(time / duration);

        int targetVisibleCharacters = (int)(ratio * length);

        // 整数値が変わったときだけmaxVisibleCharactersを更新
        if (text.maxVisibleCharacters != targetVisibleCharacters)
        {
            text.maxVisibleCharacters = targetVisibleCharacters;
        }
    }

    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        // クリップ最後はすべて文字を表示する
        text.maxVisibleCharacters = length;
    }
}

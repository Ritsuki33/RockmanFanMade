using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TextTypingPlayableAsset : PlayableAsset
{
    public TextTypingPlayableBehavior template = new TextTypingPlayableBehavior();
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        return ScriptPlayable<TextTypingPlayableBehavior>.Create(graph, template);
    }
}

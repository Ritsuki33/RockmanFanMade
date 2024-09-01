using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ShaderPropertyId
{
    static readonly public int IsFadeColorID = Shader.PropertyToID("_IsFadeColor");
    static readonly public int RimLightColorId = Shader.PropertyToID("_RimLightColor");
    static readonly public int FadeLightId = Shader.PropertyToID("_FadeLight");
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class UiManager : SingletonComponent<UiManager>
{
    [SerializeField] ReadyUi readyUi = default;
    [SerializeField] FadeInManager fadeInManager = default;

    public ReadyUi ReadyUi => readyUi;
    public FadeInManager FadeInManager => fadeInManager;
}

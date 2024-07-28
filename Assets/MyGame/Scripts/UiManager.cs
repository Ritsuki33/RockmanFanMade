using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : SingletonComponent<UiManager>
{
    [SerializeField] ReadyUi readyUi = default;

    public ReadyUi ReadyUi => readyUi;
}

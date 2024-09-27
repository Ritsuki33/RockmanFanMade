using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : StageObject
{
    [SerializeField] PlayerController playerController = default;


    private void OnEnable()
    {
        EventTriggerManager.Instance.Subscribe(EventType.ChangeCameraStart, PlayerPause);
        EventTriggerManager.Instance.Subscribe(EventType.ChangeCameraEnd, PlayerPuaseCancel);
    }

    private void OnDisable()
    {
        EventTriggerManager.Instance.Unsubscribe(EventType.ChangeCameraStart, PlayerPause);
        EventTriggerManager.Instance.Unsubscribe(EventType.ChangeCameraEnd, PlayerPuaseCancel);
    }

    /// <summary>
    /// プレイヤーのポーズ
    /// </summary>
    public void PlayerPause()
    {
        playerController.enabled = false;
    }

    /// <summary>
    /// プレイヤーのポーズキャンセル（一つ前の状態に戻す）
    /// </summary>
    public void PlayerPuaseCancel()
    {
        playerController.enabled = true;
    }

    public void AutoMoveTowards(Transform bamili,Action actionFinishCallback)
    {
        playerController.AutoMoveTowards(bamili, actionFinishCallback);
    }

    public void InputPermission()
    {
        playerController.InputPermission();
    }
}

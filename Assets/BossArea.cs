using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossArea : BaseArea
{
    [SerializeField] BossController boss = default;
    [SerializeField] Transform bamili = default;

    private void Awake()
    {
        boss.gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        EventTriggerManager.Instance.Subscribe(EventType.EnterArea, PlayerMove);
        EventTriggerManager.Instance.Subscribe(EventType.EnemyPauseEnd, HpBarSet);
        EventTriggerManager.Instance.Subscribe(EventType.PlayerMoveEnd, BossAppeare);
        EventTriggerManager.Instance.Subscribe(EventType.HpBarSetEnd, BattleStart);
    }

    private void OnDisable()
    {
        EventTriggerManager.Instance.Unsubscribe(EventType.EnterArea, PlayerMove);
        EventTriggerManager.Instance.Unsubscribe(EventType.EnemyPauseEnd, HpBarSet);
        EventTriggerManager.Instance.Unsubscribe(EventType.PlayerMoveEnd, BossAppeare);
        EventTriggerManager.Instance.Unsubscribe(EventType.HpBarSetEnd, BattleStart);
    }

    void PlayerMove()
    {
        GameManager.Instance.Player.AutoMoveTowards(bamili);
    }


    private void BossAppeare()
    {
        boss.Appeare();
    }

    void HpBarSet()
    {
        StartCoroutine(CoHpBarSet());

        IEnumerator CoHpBarSet()
        {
            UiManager.Instance.HpBar.gameObject.SetActive(true);
            UiManager.Instance.HpBar.SetParam(0.0f);
            bool finished = false;
            UiManager.Instance.HpBar.ParamChangeAnimation(1.0f, () => { finished = true; });
            while(finished)yield return null;

            EventTriggerManager.Instance.Notify(EventType.HpBarSetEnd);
        }
    }

    void BattleStart()
    {
        GameManager.Instance.Player.InputPermission();
    }
}

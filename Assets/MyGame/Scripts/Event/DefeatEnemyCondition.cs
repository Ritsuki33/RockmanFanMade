using System;
using UnityEngine;

/// <summary>
/// 条件クラス　敵関係のベース
/// </summary>
[Serializable]
public abstract class BaseEnemyCondition
{
    [SerializeField,Header("条件達成後、発生するイベント")] protected ActionChainExecuter actionChainExecuter = default;

    /// <summary>
    /// 撃破メソッド
    /// </summary>
    /// <param name="target"></param>
    public void Defeated(EnemyObject target)
    {
        if (!ValidateConditions(target)) return;

        actionChainExecuter.StartEvent();

        EventTriggerManager.Instance.EenemyEventTriggers.Unsubscribe(EnemyEventType.Defeated, Defeated);
    }

    /// <summary>
    /// 条件達成の確認
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public abstract bool ValidateConditions(EnemyObject target);
}

/// <summary>
/// 条件、ターゲットの敵を倒す
/// </summary>
[Serializable]
public class DefeatTargetEnemyCondition : BaseEnemyCondition
{
    [SerializeField,Header("討伐対象の敵リスト")] EnemyObject[] targets = default;
    int count = 0;

    public override bool ValidateConditions(EnemyObject target)
    {
        // 指定した敵の討伐数カウント
        foreach (EnemyObject t in targets)
        {
            if (t == target)
            {
                count++;
            }
        }

        return count >= targets.Length;
    }
}

/// <summary>
/// 条件、一定数撃破
/// </summary>
[Serializable]
public class DefeatEnemyNunCondition : BaseEnemyCondition
{
    [SerializeField] int defeatNum;
    int count = 0;
  
    public override bool ValidateConditions(EnemyObject target)
    {
        // 討伐数カウント
        count++;

        return count > defeatNum;
    }
}
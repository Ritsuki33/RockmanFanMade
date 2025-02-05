using System.Linq;
using UnityEngine;

public static class Probability
{
    /// <summary>
    /// 確率でtrueを返す
    /// </summary>
    /// <param name="probability"></param>
    /// <returns></returns>
    public static bool GetBoolean(double probability)
    {
        if (probability < 0)
        {
            // 常にfalse
            return false;

        }
        else if(probability > 1)
        {
            // 常にtrue
            return true;
        }

        return Random.value < probability;
    }

    /// <summary>
    /// 確率でメソッド分岐
    /// </summary>
    /// <param name="actions"></param>
    public static void BranchMethods(params (float, System.Action)[] actions)
    {
        // 重みの合計値
        float sumProbability = actions.Aggregate(0f, (acc, x) => acc + x.Item1); ;

        float randomValue = Random.value;
        float probability = 0;
        foreach (var action in actions)
        {
            probability += action.Item1 / sumProbability;

            if (randomValue <= probability)
            {
                action.Item2?.Invoke();
                break;
            }
        }
    }
}

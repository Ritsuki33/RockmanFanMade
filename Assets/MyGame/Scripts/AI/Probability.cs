using System.Linq;
using UnityEngine;

public static class Probability
{
    /// <summary>
    /// �m����true��Ԃ�
    /// </summary>
    /// <param name="probability"></param>
    /// <returns></returns>
    public static bool GetBoolean(double probability)
    {
        if (probability < 0)
        {
            // ���false
            return false;

        }
        else if(probability > 1)
        {
            // ���true
            return true;
        }

        return Random.value < probability;
    }

    /// <summary>
    /// �m���Ń��\�b�h����
    /// </summary>
    /// <param name="actions"></param>
    public static void BranchMethods(params (float, System.Action)[] actions)
    {
        // �d�݂̍��v�l
        float sumProbability = actions.Aggregate(0f, (acc, x) => acc + x.Item1); ;

        float randomValue = Random.value;
        float probability = 0;
        foreach (var action in actions)
        {
            probability += action.Item1 / sumProbability;

            if (randomValue <= probability)
            {
                action.Item2.Invoke();
                break;
            }
        }
    }
}

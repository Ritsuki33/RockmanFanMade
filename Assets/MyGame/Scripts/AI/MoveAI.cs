using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MoveAI 
{
    /// <summary>
    /// ある値を境界線として超えたかどうか
    /// </summary>
    /// <param name="preVal"></param>
    /// <param name="curVal"></param>
    /// <param name="targetVal"></param>
    /// <returns></returns>
    public static bool IsPassedParam(float preVal,float curVal,float targetVal)
    {
        return (preVal < targetVal && curVal >= targetVal) || (preVal > targetVal && curVal <= targetVal);
    }
}

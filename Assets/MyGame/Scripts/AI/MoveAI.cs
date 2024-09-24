using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MoveAI 
{
    public static bool IsPassedParam(float preVal,float curVal,float targetVal)
    {
        return (preVal < targetVal && curVal >= targetVal) || (preVal > targetVal && curVal <= targetVal);
    }
}

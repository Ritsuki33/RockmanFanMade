using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class VirticalSelectContoller<T, D> : SelectController<T, D> where T : BaseSelect<D>
{
    /// <summary>
    /// コントローラー
    /// </summary>
    /// <param name="info"></param>
    public override void InputUpdate(InputInfo info)
    {
        int next= currentIndex;
        if (info.up)
        {
            next -= 1;
        }
        else if (info.down)
        {
            next += 1;
        }

        if (next != currentIndex && (0 <= next && next < selects.Count))
        {
            UpdateCursor(next);
        }

        if (info.decide)
        {
            selects[currentIndex].Selected();
        }
    }
}

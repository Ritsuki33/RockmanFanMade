using UnityEngine;
using UnityEngine.UI;

public abstract class GridSelectController<T, D> : SelectController<T, D> where T : BaseSelect<D>
{
    [SerializeField] GridLayoutGroup gridLayoutGroup = default;

    int ColumnCount => gridLayoutGroup.constraintCount;

    /// <summary>
    /// コントローラー
    /// </summary>
    /// <param name="info"></param>
    public override void InputUpdate(InputInfo info)
    {
        int next = currentIndex;
        if (info.up)
        {
            next -= ColumnCount;
        }
        else if (info.down)
        {
            next += ColumnCount;
        }
        else if (info.left)
        {
            next -= 1;
        }
        else if (info.right)
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

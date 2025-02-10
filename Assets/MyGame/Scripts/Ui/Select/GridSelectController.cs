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
    public override void InputUpdate(InputDirection dir)
    {
        int next = currentIndex;
        switch (dir)
        {
            case InputDirection.Up:
                next -= ColumnCount;
                break;
            case InputDirection.Down:
            next += ColumnCount;
                break;
            case InputDirection.Left:
            next -= 1;
                break;
            case InputDirection.Right:
            next += 1;
                break;
            default:
                break;
        }

        if (next != currentIndex && (0 <= next && next < selects.Count))
        {
            UpdateCursor(next);
        }
    }

    public void Selected()
    {
        selects[currentIndex].Selected();
    }
}

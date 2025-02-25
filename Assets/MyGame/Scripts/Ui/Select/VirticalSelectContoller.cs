public abstract class VirticalSelectContoller<TSelect, TData> : SelectController<TSelect, TData> where TSelect : BaseSelector<TData>
{
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
                next -= 1;
                break;
            case InputDirection.Down:
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

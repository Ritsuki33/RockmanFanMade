using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MainMenuCursorSelect : VirticalSelectContoller<TitleCursorSelector, SelectInfo>
{
    [SerializeField] Transform cursor = default;

    public void HideCursor()
    {
        cursor.gameObject.SetActive(false);
    }

    public override void DisplayCursor()
    {
        cursor.gameObject.SetActive(true);

        selects[currentIndex].SetCursor(cursor);
    }
}

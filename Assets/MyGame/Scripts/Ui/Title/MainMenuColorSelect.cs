using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuColorSelect : VirticalSelectContoller<ColorSelector, SelectInfo>
{
    public override void DisplayCursor()
    {
        selects[preIndex].ChangeColor(false);
        selects[currentIndex].ChangeColor(true);
    }
}

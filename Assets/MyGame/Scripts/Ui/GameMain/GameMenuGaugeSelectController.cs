using System;
using UnityEngine;

[Serializable]
public struct GameMenuGaugeInfo
{
    public int id;
    public string text;
}



public class GameMenuGaugeSelectController : VirticalSelectContoller<MenuGuageSelector, SelectInfo>
{
    public override void DisplayCursor()
    {

    }
}

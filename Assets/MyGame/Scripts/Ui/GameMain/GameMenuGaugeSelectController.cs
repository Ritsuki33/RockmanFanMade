using System;
using UnityEngine;

[Serializable]
public struct GameMenuGaugeInfo
{
    public int id;
    public PlayerWeaponType weaponType;
}



public class GameMenuGaugeSelectController : VirticalSelectContoller<MenuGuageSelector, GameMenuGaugeInfo>
{
    public override void DisplayCursor() { }
}

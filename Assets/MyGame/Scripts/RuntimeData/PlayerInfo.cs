using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo
{
    public PlayerParamStatus StatusParam => (WorldManager.Instance.Player == null) ? null : WorldManager.Instance.Player.paramStatus;

    public PlayerWeaponStatus PlayerWeaponStatus => (WorldManager.Instance.Player == null) ? null : WorldManager.Instance.Player.playerWeaponStatus;
}

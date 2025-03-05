using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo
{
    public ParamStatus StatusParam => (WorldManager.Instance.Player == null) ? null : WorldManager.Instance.Player.paramStatus;

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeDataHolder
{
    private PlayerInfo playerInfo = new PlayerInfo();
    private StageInfo stageInfo = new StageInfo();
    public PlayerInfo PlayerInfo => playerInfo;

    public StageInfo StageInfo => stageInfo;

}

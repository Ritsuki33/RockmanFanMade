using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeDataHolder
{
    private PlayerInfo playerInfo = new PlayerInfo();
    private StageInfo stageInfo = new StageInfo();
    public PlayerInfo PlayerInfo => playerInfo;
    public StageInfo StageInfo => stageInfo;
    private PlayerWeaponInfo playerWeaponInfo = new PlayerWeaponInfo();
    public PlayerWeaponInfo PlayerWeaponInfo => playerWeaponInfo;
    private GameStateInfo gameStateInfo = new GameStateInfo();
    public GameStateInfo GameStateInfo => gameStateInfo;

    public void Initialize()
    {
        playerWeaponInfo.LoadPlayerWeaponData();
    }
}

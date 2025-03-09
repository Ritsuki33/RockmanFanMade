using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerWeaponStatus
{
    IPlayerWeapon GetPlayerWeapon(PlayerWeaponType type);
}

public class PlayerWeaponStatus : IPlayerWeaponStatus
{
    Dictionary<PlayerWeaponType, IPlayerWeapon> playerWeaponDic = new Dictionary<PlayerWeaponType, IPlayerWeapon>();

    public void Initialize(StagePlayer player)
    {
        for (int i = 0; i < PlayerWeaponListData.Count; i++)
        {
            switch (PlayerWeaponListData[i].WeaponType)
            {
                case PlayerWeaponType.RockBuster:
                    playerWeaponDic.Add(PlayerWeaponListData[i].WeaponType, new RockBusterWeapon(player));
                    break;
                case PlayerWeaponType.ThunderBolt:
                    playerWeaponDic.Add(PlayerWeaponListData[i].WeaponType, new ThunderBoltWeapon(player));
                    break;
            }
        }
    }

    public IPlayerWeapon GetPlayerWeapon(PlayerWeaponType type)
    {
        playerWeaponDic.TryGetValue(type, out IPlayerWeapon weapon);
        return weapon;
    }

    PlayerWeaponListData PlayerWeaponListData => ProjectManager.Instance.RDH.PlayerWeaponInfo.PlayerWeaponListData;
}

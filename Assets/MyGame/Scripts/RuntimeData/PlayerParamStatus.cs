using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerParamStatus : IParamStatus
{
    void OnChangeWeapon(PlayerWeaponData weaponData);
}

public class PlayerParamStatus : ParamStatus, IPlayerParamStatus
{
    public int weaponId = 0;
    private StagePlayer player;
    public PlayerParamStatus(int hp, int maxHp) : base(hp, maxHp) { }

    Dictionary<PlayerWeaponType, IPlayerWeapon> playerWeaponDic = new Dictionary<PlayerWeaponType, IPlayerWeapon>();

    private PlayerWeaponListData PlayerWeaponListData => ProjectManager.Instance.RDH.PlayerWeaponInfo.PlayerWeaponListData;
    public void Initialize(StagePlayer player)
    {
        this.player = player;

        for (int i = 0; i < PlayerWeaponListData.Count; i++)
        {
            switch (PlayerWeaponListData[i].WeaponType)
            {
                case PlayerWeaponType.RockBuster:
                    playerWeaponDic.Add(PlayerWeaponListData[i].WeaponType, new RockBuster(player));
                    break;
                case PlayerWeaponType.ThunderBolt:
                    playerWeaponDic.Add(PlayerWeaponListData[i].WeaponType, new ThunderBolt(player));
                    break;
            }
        }
    }

    public void OnChangeWeapon(PlayerWeaponData weaponData)
    {
        this.weaponId = weaponData.Id;
        if (playerWeaponDic.TryGetValue(weaponData.WeaponType, out var weapon))
        {
            player.ChangeWeapon(weapon, weaponData.Color1, weaponData.Color2);
            Debug.Log($"ChangeWeapon: {weaponData.Id}, {weaponData.WeaponType}, {weaponData.Color1}, {weaponData.Color2}");
        }
        else
        {
            Debug.LogError($"Weapon not found: {weaponData.WeaponType}");
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerParamStatus : IParamStatus
{
    void OnChangeWeapon(PlayerWeaponType type);

    event Action<int, int> CurrentWeaponGuageChangeCallback;
    event Action<int, int, Action> CurrentWeaponGuageRecoveryCallback;

    event Action<PlayerWeaponData> ChangeWeaponCallback;

    IPlayerWeapon CurrentWeapon { get; }
}

public class PlayerParamStatus : ParamStatus, IPlayerParamStatus
{
    public PlayerWeaponData curWeaponData = null;
    private StagePlayer player;
    public PlayerParamStatus(int hp, int maxHp) : base(hp, maxHp) { }

    private IPlayerWeapon currentWeapon = null;

    public event Action<PlayerWeaponData> ChangeWeaponCallback;


    public event Action<int, int> CurrentWeaponGuageChangeCallback
    {
        add => currentWeapon.EnergyChangeCallback += value;
        remove => currentWeapon.EnergyChangeCallback -= value;
    }
    public event Action<int, int, Action> CurrentWeaponGuageRecoveryCallback
    {
        add => currentWeapon.EnergyRecoveryCallback += value;
        remove => currentWeapon.EnergyRecoveryCallback -= value;
    }

    public IPlayerWeapon CurrentWeapon => currentWeapon;


    public void Initialize(StagePlayer player)
    {
        this.player = player;
    }

    public void OnChangeWeapon(PlayerWeaponType type)
    {
        PlayerWeaponData weaponData = null;
        for (int i = 0; i < PlayerWeaponInfo.PlayerWeaponListData.Count; i++)
        {
            var data = PlayerWeaponInfo.PlayerWeaponListData[i];
            if (data.WeaponType == type)
            {
                weaponData = data;
                break;
            }
        }

        if (weaponData == null)
        {
            Debug.LogError($"Weapon not found: {type}");
            return;
        }

        var weapon = player.playerWeaponStatus.GetPlayerWeapon(type);
        if (weapon != null)
        {
            Debug.Log($"ChangeWeapon: {weaponData.Id}, {weaponData.WeaponType}, {weaponData.Color1}, {weaponData.Color2}");
            currentWeapon = weapon;
            ChangeWeaponCallback?.Invoke(weaponData);
            curWeaponData = weaponData;
        }
        else
        {
            Debug.LogError($"Weapon not found: {weaponData.WeaponType}");
        }
    }

    public void RecoveryEnergy(int amount, Action callback) => currentWeapon.RecoveryEnergy(amount, callback);

    public override void OnRefresh()
    {
        base.OnRefresh();
        ChangeWeaponCallback?.Invoke(curWeaponData);
        currentWeapon?.OnRefresh();
    }
    PlayerWeaponInfo PlayerWeaponInfo => ProjectManager.Instance.RDH.PlayerWeaponInfo;
}

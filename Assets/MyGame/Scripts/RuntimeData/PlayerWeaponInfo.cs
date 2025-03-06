using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponInfo : MonoBehaviour
{
    private PlayerWeaponListData playerWeaponListData;

    public PlayerWeaponListData PlayerWeaponListData => playerWeaponListData;

    public void LoadPlayerWeaponData()
    {
        playerWeaponListData = AddressableAssetLoadUtility.LoadAsset<PlayerWeaponListData>("PlayerWeaponListData");
    }
}

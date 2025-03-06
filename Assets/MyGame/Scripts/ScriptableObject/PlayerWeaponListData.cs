using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PlayerWeaponListData", menuName = "ScriptableObject/CreatePlayerWeaponListData")]
public class PlayerWeaponListData : ScriptableObject
{
    public PlayerWeaponData[] playerWeaponData;


    public PlayerWeaponData this[int index]
    {
        get
        {
            return playerWeaponData[index];
        }
    }

    public int Count => playerWeaponData.Length;
}

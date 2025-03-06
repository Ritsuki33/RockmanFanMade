using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerWeaponType
{
    RockBuster,
    ThunderBolt,
}

[CreateAssetMenu(fileName = "PlayerWeaponData", menuName = "ScriptableObject/CreatePlayerWeaponData")]
public class PlayerWeaponData : ScriptableObject
{
    /// <summary>
    /// 武器のID
    /// </summary>
    [SerializeField] private int id;

    [SerializeField] private PlayerWeaponType weaponType;
    /// <summary>
    /// 武器の表示名
    /// </summary>
    [SerializeField] private string displayName;
    /// <summary>
    /// プレイヤーの色1
    /// </summary>
    [SerializeField] private Color color1;
    /// <summary>
    /// プレイヤーの色2
    /// </summary>
    [SerializeField] private Color color2;

    /// <summary>
    /// 武器を使用可能にするかどうか
    /// </summary>
    public bool enable = true;


    public int Id => id;
    public PlayerWeaponType WeaponType => weaponType;
    public string DisplayName => displayName;
    public Color Color1 => color1;
    public Color Color2 => color2;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class BlockTilemap : MonoBehaviour
{
    [SerializeField] Tilemap tilemap;

    private ContactPoint2D[] contacts = null;

    RbCollide rbCollide = new RbCollide();

    private void Awake()
    {
        rbCollide.Init();
        rbCollide.onTriggerStayRockBusterDamage += OnTriggerStayRockBusterDamage;
    }

    /// <summary>
    /// ワールド座標から該当のタイルを検出し
    /// </summary>
    /// <param name="worldPosition"></param>
    public void BreakTileAt(RockBusterDamage damage)
    {
        Vector3 worldPosition = damage.transform.position;
        Vector3Int cellPosition = tilemap.WorldToCell(worldPosition);
        TileBase tile = tilemap.GetTile(cellPosition);

        if (tile is not null)
        {
            Vector3 tileCenter = tilemap.GetCellCenterWorld(cellPosition);

            var effect = ObjectManager.Instance.OnGet<PsObject>(PoolType.BlockBreakEffect);
            effect.transform.position_xy (tileCenter);
            tilemap.SetTile(cellPosition, null);

            if (damage.baseDamageValue == 1)
            {
                damage.projectile.Delete();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        rbCollide.OnTriggerStay(collision);
    }

    private void OnTriggerStayRockBusterDamage(RockBusterDamage damage)
    {
        BreakTileAt(damage);
    }
}

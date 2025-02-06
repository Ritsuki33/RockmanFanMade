using System;
using UnityEngine;
using UnityEngine.Tilemaps;


/// <summary>
/// 衝突時の取得コンポーネント
/// スクリプトテンプレートから自動生成
/// </summary>
public class BlockTilemap : MonoBehaviour, IRbVisitable, IExRbVisitable
{
    #region 編集禁止
    protected virtual void AcceptOnTriggerEnter(IRbVisitor visitor) => visitor.OnTriggerEnter(this);
    protected virtual void AcceptOnCollisionEnter(IRbVisitor visitor) => visitor.OnCollisionEnter(this);
    protected virtual void AcceptOnCollisionExit(IRbVisitor visitor) => visitor.OnCollisionExit(this);
    protected virtual void AcceptOnCollisionStay(IRbVisitor visitor) => visitor.OnCollisionStay(this);
    protected virtual void AcceptOnTriggerExit(IRbVisitor visitor) => visitor.OnTriggerExit(this);
    protected virtual void AcceptOnTriggerStay(IRbVisitor visitor) => visitor.OnTriggerStay(this);

    protected virtual void AcceptOnHitEnter(IExRbVisitor visitor) => visitor.OnHitEnter(this);
    protected virtual void AcceptOnHitStay(IExRbVisitor visitor) => visitor.OnHitStay(this);
    protected virtual void AcceptOnHitExit(IExRbVisitor visitor) => visitor.OnHitExit(this);
    protected virtual void AcceptOnBottomHitEnter(IExRbVisitor visitor) => visitor.OnBottomHitEnter(this);
    protected virtual void AcceptOnBottomHitStay(IExRbVisitor visitor) => visitor.OnBottomHitStay(this);
    protected virtual void AcceptOnBottomHitExit(IExRbVisitor visitor) => visitor.OnBottomHitExit(this);
    protected virtual void AcceptOnTopHitEnter(IExRbVisitor visitor) => visitor.OnTopHitEnter(this);
    protected virtual void AcceptOnTopHitStay(IExRbVisitor visitor) => visitor.OnTopHitStay(this);
    protected virtual void AcceptOnTopHitExit(IExRbVisitor visitor) => visitor.OnTopHitExit(this);
    protected virtual void AcceptOnLeftHitEnter(IExRbVisitor visitor) => visitor.OnLeftHitEnter(this);
    protected virtual void AcceptOnLeftHitStay(IExRbVisitor visitor) => visitor.OnLeftHitStay(this);
    protected virtual void AcceptOnLeftHitExit(IExRbVisitor visitor) => visitor.OnLeftHitExit(this);
    protected virtual void AcceptOnRightHitEnter(IExRbVisitor visitor) => visitor.OnRightHitEnter(this);
    protected virtual void AcceptOnRightHitStay(IExRbVisitor visitor) => visitor.OnRightHitStay(this);
    protected virtual void AcceptOnRightHitExit(IExRbVisitor visitor) => visitor.OnRightHitExit(this);

    void IRbVisitable.AcceptOnTriggerEnter(IRbVisitor visitor) => AcceptOnTriggerEnter(visitor);
    void IRbVisitable.AcceptOnCollisionEnter(IRbVisitor visitor) => AcceptOnCollisionEnter(visitor);
    void IRbVisitable.AcceptOnCollisionExit(IRbVisitor visitor) => AcceptOnCollisionExit(visitor);
    void IRbVisitable.AcceptOnCollisionStay(IRbVisitor visitor) => AcceptOnCollisionStay(visitor);
    void IRbVisitable.AcceptOnTriggerExit(IRbVisitor visitor) => AcceptOnTriggerExit(visitor);
    void IRbVisitable.AcceptOnTriggerStay(IRbVisitor visitor) => AcceptOnTriggerStay(visitor);

    void IExRbVisitable.AcceptOnHitEnter(IExRbVisitor visitor) => AcceptOnHitEnter(visitor);
    void IExRbVisitable.AcceptOnHitStay(IExRbVisitor visitor) => AcceptOnHitStay(visitor);
    void IExRbVisitable.AcceptOnHitExit(IExRbVisitor visitor) => AcceptOnHitExit(visitor);
    void IExRbVisitable.AcceptOnBottomHitEnter(IExRbVisitor visitor) => AcceptOnBottomHitEnter(visitor);
    void IExRbVisitable.AcceptOnBottomHitStay(IExRbVisitor visitor) => AcceptOnBottomHitStay(visitor);
    void IExRbVisitable.AcceptOnBottomHitExit(IExRbVisitor visitor) => AcceptOnBottomHitExit(visitor);
    void IExRbVisitable.AcceptOnTopHitEnter(IExRbVisitor visitor) => AcceptOnTopHitEnter(visitor);
    void IExRbVisitable.AcceptOnTopHitStay(IExRbVisitor visitor) => AcceptOnTopHitStay(visitor);
    void IExRbVisitable.AcceptOnTopHitExit(IExRbVisitor visitor) => AcceptOnTopHitExit(visitor);
    void IExRbVisitable.AcceptOnLeftHitEnter(IExRbVisitor visitor) => AcceptOnLeftHitEnter(visitor);
    void IExRbVisitable.AcceptOnLeftHitStay(IExRbVisitor visitor) => AcceptOnLeftHitStay(visitor);
    void IExRbVisitable.AcceptOnLeftHitExit(IExRbVisitor visitor) => AcceptOnLeftHitExit(visitor);
    void IExRbVisitable.AcceptOnRightHitEnter(IExRbVisitor visitor) => AcceptOnRightHitEnter(visitor);
    void IExRbVisitable.AcceptOnRightHitStay(IExRbVisitor visitor) => AcceptOnRightHitStay(visitor);
    void IExRbVisitable.AcceptOnRightHitExit(IExRbVisitor visitor) => AcceptOnRightHitExit(visitor);
    #endregion

    // ここから定義
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
    public bool BreakTileAt(Vector3 worldPosition)
    {
        Vector3Int cellPosition = tilemap.WorldToCell(worldPosition);
        TileBase tile = tilemap.GetTile(cellPosition);

        if (tile is not null)
        {
            Vector3 tileCenter = tilemap.GetCellCenterWorld(cellPosition);

            var effect = ObjectManager.Instance.OnGet<PsObject>(PoolType.BlockBreakEffect);
            effect.transform.position_xy(tileCenter);
            tilemap.SetTile(cellPosition, null);

            return true;
        }

        return false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        rbCollide.OnTriggerStay(collision);
    }

    private void OnTriggerStayRockBusterDamage(RockBusterDamage damage)
    {
        if (BreakTileAt(damage.transform.position))
        {
            if (damage.baseDamageValue == 1)
            {
                damage.projectile.Delete();
            }
        }
    }
}

# region 編集禁止
public partial interface IRbVisitor : IRbVisitor<BlockTilemap>
{ }

public partial interface IExRbVisitor : IExRbVisitor<BlockTilemap>
{ }

public partial interface IStateRbVisitor<T> : IStateRbVisitor<T, BlockTilemap>
{ }

public partial interface IStateExRbVisitor<T> : IStateExRbVisitor<T, BlockTilemap>
{ }

public partial interface ISubStateRbVisitor<T, PS> : ISubStateRbVisitor<T, PS, BlockTilemap>
{ }

public partial interface ISubStateExRbVisitor<T, PS> : ISubStateExRbVisitor<T, PS, BlockTilemap>
{ }



public partial class InheritRbState<T, TS, SM, S>
{
    virtual protected void OnTriggerEnter(T obj, BlockTilemap collision) { }
    virtual protected void OnTriggerStay(T obj, BlockTilemap collision) { }
    virtual protected void OnTriggerExit(T obj, BlockTilemap collision) { }

    virtual protected void OnCollisionEnter(T obj, BlockTilemap collision) { }
    virtual protected void OnCollisionStay(T obj, BlockTilemap collision) { }
    virtual protected void OnCollisionExit(T obj, BlockTilemap collision) { }

    void IStateRbVisitor<T, BlockTilemap>.OnTriggerEnter(T obj, BlockTilemap collision)
    {
        OnTriggerEnter(obj, collision);
        subStateMachine?.OnTriggerEnter(obj, this as TS, collision);
    }


    void IStateRbVisitor<T, BlockTilemap>.OnTriggerStay(T obj, BlockTilemap collision)
    {
        OnTriggerStay(obj, collision);
        subStateMachine?.OnTriggerStay(obj, this as TS, collision);
    }

    void IStateRbVisitor<T, BlockTilemap>.OnTriggerExit(T obj, BlockTilemap collision)
    {
        OnTriggerExit(obj, collision);
        subStateMachine?.OnTriggerExit(obj, this as TS, collision);
    }


    void IStateRbVisitor<T, BlockTilemap>.OnCollisionEnter(T obj, BlockTilemap collision)
    {
        OnCollisionEnter(obj, collision);
        subStateMachine?.OnCollisionEnter(obj, this as TS, collision);
    }

    void IStateRbVisitor<T, BlockTilemap>.OnCollisionStay(T obj, BlockTilemap collision)
    {
        OnCollisionStay(obj, collision);
        subStateMachine?.OnCollisionStay(obj, this as TS, collision);
    }

    void IStateRbVisitor<T, BlockTilemap>.OnCollisionExit(T obj, BlockTilemap collision)
    {
        OnCollisionExit(obj, collision);
        subStateMachine?.OnCollisionExit(obj, this as TS, collision);
    }

}


public partial class InheritExRbState<T, TS, SM, S>
{
    virtual protected void OnHitEnter(T obj, BlockTilemap collision) { }
    virtual protected void OnBottomHitEnter(T obj, BlockTilemap collision) { }
    virtual protected void OnTopHitEnter(T obj, BlockTilemap collision) { }

    virtual protected void OnLeftHitEnter(T obj, BlockTilemap collision) { }
    virtual protected void OnRightHitEnter(T obj, BlockTilemap collision) { }
    virtual protected void OnHitStay(T obj, BlockTilemap collision) { }

    virtual protected void OnBottomHitStay(T obj, BlockTilemap collision) { }
    virtual protected void OnTopHitStay(T obj, BlockTilemap collision) { }
    virtual protected void OnLeftHitStay(T obj, BlockTilemap collision) { }

    virtual protected void OnRightHitStay(T obj, BlockTilemap collision) { }
    virtual protected void OnHitExit(T obj, BlockTilemap collision) { }
    virtual protected void OnBottomHitExit(T obj, BlockTilemap collision) { }

    virtual protected void OnTopHitExit(T obj, BlockTilemap collision) { }
    virtual protected void OnLeftHitExit(T obj, BlockTilemap collision) { }
    virtual protected void OnRightHitExit(T obj, BlockTilemap collision) { }

    void IStateExRbVisitor<T, BlockTilemap>.OnHitEnter(T obj, BlockTilemap hit)
    {
        OnHitEnter(obj, hit);
        subStateMachine?.OnHitEnter(obj, this as TS, hit);
    }


    void IStateExRbVisitor<T, BlockTilemap>.OnBottomHitEnter(T obj, BlockTilemap hit)
    {
        OnBottomHitEnter(obj, hit);
        subStateMachine?.OnBottomHitEnter(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, BlockTilemap>.OnTopHitEnter(T obj, BlockTilemap hit)
    {
        OnTopHitEnter(obj, hit);
        subStateMachine?.OnTopHitEnter(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, BlockTilemap>.OnLeftHitEnter(T obj, BlockTilemap hit)
    {
        OnLeftHitEnter(obj, hit);
        subStateMachine?.OnLeftHitEnter(obj, this as TS, hit);
    }
    void IStateExRbVisitor<T, BlockTilemap>.OnRightHitEnter(T obj, BlockTilemap hit)
    {
        OnRightHitEnter(obj, hit);
        subStateMachine?.OnRightHitEnter(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, BlockTilemap>.OnHitStay(T obj, BlockTilemap hit)
    {
        OnHitStay(obj, hit);
        subStateMachine?.OnHitStay(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, BlockTilemap>.OnBottomHitStay(T obj, BlockTilemap hit)
    {
        OnBottomHitStay(obj, hit);
        subStateMachine?.OnBottomHitStay(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, BlockTilemap>.OnTopHitStay(T obj, BlockTilemap hit)
    {
        OnTopHitStay(obj, hit);
        subStateMachine?.OnTopHitStay(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, BlockTilemap>.OnLeftHitStay(T obj, BlockTilemap hit)
    {
        OnLeftHitStay(obj, hit);
        subStateMachine?.OnLeftHitStay(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, BlockTilemap>.OnRightHitStay(T obj, BlockTilemap hit)
    {
        OnRightHitStay(obj, hit);
        subStateMachine?.OnRightHitStay(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, BlockTilemap>.OnHitExit(T obj, BlockTilemap hit)
    {
        OnHitExit(obj, hit);
        subStateMachine?.OnHitExit(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, BlockTilemap>.OnBottomHitExit(T obj, BlockTilemap hit)
    {
        OnBottomHitExit(obj, hit);
        subStateMachine?.OnBottomHitExit(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, BlockTilemap>.OnTopHitExit(T obj, BlockTilemap hit)
    {
        OnTopHitExit(obj, hit);
        subStateMachine?.OnTopHitExit(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, BlockTilemap>.OnLeftHitExit(T obj, BlockTilemap hit)
    {
        OnLeftHitExit(obj, hit);
        subStateMachine?.OnLeftHitExit(obj, this as TS, hit);
    }

    void IStateExRbVisitor<T, BlockTilemap>.OnRightHitExit(T obj, BlockTilemap hit)
    {
        OnRightHitExit(obj, hit);
        subStateMachine?.OnRightHitExit(obj, this as TS, hit);
    }
}


public partial class InheritRbSubState<T, TS, PS, SM, S>
{
    virtual protected void OnTriggerEnter(T obj, PS parent, BlockTilemap collision) { }
    virtual protected void OnTriggerStay(T obj, PS parent, BlockTilemap collision) { }
    virtual protected void OnTriggerExit(T obj, PS parent, BlockTilemap collision) { }

    virtual protected void OnCollisionEnter(T obj, PS parent, BlockTilemap collision) { }
    virtual protected void OnCollisionStay(T obj, PS parent, BlockTilemap collision) { }
    virtual protected void OnCollisionExit(T obj, PS parent, BlockTilemap collision) { }

    void ISubStateRbVisitor<T, PS, BlockTilemap>.OnTriggerEnter(T obj, PS parent, BlockTilemap collision)
    {
        OnTriggerEnter(obj, parent, collision);
        subStateMachine?.OnTriggerEnter(obj, this as TS, collision);
    }

    void ISubStateRbVisitor<T, PS, BlockTilemap>.OnTriggerStay(T obj, PS parent, BlockTilemap collision)
    {
        OnTriggerStay(obj, parent, collision);
        subStateMachine?.OnTriggerStay(obj, this as TS, collision);
    }

    void ISubStateRbVisitor<T, PS, BlockTilemap>.OnTriggerExit(T obj, PS parent, BlockTilemap collision)
    {
        OnTriggerExit(obj, parent, collision);
        subStateMachine?.OnTriggerExit(obj, this as TS, collision);
    }

    void ISubStateRbVisitor<T, PS, BlockTilemap>.OnCollisionEnter(T obj, PS parent, BlockTilemap collision)
    {
        OnCollisionEnter(obj, parent, collision);
        subStateMachine?.OnCollisionEnter(obj, this as TS, collision);
    }

    void ISubStateRbVisitor<T, PS, BlockTilemap>.OnCollisionStay(T obj, PS parent, BlockTilemap collision)
    {
        OnCollisionStay(obj, parent, collision);
        subStateMachine?.OnCollisionStay(obj, this as TS, collision);
    }

    void ISubStateRbVisitor<T, PS, BlockTilemap>.OnCollisionExit(T obj, PS parent, BlockTilemap collision)
    {
        OnCollisionExit(obj, parent, collision);
        subStateMachine?.OnCollisionExit(obj, this as TS, collision);
    }
}


public partial class InheritExRbSubState<T, TS, PS, SM, S>
{
    virtual protected void OnHitEnter(T obj, PS parent, BlockTilemap collision) { }
    virtual protected void OnBottomHitEnter(T obj, PS parent, BlockTilemap collision) { }
    virtual protected void OnTopHitEnter(T obj, PS parent, BlockTilemap collision) { }

    virtual protected void OnLeftHitEnter(T obj, PS parent, BlockTilemap collision) { }
    virtual protected void OnRightHitEnter(T obj, PS parent, BlockTilemap collision) { }
    virtual protected void OnHitStay(T obj, PS parent, BlockTilemap collision) { }

    virtual protected void OnBottomHitStay(T obj, PS parent, BlockTilemap collision) { }
    virtual protected void OnTopHitStay(T obj, PS parent, BlockTilemap collision) { }
    virtual protected void OnLeftHitStay(T obj, PS parent, BlockTilemap collision) { }

    virtual protected void OnRightHitStay(T obj, PS parent, BlockTilemap collision) { }
    virtual protected void OnHitExit(T obj, PS parent, BlockTilemap collision) { }
    virtual protected void OnBottomHitExit(T obj, PS parent, BlockTilemap collision) { }

    virtual protected void OnTopHitExit(T obj, PS parent, BlockTilemap collision) { }
    virtual protected void OnLeftHitExit(T obj, PS parent, BlockTilemap collision) { }
    virtual protected void OnRightHitExit(T obj, PS parent, BlockTilemap collision) { }

    void ISubStateExRbVisitor<T, PS, BlockTilemap>.OnHitEnter(T obj, PS parent, BlockTilemap hit)
    {
        OnHitEnter(obj, parent, hit);
        subStateMachine?.OnHitEnter(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, BlockTilemap>.OnBottomHitEnter(T obj, PS parent, BlockTilemap hit)
    {
        OnBottomHitEnter(obj, parent, hit);
        subStateMachine?.OnBottomHitEnter(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, BlockTilemap>.OnTopHitEnter(T obj, PS parent, BlockTilemap hit)
    {
        OnTopHitEnter(obj, parent, hit);
        subStateMachine?.OnTopHitEnter(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, BlockTilemap>.OnLeftHitEnter(T obj, PS parent, BlockTilemap hit)
    {
        OnLeftHitEnter(obj, parent, hit);
        subStateMachine?.OnLeftHitEnter(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, BlockTilemap>.OnRightHitEnter(T obj, PS parent, BlockTilemap hit)
    {
        OnRightHitEnter(obj, parent, hit);
        subStateMachine?.OnRightHitEnter(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, BlockTilemap>.OnHitStay(T obj, PS parent, BlockTilemap hit)
    {
        OnHitStay(obj, parent, hit);
        subStateMachine?.OnHitStay(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, BlockTilemap>.OnBottomHitStay(T obj, PS parent, BlockTilemap hit)
    {
        OnBottomHitStay(obj, parent, hit);
        subStateMachine?.OnBottomHitStay(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, BlockTilemap>.OnTopHitStay(T obj, PS parent, BlockTilemap hit)
    {
        OnTopHitStay(obj, parent, hit);
        subStateMachine?.OnTopHitStay(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, BlockTilemap>.OnLeftHitStay(T obj, PS parent, BlockTilemap hit)
    {
        OnLeftHitStay(obj, parent, hit);
        subStateMachine?.OnLeftHitStay(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, BlockTilemap>.OnRightHitStay(T obj, PS parent, BlockTilemap hit)
    {
        OnRightHitStay(obj, parent, hit);
        subStateMachine?.OnRightHitStay(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, BlockTilemap>.OnHitExit(T obj, PS parent, BlockTilemap hit)
    {
        OnHitExit(obj, parent, hit);
        subStateMachine?.OnHitExit(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, BlockTilemap>.OnBottomHitExit(T obj, PS parent, BlockTilemap hit)
    {
        OnBottomHitExit(obj, parent, hit);
        subStateMachine?.OnBottomHitExit(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, BlockTilemap>.OnTopHitExit(T obj, PS parent, BlockTilemap hit)
    {
        OnTopHitExit(obj, parent, hit);
        subStateMachine?.OnTopHitExit(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, BlockTilemap>.OnLeftHitExit(T obj, PS parent, BlockTilemap hit)
    {
        OnLeftHitExit(obj, parent, hit);
        subStateMachine?.OnLeftHitExit(obj, this as TS, hit);
    }

    void ISubStateExRbVisitor<T, PS, BlockTilemap>.OnRightHitExit(T obj, PS parent, BlockTilemap hit)
    {
        OnRightHitExit(obj, parent, hit);
        subStateMachine?.OnRightHitExit(obj, this as TS, hit);
    }
}

public partial class InheritRbStateMachine<T, S>
{
    public void OnCollisionEnter(T obj, BlockTilemap collision) => curState.OnCollisionEnter(obj, collision);
    public void OnCollisionExit(T obj, BlockTilemap collision) => curState.OnCollisionExit(obj, collision);
    public void OnCollisionStay(T obj, BlockTilemap collision) => curState.OnCollisionStay(obj, collision);
    public void OnTriggerEnter(T obj, BlockTilemap collision) => curState.OnTriggerEnter(obj, collision);
    public void OnTriggerExit(T obj, BlockTilemap collision) => curState.OnTriggerEnter(obj, collision);
    public void OnTriggerStay(T obj, BlockTilemap collision) => curState.OnTriggerEnter(obj, collision);
}

public partial class InheritExRbStateMachine<T, S>
{
    public void OnHitEnter(T obj, BlockTilemap hit) => curState.OnHitEnter(obj, hit);
    public void OnBottomHitEnter(T obj, BlockTilemap hit) => curState.OnBottomHitEnter(obj, hit);
    public void OnTopHitEnter(T obj, BlockTilemap hit) => curState.OnTopHitEnter(obj, hit);
    public void OnLeftHitEnter(T obj, BlockTilemap hit) => curState.OnLeftHitEnter(obj, hit);
    public void OnRightHitEnter(T obj, BlockTilemap hit) => curState.OnRightHitEnter(obj, hit);
    public void OnHitStay(T obj, BlockTilemap hit) => curState.OnHitStay(obj, hit);
    public void OnBottomHitStay(T obj, BlockTilemap hit) => curState.OnBottomHitStay(obj, hit);
    public void OnTopHitStay(T obj, BlockTilemap hit) => curState.OnTopHitStay(obj, hit);
    public void OnLeftHitStay(T obj, BlockTilemap hit) => curState.OnLeftHitStay(obj, hit);
    public void OnRightHitStay(T obj, BlockTilemap hit) => curState.OnRightHitStay(obj, hit);
    public void OnHitExit(T obj, BlockTilemap hit) => curState.OnHitExit(obj, hit);
    public void OnBottomHitExit(T obj, BlockTilemap hit) => curState.OnBottomHitExit(obj, hit);
    public void OnTopHitExit(T obj, BlockTilemap hit) => curState.OnTopHitExit(obj, hit);
    public void OnLeftHitExit(T obj, BlockTilemap hit) => curState.OnLeftHitExit(obj, hit);
    public void OnRightHitExit(T obj, BlockTilemap hit) => curState.OnRightHitExit(obj, hit);
}

public partial class InheritRbSubStateMachine<T, PS, S>
{
    public void OnCollisionEnter(T obj, PS parent, BlockTilemap collision) => curState?.OnCollisionEnter(obj, parent, collision);
    public void OnCollisionExit(T obj, PS parent, BlockTilemap collision) => curState?.OnCollisionExit(obj, parent, collision);
    public void OnCollisionStay(T obj, PS parent, BlockTilemap collision) => curState?.OnCollisionStay(obj, parent, collision);
    public void OnTriggerEnter(T obj, PS parent, BlockTilemap collision) => curState?.OnTriggerEnter(obj, parent, collision);
    public void OnTriggerExit(T obj, PS parent, BlockTilemap collision) => curState?.OnTriggerExit(obj, parent, collision);
    public void OnTriggerStay(T obj, PS parent, BlockTilemap collision) => curState?.OnTriggerStay(obj, parent, collision);
}

public partial class InheritExRbSubStateMachine<T, PS, S>
{
    public void OnHitEnter(T obj, PS parent, BlockTilemap hit) => curState.OnHitEnter(obj, parent, hit);
    public void OnBottomHitEnter(T obj, PS parent, BlockTilemap hit) => curState.OnBottomHitEnter(obj, parent, hit);
    public void OnTopHitEnter(T obj, PS parent, BlockTilemap hit) => curState.OnTopHitEnter(obj, parent, hit);
    public void OnLeftHitEnter(T obj, PS parent, BlockTilemap hit) => curState.OnLeftHitEnter(obj, parent, hit);
    public void OnRightHitEnter(T obj, PS parent, BlockTilemap hit) => curState.OnRightHitEnter(obj, parent, hit);
    public void OnHitStay(T obj, PS parent, BlockTilemap hit) => curState.OnHitStay(obj, parent, hit);
    public void OnBottomHitStay(T obj, PS parent, BlockTilemap hit) => curState.OnBottomHitStay(obj, parent, hit);
    public void OnTopHitStay(T obj, PS parent, BlockTilemap hit) => curState.OnTopHitStay(obj, parent, hit);
    public void OnLeftHitStay(T obj, PS parent, BlockTilemap hit) => curState.OnLeftHitStay(obj, parent, hit);
    public void OnRightHitStay(T obj, PS parent, BlockTilemap hit) => curState.OnRightHitStay(obj, parent, hit);
    public void OnHitExit(T obj, PS parent, BlockTilemap hit) => curState.OnHitExit(obj, parent, hit);
    public void OnBottomHitExit(T obj, PS parent, BlockTilemap hit) => curState.OnBottomHitExit(obj, parent, hit);
    public void OnTopHitExit(T obj, PS parent, BlockTilemap hit) => curState.OnTopHitExit(obj, parent, hit);
    public void OnLeftHitExit(T obj, PS parent, BlockTilemap hit) => curState.OnLeftHitExit(obj, parent, hit);
    public void OnRightHitExit(T obj, PS parent, BlockTilemap hit) => curState.OnRightHitExit(obj, parent, hit);
}


public partial class RbCollide
{
    void IRbVisitor<BlockTilemap>.OnCollisionEnter(BlockTilemap collision) => onCollisionEnterBlockTilemap?.Invoke(collision);
    void IRbVisitor<BlockTilemap>.OnCollisionExit(BlockTilemap collision) => onCollisionExitBlockTilemap?.Invoke(collision);
    void IRbVisitor<BlockTilemap>.OnCollisionStay(BlockTilemap collision) => onCollisionStayBlockTilemap?.Invoke(collision);
    void IRbVisitor<BlockTilemap>.OnTriggerEnter(BlockTilemap collision) => onTriggerEnterBlockTilemap?.Invoke(collision);
    void IRbVisitor<BlockTilemap>.OnTriggerExit(BlockTilemap collision) => onTriggerExitBlockTilemap?.Invoke(collision);
    void IRbVisitor<BlockTilemap>.OnTriggerStay(BlockTilemap collision) => onTriggerStayBlockTilemap?.Invoke(collision);

    public event Action<BlockTilemap> onCollisionEnterBlockTilemap;
    public event Action<BlockTilemap> onCollisionExitBlockTilemap;
    public event Action<BlockTilemap> onCollisionStayBlockTilemap;
    public event Action<BlockTilemap> onTriggerEnterBlockTilemap;
    public event Action<BlockTilemap> onTriggerExitBlockTilemap;
    public event Action<BlockTilemap> onTriggerStayBlockTilemap;
}

public partial class ExRbHit
{
    void IExRbVisitor<BlockTilemap>.OnHitEnter(BlockTilemap hit) => onHitEnterBlockTilemap?.Invoke(hit);
    void IExRbVisitor<BlockTilemap>.OnBottomHitEnter(BlockTilemap hit) => onBottomHitEnterBlockTilemap?.Invoke(hit);
    void IExRbVisitor<BlockTilemap>.OnTopHitEnter(BlockTilemap hit) => onTopHitEnterBlockTilemap?.Invoke(hit);
    void IExRbVisitor<BlockTilemap>.OnLeftHitEnter(BlockTilemap hit) => onLeftHitEnterBlockTilemap?.Invoke(hit);
    void IExRbVisitor<BlockTilemap>.OnRightHitEnter(BlockTilemap hit) => onRightHitEnterBlockTilemap?.Invoke(hit);
    void IExRbVisitor<BlockTilemap>.OnHitStay(BlockTilemap hit) => onHitStayBlockTilemap?.Invoke(hit);
    void IExRbVisitor<BlockTilemap>.OnBottomHitStay(BlockTilemap hit) => onBottomHitStayBlockTilemap?.Invoke(hit);
    void IExRbVisitor<BlockTilemap>.OnTopHitStay(BlockTilemap hit) => onTopHitStayBlockTilemap?.Invoke(hit);
    void IExRbVisitor<BlockTilemap>.OnLeftHitStay(BlockTilemap hit) => onLeftHitStayBlockTilemap?.Invoke(hit);
    void IExRbVisitor<BlockTilemap>.OnRightHitStay(BlockTilemap hit) => onRightHitStayBlockTilemap?.Invoke(hit);
    void IExRbVisitor<BlockTilemap>.OnHitExit(BlockTilemap hit) => onHitExitBlockTilemap?.Invoke(hit);
    void IExRbVisitor<BlockTilemap>.OnBottomHitExit(BlockTilemap hit) => onBottomHitExitBlockTilemap?.Invoke(hit);
    void IExRbVisitor<BlockTilemap>.OnTopHitExit(BlockTilemap hit) => onTopHitExitBlockTilemap?.Invoke(hit);
    void IExRbVisitor<BlockTilemap>.OnLeftHitExit(BlockTilemap hit) => onLeftHitExitBlockTilemap?.Invoke(hit);
    void IExRbVisitor<BlockTilemap>.OnRightHitExit(BlockTilemap hit) => onRightHitExitBlockTilemap?.Invoke(hit);

    public event Action<BlockTilemap> onHitEnterBlockTilemap;
    public event Action<BlockTilemap> onBottomHitEnterBlockTilemap;
    public event Action<BlockTilemap> onTopHitEnterBlockTilemap;
    public event Action<BlockTilemap> onLeftHitEnterBlockTilemap;
    public event Action<BlockTilemap> onRightHitEnterBlockTilemap;
    public event Action<BlockTilemap> onHitStayBlockTilemap;
    public event Action<BlockTilemap> onBottomHitStayBlockTilemap;
    public event Action<BlockTilemap> onTopHitStayBlockTilemap;
    public event Action<BlockTilemap> onLeftHitStayBlockTilemap;
    public event Action<BlockTilemap> onRightHitStayBlockTilemap;
    public event Action<BlockTilemap> onHitExitBlockTilemap;
    public event Action<BlockTilemap> onBottomHitExitBlockTilemap;
    public event Action<BlockTilemap> onTopHitExitBlockTilemap;
    public event Action<BlockTilemap> onLeftHitExitBlockTilemap;
    public event Action<BlockTilemap> onRightHitExitBlockTilemap;

    void SetInterpreterBlockTilemap(IHitInterpreter hitInterpreter)
    {
        onHitEnterBlockTilemap = hitInterpreter.OnHitEnter;
        onBottomHitEnterBlockTilemap = hitInterpreter.OnBottomHitEnter;
        onTopHitEnterBlockTilemap = hitInterpreter.OnTopHitEnter;
        onLeftHitEnterBlockTilemap = hitInterpreter.OnLeftHitEnter;
        onRightHitEnterBlockTilemap = hitInterpreter.OnRightHitEnter;
        onHitStayBlockTilemap = hitInterpreter.OnHitStay;
        onBottomHitStayBlockTilemap = hitInterpreter.OnBottomHitStay;
        onTopHitStayBlockTilemap = hitInterpreter.OnTopHitStay;
        onLeftHitStayBlockTilemap = hitInterpreter.OnLeftHitStay;
        onRightHitStayBlockTilemap = hitInterpreter.OnRightHitStay;
        onHitExitBlockTilemap = hitInterpreter.OnHitExit;
        onBottomHitExitBlockTilemap = hitInterpreter.OnBottomHitExit;
        onTopHitExitBlockTilemap = hitInterpreter.OnTopHitExit;
        onLeftHitExitBlockTilemap = hitInterpreter.OnLeftHitExit;
        onRightHitExitBlockTilemap = hitInterpreter.OnRightHitExit;
    }
}

public partial interface IHitInterpreter : IHitInterpreter<BlockTilemap> { }
#endregion

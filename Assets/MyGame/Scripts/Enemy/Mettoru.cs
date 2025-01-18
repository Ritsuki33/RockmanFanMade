using UnityEngine;

public class Mettoru : StageEnemy,IDirect
{
    [SerializeField] MettoruBehavior mettoruController;
    [SerializeField] Direct direct;

    /// <summary>
    /// 初期化
    /// </summary>
    protected override void Init()
    {
        base.Init();
        //mettoruController.Init();
    }

    public bool IsRight => direct.IsRight;
    public void TurnFace() => direct.TurnFace();
    public void TurnTo(bool isRight) => direct.TurnTo(isRight);
    public void TurnToTarget(Vector2 targetPos) => direct.TurnToTarget(targetPos);
}

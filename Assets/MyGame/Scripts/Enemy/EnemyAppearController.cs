using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAppearController : StateMachine<EnemyAppearController>
{
    [SerializeField] EnemyObject enemy = default;

    public bool IsDeath => !enemy.gameObject.activeSelf;

    enum StateID
    {
        OutOfCamera,
        Appering,
        Deading
    }
    private void Awake()
    {
        AddState((int)StateID.OutOfCamera, new OutOfCamera());
        AddState((int)StateID.Appering, new Appering());
        AddState((int)StateID.Deading, new Deading());

        TransitReady((int)StateID.OutOfCamera, true);
    }

    /// <summary>
    /// カメラ外
    /// </summary>
    class OutOfCamera : State<EnemyAppearController>
    {
        protected override void Enter(EnemyAppearController mettoruStageController, int preId)
        {
            mettoruStageController.enemy.Init();
            mettoruStageController.enemy.transform.position = mettoruStageController.transform.position;
            mettoruStageController.enemy.gameObject.SetActive(false);
        }
        protected override void Update(EnemyAppearController mettoruStageController, IParentState parent)
        {
            if (!GameManager.Instance.MainCameraControll.CheckOutOfView(mettoruStageController.gameObject))
            {
                mettoruStageController.TransitReady((int)StateID.Appering);
            }
        }
    }

    /// <summary>
    /// 敵が見えている状態
    /// </summary>
    class Appering: State<EnemyAppearController>
    {
        protected override void Enter(EnemyAppearController mettoruStageController, int preId)
        {
            mettoruStageController.enemy.gameObject.SetActive(true);
        }

        protected override void Update(EnemyAppearController mettoruStageController, IParentState parent)
        {
            if (mettoruStageController.IsDeath)
            {
                mettoruStageController.TransitReady((int)StateID.Deading);
            }
            else if (GameManager.Instance.MainCameraControll.CheckOutOfView(mettoruStageController.enemy.gameObject))
            {
                mettoruStageController.TransitReady((int)StateID.OutOfCamera);
            }
        }
    }

    /// <summary>
    /// 敵が死んだ状態
    /// </summary>
    class Deading : State<EnemyAppearController>
    {
        protected override void Update(EnemyAppearController mettoruStageController, IParentState parent)
        {
            if (GameManager.Instance.MainCameraControll.CheckOutOfView(mettoruStageController.gameObject))
            {
                mettoruStageController.TransitReady((int)StateID.OutOfCamera);

            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MettoruStageController : StateMachine<MettoruStageController>
{
    [SerializeField] Mettoru mettoru = default;

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

        TransitReady((int)StateID.OutOfCamera);
    }

    /// <summary>
    /// カメラ外
    /// </summary>
    class OutOfCamera : State<MettoruStageController>
    {
        protected override void Enter(MettoruStageController mettoruStageController, int preId)
        {
            mettoruStageController.mettoru.Init();
            mettoruStageController.mettoru.transform.position = mettoruStageController.transform.position;
            mettoruStageController.mettoru.gameObject.SetActive(false);
        }
        protected override void Update(MettoruStageController mettoruStageController, IParentState parent)
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
    class Appering: State<MettoruStageController>
    {
        protected override void Enter(MettoruStageController mettoruStageController, int preId)
        {
            mettoruStageController.mettoru.gameObject.SetActive(true);
        }

        protected override void Update(MettoruStageController mettoruStageController, IParentState parent)
        {
            if (mettoruStageController.mettoru.IsDeath)
            {
                mettoruStageController.TransitReady((int)StateID.Deading);
            }
            else if (GameManager.Instance.MainCameraControll.CheckOutOfView(mettoruStageController.mettoru.gameObject))
            {
                mettoruStageController.TransitReady((int)StateID.OutOfCamera);
            }
        }
    }

    /// <summary>
    /// 敵が死んだ状態
    /// </summary>
    class Deading : State<MettoruStageController>
    {
        protected override void Update(MettoruStageController mettoruStageController, IParentState parent)
        {
            if (GameManager.Instance.MainCameraControll.CheckOutOfView(mettoruStageController.gameObject))
            {
                mettoruStageController.TransitReady((int)StateID.OutOfCamera);

            }
        }
    }
}

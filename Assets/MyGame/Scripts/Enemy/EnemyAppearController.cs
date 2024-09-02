using UnityEngine;

public class EnemyAppearController : StateMachine<EnemyAppearController>
{
    [SerializeField] EnemyObject enemy = default;

    public bool IsDeath => !enemy.gameObject.activeSelf;

    enum StateID
    {
        OutOfCamera,
        Appering,
        Disappearing
    }
    private void Awake()
    {
        AddState((int)StateID.OutOfCamera, new OutOfCamera());
        AddState((int)StateID.Appering, new Appering());
        AddState((int)StateID.Disappearing, new Disappearing());

        TransitReady((int)StateID.OutOfCamera, true);

        enemy.gameObject.SetActive(false);
    }

    /// <summary>
    /// カメラ外
    /// </summary>
    class OutOfCamera : State<EnemyAppearController>
    {
        protected override void Enter(EnemyAppearController enemyAppearController, int preId)
        {
            enemyAppearController.enemy.Init();
            enemyAppearController.enemy.transform.position = enemyAppearController.transform.position;
            enemyAppearController.enemy.gameObject.SetActive(false);
        }
        protected override void Update(EnemyAppearController enemyAppearController, IParentState parent)
        {
            if (!GameManager.Instance.MainCameraControll.CheckOutOfView(enemyAppearController.gameObject))
            {
                enemyAppearController.TransitReady((int)StateID.Appering);
            }
        }
    }

    /// <summary>
    /// 敵が見えている状態
    /// </summary>
    class Appering: State<EnemyAppearController>
    {
        protected override void Enter(EnemyAppearController enemyAppearController, int preId)
        {
            enemyAppearController.enemy.gameObject.SetActive(true);
        }

        protected override void Update(EnemyAppearController enemyAppearController, IParentState parent)
        {
            if (enemyAppearController.IsDeath|| GameManager.Instance.MainCameraControll.CheckOutOfView(enemyAppearController.enemy.gameObject))
            {
                enemyAppearController.TransitReady((int)StateID.Disappearing);
            }
        }
    }

    /// <summary>
    /// 消えている状態
    /// </summary>
    class Disappearing : State<EnemyAppearController>
    {
        protected override void Enter(EnemyAppearController enemyAppearController, int preId)
        {
            enemyAppearController.enemy.gameObject.SetActive(false);
        }
        protected override void Update(EnemyAppearController enemyAppearController, IParentState parent)
        {
            if (GameManager.Instance.MainCameraControll.CheckOutOfView(enemyAppearController.gameObject))
            {
                enemyAppearController.TransitReady((int)StateID.OutOfCamera);
            }
        }
    }
}

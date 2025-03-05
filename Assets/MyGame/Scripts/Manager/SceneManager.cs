using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum ManagerType
{
    None,
    Title,
    BossSelect,
    GameMain,
}

public class SceneManager : SingletonComponent<SceneManager>
{
    [SerializeField] TitleManager titleManager;
    [SerializeField] GameMainManager gameMainManager;
    [SerializeField] BossSelectManager bossSelectManager;

    private IManager manager = null;

    ManagerType request = default;

    Dictionary<ManagerType, IManager> managerList = new Dictionary<ManagerType, IManager>();

    Coroutine couroutine = null;
    private void Start()
    {
        managerList.Add(ManagerType.Title, titleManager);
        managerList.Add(ManagerType.BossSelect, bossSelectManager);
        managerList.Add(ManagerType.GameMain, gameMainManager);

        managerList.ToList().ForEach(manager => manager.Value.SetActive(false));
    }

    private void Update()
    {
        if (request != ManagerType.None)
        {
            couroutine = StartCoroutine(CoChangeManager(request));
            request = ManagerType.None;
        }

        if (couroutine == null) manager?.OnUpdate();


        IEnumerator CoChangeManager(ManagerType request)
        {
            if (manager != null)
            {
                yield return manager.OnEnd();
                manager.SetActive(false);
                yield return manager.Dispose();
            }

            if (managerList.ContainsKey(request))
            {
                manager = managerList[request];
                yield return manager.Init();
                manager.SetActive(true);

                yield return manager.OnStart();
            }
            couroutine = null;
        }
    }

    public void ChangeManager(ManagerType type)
    {
        request = type;
    }
}

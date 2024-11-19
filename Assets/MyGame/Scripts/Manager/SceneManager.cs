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
            if (manager != null)
            {
                manager.Terminate();
                manager.SetActive(false);
            }

            if (managerList.ContainsKey(request))
            {
                manager = managerList[request];
                manager.SetActive(true);
                manager.Init();
            }

            request = ManagerType.None;
        }

        manager?.OnUpdate();
    }

    public void ChangeManager(ManagerType type)
    {
        request = type;
    }
}

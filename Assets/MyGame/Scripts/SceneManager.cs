using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum ManagerType
{
    None,
    Title,
    ActionPart
}

public class SceneManager : SingletonComponent<SceneManager>
{
    [SerializeField] TitleManager titleManager;
    [SerializeField] GameMainManager actionPartManager;

    private IManager manager = null;

    ManagerType request = default;

    Dictionary<ManagerType, IManager> managerList = new Dictionary<ManagerType, IManager>();

    private void Start()
    {
        managerList.Add(ManagerType.Title, titleManager);
        managerList.Add(ManagerType.ActionPart, actionPartManager);

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

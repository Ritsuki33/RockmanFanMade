//using System.Collections.Generic;
//using UnityEngine;

//public class GameManagerController : MonoBehaviour
//{
//    [SerializeField] private ProjectManager gameManager;

//    private IManager manager = null;

//    ManagerType request = default;

//    Dictionary<ManagerType, IManager> managerList = new Dictionary<ManagerType, IManager>();
//    public enum ManagerType
//    {
//        None,
//        Title,
//        ActionPart
//    }

//    private void Awake()
//    {
//        managerList.Add(ManagerType.Title, gameManager.TitleManager);
//        managerList.Add(ManagerType.ActionPart, gameManager.ActionPartManager);
//    }

//    private void Update()
//    {
//        if (request != ManagerType.None)
//        {
//            if (manager)
//            {
//                manager.Terminate();
//                manager.gameObject.SetActive(false);
//            }

//            if (managerList.ContainsKey(request))
//            {
//                manager = managerList[request];
//                manager.Init();
//            }

//            manager.gameObject.SetActive(true);
//            request = ManagerType.None;
//        }

//        manager.OnUpdate();
//    }


//    public void ChangeManager(ManagerType type)
//    {
//        request = type;
//    }
//}

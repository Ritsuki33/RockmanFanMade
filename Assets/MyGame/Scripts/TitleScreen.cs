using UnityEngine;

public class TitleScreen : MonoBehaviour
{
    [SerializeField] MainMenuColorSelect controller;

   
    public void Open()
    {
        this.gameObject.SetActive(true);

        SelectInfo[] selects = { new SelectInfo(1, "start"), new SelectInfo(2, "load data") };
        controller.Init(selects, Selected);
    }

    public void Close()
    {
        this.gameObject.SetActive(false);
    }

    public void InputUpdate(TitleManager.InputInfo info)
    {
        //if (info.decide)
        //{
        //    Close();
        //    SceneManager.Instance.ChangeManager(ManagerType.ActionPart);
        //}

        controller.InputUpdate(info);
    }

    public void Selected(SelectInfo data)
    {
        Debug.Log(data.id);
    }
}

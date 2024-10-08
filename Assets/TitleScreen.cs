using UnityEngine;

public class TitleScreen : MonoBehaviour
{
    public void Open()
    {
        this.gameObject.SetActive(true);
    }

    public void Close()
    {
        this.gameObject.SetActive(false);
    }

    public void InputUpdate(TitleManager.InputInfo info)
    {
        if (info.decide)
        {
            Close();
            SceneManager.Instance.ChangeManager(ManagerType.ActionPart);
        }
    }
}

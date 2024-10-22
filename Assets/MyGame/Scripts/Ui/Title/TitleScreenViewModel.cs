using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenViewModel : BaseViewModel
{
    List<SelectInfo> select = new List<SelectInfo>();
    public List<SelectInfo> Selects => select;

    protected override IEnumerator Configure()
    {
        select.Add(new SelectInfo(1, "start"));
        select.Add(new SelectInfo(2, "load data"));

        yield return null;
    }

    public void Selected(SelectInfo data)
    {
        if (data.id==1)
        {
            SceneManager.Instance.ChangeManager(ManagerType.ActionPart);
        }
        else
        {
            Debug.Log("まだ実装されていません。");
        }
    }
}

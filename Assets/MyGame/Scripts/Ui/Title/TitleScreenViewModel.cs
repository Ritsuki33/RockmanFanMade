using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenViewModel : BaseViewModel<TitleManager.ScreenType>
{
    List<SelectInfo> select = new List<SelectInfo>();
    public List<SelectInfo> Selects => select;

    protected override IEnumerator Configure()
    {
        select.Add(new SelectInfo(1, "START"));
        // select.Add(new SelectInfo(2, "LOAD DATA"));

        yield return null;
    }

    public void Selected(SelectInfo data)
    {
        if (data.id == 1)
        {
            AudioManager.Instance.PlaySystem(SECueIDs.start);
            TitleManager.Instance.TransitToBossSelect();
        }
        else
        {
            AudioManager.Instance.PlaySystem(SECueIDs.error);
            Debug.Log("まだ実装されていません。");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSelectController : GridSelectController<BossSelectCursorSelector, BossSelectInfo>
{
    [SerializeField] Transform cursor = default;

    public override void DisplayCursor()
    {
        cursor.gameObject.SetActive(true);

        selects[currentIndex].SetCursor(cursor);
    }
}

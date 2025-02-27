using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSelectController : GridSelectController<BossSelectCursorSelector, BossSelectInfo>
{
    [SerializeField] Transform cursorRoot = default;
    [SerializeField] Transform cursor = default;

    public override void DisplayCursor()
    {
        cursor.gameObject.SetActive(true);

        selects[currentIndex].SetCursor(cursor);
    }

    public override void OnDestroy()
    {
        cursor.SetParent(cursorRoot, false);
        base.OnDestroy();
    }
}

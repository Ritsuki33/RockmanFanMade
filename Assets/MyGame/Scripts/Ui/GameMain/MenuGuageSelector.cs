using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuGuageSelector : BaseSelector<SelectInfo>
{
    [SerializeField] GameMenuGaugeBar gaugeBar;

    public GameMenuGaugeBar GaugeBar => gaugeBar;
    protected override void OnSetup(SelectInfo data)
    {
    }

    public override void OnCursorEnter()
    {
        gaugeBar.SetGrayScale(true);
    }
    public override void OnCursorExit()
    {
        gaugeBar.SetGrayScale(false);
    }
}

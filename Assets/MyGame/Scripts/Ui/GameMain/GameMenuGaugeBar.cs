using UnityEngine;
using UnityEngine.UI;

public class GameMenuGaugeBar : GaugeBar
{
    [SerializeField] Image gram;

    private Material _matGram;

    protected override void Awake()
    {
        base.Awake();
        _matGram = gram.material;

        SetParam(1);
    }

    public void SetGrayScale(bool isGray)
    {
        _matGram.SetFloat("_ColorChange", isGray ? 1 : 0);
    }
}

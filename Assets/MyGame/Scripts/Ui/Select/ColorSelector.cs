using TMPro;
using UnityEngine;

public class ColorSelector : BaseSelect<SelectInfo>
{
    [SerializeField] Color normal;
    [SerializeField] Color highLight;

    [SerializeField] TextMeshProUGUI text = default;

    protected override void OnSetup(SelectInfo data)
    {
        this.text.text = data.text;
        this.text.color = normal;
    }

    public void ChangeColor(bool isHighLight)
    {
        this.text.color = (isHighLight) ? highLight : normal;
    }
}

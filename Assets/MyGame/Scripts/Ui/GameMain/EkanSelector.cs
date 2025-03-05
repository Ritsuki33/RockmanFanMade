using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSelector : BaseSelector<SelectInfo>
{
    [SerializeField] Image image;

    private Material material;
    protected override void OnSetup(SelectInfo data)
    {
        material = image.material;
    }

    public override void OnCursorEnter()
    {
        material.SetFloat("_GrayScale", 0);
    }

    public override void OnCursorExit()
    {
        material.SetFloat("_GrayScale", 1);
    }
}

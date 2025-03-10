using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// フッターUIのキーガイド
/// </summary>
public class KeyGuide : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;

    public void Setup(string text)
    {
        this.text.text = text;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageObject : MonoBehaviour
{
    [SerializeField]  MaterialController materialController = default;

    /// <summary>
    /// floatのセット
    /// </summary>
    /// <param name="propertyID"></param>
    /// <param name="val"></param>
    public void SetMaterialParam(int propertyID, float val) => materialController.SetFloat(propertyID, val);

    /// <summary>
    /// カラーのセット
    /// </summary>
    /// <param name="propertyID"></param>
    /// <param name="color"></param>
    public void SetMaterialParam(int propertyID, Color color) => materialController.SetColor(propertyID, color);
}

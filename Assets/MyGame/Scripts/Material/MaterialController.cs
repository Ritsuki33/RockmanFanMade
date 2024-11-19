using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialController : MonoBehaviour
{
    [SerializeField] bool sharedMaterial = false;
    private Material material;
    private void Awake()
    {
        if (material == null)
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

            material = (sharedMaterial) ? spriteRenderer.sharedMaterial : spriteRenderer.material;
        }
    }

    /// <summary>
    /// floatのセット
    /// </summary>
    /// <param name="propertyID"></param>
    /// <param name="val"></param>
    public void SetFloat(int propertyID, float val)
    {
        if (material == null)
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

            material = (sharedMaterial) ? spriteRenderer.sharedMaterial : spriteRenderer.material;
        }
        material.SetFloat(propertyID, val);
    }

    /// <summary>
    /// カラーのセット
    /// </summary>
    /// <param name="propertyID"></param>
    /// <param name="color"></param>
    public void SetColor(int propertyID, Color color)
    {
        if (material == null)
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

            material = (sharedMaterial) ? spriteRenderer.sharedMaterial : spriteRenderer.material;
        }
        material.SetColor(propertyID, color);
    }
}

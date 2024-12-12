using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollSpriteController : MonoBehaviour
{
    static int scroll_x_id =Shader.PropertyToID("_ScrollX");
    static int scroll_y_id = Shader.PropertyToID("_ScrollY");

    [SerializeField] SpriteRenderer spriteRenderer;

    public void Scroll(Vector2 offset)
    {
        spriteRenderer.material.SetFloat(scroll_x_id, offset.x);
        spriteRenderer.material.SetFloat(scroll_y_id, offset.y);
    }

}

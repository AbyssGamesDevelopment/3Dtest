using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteChanger : MonoBehaviour
{
    public Sprite[] sprites;
    private Image spriteRenderer;

    public void Start()
    {
        spriteRenderer = GetComponent<Image>();
    }

    public void set(int id)
    {
        spriteRenderer.sprite = sprites[id];
    }
}

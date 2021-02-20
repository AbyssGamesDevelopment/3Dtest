using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    public float moveSpeed, fadeOutSpeed;
    public Sprite[] sprites;
    private SpriteRenderer spriteRenderer;
    private float a = 0.8f;

    public void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void setSprite(int id)
    {
        spriteRenderer.sprite = sprites[id];
    }

    public void FixedUpdate()
    {
        transform.Translate(new Vector3(0, moveSpeed, 0));
        spriteRenderer.color = new Color(1, 1, 1, a);
        a -= fadeOutSpeed;

        if (a <= 0)
            Destroy(gameObject);
    }
}
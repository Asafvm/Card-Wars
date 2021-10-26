using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Card : MonoBehaviour
{
    public CardValue value;
    public Sprite cardFace, cardBack;
    public bool isFaceDown = true;
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    private void Update()
    {
        if (isFaceDown)
            spriteRenderer.sprite = cardBack;
        else
            spriteRenderer.sprite = cardFace;
        spriteRenderer.material.SetTexture("_MainTex", spriteRenderer.sprite.texture);



    }
    void Start()
    {
        spriteRenderer.enabled = true;
        isFaceDown = true;
        spriteRenderer.material.SetFloat("_Fade", 1);
        
    }

    public int GetValue()
    {
        return (int)value;
    }

    //Card animation callback
    public void Flip()
    {
        isFaceDown = !isFaceDown;
    }

  


    
}

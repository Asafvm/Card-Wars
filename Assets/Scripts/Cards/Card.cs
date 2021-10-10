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

    void Start()
    {
        isFaceDown = true;
        UpdateSprite();
    }

	private void UpdateSprite()
	{
		if(isFaceDown)
            spriteRenderer.sprite = cardBack;
        else
            spriteRenderer.sprite = cardFace;
    }
    

    //Card animation callback
    public void Flip()
    {
        isFaceDown = !isFaceDown;
        UpdateSprite();
    }


}

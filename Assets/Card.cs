using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public Sprite cardFace, cardBack;
    private bool isFaceDown = true;
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

	private void OnMouseDown()
	{
        GetComponent<Animator>().SetTrigger("FlipTrigger");
	}
}

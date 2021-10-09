﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public CardValue value;
    public Sprite cardFace, cardBack;
    private bool isFaceDown = true;
    SpriteRenderer spriteRenderer;
    [SerializeField] ParticleSystem winEffect = null;
    Vector3 destination;
    float animationTime;

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
    private void Update()
    {
        if(destination!=transform.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime / animationTime);
            return;
        }
    }

    public void ShowWinEffect()
    {
        if (winEffect != null) winEffect.Play();
    }

    internal void SetDestination(Vector3 destination, float time)
    {
        this.destination = destination;
        animationTime = time;
    }

    //Card animation callback
    public void Flip()
    {
        isFaceDown = !isFaceDown;
        UpdateSprite();
    }

}

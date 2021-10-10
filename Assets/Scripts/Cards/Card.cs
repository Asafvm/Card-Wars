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
    Vector3 destination;
    float animationTime;
    Animator cardAnimator;
    Transform parent;

    private void Awake()
	{
        spriteRenderer = GetComponent<SpriteRenderer>();
        cardAnimator = GetComponent<Animator>();

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
        if(destination!=transform.position )
        {
            if (animationTime == 0) animationTime = .1f;
            transform.position = Vector3.MoveTowards(transform.position, destination, 
                (Vector3.Distance(destination,transform.position) / animationTime) *Time.deltaTime);
            return;
        }
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


    public void HandleCardTransitions(Transform cardDestination, string animationTrigger)
    {
        gameObject.SetActive(true);

        if(!String.IsNullOrEmpty(animationTrigger))
            cardAnimator.SetTrigger(animationTrigger);
        SetDestination(cardDestination.position, GetAnimationTime(cardAnimator) / 2);
        transform.parent = cardDestination;
    }

    public float GetAnimationTime(Animator cardAnimator)
    {
        AnimationClip[] clips = cardAnimator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            switch (clip.name)
            {
                case "Flip":
                    return clip.length;
                case "Throw":
                    return clip.length;
            }
        }
        return 0;

    }
}

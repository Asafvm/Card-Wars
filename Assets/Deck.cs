using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;

using UnityEngine;
using UnityEngine.Events;

public class Deck : MonoBehaviour
{
    public Stack<Card> cards = new Stack<Card>();
    [SerializeField] Transform cardDestination;

    public event Action OnDeckInteraction;

    public void PopulateDeck(List<Card> cards)
    {
        foreach (Card card in cards)
            this.cards.Push(card);
        
    }

    private void OnMouseDown()
    {
        if(Input.GetMouseButtonDown(0))
        {
            OnDeckInteraction?.Invoke();
        }
    }

    public void SpawnCard()
    {
        if (cards.Count == 0) return;   //do nothing if empty
        Card card = cards.Pop();
        Animator cardAnimator = card.GetComponent<Animator>();

        card.transform.position = transform.position;
        card.gameObject.SetActive(true);
        
        cardAnimator.SetTrigger("FlipTrigger");
        card.SetDestination(cardDestination.position, GetAnimationTime(cardAnimator)/2);
        card.transform.parent = cardDestination;


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

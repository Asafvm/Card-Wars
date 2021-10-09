using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;

using TMPro;

using UnityEngine;
using UnityEngine.Events;

public class Deck : MonoBehaviour
{
    public Stack<Card> cards = new Stack<Card>();
    [SerializeField] Transform cardDestination;
    [SerializeField] TextMeshProUGUI cardsLeftText;
    [SerializeField] GameObject deckCover;
    public event Action OnDeckInteraction;

    private void Awake()
    {
        UpdateCardCount();
    }
    public void PopulateDeck(List<Card> cards)
    {
        foreach (Card card in cards)
            this.cards.Push(card);
        
    }
    public void PopulateDeck(Card card)
    {
            cards.Push(card);
        UpdateCardCount();
    }

    private void UpdateCardCount()
    {
        cardsLeftText.text = cards.Count.ToString();
        if (cards.Count == 0)
            deckCover.SetActive(false);
        else
            deckCover.SetActive(true);
    }

    private void OnMouseDown()
    {
        if(Input.GetMouseButtonDown(0) && cards.Count>0)
        {
            OnDeckInteraction?.Invoke();
        }
    }

    public void SpawnCard()
    {
        if (cards.Count == 0) return;   //do nothing if empty

        
        HandleCardTransitions();
        UpdateCardCount();
    }

    private void HandleCardTransitions()
    {
        Card card = cards.Pop();
        Animator cardAnimator = card.GetComponent<Animator>();
        card.transform.position = transform.position;
        card.gameObject.SetActive(true);

        cardAnimator.SetTrigger("FlipTrigger");
        card.SetDestination(cardDestination.position, GetAnimationTime(cardAnimator) / 2);
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

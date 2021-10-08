using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public Stack<Card> cards = new Stack<Card>();
    [SerializeField] Transform cardDestination;
    
    public void PopulateDeck(List<Card> cards)
    {
        foreach (Card card in cards)
            this.cards.Push(card);
        
    }

    private void OnMouseDown()
    {
        if(Input.GetMouseButtonDown(0))
        {
            SpawnCard();
        }
    }

    private void SpawnCard()
    {
        if (cards.Count == 0) return;   //do nothing if empty
        Card card = cards.Pop();
        card.transform.position = transform.position;
        card.transform.parent = cardDestination;
        card.gameObject.SetActive(true);
        card.SetDestination(cardDestination.position, .3f);
        card.GetComponent<Animator>().SetTrigger("ThrowTrigger");

    }
}

using System;
using System.Collections;
using System.Security.Cryptography;

using UnityEngine;
using UnityEngine.Events;

using static MainDeckHandler;

public partial class MainDeckHandler : Deck
{

    [Serializable]
    public class DeckChangeEvent : UnityEvent<string> { }
    [SerializeField] DeckChangeEvent OnDeckChangedEvent;



    public void StartGame()
    {
        CollectCards(); //collect and hide cards
        GetComponent<Deck>().InitDeck();

        
    }

    private void CollectCards()
    {
        Card[] cards = FindObjectsOfType<Card>();
        if (cards.Length == 0) return;
        foreach (Card card in cards)
        {
            card.transform.position = transform.position;
            card.transform.parent.SetParent(transform, false);
            card.gameObject.SetActive(false);
        }
    }

    private void OnTransformChildrenChanged()
    {
        OnDeckChangedEvent?.Invoke(transform.childCount.ToString());
        if(transform.childCount >0)
            transform.GetChild(transform.childCount - 1).gameObject.SetActive(false);
    }

    public Card GetCard(int i)
    {
        if (i > -1 && i < cardPool.Count)
            return cardPool[i];
        return null;
    }
}

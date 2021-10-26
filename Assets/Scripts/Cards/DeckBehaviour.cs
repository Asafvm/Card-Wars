using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;

using TMPro;

using UnityEngine;
using UnityEngine.Events;

public class DeckBehaviour : MonoBehaviour
{
    public Queue<Card> cards = new Queue<Card>();
    [SerializeField] Transform secondaryDeck;
    [SerializeField] TextMeshProUGUI cardsLeftText;
    [SerializeField] GameObject deckCover;
    //[SerializeField] ParticleSystem glowEfect;
    public event Action OnDeckInteraction;

    private void Awake()
    {
        ToggleVisuals();
    }

    private void OnTransformChildrenChanged()
    {
        ToggleVisuals();
        {
            Transform child = transform.GetChild(transform.childCount-1);
            //insert card to queue
            child.gameObject.SetActive(false);
            cards.Enqueue(child.GetComponent<Card>());
        }

    }

    private void ToggleVisuals()
    {
        if (deckCover != null)
            deckCover.SetActive(transform.childCount > 0);
        cardsLeftText.text = transform.childCount.ToString();

    }



    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0) && cards.Count > 0)
        {
            OnDeckInteraction?.Invoke();
        }
    }


    public void SpawnCard(bool faceDown)
    {
        if (cards.Count == 0) return;   //do nothing if empty

        Card card = cards.Dequeue();
        card.isFaceDown = true;
        card.transform.rotation = Quaternion.Euler(0, 0, 0);
        card.transform.position = transform.position;

        card.GetComponent<CardMover>().HandleCardTransitions(secondaryDeck, faceDown ? null : CardAnimations.flipCard);
    }


    public int CheckScore()
    {
        return secondaryDeck.GetComponent<SecondaryDeck>().GetCurrentCardValue();
        
    }

    internal bool isSecondaryDeckReady()
    {
        return secondaryDeck.childCount == 0;
    }
}

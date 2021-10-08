using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionManager : MonoBehaviour
{
    [SerializeField] Transform PlayerPosition, OpponentPosition;
    DeckHandler deckHandler;
    Deck playerDeck, opponentDeck;


    private void Awake()
    {
        
    }
    void Start()
    {
        deckHandler = FindObjectOfType<DeckHandler>();
        playerDeck = PlayerPosition.GetComponentInChildren<Deck>();
        opponentDeck = OpponentPosition.GetComponentInChildren<Deck>();
        InitDecks();
    }

    private void InitDecks()
    {
        List<Card> cards = new List<Card>();
        for(int i=0; i<26; i++)
        {
            Card card = deckHandler.GetCard(i);
            cards.Add(card);

        }
        
        playerDeck.PopulateDeck(cards);
        cards.Clear();
        for (int i = 26; i < 52; i++)
        {
            Card card = deckHandler.GetCard(i);
            cards.Add(card);

        }

        opponentDeck.PopulateDeck(cards);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

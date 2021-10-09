using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionManager : MonoBehaviour
{
    DeckHandler deckHandler;
    Deck[] decks;

    private void Awake()
    {
        deckHandler = FindObjectOfType<DeckHandler>();
        decks = FindObjectsOfType<Deck>();
    }
    void Start()
    {
        InitDecks();
    }

    private void OnEnable()
    {
        foreach (Deck deck in decks)

            deck.OnDeckInteraction += SpawnCardsFormDecks;
    }
    private void OnDisable()
    {
        foreach (Deck deck in decks)
            deck.OnDeckInteraction -= SpawnCardsFormDecks;
    }
    private void InitDecks()
    {
        int totalCardsInDeck = deckHandler.GetDeckSize();
        int cardsPerPlayer = Mathf.FloorToInt(totalCardsInDeck / decks.Length);
        Debug.Log($"Dealing {totalCardsInDeck} cards to {decks.Length} players. {cardsPerPlayer} cards per player");



        List<Card> cards = new List<Card>();
        for(int i=0; i< cardsPerPlayer* decks.Length; i++)
        {
            Card card = deckHandler.GetCard(i);
            cards.Add(card);
        }
        
        //playerDeck.PopulateDeck(cards);
        //cards.Clear();
        //for (int i = 26; i < 52; i++)
        //{
        //    Card card = deckHandler.GetCard(i);
        //    cards.Add(card);

        //}

        //opponentDeck.PopulateDeck(cards);
    }

    
    private void SpawnCardsFormDecks()
    {
        foreach (Deck deck in decks)
            deck.SpawnCard();
    }
}

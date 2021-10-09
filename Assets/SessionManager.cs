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
        if (decks.Length < 2)
        {
            Debug.LogError("Not enough players to begin");
            return;
        }
        
        StartGameSequence();
        
    }

    private void StartGameSequence()
    {
        InitDecks();

    }

    private void OnEnable()
    {
        foreach (Deck deck in decks)

            deck.OnDeckInteraction += HandleRound;
    }
    private void OnDisable()
    {
        foreach (Deck deck in decks)
            deck.OnDeckInteraction -= HandleRound;
    }
    private void InitDecks()
    {
        int totalCardsInDeck = deckHandler.GetDeckSize();
        int cardsPerPlayer = Mathf.FloorToInt(totalCardsInDeck / decks.Length);
        Debug.Log($"Dealing {totalCardsInDeck} cards to {decks.Length} players. {cardsPerPlayer} cards per player");

        List<Card> cards = new List<Card>();
        for(int i=0; i< cardsPerPlayer* decks.Length; i++)
        {
            GameObject card = deckHandler.GetCard(i);
            decks[i % decks.Length].PopulateDeck(card.GetComponent<Card>());
        }
        
        
    }

    
    private void HandleRound()
    {
        SpawnCards();
        CheckScore();

    }

    private void CheckScore()
    {
        
        int winningDeck = 0, score = 0;
        for(int i=0; i<decks.Length; i++)
        {
            int tempScore = decks[i].GetComponentInChildren<ScoreHandler>().GetCurrentCardValue();
            if (tempScore < score) continue;
            score = tempScore;
            winningDeck = i;
        }
        decks[winningDeck].GetComponentInChildren<ScoreHandler>().HandleWin();
            
    }

    private void SpawnCards()
    {
        foreach (Deck deck in decks)
            deck.SpawnCard();
    }
}

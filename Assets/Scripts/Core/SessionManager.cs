using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class SessionManager : MonoBehaviour
{
    DeckHandler deckHandler;
    DeckBehaviour[] decks;
    NotificationsHandler notificationsHandler;
    GameState state;


    [Header("Tune Ups")]
    [SerializeField] float timeBetweenDealingCards = .05f;

    public event Action<Transform> OnMatchWin;

    private void Awake()
    {
        SetGameState(GameState.Setup);
        deckHandler = FindObjectOfType<DeckHandler>();
        decks = FindObjectsOfType<DeckBehaviour>();
        notificationsHandler = FindObjectOfType<NotificationsHandler>();

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
        deckHandler.StartGame();
        StartCoroutine(InitDecks());

    }

    private void OnEnable()
    {
        foreach (DeckBehaviour deck in decks)

            deck.OnDeckInteraction += HandleRound;
    }
    private void OnDisable()
    {
        foreach (DeckBehaviour deck in decks)
            deck.OnDeckInteraction -= HandleRound;
    }
    private IEnumerator InitDecks()
    {
        int totalCardsInDeck = 4;// deckHandler.GetDeckSize();
        int cardsPerPlayer = Mathf.FloorToInt(totalCardsInDeck / decks.Length);
        Debug.Log($"Dealing {totalCardsInDeck} cards to {decks.Length} players. {cardsPerPlayer} cards per player");

        List<Card> cards = new List<Card>();
        for (int i = 0; i < cardsPerPlayer * decks.Length; i++)
        {
            int deckIndex = i % decks.Length;
            Card card = deckHandler.GetCard(i).GetComponent<Card>();
            card.gameObject.SetActive(true);

            card.HandleCardTransitions(decks[deckIndex].transform, CardAnimations.throwCard);
            decks[deckIndex].PopulateDeck(card);
            yield return new WaitForSeconds(timeBetweenDealingCards);
        }
        state = GameState.Idle;

    }




    private void CheckScore()
    {
        
        int winningDeck = -1, score = -1;
        for(int deckIndex=0; deckIndex<decks.Length; deckIndex++)
        {
            int tempScore = decks[deckIndex].CheckScore();
            Debug.Log($"{tempScore}");

            if (tempScore == score)
            {
                StartCoroutine(HandleWar());
                return;
            }
            if (tempScore < score) continue;
            score = tempScore;
            winningDeck = deckIndex;
        }
        if (winningDeck < 0) return;

        OnMatchWin?.Invoke(decks[winningDeck].transform);
    }
    private void HandleRound()
    {
        if (state != GameState.Idle) return;
        SetGameState(GameState.Round);
        SpawnCardsFaceDown(false);
        CheckScore();
        
    }


    private IEnumerator HandleWar()
    {

        yield return new WaitForSeconds(1.5f);  //dramatic pause
        SetGameState(GameState.War);

        if (!CheckWarConditions()) yield break;


        SpawnCardsFaceDown(true);
        yield return new WaitForSeconds(.5f);
        SpawnCardsFaceDown(false);
        CheckScore();
    }

    private bool CheckWarConditions()
    {
        foreach (DeckBehaviour deck in decks)
            if (deck.transform.childCount < 2)
            {
                SetGameState(GameState.Win);
                return false;
            }
        return true;
    }

    private void SpawnCardsFaceDown(bool faceDown)
    {
        foreach (DeckBehaviour deck in decks)
            deck.SpawnCard(faceDown);
    }



    private void Update()
    {
        switch (state)
        {
            case GameState.Setup:
                ToggleControls(false);
                DisplayMessage("Getting Ready...");
                break;
            case GameState.Idle:
                ToggleControls(true);
                DisplayMessage("Ready!");
                CheckForGameOverCondition();
                break;
            case GameState.Round:
                ToggleControls(false);
                DisplayMessage("");
                if (AllSecondaryDecksCleared()) SetGameState(GameState.Idle);
                break;
            case GameState.War:
                ToggleControls(false);
                DisplayMessage("War!");
                if (AllSecondaryDecksCleared()) SetGameState(GameState.Idle);
                NotificationAnimation();
                break;
            case GameState.Win:
                ToggleControls(false);
                string winner = GetWinnerName();
                DisplayMessage($"Game Over\n{winner}");
                break;
        }

    }

    private string GetWinnerName()
    {
        int cardsLeft = -1, deckIndex = -1;
        for (int i = 0; i < decks.Length; i++)
        {
            DeckBehaviour deck = decks[i];
            if (deck.transform.childCount > cardsLeft)
            {
                cardsLeft = deck.transform.childCount;
                deckIndex = i;
            }
                
            else if (cardsLeft == deck.transform.childCount)
                return "It's a Draw!";     
        }

        return $"{decks[deckIndex].transform.parent.name} Won!";

    }

    private void SetGameState(GameState state)
    {
        this.state = state;
    }
    private void CheckForGameOverCondition()
    {
        foreach (DeckBehaviour deck in decks)
        {
            if (deck.transform.childCount == 0)
            {
                SetGameState(GameState.Win);
                return;
            }
        }
            
    }

    private void NotificationAnimation()
    {
        notificationsHandler.GetComponentInChildren<Animation>().Play();
    }

    private void DisplayMessage(string message)
    {
        notificationsHandler.UpdateNotification(message);
    }
    private bool AllSecondaryDecksCleared()
    {
        foreach (DeckBehaviour deck in decks)
            if (!deck.isSecondaryDeckReady()) return false;
        return true;
    }

    private void ToggleControls(bool active)
    {
        foreach (DeckBehaviour deck in decks)
            deck.GetComponent<BoxCollider2D>().enabled = active;
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.Events;

public class SessionManager : MonoBehaviour
{
    MainDeckHandler mainDeckHandler;
    DeckBehaviour[] decks;
    NotificationsHandler notificationsHandler;
    GameState state;

    [Serializable]
    public class StateEvent : UnityEvent<GameState> { }
    [SerializeField] public StateEvent OnStateChanged;

    [Header("Tune Ups")]
    [SerializeField] float timeBetweenDealingCards = .05f;

    public event Action<Transform> OnMatchWin;

    private void Awake()
    {
        mainDeckHandler = FindObjectOfType<MainDeckHandler>();
        decks = FindObjectsOfType<DeckBehaviour>();
        notificationsHandler = FindObjectOfType<NotificationsHandler>();

    }


    void Start()
    {
        SetGameState(GameState.Setup);

        if (decks.Length < 2)
        {
            Debug.LogError("Not enough players to begin");
            return;
        }
        StartGameSequence();
    }

    private void StartGameSequence()
    {
        mainDeckHandler.StartGame();
        StartCoroutine(InitDecksAsync());
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
    private IEnumerator InitDecksAsync()
    {
        //Deal even amount of card to all players
        int totalCardsInDeck = mainDeckHandler.GetDeckSize();
        int cardsPerPlayer = Mathf.FloorToInt(totalCardsInDeck / decks.Length);
        Debug.Log($"Dealing {totalCardsInDeck} cards to {decks.Length} players. {cardsPerPlayer} cards per player");

        for (int index = 0; index < cardsPerPlayer * decks.Length; index++)
        {
            int deckIndex = index % decks.Length;
            Card card;
            DrawCard(index, out card);

            //animate card dealing
            AnimateCard(deckIndex, card.gameObject);

            yield return new WaitForSeconds(timeBetweenDealingCards);

        }
        state = GameState.Idle;
    }

    private void DrawCard(int i, out Card card)
    {
        card = mainDeckHandler.GetCard(i);
        if (card != null)
            card.gameObject.SetActive(true);
    }

    private void AnimateCard(int deckIndex, GameObject cardObject)
    {
        CardMover cardMover = cardObject.GetComponent<CardMover>();
        cardMover.HandleCardTransitions(decks[deckIndex].transform, CardAnimations.throwCard);
    }

    private void HandleRound()
    {
        if (state != GameState.Idle) return;
        SetGameState(GameState.Round);
        BattleSequence();

    }

    private void BattleSequence()
    {
        SpawnCardsFaceDown(false);
        if (CheckScoreAsync()) return;
        else StartCoroutine(HandleWarAsync());
    }

    private IEnumerator HandleWarAsync()
    {
        yield return new WaitForSeconds(.5f);//dramatic pause

        SetGameState(GameState.War);

        //check if all players able to start war phase
        //if (!CheckWarConditions()) yield break;
        if (!CheckWarConditions()) yield break; //not tested yet

        //start war phase
        SpawnCardsFaceDown(true);
        yield return new WaitForSeconds(.5f);
        BattleSequence();

    }

    private bool CheckScoreAsync()
    {
        //check and compare value of the latest card in the secondary deck
        int winningDeck = -1, score = -1;
        for (int deckIndex = 0; deckIndex < decks.Length; deckIndex++)
        {
            int tempScore = decks[deckIndex].CheckScore();
            Debug.Log($"{decks[deckIndex].transform.parent.name} Drew {tempScore}");
            if (tempScore == score)
            {
                return false;
            }
            if (tempScore < score) continue;
            score = tempScore;
            winningDeck = deckIndex;
        }
        //declare winning deck
        OnMatchWin?.Invoke(decks[winningDeck].transform);
        return true;
    }


    private bool CheckWarConditions()
    {
        foreach (DeckBehaviour deck in decks)
            if (deck.transform.childCount < 2)
            {
                SetGameState(GameState.Win);    //end game at this point
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
        //Game state machine
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
        OnStateChanged?.Invoke(state);
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
            if (TryGetComponent(out BoxCollider2D collider2D))
                collider2D.enabled = active;
    }
}
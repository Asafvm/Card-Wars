using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;

using UnityEngine;
using UnityEngine.Events;

using Random = System.Random;

public class DeckHandler : MonoBehaviour
{
    public Sprite[] cardFaces;
    public Sprite cardBack;
    public Card cardPrefab;
    List<Card> cardPool = new List<Card>();

    public static string[] suits = new string[] { "H", "S", "D", "C" };
    public static string[] ranks = new string[] { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };

    [Serializable]
    public class DeckChangeEvent : UnityEvent<string> { }
    [SerializeField] DeckChangeEvent OnDeckChangedEvent;

    

    List<string> deck = new List<string>();
    void Awake()
    {
        GenerateDeck();
        Debug.Log("Deck ready");
    }

    public void StartGame()
    {
        CollectCards(); //collect and hide cards
        ShuffleDeck();  //shuffle before the game begins
    }

    private void CollectCards()
    {
        Card[] cards = FindObjectsOfType<Card>();
        foreach(Card card in cards)
        {
            card.transform.position = transform.position;
            card.gameObject.SetActive(false);
        }
    }

    private IEnumerator OnTransformChildrenChanged()
    {
        OnDeckChangedEvent?.Invoke(transform.childCount.ToString());

        yield return new WaitForSeconds(2f); //let animation finish
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(false);
    }

    private void CreateNewCard(int index)
    {
        Card card = Instantiate(cardPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        card.cardBack = cardBack;
        card.cardFace = cardFaces[index];
        card.name = deck[index];
        card.value = (CardValue)((index + 1) - 13 * Mathf.FloorToInt(index / 13));    //get card value from running index
        card.gameObject.SetActive(false);
        card.gameObject.layer = LayerMask.NameToLayer("UI");
        card.transform.parent = GameObject.Find("Deck").transform;
        cardPool.Add(card);
    }

    internal int GetDeckSize()
    {
        return cardFaces.Length;
    }

    public GameObject GetCard(int index)
    {
        if (index < 0 || index > cardPool.Count) return null;
        return cardPool[index].gameObject;
    }

    private void PrintDeck()
    {
        float yOffset = 0f, zOffset = 0f;

        foreach (Card card in cardPool)
        {
            card.transform.position = transform.position - new Vector3(0, yOffset, zOffset);
            card.gameObject.SetActive(true);
            yOffset += .03f;
            zOffset += .01f;
        }
    }



    private void GenerateDeck()
    {
        foreach (string suit in suits)
        {
            foreach (string rank in ranks)
            {
                deck.Add(suit + rank);
            }
        }

        CreateCardPool();

    }
    private void CreateCardPool()
    {
        for (int index = 0; index < deck.Count; index++)
        {
            CreateNewCard(index);

        }

    }
    //code taken from stackoverflow
    private void ShuffleDeck()
    {
        Random rng = new Random();
        int n = cardPool.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            Card value = cardPool[k];
            cardPool[k] = cardPool[n];
            cardPool[n] = value;
        }
    }
}

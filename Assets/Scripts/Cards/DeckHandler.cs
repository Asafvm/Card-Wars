using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class DeckHandler : MonoBehaviour
{
    public Sprite[] cardFaces;
    public Sprite cardBack;
    public Card cardPrefab;
    List<Card> cardPool = new List<Card>();

    public static string[] suits = new string[] { "H", "S", "D", "C" };
    public static string[] ranks = new string[] { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };

    List<string> deck = new List<string>();
    void Awake()
    {
        GenerateDeck();
        ShuffleDeck();
        Debug.Log("Deck ready");
        //PrintDeck(); //display all cards
    }

    private void CreateCardPool()
    {
        foreach (string card in deck)
        {
            Card c = GetCard(deck.IndexOf(card));
            c.transform.parent = GameObject.Find("Deck").transform;
            cardPool.Add(c);

        }

    }

    internal int GetDeckSize()
    {
        return cardFaces.Length;
    }

    public Card GetCard(int index)
    {
        Card c = Instantiate(cardPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        c.cardBack = cardBack;
        c.cardFace = cardFaces[index];
        c.name = deck[index];

        c.gameObject.SetActive(false);
        return c;
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

    private void ShuffleDeck()
    {

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


}

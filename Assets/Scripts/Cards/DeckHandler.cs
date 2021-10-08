using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckHandler : MonoBehaviour
{
    public Sprite[] cardFaces;
    public Sprite cardBack;
    public Card cardPrefab;

    private static int index = 0;

    public static string[] suits = new string[] { "H", "S", "D", "C" };
    public static string[] ranks = new string[] { "A","2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };

    List<string> deck = new List<string>();
    void Start()
    {
        GenerateDeck();
        ShuffleDeck();

        PrintDeck();
	}

	public Card GetNextCard()
	{
        if (index >= deck.Count) return null;
        return GetCard(index++);

	}

	private Card GetCard(int index)
	{
        Card c = Instantiate(cardPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        c.cardBack = cardBack;
        c.cardFace = cardFaces[index];
        c.name = deck[index];
        return c;
    }

	private void PrintDeck()
    {
        float yOffset = 0f, zOffset = 0f;

        foreach (string card in deck)
        {
            
            yOffset += .3f;
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
	}

	    
}

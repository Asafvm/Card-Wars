using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class CardSpawner : MonoBehaviour
{
	DeckHandler deck;

	private void Start()
	{
		deck = FindObjectOfType<DeckHandler>();
	}
	private void OnMouseDown()
	{
		if (Input.GetMouseButtonDown(0))
		{
			SpawnCard();
		}
	}

	private void SpawnCard()
	{
		Card card = deck.GetNextCard();
		Debug.Log($"Drew {card.name}");
	}
}

using System.Collections.Generic;
using System.Reflection;

using UnityEngine;
using UnityEngine.SocialPlatforms;

using Random = System.Random;

public class Deck : MonoBehaviour//, IDeckCreator
{
    protected List<Card> cardPool = null;
    [SerializeField]
    private Card cardPrefab = null;
    [SerializeField]
    private DeckConfig deckConfig = null;

    internal int GetDeckSize()
    {
        return cardPool.Count;
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

    public void InitDeck()
    {
        if (cardPool == null)
            cardPool = GenerateDeck();
        ShuffleDeck();
    }

    public Card GenerateCard(int index)
    {
        Card card = Instantiate(cardPrefab, transform.localPosition, Quaternion.identity);
        card.cardBack = deckConfig.cardBack;
        card.cardFace = deckConfig.cardFaces[index];
        card.name = deckConfig.GetCardNames()[index];
        card.value = (CardValue)((index + 1) - 13 * Mathf.FloorToInt(index / 13));    //get card value from running index
        card.gameObject.SetActive(false);
        card.gameObject.layer = LayerMask.NameToLayer("UI");
        card.transform.SetParent(transform, false);
        return card;
    }

    public List<Card> GenerateDeck()
    {
        List<Card> deck = new List<Card>();
        List<string> cards = deckConfig.GetCardNames();
        Debug.Log($"{transform.parent.name} Generating Deck with {cards.Count} cards");

        for (int index = 0; index < cards.Count; index++)
        {

            deck.Add(GenerateCard(index));
        }
        return deck;
    }
}


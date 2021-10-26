using System.Collections.Generic;

using UnityEngine;

internal interface IDeckCreator
{
    
    /// <summary>
    /// The card GameObject prefab
    /// </summary>
    [SerializeField]
    Card CardPrefab { get; set; }

    /// <summary>
    /// Represent the selected card set
    /// </summary>
    [SerializeField]
    DeckConfig DeckConfig { get; set; }

    /// <summary>
    /// Retuens a Card object by given index
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    Card GenerateCard(int index);
    /// <summary>
    /// Returns a list of cards representing a deck
    /// </summary>
    /// <returns></returns>
    List<Card> GenerateDeck();

}
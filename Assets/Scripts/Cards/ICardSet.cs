using System.Collections.Generic;

using UnityEngine;

internal interface ICardSet
{
    /// <summary>
    /// List of sorted card faces by suit then rank
    /// </summary>
    [SerializeField] Sprite[] CardFaces { get; set; }
    /// <summary>
    /// Card backside image
    /// </summary>
    [SerializeField] Sprite CardBack { get; set; }
    /// <summary>
    /// Number of sets in the deck
    /// </summary>
    string[] Suits { get; set; }
    /// <summary>
    /// Number of ranks in a set
    /// </summary>
    string[] Ranks { get; set; }
    /// <summary>
    /// Represents the deck in string form. Ordered by suit then rank
    /// </summary>
    List<string> CardNames { get; set; }
    /// <summary>
    /// Returns a list of strings representing the deck (each string is a card name)
    /// </summary>
    List<string> GetCardNames();
}
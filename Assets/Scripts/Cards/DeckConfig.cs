using System.Collections.Generic;
using System.Runtime.InteropServices;

using UnityEngine;


[CreateAssetMenu(fileName = "Card Set", menuName = "Deck/Create New Card Set", order = 0)]
public class DeckConfig : ScriptableObject//, ICardSet
{
    [SerializeField] public Sprite[] cardFaces;
    [SerializeField] public Sprite cardBack;
    public string[] Suits { get; set; } = new string[] { "H", "S", "D", "C" };
    public string[] Ranks { get; set; } = new string[] { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };
    public List<string> CardNames { get; set; }

    public List<string> GetCardNames()
    {
        CardNames.Clear();
        foreach (string suit in Suits)
        {
            foreach (string rank in Ranks)
            {
                CardNames.Add(suit + rank);
            }
        }
        return CardNames;
    }

}

using System;
using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;

public class ScoreHandler : MonoBehaviour
{
    int currentScore = 0;
    int currentValue = 0;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] Transform cardsToCheck;

    private void Update()
    {
        UpdateScore();
    }

    private void UpdateScore()
    {
        if(currentScore==0)
            scoreText.text = "";
        scoreText.text = $"Wins: {currentScore}";
    }

    public int GetCurrentCardValue()
    {
        Card card = GetCurrentCard();
        if (card == null) return 0;
        return (int)card.value;
    }

    private Card GetCurrentCard()
    {

        if (cardsToCheck.childCount < 1) return null;
        int cardsNested = cardsToCheck.childCount;
        Card card = cardsToCheck.GetChild(cardsNested - 1).GetComponent<Card>();
        return card;
        
    }

    private void AwardPoint()
    {
        currentScore++;
        UpdateScore();
    }

    internal void HandleWin()
    {
        StartCoroutine(Win());
        

    }

    private IEnumerator Win()
    {
        yield return new WaitForSeconds(1f);
        AwardPoint();
        GetCurrentCard().ShowWinEffect();
    }
}

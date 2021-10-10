using System;
using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;

public class ScoreHandler : MonoBehaviour
{
    int currentScore = 0;
    [SerializeField] TextMeshProUGUI scoreText;

    SessionManager sessionManager;
    private void Awake()
    {
        sessionManager = FindObjectOfType<SessionManager>();
    }

    private void OnEnable()
    {
        sessionManager.OnMatchWin += HandleWin;
    }

    private void OnDisable()
    {
        sessionManager.OnMatchWin -= HandleWin;
    }

    private void Update()
    {
        UpdateScore();
    }


    private void UpdateScore()
    {
        if (currentScore == 0)
        {
            scoreText.text = "";
            return;
        }
            
        scoreText.text = $"Wins: {currentScore}";
    }

    

   

    private IEnumerator AwardPoint()
    {

        yield return new WaitForSeconds(.7f); //sync with visual effects in SecondaryDeck
        currentScore++;
        UpdateScore();
    }

    internal void HandleWin(Transform winningDeck)
    {


        if (transform.parent.GetComponentInChildren<DeckBehaviour>().transform != winningDeck) return;
        StartCoroutine(AwardPoint());
    }

    
  

}

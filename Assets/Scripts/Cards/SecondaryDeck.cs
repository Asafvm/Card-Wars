using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

using UnityEngine;

public class SecondaryDeck : MonoBehaviour
{
    [SerializeField] ParticleSystem winEffect = null;
    private SessionManager sessionManager;
    private void Awake()
    {
        sessionManager = FindObjectOfType<SessionManager>();
    }

    private void OnEnable()
    {
        sessionManager.OnMatchWin += HandleWinAsync;
    }

    private void OnDisable()
    {
        sessionManager.OnMatchWin -= HandleWinAsync;
    }
 
    private Card GetCurrentCard()
    {
        if (transform.childCount < 1) return null;
        int cardsNested = transform.childCount;
        Card card = transform.GetChild(cardsNested - 1).GetComponent<Card>();
        return card;

    }
    public int GetCurrentCardValue()
    {
        Card card = GetCurrentCard();
        if (card == null)
        {
            Debug.Log("No card in deck");
            return 0;
        }
            
        return card.GetValue();
    }


    private void HandleWinAsync(Transform winningDeck)
    {
        StartCoroutine(WinRoutineAsync(winningDeck));
    }

    private IEnumerator WinRoutineAsync(Transform winningDeck)
    {
        yield return new WaitForSeconds(.7f);   //sync with visual effects in ScoreHandler
        ShowWinEffectAsync(winningDeck);
        yield return new WaitForSeconds(1.5f);
        ClearSecondaryDeckAsync(winningDeck);
    }

    private void ClearSecondaryDeckAsync(Transform winningDeck)
    {
        if (transform.childCount < 1) return;
        do
        {
            Transform cardObject = transform.GetChild(0);
            cardObject.GetComponent<CardMover>().HandleCardTransitions(winningDeck, CardAnimations.throwCard);
        } while (transform.childCount > 0);
    }

    public void ShowWinEffectAsync(Transform winningDeck)
    {
        if (transform.parent.GetComponentInChildren<DeckBehaviour>().transform != winningDeck)
        {
            Card card = GetCurrentCard();
            card.GetComponent<Dissolve>().StartDissolvingAsync();
            return;
        }
            


        if (winEffect != null)
        {
            Card card = GetCurrentCard();
            ParticleSystem ps;
            if (card == null)
                ps =Instantiate(winEffect, transform.position, Quaternion.identity);
            else
                ps =Instantiate(winEffect, card.transform.position, Quaternion.identity);

            //play sound effect
            if (PlayerPrefs.GetInt("Sound") == 1)
                GetComponent<AudioSource>().Play();
            //scale effect to card size
            ps.transform.SetParent(card.transform, false);
            ps.transform.localPosition = Vector3.zero;
        }

    }
}

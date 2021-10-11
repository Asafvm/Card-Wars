using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class SecondaryDeck : MonoBehaviour
{
    [SerializeField] ParticleSystem winEffect = null;
    private SessionManager sessionManager;
    private const float yOffset = .3f, zOffset = .02f;
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
    private void OnTransformChildrenChanged()
    {
        OrginizeTransformChildren();
    }

    private void OrginizeTransformChildren()
    {

        if (transform.childCount < 1) return;
        for (int i = 0; i < transform.childCount; i++)
        {
            float forward = transform.rotation.z == 0 ? 1 : -1; //stack will build upwards depending on which side of the table the player is

            transform.GetChild(i).GetComponent<CardMover>().SetDestination(transform.position + new Vector3(0, forward * yOffset * i, -(zOffset * i)), 0);
        }
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
        if (card == null) return 0;
        return (int)card.value;
    }


    private void HandleWin(Transform winningDeck)
    {
        StartCoroutine(WinRoutine(winningDeck));
    }

    private IEnumerator WinRoutine(Transform winningDeck)
    {
        yield return new WaitForSeconds(.7f);   //sync with visual effects in ScoreHandler
        ShowWinEffect(winningDeck);
        yield return new WaitForSeconds(1.5f);
        ClearSecondaryDeck(winningDeck);
    }

    private void ClearSecondaryDeck(Transform winningDeck)
    {
        if (transform.childCount < 1) return;

        do
        {
            transform.GetChild(0).GetComponent<CardMover>().HandleCardTransitions(winningDeck, CardAnimations.throwCard);
        } while (transform.childCount > 0);
        
    }

    public void ShowWinEffect(Transform winningDeck)
    {
        if (transform.parent.GetComponentInChildren<DeckBehaviour>().transform != winningDeck) return;

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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CardMover : MonoBehaviour
{
    Animator cardAnimator;

    Vector3 destination;
    float animationTime;
    [SerializeField] UnityEvent OnCardMove;
    private void Awake()
    {
        cardAnimator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (destination != transform.position)
        {
            if (animationTime == 0) animationTime = .1f;
            transform.position = Vector3.MoveTowards(transform.position, destination,
                (Vector3.Distance(destination, transform.position) / animationTime) * Time.deltaTime);
            return;
        }
    }

    internal void SetDestination(Vector3 destination, float time)
    {

        this.destination = destination;
        animationTime = time;
    }
    public void HandleCardTransitions(Transform cardDestination, string animationTrigger)
    {
        gameObject.SetActive(true);

        if (!String.IsNullOrEmpty(animationTrigger))
            cardAnimator.SetTrigger(animationTrigger);
        SetDestination(cardDestination.position, GetAnimationTime(cardAnimator) / 2);
        transform.parent = cardDestination;
        if(PlayerPrefs.GetInt("Sound") == 1)
            OnCardMove?.Invoke();
    }

    public float GetAnimationTime(Animator cardAnimator)
    {
        AnimationClip[] clips = cardAnimator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            switch (clip.name)
            {
                case "Flip":
                    return clip.length;
                case "Throw":
                    return clip.length;
            }
        }
        return 0;

    }
}

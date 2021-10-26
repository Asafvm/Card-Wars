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

    Transform targetParent;
    private void Awake()
    {
        cardAnimator = GetComponent<Animator>();
    }
    private void Update()
    {
       
    }

    
    public void HandleCardTransitions(Transform cardDestination, string animationTrigger)
    {
        gameObject.SetActive(true);

        if (!String.IsNullOrEmpty(animationTrigger))
            cardAnimator.SetTrigger(animationTrigger);
        float animationTime = GetAnimationTime(cardAnimator);
        StartCoroutine(MoveTo(cardDestination, animationTime));


        if (PlayerPrefs.GetInt("Sound") == 1)    //sounds enabled?
            OnCardMove?.Invoke();
    }

    private IEnumerator MoveTo(Transform destination, float animationTime)
    {
        transform.SetParent(destination, false);

        while ((destination.position - transform.position).magnitude > 1f)
        {
            if (animationTime == 0) animationTime = .1f;

            transform.position = Vector3.MoveTowards(transform.position, destination.position,
                (Vector3.Distance(destination.position, transform.position) / animationTime) * Time.deltaTime);
            yield return null;
        }

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

    //public IEnumerator AttachToParent(float animationDelay)
    //{
    //    yield return new WaitForSeconds(animationDelay);
    //    transform.SetParent(targetParent);
    //}
}

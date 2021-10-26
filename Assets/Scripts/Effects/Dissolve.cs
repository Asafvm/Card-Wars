using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

using UnityEngine;

public class Dissolve : MonoBehaviour
{
    
    
    private SpriteRenderer spriteRenderer;
    private float fade = 1f;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private IEnumerator DissolveEffect()
    {
        do
        {
            spriteRenderer.material.SetFloat("_Fade", fade);

            fade -= Time.deltaTime;
            if (fade < 0) fade = 0;
            yield return null;

        } while (fade > 0);


    }

    public void StartDissolvingAsync()
    {
        fade = 1f;
        StartCoroutine(DissolveEffect());
    }
}

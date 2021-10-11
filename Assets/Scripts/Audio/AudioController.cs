using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] AudioClip backgroundMusic, warTheme, gameEndTheme;
    AudioSource source;
    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    public void PlayMusic(GameState state)
    {
        switch (state)
        {
            case GameState.Setup:
                PlayBackgroundMusic();
                break;
            case GameState.Idle:
                PlayBackgroundMusic();
                break;
            case GameState.Round:
                PlayBackgroundMusic();
                break;
            case GameState.War:
                PlayWarMusic();
                break;
            case GameState.Win:
                PlayVictoryMusic();
                break;
        }
    }

    private void PlayVictoryMusic()
    {
        if (source.clip == gameEndTheme) return;
        source.clip = gameEndTheme;
        source.Play();
    }

    private void PlayWarMusic()
    {
        if (source.clip == warTheme) return;
        source.clip = warTheme;
        source.Play();
    }

    private void PlayBackgroundMusic()
    {
        if (source.clip == backgroundMusic) return;
        source.clip = backgroundMusic;
        source.Play();
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundPrefsCheck : MonoBehaviour
{
    [SerializeField] Button soundOn, soundOff;
    void Start()
    {
        bool enableSound = PlayerPrefs.GetInt("Sound") == 1;
        soundOn.gameObject.SetActive(!enableSound);
        soundOff.gameObject.SetActive(enableSound);
    }

}

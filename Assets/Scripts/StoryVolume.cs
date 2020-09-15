using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryVolume : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("MusicVolume");
    }
}

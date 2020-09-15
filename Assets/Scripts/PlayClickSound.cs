using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayClickSound : MonoBehaviour
{
    Button button;
    public AudioSource ass;
    public AudioClip ding;
    bool playDing;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(playSound);
    }

    void playSound()
    {
        if (!playDing)
            ass.Play();
    }

    public void PlayDingInstead()
    {
        playDing = true;
    }

    public void PlayDing()
    {
        ass.PlayOneShot(ding);
    }
}

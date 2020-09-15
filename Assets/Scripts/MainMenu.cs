using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        if (!PlayerPrefs.HasKey("SoundVolume"))
            PlayerPrefs.SetFloat("SoundVolume", 0.5f);
        if (!PlayerPrefs.HasKey("MusicVolume"))
            PlayerPrefs.SetFloat("MusicVolume", 0.5f);

        if (name == "MainMenu")
        {
            transform.parent.Find("Music").GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("MusicVolume");
            GetComponentInParent<AudioSource>().volume = PlayerPrefs.GetFloat("SoundVolume");

            Button[] buttons = transform.parent.gameObject.GetComponentsInChildren<Button>(true);
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].gameObject.AddComponent<PlayClickSound>().ass = GetComponentInParent<AudioSource>();
            }
        }
    }

    public void PlayGame ()
    {
        SceneManager.LoadScene("Story");
        RemvoveSave();
    }

    void RemvoveSave()
    {
        float tempVolume = PlayerPrefs.GetFloat("MusicVolume");
        float tempVolume2 = PlayerPrefs.GetFloat("SoundVolume");
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetFloat("MusicVolume", tempVolume);
        PlayerPrefs.SetFloat("SoundVolume", tempVolume2);
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void quitToMenu(bool removeSave)
    {
        if (removeSave)
            RemvoveSave();
        SceneManager.LoadScene("Menu");
    }

    public void QuitGame ()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }

    public void optionsSetup(GameObject options)
    {
        if (PlayerPrefs.HasKey("SoundVolume"))
            options.transform.Find("sounds").GetComponent<Slider>().value = PlayerPrefs.GetFloat("SoundVolume");
        if (PlayerPrefs.HasKey("MusicVolume"))
            options.transform.Find("music").GetComponent<Slider>().value = PlayerPrefs.GetFloat("MusicVolume");
    }

    public void changeSoundVolume(Slider slidey)
    {
        PlayerPrefs.SetFloat("SoundVolume", slidey.value);
        GetComponentInParent<AudioSource>().volume = slidey.value;
    }

    public void changeMusicVolume(Slider slidey)
    {
        PlayerPrefs.SetFloat("MusicVolume", slidey.value);
        transform.parent.Find("Music").GetComponent<AudioSource>().volume = slidey.value;
    }
}

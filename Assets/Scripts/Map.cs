using System;
using UnityEngine;
using UnityEngine.UI;

public class Map : MonoBehaviour
{
    Button next, back;
    [Range(0, 3)]
    public int levelsNeededToPass;

    StartBattle[] battleButtons;

    void OnEnable()
    {
        battleButtons = GetComponentsInChildren<StartBattle>(true);

        if (transform.Find("NextLevel") != null)
            next = transform.Find("NextLevel").GetComponent<Button>();
        if (transform.Find("BackLevel") != null)
            back = transform.Find("BackLevel").GetComponent<Button>();

        if (next)
            next.onClick.AddListener(NextLevel);
        if (back)
            back.onClick.AddListener(BackLevel);

        foreach (StartBattle sb in battleButtons)
        {
            if (!PlayerPrefs.HasKey("BeatBattle" + sb.name.Split('-')[1]))
                PlayerPrefs.SetInt("BeatBattle" + sb.name.Split('-')[1], 0);
            else if (PlayerPrefs.HasKey("BeatBattle" + sb.name.Split('-')[1]) && PlayerPrefs.GetInt("BeatBattle" + sb.name.Split('-')[1]) == 1)
                sb.transform.GetChild(0).gameObject.SetActive(true);
            sb.ShowPreview();
        }

    }

    private void Update()
    {
        int good = 0;
        foreach (StartBattle sb in battleButtons)
        {
            if (sb.transform.GetChild(0).gameObject.activeSelf)
                good++;
        }
        if (good >= levelsNeededToPass)
            transform.Find("NextLevel").gameObject.SetActive(true);
        else
            transform.Find("NextLevel").gameObject.SetActive(false);
    }

    void NextLevel()
    {
        if (transform.parent.GetChild(transform.GetSiblingIndex() + 1) != null)
        {
            transform.parent.GetChild(transform.GetSiblingIndex() + 1).gameObject.SetActive(true);
            if (transform.parent.GetChild(transform.GetSiblingIndex() + 1).name.Contains("Level"))
                PlayerPrefs.SetInt("stage", Convert.ToInt32(transform.parent.GetChild(transform.GetSiblingIndex() + 1).name.Split(' ')[1]));
            gameObject.SetActive(false);
        }
    }

    void BackLevel()
    {
        if (transform.parent.GetChild(transform.GetSiblingIndex() - 1) != null)
        {
            transform.parent.GetChild(transform.GetSiblingIndex() - 1).gameObject.SetActive(true);
            if (name.Contains("Level"))
                PlayerPrefs.SetInt("stage", Convert.ToInt32(name.Split(' ')[1]) - 1);
            gameObject.SetActive(false);
        }
    }

    public void updateButtons()
    {
        OnEnable();
    }
}

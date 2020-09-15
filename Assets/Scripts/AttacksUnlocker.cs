using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AttacksUnlocker : MonoBehaviour
{

    int playerLevel;

    GameObject[] unlockableButtons;
    Sprite[] buttonsOriginalTextures;

    public Sprite lockSprite;

    [HideInInspector]
    public int[] coolDownCounters;

    void Start()
    {
        coolDownCounters = new int[transform.childCount - 1];
        playerLevel = PlayerPrefs.GetInt("level");
        unlockableButtons = new GameObject[transform.childCount - 1];
        buttonsOriginalTextures = new Sprite[unlockableButtons.Length];
        for (int i = 0; i < unlockableButtons.Length; i++)
        {
            unlockableButtons[i] = transform.GetChild(i).gameObject;
            buttonsOriginalTextures[i] = unlockableButtons[i].GetComponent<Image>().sprite;
            if (playerLevel < Convert.ToInt32(unlockableButtons[i].name.Split(' ')[1].Split('-')[1]))
            {
                unlockableButtons[i].GetComponent<Image>().sprite = lockSprite;
                unlockableButtons[i].GetComponent<Button>().interactable = false;
                unlockableButtons[i].transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }

    private void OnEnable()
    {
        coolDownCounters = new int[transform.childCount - 1];
        unlockableButtons = new GameObject[transform.childCount - 1];
        for (int i = 0; i < unlockableButtons.Length; i++)
        {
            unlockableButtons[i] = transform.GetChild(i).gameObject;
            EndCoolDown(i);
        }
    }

    private void Update()
    {
        if (transform.parent.name != "ButtonsHolder")
        {
            transform.GetChild(transform.childCount - 1).GetChild(0).GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetInt("ownedPotions1").ToString();
            if (PlayerPrefs.GetInt("ownedPotions1") == 0)
                transform.GetChild(transform.childCount - 1).GetComponent<Button>().interactable = false;
            else
                transform.GetChild(transform.childCount - 1).GetComponent<Button>().interactable = true;
        }
    }

    public void CheckUnlocks()
    {
        playerLevel = PlayerPrefs.GetInt("level");
        for (int i = 1; i < unlockableButtons.Length; i++)
        {
            if (playerLevel >= Convert.ToInt32(unlockableButtons[i].name.Split(' ')[1].Split('-')[1]))
            {
                unlockableButtons[i].GetComponent<Image>().sprite = buttonsOriginalTextures[i];
                unlockableButtons[i].GetComponent<Button>().interactable = true;
                if (unlockableButtons[i].transform.GetSiblingIndex() == transform.childCount - 2 && unlockableButtons[i].transform.GetChild(0).GetComponent<Slider>().value < unlockableButtons[i].transform.GetChild(0).GetComponent<Slider>().maxValue)
                    unlockableButtons[i].GetComponent<Button>().interactable = false;
                unlockableButtons[i].transform.GetChild(0).gameObject.SetActive(true);
            }
        }
    }

    public void CoolDown(GameObject button)
    {
        print(button);
        if (button.transform.GetSiblingIndex() < transform.childCount - 2 && button.name.Contains("Attack") && Convert.ToInt32(button.name.Split(' ')[2].Split('-')[1]) > 0){
            coolDownCounters[GetChildIndex(gameObject, button)] = Convert.ToInt32(button.name.Split(' ')[2].Split('-')[1]) + 1;
            button.GetComponent<Button>().interactable = false;
            button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = coolDownCounters[GetChildIndex(gameObject, button)].ToString();
        }
    }

    public void countDownCoolDown(int index)
    {
        if (transform.GetChild(index).GetSiblingIndex() < transform.childCount - 2)
            transform.GetChild(index).GetChild(0).GetComponent<TextMeshProUGUI>().text = coolDownCounters[index].ToString();
    }

    public void EndCoolDown(int index)
    {
        print("endcooldown");
        if (transform.GetChild(index).GetSiblingIndex() < transform.childCount - 2 && transform.GetChild(index).GetChild(0).gameObject.activeSelf)
        {
            transform.GetChild(index).GetComponent<Button>().interactable = true;
            countDownCoolDown(index);
        }
    }

    int GetChildIndex(GameObject parent, GameObject child)
    {
        for(int i = 0; i < parent.transform.childCount - 1; i++)
        {
            if (parent.transform.GetChild(i).name == child.name)
                return i;
        }
        return -1;
    }
}

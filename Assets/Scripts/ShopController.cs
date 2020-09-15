using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopController : MonoBehaviour
{
    public GameObject player;
    Level levelScript;
    GameObject attackButtons;
    int requiredGoldForLevelUpAttack = 20;
    int requiredGoldForLevelUpHealth = 30;
    int amountOfHealthToAddToMaxHealthOnUpgradeHealth = 15;


    private void OnEnable()
    {
        StartCoroutine(start());

    }

    IEnumerator start()
    {
        levelScript = GameObject.Find("Canvas").GetComponent<Level>();

        GameObject playerGO = Instantiate(player, transform.Find("PlayerShadow").GetChild(0));
        playerGO.GetComponent<CharacterAnimator>().idleFrames = levelScript.playerIdleFrames;
        playerGO.GetComponent<CharacterAnimator>().baseAttackFrames = levelScript.playerBaseAttackFrames;
        playerGO.GetComponent<CharacterAnimator>().specialAttackFrames = levelScript.playerSpecialAttackFrames;
        playerGO.GetComponent<CharacterAnimator>().ultimateAttackFrames = levelScript.playerUltimateAttackFrames;
        playerGO.GetComponent<CharacterAnimator>().defendFrames = levelScript.playerDefendFrames;
        playerGO.AddComponent(levelScript.GetPlayersEffectsAnimator());

        if (transform.Find("ButtonsHolder").childCount > 0)
        {
            foreach(Transform child in transform.Find("ButtonsHolder"))
            {
                Destroy(child.gameObject);
            }
        }
        attackButtons = Instantiate(levelScript.GetPlayersAttackButtons(), transform.Find("ButtonsHolder"));
        attackButtons.transform.SetAsLastSibling();
        attackButtons.name = "Attacks";
        attackButtons.SetActive(true);

        Destroy(attackButtons.transform.GetChild(attackButtons.transform.childCount - 1).gameObject);

        yield return new WaitForSeconds(0.01f);

        attackButtons.transform.GetChild(attackButtons.transform.childCount - 1).GetComponent<Button>().interactable = true;
        Destroy(attackButtons.transform.GetChild(attackButtons.transform.childCount - 1).GetChild(0).gameObject);
        GameObject text = attackButtons.transform.GetChild(attackButtons.transform.childCount - 2).GetChild(0).gameObject;
        Instantiate(text, attackButtons.transform.GetChild(attackButtons.transform.childCount - 1));

        attackButtons.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(LevelUpOne);
        attackButtons.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(LevelUpTwo);
        attackButtons.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(LevelUpThree);
        attackButtons.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(LevelUpFour);
        attackButtons.transform.GetChild(4).GetComponent<Button>().onClick.AddListener(LevelUpFive);
        transform.Find("UpgradeHealth").GetChild(1).GetComponent<TextMeshProUGUI>().text = "Max HP: " + PlayerPrefs.GetInt("playerMaxHealth").ToString();
        foreach(Transform child in transform.Find("Potions"))
        {
            if (child.name.Contains("Potion"))
            {
                child.Find("Buy").GetChild(0).GetComponent<TextMeshProUGUI>().text = "Buy " + child.GetChild(0).name + " | " + child.name.Split(' ')[1] + " Gold";
                child.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Owned: " + PlayerPrefs.GetInt("ownedPotions" + child.name.Split(' ')[2]);
            }
        }

        yield return new WaitForSeconds(0.1f);

        for (int i = 0; i < attackButtons.transform.childCount; i++)
        {
            print(attackButtons.transform.GetChild(i).GetChild(0).name);
            attackButtons.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Level " + PlayerPrefs.GetInt("attack" + (i + 1).ToString() + "level") + " | " + requiredGoldForLevelUpAttack.ToString() + " Gold";
        }

        Button[] buttons = GetComponentsInChildren<Button>(true);
        for (int i = 0; i < buttons.Length; i++)
        {
            if (!buttons[i].name.ToLower().Contains("back"))
                buttons[i].gameObject.GetComponent<PlayClickSound>().PlayDingInstead();
        }
    }

    void LevelUpOne()
    {
        if (PlayerPrefs.GetInt("gold") > requiredGoldForLevelUpAttack)
        {
            PlayerPrefs.SetInt("attack1level", PlayerPrefs.GetInt("attack1level") + 1);
            PlayerPrefs.SetInt("gold", PlayerPrefs.GetInt("gold") - requiredGoldForLevelUpAttack);
            attackButtons.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Level " + PlayerPrefs.GetInt("attack1level") + " | " + requiredGoldForLevelUpAttack.ToString() + " Gold";
            UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<PlayClickSound>().PlayDing();
        }
    }

    void LevelUpTwo()
    {
        if (PlayerPrefs.GetInt("gold") > requiredGoldForLevelUpAttack)
        {
            PlayerPrefs.SetInt("attack2level", PlayerPrefs.GetInt("attack2level") + 1);
            PlayerPrefs.SetInt("gold", PlayerPrefs.GetInt("gold") - requiredGoldForLevelUpAttack);
            attackButtons.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Level " + PlayerPrefs.GetInt("attack2level") + " | " + requiredGoldForLevelUpAttack.ToString() + " Gold";
            UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<PlayClickSound>().PlayDing();
        }
    }

    void LevelUpThree()
    {
        if (PlayerPrefs.GetInt("gold") > requiredGoldForLevelUpAttack)
        {
            PlayerPrefs.SetInt("attack3level", PlayerPrefs.GetInt("attack3level") + 1);
            PlayerPrefs.SetInt("gold", PlayerPrefs.GetInt("gold") - requiredGoldForLevelUpAttack);
            attackButtons.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Level " + PlayerPrefs.GetInt("attack3level") + " | " + requiredGoldForLevelUpAttack.ToString() + " Gold";
            UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<PlayClickSound>().PlayDing();
        }
    }

    void LevelUpFour()
    {
        if (PlayerPrefs.GetInt("gold") > requiredGoldForLevelUpAttack)
        {
            PlayerPrefs.SetInt("attack4level", PlayerPrefs.GetInt("attack4level") + 1);
            PlayerPrefs.SetInt("gold", PlayerPrefs.GetInt("gold") - requiredGoldForLevelUpAttack);
            attackButtons.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Level " + PlayerPrefs.GetInt("attack4level") + " | " + requiredGoldForLevelUpAttack.ToString() + " Gold";
            UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<PlayClickSound>().PlayDing();
        }
    }

    void LevelUpFive()
    {
        if (PlayerPrefs.GetInt("gold") > requiredGoldForLevelUpAttack)
        {
            PlayerPrefs.SetInt("attack5level", PlayerPrefs.GetInt("attack5level") + 1);
            PlayerPrefs.SetInt("gold", PlayerPrefs.GetInt("gold") - requiredGoldForLevelUpAttack);
            attackButtons.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Level " + PlayerPrefs.GetInt("attack5level") + " | " + requiredGoldForLevelUpAttack.ToString() + " Gold";
            UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<PlayClickSound>().PlayDing();
        }
    }

    public void LevelUpHealth()
    {
        if (PlayerPrefs.GetInt("gold") > requiredGoldForLevelUpHealth)
        {
            PlayerPrefs.SetInt("playerMaxHealth", PlayerPrefs.GetInt("playerMaxHealth") + amountOfHealthToAddToMaxHealthOnUpgradeHealth);
            PlayerPrefs.SetInt("gold", PlayerPrefs.GetInt("gold") - requiredGoldForLevelUpHealth);
            transform.Find("UpgradeHealth").GetChild(1).GetComponent<TextMeshProUGUI>().text = "Max HP: " + PlayerPrefs.GetInt("playerMaxHealth").ToString();
            UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<PlayClickSound>().PlayDing();
        }
    }

    public void BuyPotion(GameObject potion)
    {
        if (PlayerPrefs.GetInt("gold") > Convert.ToInt32(potion.name.Split(' ')[1]))
        {
            PlayerPrefs.SetInt("ownedPotions" + potion.name.Split(' ')[2], PlayerPrefs.GetInt("ownedPotions" + potion.name.Split(' ')[2]) + 1);
            PlayerPrefs.SetInt("gold", PlayerPrefs.GetInt("gold") - Convert.ToInt32(potion.name.Split(' ')[1]));
            potion.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Owned: " + PlayerPrefs.GetInt("ownedPotions" + potion.name.Split(' ')[2]);
            UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<PlayClickSound>().PlayDing();
        }
    }
}

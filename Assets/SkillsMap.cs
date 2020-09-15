using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillsMap : MonoBehaviour
{
    public GameObject player;
    GameObject attackButtons;
    Level levelScript;
    public Sprite potionSprite, boxSprite;

    void OnEnable()
    {
        levelScript = GameObject.Find("Canvas").GetComponent<Level>();

        GameObject playerGO = Instantiate(player, transform.Find("PlayerShadow").GetChild(0));
        playerGO.GetComponent<CharacterAnimator>().idleFrames = levelScript.playerIdleFrames;
        playerGO.GetComponent<CharacterAnimator>().baseAttackFrames = levelScript.playerBaseAttackFrames;
        playerGO.GetComponent<CharacterAnimator>().specialAttackFrames = levelScript.playerSpecialAttackFrames;
        playerGO.GetComponent<CharacterAnimator>().ultimateAttackFrames = levelScript.playerUltimateAttackFrames;
        playerGO.GetComponent<CharacterAnimator>().defendFrames = levelScript.playerDefendFrames;
        playerGO.AddComponent(levelScript.GetPlayersEffectsAnimator());
        bool alreadyThere = false;
        if (transform.Find("ButtonsHolder").childCount > 0)
        {
            alreadyThere = true;
            foreach (Transform child in transform.Find("ButtonsHolder"))
            {
                Destroy(child.gameObject);
            }
        }
        attackButtons = Instantiate(levelScript.GetPlayersAttackButtons(), transform.Find("ButtonsHolder"));
        attackButtons.transform.SetAsLastSibling();
        attackButtons.name = "Attacks";
        attackButtons.SetActive(true);

        Destroy(attackButtons.transform.GetChild(attackButtons.transform.childCount - 1).gameObject);

        attackButtons.transform.GetChild(attackButtons.transform.childCount - 1).GetComponent<Button>().interactable = true;
        Destroy(attackButtons.transform.GetChild(attackButtons.transform.childCount - 1).GetChild(0).gameObject);

        if (!alreadyThere)
        {
            foreach (Transform child in transform.Find("ButtonsHolder").Find("Attacks"))
            {
                child.GetComponent<Image>().raycastTarget = false;
                Destroy(child.GetChild(0).gameObject);
            }
        }

        if (PlayerPrefs.GetInt("ownedPotions1") > 0)
        {
            transform.Find("Items").GetChild(0).GetComponent<Image>().sprite = potionSprite;
            transform.Find("Items").GetChild(0).GetChild(0).gameObject.SetActive(true);
            transform.Find("Items").GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetInt("ownedPotions1").ToString();
        }
        else
        {
            transform.Find("Items").GetChild(0).GetComponent<Image>().sprite = boxSprite;
            transform.Find("Items").GetChild(0).GetChild(0).gameObject.SetActive(false);
        }
    }
}

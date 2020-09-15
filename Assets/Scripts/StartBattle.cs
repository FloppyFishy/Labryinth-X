using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartBattle : MonoBehaviour
{

    Button self;
    public GameObject enemy;
    GameObject preview, battle;

    StartBattle[] battleButtons;

    void OnEnable()
    {
        self = GetComponent<Button>();
        self.onClick.AddListener(InitiateBattle);

        battleButtons = transform.parent.parent.gameObject.GetComponentsInChildren<StartBattle>(true);
        battle = transform.parent.Find("Battle" + transform.parent.name.Split(' ')[1]).gameObject;
    }

    void InitiateBattle()
    {
        RemovePreview();
        foreach (StartBattle sb in battleButtons)
        {
            sb.RemovePreview();
        }
        
        transform.parent.parent.Find("Music").GetComponent<AudioSource>().clip = transform.parent.parent.GetComponent<Level>().battleMusic;
        transform.parent.parent.Find("Music").GetComponent<AudioSource>().Play();
        
        battle.transform.Find("BattleSystem").GetComponent<BattleSystem>().EnemyPrefab = enemy;
        battle.transform.Find("BattleSystem").GetComponent<BattleSystem>().enabled = true;
        battle.SetActive(true);
        transform.parent.parent.Find("debug level display").gameObject.SetActive(false);
        battle.gameObject.name = battle.gameObject.name + "-" + name.Split('-')[1];
    }

    public void RemovePreview()
    {
        Destroy(preview);
    }

    public void ShowPreview()
    {
        if (!transform.GetChild(0).gameObject.activeSelf && transform.childCount == 1)
        {
            preview = Instantiate(enemy, transform);
            if (enemy.name == "King Slime")
                preview.transform.localScale = preview.transform.localScale * 7;
            else
                preview.transform.localScale = preview.transform.localScale * 13;
            preview.transform.localPosition += new Vector3(0f, 5.9f, 0f);
        }
    }
}

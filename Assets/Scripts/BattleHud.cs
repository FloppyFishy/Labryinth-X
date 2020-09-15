using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHud : MonoBehaviour
{

    public Text nameText;
    public Text hpValue;
    public Slider hpSlider;
    public Text levelText;
    int maxHP;

    public void SetHUD(Unit unit)
    {
        nameText.text = unit.unitName;
        if (unit.name.Contains("Player"))
            maxHP = PlayerPrefs.GetInt("playerMaxHealth");
        else
            maxHP = unit.maxHP;
        if (hpSlider.value != 0)
        {
            unit.currentHP = Mathf.RoundToInt(hpSlider.value);
        }
        else if (hpSlider.value == 0)
        {
            if (unit.name.Contains("Player"))
            {
                hpSlider.maxValue = PlayerPrefs.GetInt("playerMaxHealth");
                if (unit.currentHP == unit.maxHP)
                    hpSlider.value = PlayerPrefs.GetInt("playerMaxHealth");
            }
            else
            {
                hpSlider.maxValue = unit.maxHP;
                hpSlider.value = unit.currentHP;
            }
        }
        if (!unit.name.Contains("Player"))
            levelText.text = "Lv" + unit.unitLevel;
        else
        {
            levelText.text = "Lv" + PlayerPrefs.GetInt("level").ToString();
            nameText.text = PlayerPrefs.GetString("PlayerName");
        }
            
        hpValue.text = hpSlider.value.ToString() + "/" + maxHP.ToString();
    }

    public void SetHP(int hp)
    {
        hpSlider.value = hp;
        hpValue.text = hp.ToString() + "/" + maxHP.ToString();
    }
}


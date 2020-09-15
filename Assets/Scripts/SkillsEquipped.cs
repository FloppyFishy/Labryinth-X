using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillsEquipped : MonoBehaviour
{

    GameObject[] buttons;

    void OnEnable()
    {
        buttons = new GameObject[transform.childCount];
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i] = transform.GetChild(i).gameObject;
            if (PlayerPrefs.GetInt("level") < Convert.ToInt32(buttons[i].name.Split(' ')[1].Split('-')[1]))
                buttons[i].transform.GetChild(0).gameObject.SetActive(true);
            else
                buttons[i].transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

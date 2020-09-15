using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UltimateSliderController : MonoBehaviour
{
    Slider slider;

    void Start()
    {
        slider = GetComponent<Slider>();
        if (slider.value != slider.maxValue)
            transform.parent.GetComponent<Button>().interactable = false;
    }

    public void TakeDamage(float damage)
    {
        if (gameObject.activeSelf)
        {
            slider.value += damage;
            if (slider.value == slider.maxValue)
                transform.parent.GetComponent<Button>().interactable = true;
        }
    }

    public void UltimateButtonClicked()
    {
        if (transform.parent.parent.parent.parent.Find("BattleSystem").GetComponent<BattleSystem>().state == BattleState.PLAYERTURN)
        {
            transform.parent.GetComponent<Button>().interactable = false;
            slider.value = 0;
        }
    }
}

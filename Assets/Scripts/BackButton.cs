using UnityEngine;
using UnityEngine.UI;

public class BackButton : MonoBehaviour
{
    Button backButton;

    void Start()
    {
        backButton = GetComponent<Button>();
        backButton.onClick.AddListener(Back);
    }

    void Back()
    {
        transform.parent.gameObject.SetActive(false);
        print("Level " + PlayerPrefs.GetInt("stage").ToString());
        transform.parent.parent.Find("Level " + PlayerPrefs.GetInt("stage").ToString()).gameObject.SetActive(true);
    }
}

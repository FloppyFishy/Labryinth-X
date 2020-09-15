using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Level : MonoBehaviour
{
    public TextMeshProUGUI leveldisplay;
    public int amountOfBattles;
    [Range(0, 1)]
    public float clickVolume;
    public AudioClip gameMusic;
    public AudioClip battleMusic;
    public AudioClip dingSound;

    [SerializeField]
    public Sprite[] characters;

    public Sprite[] playerIdleFrames;
    public Sprite[] playerBaseAttackFrames;
    public Sprite[] playerSpecialAttackFrames;
    public Sprite[] playerUltimateAttackFrames;
    public Sprite[] playerDefendFrames;

    GameObject[] gameComponents;

    private void OnValidate()
    {
        if (characters == null)
            characters = new Sprite[transform.Find("CharacterSelection").Find("CharacterViewer").Find("Characters").childCount];

        for(int i = 0; i < transform.Find("CharacterSelection").Find("CharacterViewer").Find("Characters").childCount; i++)
        {
            characters[i] = transform.Find("CharacterSelection").Find("CharacterViewer").Find("Characters").GetChild(i).GetChild(0).GetComponent<SpriteRenderer>().sprite;
        }
    }

    private void Start()
    {
        //use this below to reset all player values
        //PlayerPrefs.DeleteAll();

        if (!PlayerPrefs.HasKey("xp"))
        {
            print("hi");
            PlayerPrefs.SetInt("xp", 0);
            transform.Find("debug level display").gameObject.SetActive(false);
            transform.Find("CharacterSelection").gameObject.SetActive(true);
            transform.Find("Level 1").gameObject.SetActive(false);
        }
        else
            transform.Find("debug level display").gameObject.SetActive(true);
        if (!PlayerPrefs.HasKey("level"))
            PlayerPrefs.SetInt("level", 1);
        if (!PlayerPrefs.HasKey("attack1level"))
            PlayerPrefs.SetInt("attack1level", 1);
        if (!PlayerPrefs.HasKey("attack2level"))
            PlayerPrefs.SetInt("attack2level", 1);
        if (!PlayerPrefs.HasKey("attack3level"))
            PlayerPrefs.SetInt("attack3level", 1);
        if (!PlayerPrefs.HasKey("attack4level"))
            PlayerPrefs.SetInt("attack4level", 1);
        if (!PlayerPrefs.HasKey("attack5level"))
            PlayerPrefs.SetInt("attack5level", 1);
        if (!PlayerPrefs.HasKey("playerMaxHealth"))
            PlayerPrefs.SetInt("playerMaxHealth", 100);
        if (!PlayerPrefs.HasKey("ownedPotions1"))
            PlayerPrefs.SetInt("ownedPotions1", 0);
        if (!PlayerPrefs.HasKey("gold"))
            PlayerPrefs.SetInt("gold", 0);
        if (!PlayerPrefs.HasKey("requiredXP"))
            PlayerPrefs.SetInt("requiredXP", 100);
        if (!PlayerPrefs.HasKey("SoundVolume"))
            PlayerPrefs.SetFloat("SoundVolume", 0.5f);
        if (!PlayerPrefs.HasKey("MusicVolume"))
            PlayerPrefs.SetFloat("MusicVolume", 0.5f);
        if (!PlayerPrefs.HasKey("Character"))
            PlayerPrefs.SetInt("Character", 0);
        else
        {
            playerIdleFrames = transform.Find("CharacterSelection").Find("CharacterViewer").Find("Characters").GetChild(PlayerPrefs.GetInt("Character")).GetChild(0).GetComponent<CharacterAnimator>().idleFrames;
            playerBaseAttackFrames = transform.Find("CharacterSelection").Find("CharacterViewer").Find("Characters").GetChild(PlayerPrefs.GetInt("Character")).GetChild(0).GetComponent<CharacterAnimator>().baseAttackFrames;
            playerSpecialAttackFrames = transform.Find("CharacterSelection").Find("CharacterViewer").Find("Characters").GetChild(PlayerPrefs.GetInt("Character")).GetChild(0).GetComponent<CharacterAnimator>().specialAttackFrames;
            playerUltimateAttackFrames = transform.Find("CharacterSelection").Find("CharacterViewer").Find("Characters").GetChild(PlayerPrefs.GetInt("Character")).GetChild(0).GetComponent<CharacterAnimator>().ultimateAttackFrames;
            playerDefendFrames = transform.Find("CharacterSelection").Find("CharacterViewer").Find("Characters").GetChild(PlayerPrefs.GetInt("Character")).GetChild(0).GetComponent<CharacterAnimator>().defendFrames;
        }

        PlayerPrefs.SetInt("stage", 1);
        if (!PlayerPrefs.HasKey("PlayerName"))
            PlayerPrefs.SetString("PlayerName", "Player");

        for (int i = 1; i <= amountOfBattles; i++)
        {
            if (!PlayerPrefs.HasKey("BeatBattle" + i))
                PlayerPrefs.SetInt("BeatBattle" + i, 0);
        }

        Button[] buttons = GetComponentsInChildren<Button>(true);
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].gameObject.AddComponent<PlayClickSound>().ass = GetComponent<AudioSource>();
            buttons[i].gameObject.GetComponent<PlayClickSound>().ding = dingSound;
        }
        transform.Find("Music").GetComponent<AudioSource>().Play();
        transform.Find("Music").GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("MusicVolume");
        GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("SoundVolume");
    }  

    void Update()
    {
        if (leveldisplay != null)
            leveldisplay.text = "XP: " + PlayerPrefs.GetInt("xp").ToString() + " Level: " + PlayerPrefs.GetInt("level").ToString() + " Stage: " + PlayerPrefs.GetInt("stage").ToString() + " Gold: " + PlayerPrefs.GetInt("gold").ToString();
    }

    public void addXP(int amount, AttacksUnlocker au)
    {
        int xp = PlayerPrefs.GetInt("xp");
        int requiredXP = PlayerPrefs.GetInt("requiredXP");
        for (int i = 0; i < amount; i++)
        {
            xp++;
            if (xp == requiredXP)
            {
                LevelUp();
                requiredXP = requiredXP + 2;
                xp = 0;
            }
        }
        print("xp is now " + xp);
        PlayerPrefs.SetInt("xp", xp);
        PlayerPrefs.SetInt("requiredXP", requiredXP);
        au.CheckUnlocks();
    }

    public EffectsAnimator GetPlayersEffectsAnimator()
    {
        return transform.Find("CharacterSelection").Find("CharacterViewer").Find("Characters").GetChild(PlayerPrefs.GetInt("Character")).GetChild(0).GetComponent<EffectsAnimator>();
    }

    public GameObject GetPlayersAttackButtons()
    {
        return transform.Find("CharacterSelection").Find("CharacterViewer").Find("Characters").GetChild(PlayerPrefs.GetInt("Character")).GetChild(1).GetChild(0).gameObject;
    }

    GameObject GetCurrentLevel()
    {
        foreach(Transform child in transform)
        {
            if (child.name == "Level " + PlayerPrefs.GetInt("stage").ToString())
                return child.gameObject;
        }
        return null;
    }

    BattleSystem GetCurrentBattleSystem()
    {
        foreach (Transform child in transform)
        {
            if (child.name == "Level " + PlayerPrefs.GetInt("stage").ToString())
                return child.transform.GetChild(child.childCount-1).transform.Find("BattleSystem").GetComponent<BattleSystem>();
        }
        return null;
    }

    public void OnBaseAttackButton(int damage)
    {
        GetCurrentBattleSystem().OnBaseAttackButton(damage);
    }

    public void OnSpecialAttackButton(int damage)
    {
        GetCurrentBattleSystem().OnSpecialAttackButton(damage);
    }

    public void OnUltimateAttackButton(int damage)
    {
        GetCurrentBattleSystem().OnUltimateAttackButton(damage);
    }

    public void OnUltimateHealButton(int amountOfTurns)
    {
        GetCurrentBattleSystem().OnUltimateHealButton(amountOfTurns);
    }

    public void OnDefenseButton(int amountOfTurns)
    {
        GetCurrentBattleSystem().OnDefenseButton(amountOfTurns);
    }

    public void OnHealButton(int healAmount)
    {
        GetCurrentBattleSystem().OnHealButton(healAmount);
    }

    public void OnDamageResistance(int amountOfTurns)
    {
        GetCurrentBattleSystem().OnDamageResistance(amountOfTurns);
    }

    public void UseHealthPotion(int healingAmount)
    {
        if (PlayerPrefs.GetInt("ownedPotions1") > 0)
        {
            PlayerPrefs.SetInt("ownedPotions1", PlayerPrefs.GetInt("ownedPotions1") - 1);
            GetCurrentBattleSystem().HealPotion(healingAmount);
        }
    }

    void LevelUp()
    {
        PlayerPrefs.SetInt("level", PlayerPrefs.GetInt("level") + 1);
    }

    public void ResetSave()
    {
        PlayerPrefs.DeleteAll();
    }

    public void WonBattle(string number)
    {
        if (PlayerPrefs.HasKey("BeatBattle" + number))
            PlayerPrefs.SetInt("BeatBattle" + number, 1);
        else
            Debug.LogError("Battle doesnt exist?");

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf == true)
            {
                transform.GetChild(i).transform.Find("BattleButton-" + number.ToString()).GetChild(0).gameObject.SetActive(true);
                break;
            }
        }
    }

    public void playGameMusic()
    {
        transform.Find("Music").GetComponent<AudioSource>().clip = gameMusic;
        transform.Find("Music").GetComponent<AudioSource>().Play();
    }

    public void SkillsMenu()
    {
        StartBattle[] sbs = transform.GetComponentsInChildren<StartBattle>(true);
        foreach (StartBattle sb in sbs)
        {
            sb.RemovePreview();
        }
    }

    public void UpdateAllButtons()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name.Contains("Level") && transform.GetChild(i).gameObject.activeSelf)
                transform.GetChild(i).GetComponent<Map>().updateButtons();
        }
    }
}

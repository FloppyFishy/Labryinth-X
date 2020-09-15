using System;
using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WIN, LOST }

public class BattleSystem : MonoBehaviour
{

    public GameObject PlayerPrefab, EnemyPrefab;
    GameObject Note, playerGO, enemyGo, attackButtons;
    Button BackButton;

    Transform PlayerShadow, EnemyShadow;
    
    [HideInInspector]
    public Unit playerUnit, enemyUnit;

    [HideInInspector]
    public BattleHud playerHUD, enemyHUD;
    
    Level levelScript;

    [HideInInspector]
    public AttacksUnlocker attacksUnlocker;

    Text nameText, turnText;
    UltimateSliderController usc;

    public BattleState state;

    [HideInInspector]
    public int defenseCounter, drCounter, ultimateHealCounter;
    [HideInInspector]
    public bool attacking, battling;

    private void OnEnable()
    {
        Note = transform.parent.Find("Note").gameObject;
        playerHUD = transform.parent.Find("BattleUI").Find("PlayerHealth").GetComponent<BattleHud>();
        enemyHUD = transform.parent.Find("BattleUI").Find("EnemyHealth").GetComponent<BattleHud>();
        PlayerShadow = playerHUD.transform.Find("PlayerShadow");
        EnemyShadow = enemyHUD.transform.Find("EnemyShadow");
        nameText = playerHUD.gameObject.transform.Find("Name").GetComponent<Text>();
        turnText = transform.parent.Find("Turn").GetComponent<Text>();
        BackButton = Note.transform.Find("Back").GetComponent<Button>();
        BackButton.onClick.AddListener(OnBackButton);
        levelScript = transform.parent.parent.parent.gameObject.GetComponent<Level>();

        state = BattleState.START;
        StartCoroutine(SetupBattle());
        Note.SetActive(false);
    }

    private void Update()
    {
        if (battling)
        {
            playerHUD.SetHUD(playerUnit);
            enemyHUD.SetHUD(enemyUnit);
        }
    }

    IEnumerator SetupBattle()
    {
        playerGO = Instantiate(PlayerPrefab, PlayerShadow.GetChild(0));
        playerUnit = playerGO.GetComponent<Unit>();
        playerGO.GetComponent<CharacterAnimator>().idleFrames = levelScript.playerIdleFrames;
        playerGO.GetComponent<CharacterAnimator>().baseAttackFrames = levelScript.playerBaseAttackFrames;
        playerGO.GetComponent<CharacterAnimator>().specialAttackFrames = levelScript.playerSpecialAttackFrames;
        playerGO.GetComponent<CharacterAnimator>().ultimateAttackFrames = levelScript.playerUltimateAttackFrames;
        playerGO.GetComponent<CharacterAnimator>().defendFrames = levelScript.playerDefendFrames;
        enemyGo = Instantiate(EnemyPrefab, EnemyShadow.GetChild(0));
        enemyUnit = enemyGo.GetComponent<Unit>();

        playerGO.AddComponent(levelScript.GetPlayersEffectsAnimator());

        playerGO.GetComponent<CharacterAnimator>().opponentEF = enemyGo.GetComponent<EffectsAnimator>();
        enemyGo.GetComponent<CharacterAnimator>().opponentEF = playerGO.GetComponent<EffectsAnimator>();

        if (transform.parent.Find("BattleUI").childCount < 3)
        {
            attackButtons = Instantiate(levelScript.GetPlayersAttackButtons(), transform.parent.Find("BattleUI"));
            attackButtons.transform.SetAsLastSibling();
            attackButtons.name = "Attacks";
            attackButtons.SetActive(true);
        }else
            attackButtons.SetActive(true);

        attacksUnlocker = transform.parent.Find("BattleUI").GetComponentInChildren<AttacksUnlocker>();

        turnText.CrossFadeAlpha(0, 0f, false);
        nameText.text = enemyUnit.unitName;
        usc = transform.parent.Find("BattleUI").Find("Attacks").GetComponentInChildren<UltimateSliderController>(true);

        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);

        StartCoroutine(TurnIndicatorShowHide());

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;

        Note.SetActive(false);
        attacking = false;
        battling = true;
    }

    public IEnumerator PlayerAttack(float damage, int attack)
    {
        if (attack == 0)
            playerGO.GetComponent<CharacterAnimator>().baseAttack = true;
        else if (attack == 1)
            playerGO.GetComponent<CharacterAnimator>().specialAttack = true;
        else if (attack == 2)
            playerGO.GetComponent<CharacterAnimator>().ultimateAttack = true;

        while (playerGO.GetComponent<CharacterAnimator>().baseAttack == true || 
            playerGO.GetComponent<CharacterAnimator>().specialAttack == true || 
            playerGO.GetComponent<CharacterAnimator>().ultimateAttack == true) yield return null;

        bool isDead = enemyUnit.TakeDamage(Mathf.RoundToInt(damage));
        if (attack != 2)
            usc.TakeDamage(Mathf.RoundToInt(damage));

        enemyHUD.SetHP(enemyUnit.currentHP);

        if (isDead)
        {
            state = BattleState.WIN;
            EndBattle();
        }
        else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }

        IEnumerator EnemyTurn()
        {
            bool isDeadP = false;

            enemyGo.GetComponent<CharacterAnimator>().baseAttack = true;
            while (enemyGo.GetComponent<CharacterAnimator>().baseAttack == true) yield return null;

            if (defenseCounter == 0)
            {
                isDeadP = playerUnit.TakeDamage(UnityEngine.Random.Range(enemyUnit.enemyMinDamage, enemyUnit.enemyMaxDamage + 1));
                if (ultimateHealCounter == 0)
                    playerHUD.transform.Find("Slider").Find("Fill Area").Find("Fill").GetComponent<Image>().color = new Color(0.8207547f, 0.2748754f, 0.2748754f, 1f);
            }
            else
            {
                defenseCounter--;
                playerGO.GetComponent<CharacterAnimator>().defend = true;
                while (playerGO.GetComponent<CharacterAnimator>().defend == true) yield return null;
            }

            if (ultimateHealCounter > 0)
            {
                ultimateHealCounter--;
                yield return new WaitForSeconds(0.2f);
                playerUnit.currentHP += 10;
                if (playerUnit.currentHP > playerUnit.maxHP)
                    playerUnit.currentHP = playerUnit.maxHP;
                playerHUD.SetHP(playerUnit.currentHP);  
                if (ultimateHealCounter == 0)
                    playerHUD.transform.Find("Slider").Find("Fill Area").Find("Fill").GetComponent<Image>().color = new Color(0.8207547f, 0.2748754f, 0.2748754f, 1f);
            }

            if (drCounter > 0)
            {
                isDeadP = playerUnit.TakeDamage(UnityEngine.Random.Range(Mathf.RoundToInt(enemyUnit.enemyMinDamage * 0.5f), Mathf.RoundToInt((enemyUnit.enemyMaxDamage + 1) * 0.5f)));
                drCounter--;
            }

            playerHUD.SetHP(playerUnit.currentHP);

            yield return new WaitForSeconds(1f);

            if(isDeadP)
            {
                state = BattleState.LOST;
                EndBattle();
            }
            else
            {
                state = BattleState.PLAYERTURN;
            }
        }

        void EndBattle()
        {
            attackButtons.SetActive(false);
            if(state == BattleState.WIN)
            {
                enemyUnit.EnemyDie();
                levelScript.addXP(enemyUnit.xpDrop, attacksUnlocker);
                levelScript.WonBattle(transform.parent.name.Split('-')[1]);
                Note.SetActive(true);
                Text noteText = Note.GetComponentInChildren<Text>();
                noteText.text = "You Defeated " + EnemyPrefab.name + "\n\n";
                noteText.text = noteText.text + "XP: +" + enemyUnit.xpDrop.ToString() + "\n";
                noteText.text = noteText.text + "Gold: +" + enemyUnit.goldDrop.ToString();
            }
            else if(state == BattleState.LOST)
            {
                Note.SetActive(true);
                Text noteText = Note.GetComponentInChildren<Text>();
                noteText.text = "You Lost To " + EnemyPrefab.name + "\n\n";
                Note.transform.Find("Back").gameObject.SetActive(false);
                transform.parent.parent.parent.Find("Death").gameObject.SetActive(true);
            }
        }
        if (state != BattleState.WIN && state != BattleState.LOST)
            attacking = false;

        for (int i = 0; i < attacksUnlocker.coolDownCounters.Length; i++)
        {
            if (attacksUnlocker.coolDownCounters[i] > 0)
            {
                print(attacksUnlocker.coolDownCounters[i]);
                attacksUnlocker.coolDownCounters[i]--;
                attacksUnlocker.countDownCoolDown(i);
                if (attacksUnlocker.coolDownCounters[i] == 0) attacksUnlocker.EndCoolDown(i);
            }
            else
                attacksUnlocker.coolDownCounters[i] = 0;
        }
    }

    public void OnBaseAttackButton(int damage)
    {
        if (!attacking && state == BattleState.PLAYERTURN)
        {
            attacking = true;
            if (state != BattleState.PLAYERTURN)
                return;
            StartCoroutine(PlayerAttack(damage * (PlayerPrefs.GetInt("attack1level") > 1f ? PlayerPrefs.GetInt("attack1level") / 1.6f : 1f), 0));
            attacksUnlocker.CoolDown(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject);
            print(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);
        }
    }

    public void OnSpecialAttackButton(int damage)
    {
        if (!attacking && state == BattleState.PLAYERTURN)
        {
            attacking = true;
            if (state != BattleState.PLAYERTURN)
                return;
            StartCoroutine(PlayerAttack(damage * (PlayerPrefs.GetInt("attack4level") > 1f ? PlayerPrefs.GetInt("attack4level") / 1.6f : 1f), 1));
            attacksUnlocker.CoolDown(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject);
        }
    }

    public void OnUltimateAttackButton(int damage)
    {
        if (!attacking && state == BattleState.PLAYERTURN)
        {
            attacking = true;
            if (state != BattleState.PLAYERTURN)
                return;
            StartCoroutine(PlayerAttack(damage * (PlayerPrefs.GetInt("attack5level") > 1f ? PlayerPrefs.GetInt("attack5level") / 1.6f : 1f), 2));
        }
    }

    public void OnUltimateHealButton(int amountOfTurns)
    {
        if (!attacking && state == BattleState.PLAYERTURN)
        {
            attacking = true;
            ultimateHealCounter = amountOfTurns + (PlayerPrefs.GetInt("attack5level") > 1f ? PlayerPrefs.GetInt("attack5level") : 0);
            StartCoroutine(PlayerAttack(0, 2));
            attacksUnlocker.CoolDown(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject);
            playerHUD.transform.Find("Slider").Find("Fill Area").Find("Fill").GetComponent<Image>().color = Color.yellow;
        }
    }

    public void OnDefenseButton(int amountOfTurns)
    {
        if (!attacking && state == BattleState.PLAYERTURN)
        {
            attacking = true;
            defenseCounter = amountOfTurns + (PlayerPrefs.GetInt("attack2level") > 1f ? PlayerPrefs.GetInt("attack2level") : 0);
            StartCoroutine(PlayerAttack(0, -1));
            attacksUnlocker.CoolDown(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject);
            playerHUD.transform.Find("Slider").Find("Fill Area").Find("Fill").GetComponent<Image>().color = Color.yellow;
        }
    }

    public void OnHealButton(int healAmount)
    {
        if (!attacking && state == BattleState.PLAYERTURN)
        {
            attacking = true;
            playerUnit.currentHP += healAmount + (PlayerPrefs.GetInt("attack3level") > 1f ? PlayerPrefs.GetInt("attack3level") : 0);
            if (playerUnit.currentHP > playerUnit.maxHP)
                playerUnit.currentHP = playerUnit.maxHP;
            playerHUD.SetHP(playerUnit.currentHP);
            StartCoroutine(PlayerAttack(0, -1));
            attacksUnlocker.CoolDown(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject);
        }
    }

    public void OnDamageResistance(int amountOfTurns)
    {
        if (!attacking && state == BattleState.PLAYERTURN)
        {
            attacking = true;
            drCounter = amountOfTurns;
            StartCoroutine(PlayerAttack(0, -1));
            attacksUnlocker.CoolDown(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject);
        }
    }

    public void HealPotion(int healAmount)
    {
        if (state != BattleState.WIN || state != BattleState.LOST)
        {
            playerUnit.currentHP += healAmount;
            playerHUD.SetHP(playerUnit.currentHP);
        }
    }

    void OnBackButton()
    {
        StopAllCoroutines();
        transform.parent.name = transform.parent.name.Split('-')[0];
        try
        {
            Destroy(playerGO);
            Destroy(enemyGo);
        }
        catch(Exception e)
        {
            Debug.LogError("oh no " + e);
        }
        transform.parent.parent.parent.Find("debug level display").gameObject.SetActive(true);
        StartBattle[] sbs = transform.parent.parent.parent.Find("Level " + PlayerPrefs.GetInt("stage").ToString()).GetComponentsInChildren<StartBattle>(true);
        foreach(StartBattle sb in sbs)
        {
            sb.ShowPreview();
        }
    }

    IEnumerator TurnIndicatorShowHide()
    {
        bool shownPlayer = false;
        bool shownEnemy = false;
        while (state != BattleState.WIN && state != BattleState.LOST)
        {
            if (state == BattleState.PLAYERTURN && !shownPlayer)
            {
                turnText.text = "Player's Turn";
                turnText.CrossFadeAlpha(1, 0.1f, false);
                yield return new WaitForSeconds(1.4f);
                turnText.CrossFadeAlpha(0, 0.2f, false);
                shownPlayer = true;
                shownEnemy = false;
            }
            else if (state == BattleState.ENEMYTURN && !shownEnemy)
            {
                turnText.text = "Enemy's Turn";
                turnText.CrossFadeAlpha(1, 0.1f, false);
                yield return new WaitForSeconds(1.4f);
                turnText.CrossFadeAlpha(0, 0.1f, false);
                shownEnemy = true;
                shownPlayer = false;
            }
            yield return null;
        }
        turnText.CrossFadeAlpha(0, 0.1f, false);
        yield return new WaitForSeconds(0.5f);
        if (state == BattleState.WIN)
        {
            turnText.text = "Player Wins!";
            turnText.CrossFadeAlpha(1, 0.1f, false);
        }
        else if (state == BattleState.LOST)
        {
            turnText.text = "Enemy Wins!";
            turnText.CrossFadeAlpha(1, 0.1f, false);
        }
    }
}

public static class ExtensionMethods
{
    public static T GetCopyOf<T>(this Component comp, T other) where T : Component
    {
        Type type = comp.GetType();
        if (type != other.GetType()) return null; // type mis-match
        BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly;
        PropertyInfo[] pinfos = type.GetProperties(flags);
        foreach (var pinfo in pinfos)
        {
            if (pinfo.CanWrite)
            {
                try
                {
                    pinfo.SetValue(comp, pinfo.GetValue(other, null), null);
                }
                catch { } // In case of NotImplementedException being thrown. For some reason specifying that exception didn't seem to catch it, so I didn't catch anything specific.
            }
        }
        FieldInfo[] finfos = type.GetFields(flags);
        foreach (var finfo in finfos)
        {
            finfo.SetValue(comp, finfo.GetValue(other));
        }
        return comp as T;
    }

    public static T AddComponent<T>(this GameObject go, T toAdd) where T : Component
    {
        return go.AddComponent<T>().GetCopyOf(toAdd) as T;
    }
}

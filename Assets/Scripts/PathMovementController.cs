using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class PathMovementController : MonoBehaviour
{

    float positionStep;
    public int currentCharacter;
    Level level;
    
    void Start()
    {
        level = transform.parent.parent.parent.GetComponent<Level>();
        positionStep = 2f / transform.childCount;
        currentCharacter = 0;
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<CharacterSelection>().SetPosition(positionStep * i);
        }
    }

    public void Right()
    {
        if (!transform.GetChild(0).GetComponent<CharacterSelection>().moving)
        {
            currentCharacter = currentCharacter == 0 ? currentCharacter = transform.childCount - 1 : currentCharacter = currentCharacter - 1;
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).GetComponent<CharacterSelection>().MovePosition(true, positionStep);
                transform.GetChild(i).GetComponent<CharacterSelection>().selected = false;
            }
            transform.GetChild(currentCharacter).GetComponent<CharacterSelection>().selected = true;
        }
    }

    public void Left()
    {
        if (!transform.GetChild(0).GetComponent<CharacterSelection>().moving)
        {
            currentCharacter = currentCharacter == transform.childCount - 1 ? currentCharacter = 0 : currentCharacter = currentCharacter + 1;
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).GetComponent<CharacterSelection>().MovePosition(false, positionStep);
                transform.GetChild(i).GetComponent<CharacterSelection>().selected = false;
            }
            transform.GetChild(currentCharacter).GetComponent<CharacterSelection>().selected = true;
        }
    }

    public void StartGame()
    {
        print(currentCharacter);
        PlayerPrefs.SetInt("Character", currentCharacter);
        PlayerPrefs.SetString("PlayerName", transform.GetChild(currentCharacter).name);
        level.playerIdleFrames = transform.GetChild(currentCharacter).GetChild(0).GetComponent<CharacterAnimator>().idleFrames;
        level.playerBaseAttackFrames = transform.GetChild(currentCharacter).GetChild(0).GetComponent<CharacterAnimator>().baseAttackFrames;
        level.playerSpecialAttackFrames = transform.GetChild(currentCharacter).GetChild(0).GetComponent<CharacterAnimator>().specialAttackFrames;
        level.playerUltimateAttackFrames = transform.GetChild(currentCharacter).GetChild(0).GetComponent<CharacterAnimator>().ultimateAttackFrames;
        level.playerDefendFrames = transform.GetChild(currentCharacter).GetChild(0).GetComponent<CharacterAnimator>().defendFrames;
    }
}

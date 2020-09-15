using System.Collections;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{

    [SerializeField]
    public Sprite[] idleFrames;

    [SerializeField]
    public Sprite[] baseAttackFrames;

    [SerializeField]
    public Sprite[] specialAttackFrames;

    [SerializeField]
    public Sprite[] ultimateAttackFrames;

    [SerializeField]
    public Sprite[] defendFrames;

    public float animDelay;
    public bool baseAttack, specialAttack, ultimateAttack;
    public bool defend;

    private SpriteRenderer sr;
    private EffectsAnimator ef;
    [HideInInspector]
    public EffectsAnimator opponentEF;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        ef = GetComponent<EffectsAnimator>();
        StartCoroutine(AnimateIdle());
    }

    IEnumerator AnimateIdle()
    {
        int index = 0;
        while (true)
        {
            index = index < idleFrames.Length - 1 ? index + 1 : 0;
            sr.sprite = idleFrames[index];
            yield return new WaitForSeconds(animDelay);

            if (baseAttack)
            {
                ef.StartAnimation(0, opponentEF);
                if (baseAttackFrames.Length > 0) { sr.sprite = idleFrames[0]; StartCoroutine(AnimateBaseAttack()); break; }
                else baseAttack = false;
            }
            if (specialAttack)
            {
                ef.StartAnimation(1, opponentEF);
                if (specialAttackFrames.Length > 0) { sr.sprite = idleFrames[0]; StartCoroutine(AnimateSpecialAttack()); break; }
                else specialAttack = false;
            }
            if (ultimateAttack)
            {
                ef.StartAnimation(2, opponentEF);
                if (ultimateAttackFrames.Length > 0) { sr.sprite = idleFrames[0]; StartCoroutine(AnimateUltimateAttack()); break; }
                else ultimateAttack = false;
            }

            if (defend)
            {
                if (defendFrames.Length > 0) { sr.sprite = idleFrames[0]; StartCoroutine(AnimateDefend()); break; }
                else defend = false;
            }
        }
    }

    IEnumerator AnimateBaseAttack()
    {
        int index = 0;
        while (true)
        {
            index = index < baseAttackFrames.Length - 1 ? index + 1 : -1;
            if (index == -1) { baseAttack = false; StartCoroutine(AnimateIdle()); break; }
            sr.sprite = baseAttackFrames[index];
            yield return new WaitForSeconds(animDelay);
        }
    }

    IEnumerator AnimateSpecialAttack()
    {
        int index = 0;
        while (true)
        {
            index = index < specialAttackFrames.Length - 1 ? index + 1 : -1;
            if (index == -1) { specialAttack = false; StartCoroutine(AnimateIdle()); break; }
            sr.sprite = specialAttackFrames[index];
            yield return new WaitForSeconds(animDelay);
        }
    }

    IEnumerator AnimateUltimateAttack()
    {
        int index = 0;
        while (true)
        {
            index = index < ultimateAttackFrames.Length - 1 ? index + 1 : -1;
            if (index == -1) { ultimateAttack = false; StartCoroutine(AnimateIdle()); break; }
            sr.sprite = ultimateAttackFrames[index];
            yield return new WaitForSeconds(animDelay);
        }
    }

    IEnumerator AnimateDefend()
    {
        int index = 0;
        while (true)
        {
            index = index < defendFrames.Length - 1 ? index + 1 : -1;
            if (index == -1) { defend = false; StartCoroutine(AnimateIdle()); break; }
            sr.sprite = defendFrames[index];
            yield return new WaitForSeconds(animDelay);
        }
    }
}

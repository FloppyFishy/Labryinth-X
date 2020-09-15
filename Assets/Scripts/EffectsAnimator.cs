using System.Collections;
using UnityEngine;

public class EffectsAnimator : MonoBehaviour
{
    public float animDelay;

    [Header("Base Attack")]
    public Color baseColourOverlay = new Color(0.7735849f, 0.4123353f, 0.4123353f);
    public float bColourOverlayDelaySeconds = 0.6f;
    public bool bc_toEnemy = true, bc_both = false;
    [SerializeField]
    public Sprite[] baseOverlayFrames = new Sprite[0];
    public bool bo_toEnemy = false, bo_both = false;
    [SerializeField]
    public Sprite[] baseUnderlayFrames = new Sprite[0];
    public bool bu_toEnemy = false, bu_both = false;
    public AnimationClip bBigEffectToPlay;

    [Header("Special Attack")]
    public Color specialColourOverlay = new Color(0.7735849f, 0.4123353f, 0.4123353f);
    public float sColourOverlayDelaySeconds = 0.6f;
    public bool sc_toEnemy = true, sc_both = false;
    [SerializeField]
    public Sprite[] specialOverlayFrames = new Sprite[0];
    public bool so_toEnemy = false, so_both = false;
    [SerializeField]
    public Sprite[] specialUnderlayFrames = new Sprite[0];
    public bool su_toEnemy = false, su_both = false;
    public AnimationClip sBigEffectToPlay;

    [Header("Ultimate Attack")]
    public Color ultimateColourOverlay = new Color(0.7735849f, 0.4123353f, 0.4123353f);
    public float uColourOverlayDelaySeconds = 0.6f;
    public bool uc_toEnemy = true, uc_both = false;
    [SerializeField]
    public Sprite[] ultimateOverlayFrames = new Sprite[0];
    public bool uo_toEnemy = false, uo_both = false;
    [SerializeField]
    public Sprite[] ultimateUnderlayFrames = new Sprite[0];
    public bool uu_toEnemy = false, uu_both = false;
    public AnimationClip uBigEffectToPlay;

    Color currentColourOverlay;
    float currentColourOverlayDelay;
    Sprite[] currentOverlayFrames, currentUnderlayFrames;
    AnimationClip currentBigEffect;

    SpriteRenderer overlaySprite, underlaySprite, spriteRenderer;
    Animation bigEffectAnimation;

    private void OnValidate()
    {
        if (animDelay == 0)
            animDelay = GetComponent<CharacterAnimator>().animDelay;
    }

    // Start is called before the first frame update
    void Start()
    {
        overlaySprite = transform.Find("Lays").transform.Find("Overlay").GetComponent<SpriteRenderer>();
        underlaySprite = transform.Find("Lays").transform.Find("Underlay").GetComponent<SpriteRenderer>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        bigEffectAnimation = GameObject.Find("BigEffectsOverlay").GetComponent<Animation>();
    }

    public void StartAnimation(int number, EffectsAnimator opponent)
    {
        bool doEnemyOverlay = false, doEnemyUnderlay = false, doEnemyColour = false;
        switch (number)
        {
            case 0:
                currentOverlayFrames = baseOverlayFrames; currentUnderlayFrames = baseUnderlayFrames; currentColourOverlay = baseColourOverlay; 
                currentColourOverlayDelay = bColourOverlayDelaySeconds; currentBigEffect = bBigEffectToPlay;
                doEnemyOverlay = bo_toEnemy; doEnemyUnderlay = bu_toEnemy; doEnemyColour = bc_toEnemy;
                break;
            case 1:
                currentOverlayFrames = specialOverlayFrames; currentUnderlayFrames = specialUnderlayFrames; currentColourOverlay = specialColourOverlay; 
                currentColourOverlayDelay = sColourOverlayDelaySeconds; currentBigEffect = sBigEffectToPlay;
                doEnemyOverlay = so_toEnemy; doEnemyUnderlay = su_toEnemy; doEnemyColour = sc_toEnemy;
                break;
            case 2:
                currentOverlayFrames = ultimateOverlayFrames; currentUnderlayFrames = ultimateUnderlayFrames; currentColourOverlay = ultimateColourOverlay; 
                currentColourOverlayDelay = uColourOverlayDelaySeconds; currentBigEffect = uBigEffectToPlay;
                doEnemyOverlay = uo_toEnemy; doEnemyUnderlay = uu_toEnemy; doEnemyColour = uc_toEnemy;
                break;
            default:
                currentOverlayFrames = baseOverlayFrames; currentUnderlayFrames = baseUnderlayFrames; currentColourOverlay = Color.white; currentColourOverlayDelay = bColourOverlayDelaySeconds;
                break;
        }

        if (currentColourOverlay != Color.white && opponent) {
            if(doEnemyColour) StartCoroutine(opponent.AnimateColour(currentColourOverlay, currentColourOverlayDelay));
            else StartCoroutine(AnimateColour(currentColourOverlay, currentColourOverlayDelay));
        }

        if (currentBigEffect != null) { bigEffectAnimation.clip = currentBigEffect; bigEffectAnimation.Play(); }

        print(number + " " + ultimateUnderlayFrames.Length + " " + currentUnderlayFrames.Length);
        if (currentOverlayFrames.Length > 0 && !doEnemyOverlay) StartCoroutine(AnimateOverlay(currentOverlayFrames));
        if (opponent && currentOverlayFrames.Length > 0 && doEnemyOverlay) StartCoroutine(opponent.AnimateOverlay(currentOverlayFrames));

        if (currentUnderlayFrames.Length > 0 && !doEnemyUnderlay) StartCoroutine(AnimateUnderlay(currentUnderlayFrames));
        if (opponent && currentUnderlayFrames.Length > 0 && doEnemyUnderlay) StartCoroutine(opponent.AnimateUnderlay(currentUnderlayFrames));
    }

    IEnumerator AnimateColour(Color colourToChangeTo, float delay)
    {
        yield return new WaitForSeconds(delay);
        spriteRenderer.color = colourToChangeTo;
        yield return new WaitForSeconds(animDelay * 2);
        spriteRenderer.color = Color.white;
    }

    IEnumerator AnimateOverlay(Sprite[] frames)
    {
        int index = 0;
        while (true)
        {
            index = index < frames.Length - 1 ? index + 1 : -1;
            if (index == -1) { overlaySprite.sprite = null; break; }
            overlaySprite.sprite = frames[index];
            yield return new WaitForSeconds(animDelay);
        }
    }

    IEnumerator AnimateUnderlay(Sprite[] frames)
    {
        print("underlay");
        int index = 0;
        while (true)
        {
            index = index < frames.Length - 1 ? index + 1 : -1;
            if (index == -1) { underlaySprite.sprite = null; break; }
            underlaySprite.sprite = frames[index];
            yield return new WaitForSeconds(animDelay);
        }
    }
}

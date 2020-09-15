using System.Collections;
using TMPro;
using UnityEngine;

public class CharacterSelection : MonoBehaviour
{
    [SerializeField]
    private Transform[] routes;

    private float currentStep;

    private int path;

    private float time;

    [HideInInspector]
    public bool moving;

    [HideInInspector]
    public bool selected;

    private Vector2 objectPosition;

    private TextMeshProUGUI nameText;

    public float speedModifier = 0.5f;

    public void SetPosition(float position)
    {
        nameText = transform.parent.parent.Find("Name").GetComponent<TextMeshProUGUI>();

        int routeNumber = (position > 1) ? 1 : 0;

        Vector2 p0 = routes[routeNumber].GetChild(0).position;
        Vector2 p1 = routes[routeNumber].GetChild(1).position;
        Vector2 p2 = routes[routeNumber].GetChild(2).position;
        Vector2 p3 = routes[routeNumber].GetChild(3).position;

        time = (position > 1) ? position - 1f : position;

        objectPosition = CalcuateObjectPosition(p0, p1, p2, p3);

        transform.position = objectPosition;

        path = routeNumber;
        currentStep = time;
        if (position == 0) nameText.text = name;
    }

    public void MovePosition(bool forward, float positionStep)
    {
        float newStep = forward ? currentStep + positionStep : currentStep - positionStep;

        time = currentStep;
        nameText.text = "";

        StartCoroutine(MoveObject(newStep, forward));
    }

    IEnumerator MoveObject(float position, bool forward)
    {
        moving = true;
        Vector2 p0 = routes[path].GetChild(0).position;
        Vector2 p1 = routes[path].GetChild(1).position;
        Vector2 p2 = routes[path].GetChild(2).position;
        Vector2 p3 = routes[path].GetChild(3).position;

        float target = (position > 1) ? 1 : position;
        target = position < 0 ? 0 : target;

        if (forward)
        {
            while (time < target)
            {
                time += Time.deltaTime * speedModifier;

                objectPosition = CalcuateObjectPosition(p0, p1, p2, p3); 

                transform.position = objectPosition;
                yield return new WaitForEndOfFrame();
            }
        }
        else
        {
            while (time > target)
            {
                time -= Time.deltaTime * speedModifier;

                objectPosition = CalcuateObjectPosition(p0, p1, p2, p3);

                transform.position = objectPosition;
                yield return new WaitForEndOfFrame();
            }
        }
        
        currentStep = Mathf.Round(target * 10f) / 10f;
        time = currentStep;

        if (position > 1)
        {
            time = 0;
            path = (path == 1) ? 0 : 1;

            p0 = routes[path].GetChild(0).position;
            p1 = routes[path].GetChild(1).position;
            p2 = routes[path].GetChild(2).position;
            p3 = routes[path].GetChild(3).position;

            target = position - 1f;
            while (time < target)
            {
                time += Time.deltaTime * speedModifier;

                objectPosition = CalcuateObjectPosition(p0, p1, p2, p3);

                transform.position = objectPosition;
                yield return new WaitForEndOfFrame();
            }
            currentStep = Mathf.Round(target * 10f) / 10f;
            time = currentStep;
        }
        else if (position < 0)
        {
            time = 1;
            path = (path == 1) ? 0 : 1;

            p0 = routes[path].GetChild(0).position;
            p1 = routes[path].GetChild(1).position;
            p2 = routes[path].GetChild(2).position;
            p3 = routes[path].GetChild(3).position;

            target = position + 1f;
            while (time > target)
            {
                time -= Time.deltaTime * speedModifier;

                objectPosition = CalcuateObjectPosition(p0, p1, p2, p3);

                transform.position = objectPosition;
                yield return new WaitForEndOfFrame();
            }
            currentStep = Mathf.Round(target * 10f) / 10f;
            time = currentStep;
        }
        FinishMoving();
        moving = false;
    }

    Vector2 CalcuateObjectPosition(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
    {
        return Mathf.Pow(1 - time, 3) * p0 + 3 * Mathf.Pow(1 - time, 2) * time * p1 + 3 * (1 - time) * Mathf.Pow(time, 2) * p2 + Mathf.Pow(time, 3) * p3;
    }

    void FinishMoving()
    {
        if (!transform.parent.parent.Find("Shadow").gameObject.activeSelf)
            transform.parent.parent.Find("Shadow").gameObject.SetActive(true);

        if ((path == 1 && currentStep == 1) || (path == 0 && currentStep == 0)) nameText.text = name;

        if(selected) transform.GetChild(0).GetComponent<CharacterAnimator>().baseAttack = true;
    }
}

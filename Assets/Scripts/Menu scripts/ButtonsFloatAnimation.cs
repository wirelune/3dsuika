using System.Collections;
using UnityEngine;

public class ButtonsFloatAnimation : MonoBehaviour
{
    public float amplitude = 0.15f;
    public float moveDuration = 0.3f;
    public float delayBetweenButtons = 0.1f;
    public float pauseBetweenCycles = 1.5f;

    private Transform[] buttons;

    void Awake()
    {
        buttons = new Transform[] {
        GameObject.Find("Play").transform,
        GameObject.Find("Scores").transform,
        GameObject.Find("HowToPlay").transform
    };
    }

    void OnEnable()
    {
        StartCoroutine(AnimationLoop());
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator AnimationLoop()
    {
        while (true)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                StartCoroutine(FloatButton(buttons[i]));
                yield return new WaitForSeconds(delayBetweenButtons);
            }

            yield return new WaitForSeconds(pauseBetweenCycles);
        }
    }

    IEnumerator FloatButton(Transform t)
    {
        Vector3 origin = t.localPosition;
        Vector3 target = origin + new Vector3(0f, -amplitude, 0f);
        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            t.localPosition = Vector3.Lerp(origin, target, elapsed / moveDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        elapsed = 0f;
        while (elapsed < moveDuration)
        {
            t.localPosition = Vector3.Lerp(target, origin, elapsed / moveDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        t.localPosition = origin;
    }
}

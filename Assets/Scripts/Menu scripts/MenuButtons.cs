using System.Collections;
using UnityEngine;
using UnityEngine.N3DS;

public class MenuButtons : MonoBehaviour
{
    private SpriteRenderer[] colorButtons;
    private SpriteRenderer[] grayButtons;
    private int selectedIndex = 0;
    private bool isAnimating = false;

    public AudioClip selectSound;
    public AudioClip clickSound;
    private AudioSource audioSource;

    void Start()
    {
        colorButtons = new SpriteRenderer[] {
            GameObject.Find("Play").GetComponent<SpriteRenderer>(),
            GameObject.Find("Scores").GetComponent<SpriteRenderer>(),
            GameObject.Find("HowToPlay").GetComponent<SpriteRenderer>()
        };

        grayButtons = new SpriteRenderer[] {
            GameObject.Find("GrayPlay").GetComponent<SpriteRenderer>(),
            GameObject.Find("GrayScores").GetComponent<SpriteRenderer>(),
            GameObject.Find("GrayHowToPlay").GetComponent<SpriteRenderer>()
        };

        GameObject audioObj = GameObject.Find("AudioSource2");
        if (audioObj != null)
            audioSource = audioObj.GetComponent<AudioSource>();

        UpdateVisuals();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow) || GamePad.GetButtonTrigger(N3dsButton.Down))
        {
            selectedIndex = (selectedIndex + 1) % colorButtons.Length;
            UpdateVisuals();
            PlaySound(selectSound);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) || GamePad.GetButtonTrigger(N3dsButton.Up))
        {
            selectedIndex = (selectedIndex - 1 + colorButtons.Length) % colorButtons.Length;
            UpdateVisuals();
            PlaySound(selectSound);
        }

        if (Input.GetKeyDown(KeyCode.Return) || GamePad.GetButtonTrigger(N3dsButton.A))
        {
            if (!isAnimating)
                StartCoroutine(PressButton(selectedIndex));
        }

        bool tapped = false;
        Vector2 inputPos = Vector2.zero;
        if (Input.GetMouseButtonDown(0))
        {
            tapped = true;
            inputPos = Input.mousePosition;
            UnityEngine.Debug.Log("mouse click, inputPos=" + inputPos);
        }
        if (Input.GetMouseButtonDown(0))
        {
            tapped = true;
            inputPos = Input.mousePosition;
        }
        else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            tapped = true;
            inputPos = Input.GetTouch(0).position;
        }

        if (tapped)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(inputPos);

            GameObject[] buttonObjects = {
        GameObject.Find("Play"),
        GameObject.Find("Scores"),
        GameObject.Find("HowToPlay")
    };

            UnityEngine.Debug.Log("mousePos=" + mousePos);
            for (int i = 0; i < buttonObjects.Length; i++)
            {
                if (buttonObjects[i] == null) continue;

                SpriteRenderer sr = buttonObjects[i].GetComponent<SpriteRenderer>();
                if (sr == null) continue;

                Vector3 bp = buttonObjects[i].transform.position;
                Vector2 size = sr.bounds.size;

                if (mousePos.x > bp.x - size.x * 0.5f && mousePos.x < bp.x + size.x * 0.5f &&
                    mousePos.y > bp.y - size.y * 0.5f && mousePos.y < bp.y + size.y * 0.5f)
                {
                    if (selectedIndex != i)
                    {
                        selectedIndex = i;
                        UpdateVisuals();
                        PlaySound(selectSound);
                    }
                    else if (!isAnimating)
                    {
                        StartCoroutine(PressButton(i));
                    }
                    break;
                }
            }
        }
    }

    void UpdateVisuals()
    {
        for (int i = 0; i < grayButtons.Length; i++)
            grayButtons[i].enabled = (i != selectedIndex);
    }

    void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
            audioSource.PlayOneShot(clip);
    }

    IEnumerator PressButton(int index)
    {
        isAnimating = true;
        PlaySound(clickSound);

        Transform t = colorButtons[index].transform;
        Vector3 originalScale = t.localScale;
        Vector3 pressedScale = originalScale * 0.9f;
        float duration = 0.08f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            t.localScale = Vector3.Lerp(originalScale, pressedScale, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        elapsed = 0f;
        while (elapsed < duration)
        {
            t.localScale = Vector3.Lerp(pressedScale, originalScale, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        t.localScale = originalScale;
        isAnimating = false;
        OnButtonPressed(index);
    }

    void OnButtonPressed(int index)
    {
        if (index == 0)
        {
            CurtainTransition ct = FindObjectOfType<CurtainTransition>();
            if (ct != null) StartCoroutine(ct.CloseAndLoad());
        }
        else if (index == 1)
        {
            ScoreRanking ranking = FindObjectOfType<ScoreRanking>();
            if (ranking != null) ranking.Show();
            else UnityEngine.Debug.Log("oops");
        }
        else if (index == 2)
        {
            HowToPlay htp = FindObjectOfType<HowToPlay>();
            if (htp != null) htp.Show();
        }
    }
}

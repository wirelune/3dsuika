using System.Collections;
using UnityEngine;
using UnityEngine.N3DS;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    public GameObject fade;
    public GameObject panel;
    public GameObject buttonRestart;
    public GameObject buttonBackTitle;
    public Sprite restartActive;
    public Sprite restartInactive;
    public Sprite backActive;
    public Sprite backInactive;
    public string mainMenuScene = "MenuScene";
    public float slideDistance = 8f;
    public float slideDuration = 0.5f;
    public SpriteNumber scoreFinal;
    public AudioClip selectSound;
    public AudioClip clickSound;
    private AudioSource audioSource;

    private SpriteRenderer restartRenderer;
    private SpriteRenderer backRenderer;
    private int selectedIndex = 0;
    private bool isActive = false;
    private bool isAnimating = false;
    private Vector3 panelTargetPosition;

    void Start()
    {
        GameObject audioObj = GameObject.Find("AudioSource2");
        if (audioObj != null) audioSource = audioObj.GetComponent<AudioSource>();

        
        if (fade != null) fade.SetActive(false);
        if (panel != null)
        {
            panelTargetPosition = panel.transform.position;
            panel.transform.position = panelTargetPosition + new Vector3(0f, slideDistance + 20f, 0f);
        }

        if (buttonRestart != null)   restartRenderer = buttonRestart.GetComponent<SpriteRenderer>();
        if (buttonBackTitle != null) backRenderer    = buttonBackTitle.GetComponent<SpriteRenderer>();
    }
    void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
            audioSource.PlayOneShot(clip);
    }
    public void Show()
    {
        if (isActive) return;
        isActive = true;

        if (fade != null) fade.SetActive(true);
        StartCoroutine(SlideIn());
    }

    IEnumerator SlideIn()
    {
        UnityEngine.Debug.Log("scoreFinal = " + (scoreFinal != null ? "ok" : "null"));
        Vector3 target = panelTargetPosition;
        Vector3 start  = target + new Vector3(0f, slideDistance, 0f);
        panel.transform.position = start;

        float elapsed = 0f;
        while (elapsed < slideDuration)
        {
            panel.transform.position = Vector3.Lerp(start, target, Mathf.SmoothStep(0f, 1f, elapsed / slideDuration));
            elapsed += Time.deltaTime;
            yield return null;
        }

        panel.transform.position = target;
        if (scoreFinal != null) scoreFinal.SetValue(weefriut.TotalScore);
        selectedIndex = 0;
        UpdateVisuals();
    }

    void Update()
    {
        if (!isActive) return;

        if (Input.GetKeyDown(KeyCode.LeftArrow)  || GamePad.GetButtonTrigger(N3dsButton.Left) ||
            Input.GetKeyDown(KeyCode.RightArrow) || GamePad.GetButtonTrigger(N3dsButton.Right))
        {
            PlaySound(selectSound);
            selectedIndex = selectedIndex == 0 ? 1 : 0;
            UpdateVisuals();
        }

        if (Input.GetKeyDown(KeyCode.Return) || GamePad.GetButtonTrigger(N3dsButton.A))
        {
            if (!isAnimating) StartCoroutine(PressButton(selectedIndex));
        }
    }

    void UpdateVisuals()
    {
        if (restartRenderer != null) restartRenderer.sprite = (selectedIndex == 0) ? restartActive : restartInactive;
        if (backRenderer != null)    backRenderer.sprite    = (selectedIndex == 1) ? backActive    : backInactive;
    }

    IEnumerator PressButton(int index)
    {
        PlaySound(clickSound);
        isAnimating = true;
        GameObject[] buttonObjects = { buttonRestart, buttonBackTitle };
        Transform t = buttonObjects[index].transform;
        Vector3 originalScale = t.localScale;
        Vector3 pressedScale  = originalScale * 0.9f;
        float duration = 0.08f;
        float elapsed  = 0f;

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

        if (index == 0)
        {
            CurtainTransition ct = FindObjectOfType<CurtainTransition>();
            if (ct != null) StartCoroutine(ct.CloseAndLoad());
        }
        else
        {
            CurtainTransition ct = FindObjectOfType<CurtainTransition>();
            if (ct != null) StartCoroutine(ct.CloseAndLoadMenu());
        }
    }
}

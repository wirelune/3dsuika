using System.Collections;
using UnityEngine;
using UnityEngine.N3DS;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject fade;
    public GameObject panel;
    public GameObject buttonReturn;
    public GameObject buttonRestart;
    public GameObject buttonBackTitle;
    public Sprite returnActive;
    public Sprite returnInactive;
    public Sprite restartActive;
    public Sprite restartInactive;
    public Sprite backActive;
    public Sprite backInactive;
    public string mainMenuScene = "MenuScene";
    public float slideDistance = 8f;
    public float slideDuration = 0.5f;
    public AudioClip selectSound;
    public AudioClip clickSound;

    private AudioSource audioSource;
    private SpriteRenderer returnRenderer;
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

        if (buttonReturn != null)    returnRenderer  = buttonReturn.GetComponent<SpriteRenderer>();
        if (buttonRestart != null)   restartRenderer = buttonRestart.GetComponent<SpriteRenderer>();
        if (buttonBackTitle != null) backRenderer    = buttonBackTitle.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (GameState.isGameOver) return;

        if (GamePad.GetButtonTrigger(N3dsButton.X) || Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isActive) Open();
            else Close();
        }

        if (!isActive) return;

        if (Input.GetKeyDown(KeyCode.DownArrow) || GamePad.GetButtonTrigger(N3dsButton.Down))
        {
            selectedIndex = (selectedIndex + 1) % 3;
            UpdateVisuals();
            PlaySound(selectSound);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) || GamePad.GetButtonTrigger(N3dsButton.Up))
        {
            selectedIndex = (selectedIndex - 1 + 3) % 3;
            UpdateVisuals();
            PlaySound(selectSound);
        }

        if (Input.GetKeyDown(KeyCode.Return) || GamePad.GetButtonTrigger(N3dsButton.A))
        {
            if (!isAnimating) StartCoroutine(PressButton(selectedIndex));
        }
    }

    void Open()
    {
        isActive = true;
        Time.timeScale = 0f;
        if (fade != null) fade.SetActive(true);
        StartCoroutine(SlideIn());
    }

    void Close()
    {
        isActive = false;
        Time.timeScale = 1f;
        if (fade != null) fade.SetActive(false);
        panel.transform.position = panelTargetPosition + new Vector3(0f, slideDistance + 20f, 0f);
    }

    IEnumerator SlideIn()
    {
        Vector3 target = panelTargetPosition;
        Vector3 start  = target + new Vector3(0f, slideDistance, 0f);
        panel.transform.position = start;

        float elapsed = 0f;
        while (elapsed < slideDuration)
        {
            panel.transform.position = Vector3.Lerp(start, target, Mathf.SmoothStep(0f, 1f, elapsed / slideDuration * (1f / Time.timeScale == float.PositiveInfinity ? 1f : 1f)));
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        panel.transform.position = target;
        selectedIndex = 0;
        UpdateVisuals();
    }

    void UpdateVisuals()
    {
        if (returnRenderer != null)  returnRenderer.sprite  = (selectedIndex == 0) ? returnActive  : returnInactive;
        if (restartRenderer != null) restartRenderer.sprite = (selectedIndex == 1) ? restartActive : restartInactive;
        if (backRenderer != null)    backRenderer.sprite    = (selectedIndex == 2) ? backActive    : backInactive;
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

        GameObject[] buttonObjects = { buttonReturn, buttonRestart, buttonBackTitle };
        Transform t = buttonObjects[index].transform;
        Vector3 originalScale = t.localScale;
        Vector3 pressedScale  = originalScale * 0.9f;
        float duration = 0.08f;
        float elapsed  = 0f;

        while (elapsed < duration)
        {
            t.localScale = Vector3.Lerp(originalScale, pressedScale, elapsed / duration);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        elapsed = 0f;
        while (elapsed < duration)
        {
            t.localScale = Vector3.Lerp(pressedScale, originalScale, elapsed / duration);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        t.localScale = originalScale;
        isAnimating = false;

        if (index == 0)
        {
            Close();
        }
        else if (index == 1)
        {
            Time.timeScale = 1f;
            CurtainTransition ct = FindObjectOfType<CurtainTransition>();
            if (ct != null) StartCoroutine(ct.CloseAndLoad());
        }
        else
        {
            Time.timeScale = 1f;
            CurtainTransition ct = FindObjectOfType<CurtainTransition>();
            if (ct != null) StartCoroutine(ct.CloseAndLoadMenu());
        }
    }
}

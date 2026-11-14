using UnityEngine;
using UnityEngine.N3DS;

public class HowToPlay : MonoBehaviour
{
    public GameObject board;
    public GameObject menuButtons;
    public SpriteRenderer cardRenderer;
    public Sprite[] cards;

    private int currentIndex = 0;

    void Start()
    {
        if (board != null) board.SetActive(false);
    }

    void Update()
    {
        if (board == null || !board.activeSelf) return;

        if (Input.GetKeyDown(KeyCode.RightArrow) || GamePad.GetButtonTrigger(N3dsButton.Right))
        {
            if (currentIndex < cards.Length - 1)
            {
                currentIndex++;
                UpdateCard();
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) || GamePad.GetButtonTrigger(N3dsButton.Left))
        {
            if (currentIndex > 0)
            {
                currentIndex--;
                UpdateCard();
            }
        }

        if (Input.GetKeyDown(KeyCode.Backspace) || GamePad.GetButtonTrigger(N3dsButton.B))
        {
            Hide();
        }
    }

    public void Show()
    {
        currentIndex = 0;
        UpdateCard();

        if (board != null) board.SetActive(true);
        if (menuButtons != null) menuButtons.SetActive(false);
    }

    public void Hide()
    {
        if (board != null) board.SetActive(false);
        if (menuButtons != null) menuButtons.SetActive(true);
    }

    void UpdateCard()
    {
        if (cardRenderer != null && cards != null && currentIndex < cards.Length)
            cardRenderer.sprite = cards[currentIndex];
    }
}

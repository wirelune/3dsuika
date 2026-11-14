using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.N3DS;

public class ScoreRanking : MonoBehaviour
{
    public GameObject rankBoard;
    public GameObject menuButtons;
    public SpriteNumber score1;
    public SpriteNumber score2;
    public SpriteNumber score3;

    void Start()
    {
        if (rankBoard != null) rankBoard.SetActive(false);
    }

    void Update()
    {
        if (rankBoard == null || !rankBoard.activeSelf) return;

        if (Input.GetKeyDown(KeyCode.Backspace) || GamePad.GetButtonTrigger(N3dsButton.B))
        {
            Hide();
        }
    }

    public void Show()
    {
        if (rankBoard != null) rankBoard.SetActive(true);
        if (menuButtons != null) menuButtons.SetActive(false);
        StartCoroutine(UpdateScoresDelayed());
    }

    IEnumerator UpdateScoresDelayed()
    {
        yield return null;

        List<int> scores = ScoreData.Load();
        if (score1 != null) score1.SetValue(scores[0]);
        if (score2 != null) score2.SetValue(scores[1]);
        if (score3 != null) score3.SetValue(scores[2]);
    }

    public void Hide()
    {
        if (rankBoard != null) rankBoard.SetActive(false);
        if (menuButtons != null) menuButtons.SetActive(true);
    }
}

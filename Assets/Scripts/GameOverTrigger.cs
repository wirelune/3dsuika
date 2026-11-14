using UnityEngine;

public class GameOverTrigger : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D other)
    {
        if (GameState.isGameOver) return;

        weefriut fruit = other.GetComponent<weefriut>();
        if (fruit != null && fruit.hasLanded)
        {
            GameState.isGameOver = true;
            ScoreData.AddScore(weefriut.TotalScore);

            weefriut[] allFruits = FindObjectsOfType<weefriut>();
            foreach (weefriut f in allFruits)
            {
                f.StartCoroutine(f.ShakeAndFreeze());
            }
        }
    }
}

using System.Collections;
using UnityEngine;

public class CurtainOpen : MonoBehaviour
{
    public float duration = 0.5f;
    public float leftTargetX = -5f;
    public float rightTargetX = 5f;

    void Start()
    {
        StartCoroutine(OpenAndDestroy());
    }

    IEnumerator OpenAndDestroy()
    {
        Transform left  = GameObject.Find("zanaves3").transform;
        Transform right = GameObject.Find("zanaves4").transform;

        Vector3 leftStart  = left.position;
        Vector3 rightStart = right.position;
        Vector3 leftEnd    = new Vector3(leftTargetX,  leftStart.y,  leftStart.z);
        Vector3 rightEnd   = new Vector3(rightTargetX, rightStart.y, rightStart.z);

        float elapsed = 0f;
        while (elapsed < duration)
        {
            float t = Mathf.SmoothStep(0f, 1f, elapsed / duration);
            left.position  = Vector3.Lerp(leftStart,  leftEnd,  t);
            right.position = Vector3.Lerp(rightStart, rightEnd, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        left.position  = leftEnd;
        right.position = rightEnd;

        yield return new WaitForSeconds(5f);

        Destroy(left.gameObject);
        Destroy(right.gameObject);
        Destroy(gameObject);
    }
}

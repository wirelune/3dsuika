using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CurtainTransition : MonoBehaviour
{
    public string sceneToLoad = "MainScene";
    public string menuScene = "MainMenu1";
    public float duration = 0.5f;
   

    private Transform left;
    private Transform right;
    private bool curtainsMet = false;

    void Start()
    {
        left  = GameObject.Find("zanaves").transform;
        right = GameObject.Find("zanaves2").transform;

        CurtainCollider lc = left.GetComponent<CurtainCollider>();
        CurtainCollider rc = right.GetComponent<CurtainCollider>();
        if (lc != null) lc.onMet = () => curtainsMet = true;
        if (rc != null) rc.onMet = () => curtainsMet = true;
    }

    public IEnumerator CloseAndLoad()
    {
        curtainsMet = false;

        Vector3 leftStart  = left.position;
        Vector3 rightStart = right.position;
        Vector3 leftEnd    = new Vector3(0f, leftStart.y,  leftStart.z);
        Vector3 rightEnd   = new Vector3(0f, rightStart.y, rightStart.z);

        float elapsed = 0f;
        while (elapsed < duration && !curtainsMet)
        {
            float t = Mathf.SmoothStep(0f, 1f, elapsed / duration);
            left.position  = Vector3.Lerp(leftStart,  leftEnd,  t);
            right.position = Vector3.Lerp(rightStart, rightEnd, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene(sceneToLoad);
    }
    public IEnumerator CloseAndLoadMenu()
    {
       
        Vector3 leftStart = left.position;
        Vector3 rightStart = right.position;
        Vector3 leftEnd = new Vector3(0f, leftStart.y, leftStart.z);
        Vector3 rightEnd = new Vector3(0f, rightStart.y, rightStart.z);

        curtainsMet = false;
        float elapsed = 0f;
        while (elapsed < duration && !curtainsMet)
        {
            float t = Mathf.SmoothStep(0f, 1f, elapsed / duration);
            left.position = Vector3.Lerp(leftStart, leftEnd, t);
            right.position = Vector3.Lerp(rightStart, rightEnd, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene(menuScene);
    }
}

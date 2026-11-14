using UnityEngine;

public class MusicManager : MonoBehaviour
{
    void Awake()
    {
        if (FindObjectsOfType<MusicManager>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        GetComponent<AudioSource>().Play();
    }
}

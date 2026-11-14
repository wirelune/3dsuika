using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weefriut : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteNumber scoreDisplay;
    private int IntScore;

    private AudioSource audioSource;
    public AudioClip removeSound;
    public GameObject newfruit;
    public Sprite[] disappearFrames;
    public float disappearDuration = 0.4f;
    public bool hasLanded = false;
    public static int TotalScore = 0;

    void Start()
    {
       
        StartCoroutine(EnableColliderDelayed());
        GameObject audioObj = GameObject.Find("AudioSource2");
        if (audioObj != null)
            audioSource = audioObj.GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        scoreDisplay = GameObject.Find("ScoreText").GetComponent<SpriteNumber>();
    }
    IEnumerator EnableColliderDelayed() //hope overlapping fruits won't cause trouble anymore
    {
        Collider2D col = GetComponent<Collider2D>();
        yield return new WaitForSeconds(0.03f);
        col.enabled = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.timeScale > 0f || UnityEngine.N3DS.GamePad.GetButtonTrigger(N3dsButton.A) && Time.timeScale > 0f)
        {
            gameObject.transform.SetParent(null);
            if (rb != null)
                rb.simulated = true;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        hasLanded = true;

        if (!collision.gameObject.CompareTag(gameObject.tag)) return;

        if (GetInstanceID() < collision.gameObject.GetInstanceID())
        {
            weefriut other = collision.gameObject.GetComponent<weefriut>();
            if (other != null)
                other.StartCoroutine(other.DisappearAndDestroy());
            else
                Destroy(collision.gameObject);

            if (newfruit != null)
            {
                AddScore();

                GameObject spawned = Instantiate(newfruit, transform.position, transform.rotation);
                spawned.transform.SetParent(transform.parent, true);
                Rigidbody2D spawnedRb = spawned.GetComponent<Rigidbody2D>();
                if (spawnedRb != null)
                    spawnedRb.simulated = true;
            }

            StartCoroutine(DisappearAndDestroy());
        }
        else
        {
            StartCoroutine(DisappearAndDestroy());
        }
    }

    public IEnumerator DisappearAndDestroy()
    {
        if (rb != null) rb.simulated = false;
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;
        if (audioSource != null && removeSound != null)
            audioSource.PlayOneShot(removeSound);

        if (disappearFrames == null || disappearFrames.Length == 0)
        {
            Destroy(gameObject);
            yield break;
        }

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null)
        {
            Destroy(gameObject);
            yield break;
        }

        float timePerFrame = disappearDuration / disappearFrames.Length;

        for (int i = 0; i < disappearFrames.Length; i++)
        {
            if (disappearFrames[i] != null)
                sr.sprite = disappearFrames[i];

            yield return new WaitForSeconds(timePerFrame);
        }

        Destroy(gameObject);
    }
    public IEnumerator ShakeAndFreeze()
    {
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
            transform.rotation = Quaternion.identity;
            Rigidbody2D rb2d = GetComponent<Rigidbody2D>();
            if (rb2d != null) rb2d.freezeRotation = true;
        }

        Vector3 origin = transform.position;
        float elapsed = 0f;
        float duration = 1.5f;
        float magnitude = 0.03f;
        float shakeSpeed = 500f;

        while (elapsed < duration)
        {
            float offset = Mathf.Sin(elapsed * shakeSpeed) * magnitude;
            transform.position = new Vector3(origin.x + offset, origin.y + offset, origin.z);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = origin;
        GameOverScreen screen = FindObjectOfType<GameOverScreen>();
        if (screen != null) screen.Show();
    }
    void AddScore()
    {
        if (scoreDisplay == null) return;

        int points = 0;
        string tag = gameObject.tag;
        if (tag == "f1") points = 1;
        else if (tag == "f2") points = 3;
        else if (tag == "f3") points = 6;
        else if (tag == "f4") points = 10;
        else if (tag == "f5") points = 15;
        else if (tag == "f6") points = 21;
        else if (tag == "f7") points = 28;
        else if (tag == "f8") points = 36;
        else if (tag == "f9") points = 45;

        TotalScore += points;
        scoreDisplay.SetValue(TotalScore);
    }

}

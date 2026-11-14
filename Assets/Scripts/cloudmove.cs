using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.N3DS;

public class cloudmove : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed = 5f;
    private bool waitingForSpawn = false;
    private float spawnTimer = 0f;

    public GameObject fruit1;
    public GameObject fruit2;
    public GameObject fruit3;
    public GameObject fruit4;
    public GameObject fruit5;

    public GameObject previewFruit1;
    public GameObject previewFruit2;
    public GameObject previewFruit3;
    public GameObject previewFruit4;
    public GameObject previewFruit5;

    public GameObject ray;
    public Transform fruitpos;
    public Transform nextpos;

    public AudioSource audioSource;
    public AudioClip dropSound;

    private int nextFruitIndex = -1;
    private GameObject currentPreviewInstance = null;
    
    float offsetX = 0f;
    public float CloudminX = -3f;
    public float CloudmaxX = 3f;
    private float fradius;

    void Start()
    {
       GameState.isGameOver = false;
        rb = GetComponent<Rigidbody2D>();

        int currentIndex = Random.Range(0, 5);
        SpawnFruitAtCloud(currentIndex);
        GameObject rayObj = Instantiate(ray, fruitpos);
        rayObj.transform.localPosition = Vector3.zero;
        rayObj.transform.localScale = ray.transform.localScale;
        
        
        nextFruitIndex = Random.Range(0, 5);
        ShowNextPreview();
    }
   
    void Update()
    {
        //my code may be sloppy but at least it isnt vibe coded
        if (GameState.isGameOver) return;
        if ((Input.GetKeyDown(KeyCode.Space) && Time.timeScale > 0f || GamePad.GetButtonTrigger(N3dsButton.A)) && !waitingForSpawn && Time.timeScale > 0f)
        {
            if (audioSource != null && dropSound != null)
               audioSource.PlayOneShot(dropSound);
            waitingForSpawn = true;
            spawnTimer = 1f;

            GameObject rayObject = GameObject.Find("rayprefab(Clone)");
            if (rayObject != null)
                Destroy(rayObject);
        }

        if (waitingForSpawn)
        {
            spawnTimer -= Time.deltaTime;
            if (spawnTimer <= 0f)
            {
                waitingForSpawn = false;

                SpawnFruitAtCloud(nextFruitIndex);
                GameObject rayObj = Instantiate(ray, fruitpos);
                rayObj.transform.localPosition = Vector3.zero;
                rayObj.transform.localScale = ray.transform.localScale;

                nextFruitIndex = Random.Range(0, 5);
                ShowNextPreview();
            }
        }

        float horizontalInput = 0f;

        if (Input.GetKey(KeyCode.LeftArrow) || GamePad.GetButtonHold(N3dsButton.Left) || GamePad.CirclePad.x < -0.3f)
            horizontalInput = -1f;

        if (Input.GetKey(KeyCode.RightArrow) || GamePad.GetButtonHold(N3dsButton.Right) || GamePad.CirclePad.x > 0.3f)
            horizontalInput = 1f;

        rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);

        Vector3 p = transform.position;
        p.x = Mathf.Clamp(p.x, CloudminX + fradius, CloudmaxX - fradius);
        transform.position = p;

        rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);
    }

    void SpawnFruitAtCloud(int index)
    {
        GameObject[] fruits = { fruit1, fruit2, fruit3, fruit4, fruit5 };
        if (fruits[index] == null || fruitpos == null) return;

        GameObject instance = Instantiate(fruits[index], fruitpos);

        
        switch (index)
        {
            case 0: offsetX = 0.6f; fradius = 0.16f; break;
            case 1: offsetX = 0.5f; fradius = 0.20f; break;
            case 2: offsetX = 0.4f; fradius = 0.38f; break;
            case 3: offsetX = 0.2f; fradius = 0.48f; break;
            case 4:                 fradius = 0.87f; break;
            default: offsetX = 0f; break;
        }

        instance.transform.localPosition = Vector3.zero;
    }

    void ShowNextPreview()
    {
        if (currentPreviewInstance != null)
            Destroy(currentPreviewInstance);

        if (nextpos == null) return;

        GameObject[] previews = { previewFruit1, previewFruit2, previewFruit3, previewFruit4, previewFruit5 };
        if (previews[nextFruitIndex] == null) return;

        currentPreviewInstance = Instantiate(previews[nextFruitIndex], nextpos);
        currentPreviewInstance.transform.localPosition = Vector3.zero;
    }

}

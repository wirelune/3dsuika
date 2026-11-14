using UnityEngine;

public class FloatAnimation : MonoBehaviour
{
    public float amplitude = 0.15f;
    public float speed = 1.2f;

    private Vector3 startPos;
    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        startPos = rectTransform != null ? rectTransform.localPosition : transform.localPosition;
    }

    void Update()
    {
        Vector3 offset = new Vector3(0f, Mathf.Sin(Time.time * speed) * amplitude, 0f);
        if (rectTransform != null)
            rectTransform.localPosition = startPos + offset;
        else
            transform.localPosition = startPos + offset;
    }
}

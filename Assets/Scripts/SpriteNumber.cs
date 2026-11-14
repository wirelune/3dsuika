using UnityEngine;

public class SpriteNumber : MonoBehaviour
{
    public Sprite[] digits;
    public float digitWidth = 0.5f;
    public float digitSpacing = 0.05f;
    public int sorterOrder = 0;

    private int currentValue = 0;
    private GameObject[] digitObjects = new GameObject[0];

    void Start()
    {
        SetValue(0);
    }

    public void SetValue(int value)
    {
        currentValue = value;
        string s = value.ToString();

        foreach (GameObject go in digitObjects)
            Destroy(go);

        digitObjects = new GameObject[s.Length];

        float totalWidth = s.Length * digitWidth + (s.Length - 1) * digitSpacing;
        float startX = -totalWidth / 2f + digitWidth / 2f;

        for (int i = 0; i < s.Length; i++)
        {
            int d = s[i] - '0';
            GameObject go = new GameObject("digit_" + i);
            go.transform.SetParent(transform);
            go.transform.localPosition = new Vector3(startX + i * (digitWidth + digitSpacing), 0f, 0f);
            go.transform.localScale = Vector3.one;

            SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = digits[d];
            sr.sortingLayerName = GetComponent<SpriteRenderer>() != null
                ? GetComponent<SpriteRenderer>().sortingLayerName
                : "Default";
            sr.sortingOrder = sorterOrder;

            digitObjects[i] = go;
        }
    }

    public int GetValue()
    {
        return currentValue;
    }
}

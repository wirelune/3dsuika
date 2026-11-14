using System.Collections;
using UnityEngine;
using UnityEngine.N3DS;

public class CircleOfEvolution : MonoBehaviour
{
    public float slideSpeed = 8f;
    public float hiddenY = -10f;

    private Vector3 shownPosition;
    private Vector3 hiddenPosition;
    private bool isShown = false;

    void Start()
    {
        shownPosition = transform.position;
        hiddenPosition = new Vector3(transform.position.x, hiddenY, transform.position.z);
        transform.position = hiddenPosition;
    }

    void Update()
    {
        bool holding = Input.GetKey(KeyCode.C) || GamePad.GetButtonHold(N3dsButton.B);

        Vector3 target = holding ? shownPosition : hiddenPosition;
        transform.position = Vector3.MoveTowards(transform.position, target, slideSpeed * Time.deltaTime);
    }
}

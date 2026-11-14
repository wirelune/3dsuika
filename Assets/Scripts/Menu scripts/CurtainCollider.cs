using System;
using UnityEngine;

public class CurtainCollider : MonoBehaviour
{
    public Action onMet;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "zanaves" || other.name == "zanaves2")
        {
            if (onMet != null) onMet();
        }
    }
}

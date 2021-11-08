using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// TODO: Create a parent class with sibling FinishTrigger.cs
/// </summary>
public class DeathVolumeTrigger : MonoBehaviour
{
    public BoxCollider collider;
    // Start is called before the first frame update
    void Start()
    {
        if (!collider) collider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")) {
            other.GetComponent<PlayerController>().TakeDamage(9999);
        }
    }
}

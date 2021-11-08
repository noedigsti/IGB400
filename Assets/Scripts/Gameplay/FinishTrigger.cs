using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// TODO: Create a parent class with sibling DeathVolumeTrigger.cs
/// </summary>
public class FinishTrigger : MonoBehaviour 
{
    public BoxCollider collider;
    // Start is called before the first frame update
    void Start() {
        if(!collider) collider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")) {
            UIManager.Instance.uiBehaviour.ReturnMainMenu();
        }
    }
}

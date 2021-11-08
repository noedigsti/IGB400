using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemyEnabler : MonoBehaviour
{
    public SphereCollider collider;
    public GameObject enemy;
    private void Start() {
        collider = GetComponent<SphereCollider>();
        enemy.SetActive(false);
    }
    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("PlayerPresence")) {
            enemy.SetActive(true);
        }
    }
}

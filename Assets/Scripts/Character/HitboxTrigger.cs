using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxTrigger : MonoBehaviour
{
    public PlayerController controller;
    public CapsuleCollider col;
    string targetTag;
    public GameObject target;
    public string TargetTagname {
        set {
            targetTag = value;
        }
        get {
            return targetTag;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponentInParent<PlayerController>();
        col = GetComponent<CapsuleCollider>();
    }
    private void OnTriggerEnter(Collider other) {
        if(other.transform.CompareTag(targetTag)) {
            target = other.gameObject;
            controller.AttackTarget(target);
        }
    }
    private void OnTriggerExit(Collider other) {
        target = null;
    }
}

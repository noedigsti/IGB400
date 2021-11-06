using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingBlock : MonoBehaviour
{
    public Rigidbody rigidBody;
    public float movementSpeed = 1f;
    public float timer = 3f;
    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        Destroy(gameObject,timer);
    }
    void MoveBlock() {
        Vector3 movement = new Vector3(1,0,0) * movementSpeed * Time.deltaTime;
        rigidBody.MovePosition(transform.position + movement);
    }
    // Update is called once per frame
    void Update()
    {
        MoveBlock();
    }
}

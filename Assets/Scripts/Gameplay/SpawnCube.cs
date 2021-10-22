using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCube : MonoBehaviour
{
    public float timer = 3f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DetroyOnTimer(timer));
    }

    IEnumerator DetroyOnTimer(float timer) {
        yield return new WaitForSeconds(timer);
        Destroy(gameObject);
    }
}

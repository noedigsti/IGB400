using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEnemy : MonoBehaviour
{
    float YRotate = 90;
    public Slider hpSlider;
    public EnemyController controller;

    // Start is called before the first frame update
    void Start()
    {
        hpSlider = GetComponentInChildren<Slider>();
        controller = GetComponentInParent<EnemyController>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(new Vector3(0,YRotate,0));
        hpSlider.value = controller.GetCurrentHP;
    }
}

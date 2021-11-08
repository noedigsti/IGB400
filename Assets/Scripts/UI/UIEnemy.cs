using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEnemy : MonoBehaviour
{
    float YRotate = 90;
    public Slider hpSlider;
    public EnemyController controller;
    public GameObject parent;
    public GameObject popupText;

    // Start is called before the first frame update
    void Start()
    {
        hpSlider = GetComponentInChildren<Slider>();
        controller = GetComponentInParent<EnemyController>();
        hpSlider.maxValue = controller.GetMaxHP;
    }

    public void DisplayDamageTaken(float _damage) {
        GameObject popup = Instantiate(popupText,parent.transform);
        popup.GetComponent<TMPro.TMP_Text>().text = "-" + _damage;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(new Vector3(0,YRotate,0));
        hpSlider.value = controller.GetCurrentHP;
    }
}

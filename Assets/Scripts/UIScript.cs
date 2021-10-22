using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScript : MonoBehaviour
{
    int n = 0;
    public TMPro.TMP_Text text;

    // Start is called before the first frame update
    void Start()
    {
        n = 0;
    }

    private void Update() {
        text.text = n.ToString();
    }

    public void MoveOption() {
        GameManager.Instance.ToggleMove(!GameManager.Instance.IsPaused);
    }
}

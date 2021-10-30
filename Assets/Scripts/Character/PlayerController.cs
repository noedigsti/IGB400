using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerInput playerInput;
    private void Start() {
        playerInput = GetComponent<PlayerInput>();
    }
    public void Click(InputAction.CallbackContext ctx) {
        if(ctx.performed) {
            Debug.Log("Click performed");

            // Check if can move
            if(!GameManager.Instance.IsPaused) {
                RaycastHit hit;
                //Camera main raycast
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit)) {
                    if (hit.collider.isTrigger) {
                        Debug.Log(hit.transform.position);
                        gameObject.transform.position = hit.transform.gameObject.transform.position;

                    }
                }
            }
        }
    }
    public void OnTogglePause(InputAction.CallbackContext ctx) {
        if(ctx.performed) {
            Debug.Log("Toggle Pause");
            //GetComponent<PlayerInput>().SwitchCurrentActionMap("UI");
        }
    }
}

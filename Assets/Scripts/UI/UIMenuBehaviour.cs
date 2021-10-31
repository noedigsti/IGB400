using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMenuBehaviour : MonoBehaviour
{
    int n = 0;
    public Button pauseButton;
    public GameObject PauseCanvas;
    public PlayerController playerController;
    public void SetupBehaviour() {
        SetupPlayerController();
    }
    void SetupPlayerController() {
        playerController = GameManager.Instance.GetPlayerController();
    }
    public void UpdateUIMenuState(bool _state) {
        switch(_state) {
            case true:
                UpdateEventSystemUIInputModule();
                break;
            case false:
                break;
        }
        UpdateCoreObjects(_state);
    }
    void UpdateCoreObjects(bool _state) {
    }
    void UpdateEventSystemUIInputModule() {
        EventSystemManager.Instance.UpdateActionAssetToFocusedPlayer();
    }
    private void OnEnable() {
        pauseButton.onClick.AddListener(() => MoveOption());
    }
    private void OnDisable() {
        pauseButton.onClick.RemoveAllListeners();
    }

    public void MoveOption() {
        n++;
    }
    public void PauseGame(bool _b) {
        GameManager.Instance.TogglePause(_b);
        CameraManager.Instance.PauseGame(_b);
    }
    public void PlayerAttack() {
        playerController.OnAttack();
    }
    public void PlayerDefend() {
        playerController.OnDefend();
    }
    public void PlayerNotDefend() {
        playerController.OnNotDefend();
    }
    bool MovingCharacterLeft = false;
    public void PlayerMoveLeft() {
        MovingCharacterLeft = true;
    }
    bool MovingCharacterRight = false;
    public void PlayerMoveRight() {
        MovingCharacterRight = true;
    }
    public void PlayerStopMove() {
        MovingCharacterRight = false;
        MovingCharacterLeft = false;
        playerController.CharacterStop();
    }
    private void LateUpdate() {
        if(MovingCharacterLeft) {
            playerController.OnMoveLeft();
        }
        if(MovingCharacterRight) {
            playerController.OnMoveRight();

        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMenuBehaviour : MonoBehaviour
{
    public Button pauseButton;
    public GameObject PauseCanvas;
    public PlayerController playerController;
    public Slider playerHPBar;
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
        pauseButton.onClick.AddListener(() => PauseOption());
    }
    private void OnDisable() {
        pauseButton.onClick.RemoveAllListeners();
    }

    public void PauseOption() {

    }
    public void PauseGame(bool _b) {
        GameManager.Instance.TogglePause(_b);
        CameraManager.Instance.PauseGame(_b);
    }

    public void PlayerJump() {
        playerController.OnCharacterJump();

        EventSystemManager.Instance.eventSystem.SetSelectedGameObject(null);
    }
    public void PlayerAttack() {
        playerController.OnAttack();
    }
    public void PlayerDefend() {
        playerController.OnDefend();
    }
    public void PlayerNotDefend() {
        playerController.OnNotDefend();

        EventSystemManager.Instance.eventSystem.SetSelectedGameObject(null);
    }
    bool RunCharacter = false;
    byte counterL = 0;
    byte counterR = 0;
    bool MovingCharacterLeft = false;
    public void PlayerMoveLeft() {
        if (counterL++ >= 1) {
            RunCharacter = true;
        }
        MovingCharacterLeft = true;
    }
    bool MovingCharacterRight = false;
    public void PlayerMoveRight() {
        if(counterR++ >= 1) {
            RunCharacter = true;
        }
        MovingCharacterRight = true;
    }
    public void PlayerStopMove() {
        MovingCharacterRight = false;
        MovingCharacterLeft = false;
        StartCoroutine(ToggleRunCharacter(playerController.runDelayTimer));
        playerController.CharacterStop();
        
        EventSystemManager.Instance.eventSystem.SetSelectedGameObject(null);
    }
    IEnumerator ToggleRunCharacter(float timer) {
        RunCharacter = false;
        yield return new WaitForSeconds(timer);
        if(counterL < 2 || !RunCharacter) {
            counterL = 0;
        }
        if (counterR < 2 || !RunCharacter) {
            counterR = 0;
        }
    }
    private void Update() {
        playerHPBar.value = playerController.GetPlayerCurrentHP;

        if(MovingCharacterLeft) {
            playerController.OnMoveLeft(RunCharacter);
        }
        if(MovingCharacterRight) {
            playerController.OnMoveRight(RunCharacter);
        }
    }
    public void test(float _damage) {
        playerController.OnTakeDamage(_damage);
    }
}

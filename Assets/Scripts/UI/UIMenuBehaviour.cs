using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;


public class UIMenuBehaviour : MonoBehaviour
{
    private void Awake() {
        EnhancedTouchSupport.Enable();
    }

    public GameObject tapArea;
    public GameObject buttonMoveLeft;
    public GameObject buttonMoveRight;

    public Button pauseButton;
    public GameObject PauseCanvas;
    public PlayerController playerController;
    public Slider levelProgress;
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
        EnhancedTouchSupport.Disable();
    }

    public void PauseOption() {

    }
    public void PauseGame(bool _b) {
        GameManager.Instance.TogglePause(_b);
        CameraManager.Instance.PauseGame(_b);
    }

    public void PlayerJump() {

        playerController.OnCharacterJump();
    }
    bool Attacking = false;
    public void PlayerAttack() {
        if(Touch.activeTouches.Count > 0) {
            Debug.Log("Attackkk");
            Attacking = true;
            playerController.OnAttack();
        }
    }
    public void PlayerDefend() {
        playerController.OnDefend();
    }
    public void PlayerNotDefend() {
        PlayerStopMove();
        playerController.OnNotDefend();

    }
    void SetEmpty() {
        EventSystemManager.Instance.eventSystem.SetSelectedGameObject(null);
    }
    bool RunCharacter = false;
    byte counterL = 0;
    byte counterR = 0;
    bool MovingCharacterLeft = false;
    public void PlayerMoveLeft() {
        if(Touch.activeTouches.Count > 0) {

            if(counterL++ >= 1) {
                RunCharacter = true;
            }
            MovingCharacterLeft = true; 
        }
    }
    bool MovingCharacterRight = false;
    public void PlayerMoveRight() {
        if(Touch.activeTouches.Count > 0) {

            if(counterR++ >= 1) {
                RunCharacter = true;
            }
            MovingCharacterRight = true; 
        }
    }
    public void PlayerStopMove() {
        SetEmpty();
        Attacking = false;
        MovingCharacterRight = false;
        MovingCharacterLeft = false;
        StartCoroutine(ToggleRunCharacter(playerController.RunDelayTimer));
        playerController.CharacterStop();
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
    void UpdateGameplayUIs() {
        playerHPBar.value = playerController.GetPlayerCurrentHP;

        float p = WorldLevelManager
            .Instance
            .PlayerCurrentProgress(playerController.GetCurrentCharacterPosition());
        levelProgress.value = p;
    }
    private void LateUpdate() {
        UpdateGameplayUIs();
    }
    private void Update() {
        if (Touch.activeFingers.Count > 0) {
            foreach(var touch in Touch.activeTouches) {

                Debug.Log("Finger Index = " + touch.finger.index);
                //Debug.Log("Touch Index = " + touch.touchId);
                if (EventSystemManager.Instance.eventSystem.currentSelectedGameObject != null) {
                    //Debug.Log(EventSystemManager.Instance.eventSystem.currentSelectedGameObject.name);
                }
                if(IsPointerOverUIObject(touch)) {
                    //if(EventSystemManager.Instance.eventSystem.IsPointerOverGameObject(touch.finger.index)) {
                    //Debug.Log("Over UI");
                    //if(EventSystem.current.IsPointerOverGameObject(touch.finger.index)) {
                    //PointerEventData pointer = new PointerEventData(EventSystemManager.Instance.eventSystem);
                    //pointer.position = touch.startScreenPosition;

                    //List<RaycastResult> raycastResults = new List<RaycastResult>();
                    //EventSystemManager.Instance.eventSystem.RaycastAll(pointer,raycastResults);

                    //if(raycastResults.Count > 0) {
                    //    foreach(var go in raycastResults) {
                    //        Debug.Log(go.gameObject.name);
                    //    }
                    //}
                }
                if(MovingCharacterLeft) {
                    playerController.OnMoveLeft(RunCharacter);
                }
                if(MovingCharacterRight) {
                    playerController.OnMoveRight(RunCharacter);
                }
                if(touch.startScreenPosition.y > Screen.currentResolution.height * .25f) {
                    PlayerJump();
                }
            }
        } else {
            PlayerStopMove();
        }
    }
    /// <summary>
    /// Cast a ray to test if Input.mousePosition is over any UI object in EventSystem.current. This is a replacement
    /// for IsPointerOverGameObject() which does not work on Android in 4.6.0f3
    /// </summary>
    private bool IsPointerOverUIObject(Touch _touch) {
        // Referencing this code for GraphicRaycaster https://gist.github.com/stramit/ead7ca1f432f3c0f181f
        // the ray cast appears to require only eventData.position.
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = _touch.startScreenPosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition,results);
        if(results.Count > 0) {
            foreach(var go in results) {
                //Debug.Log(go.gameObject.name);
                if(go.gameObject.transform.CompareTag("GameUI")) {
                    Debug.Log(go.gameObject.name);
                }
            }
        }
        return results.Count > 0;
    }

    /// <summary>
    /// Cast a ray to test if screenPosition is over any UI object in canvas. This is a replacement
    /// for IsPointerOverGameObject() which does not work on Android in 4.6.0f3
    /// </summary>
    private bool IsPointerOverUIObject(Canvas canvas,Vector2 screenPosition) {
        // Referencing this code for GraphicRaycaster https://gist.github.com/stramit/ead7ca1f432f3c0f181f
        // the ray cast appears to require only eventData.position.
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = screenPosition;

        GraphicRaycaster uiRaycaster = canvas.gameObject.GetComponent<GraphicRaycaster>();
        List<RaycastResult> results = new List<RaycastResult>();
        uiRaycaster.Raycast(eventDataCurrentPosition,results);
        return results.Count > 0;
    }
    public void test(float _damage) {
        playerController.OnTakeDamage(_damage);
    }
    public void test2(float _damage) {
        playerController.OnRevive();
    }
}

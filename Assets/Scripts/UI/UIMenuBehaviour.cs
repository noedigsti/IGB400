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
    public Canvas uiCanvas;
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
    public void PlayerAttack() {
        playerController.OnAttack();
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
    bool MoveCharacterLeft = false;
    bool MoveCharacterRight = false;
    public void PlayerMoveLeft() {
        if(counterL++ >= 1) {
            RunCharacter = true;
        }
        MoveCharacterLeft = true;
    }
    public void PlayerMoveRight() {
        if(counterR++ >= 1) {
            RunCharacter = true;
        }
        MoveCharacterRight = true;
    }
    public void PlayerStopMove() {
        SetEmpty();
        MoveCharacterRight = false;
        MoveCharacterLeft = false;
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

            // Going through the active touches on the screen
            foreach(var touch in Touch.activeTouches) {
                //Debug.Log("Finger Index = " + touch.finger.index);
                //Debug.Log("Touch Index = " + touch.touchId);


                GameObject target = IsPointerOverUIObject(uiCanvas,touch.startScreenPosition,"GameUI");
                if (target != null) {
                    //Debug.Log(target.name);
                    switch(target.name) {
                        case "Jump":
                            PlayerJump(); 
                            break;
                        case "Attack":
                            //playerController.OnAttack();
                            break;
                        case "Defending":
                            break;
                        default:
                            break;
                    }
                }
                if (playerController.CurrentCharacterState != CharacterState.IsAttacking
                    &&
                    playerController.CurrentCharacterState != CharacterState.IsDefending) {
                    if(MoveCharacterLeft) {
                        playerController.OnMoveLeft(RunCharacter);
                    }
                    if(MoveCharacterRight) {
                        playerController.OnMoveRight(RunCharacter);
                    }
                }
            }
        } else {
            PlayerStopMove();
        }
    }

    /// <summary>
    /// Cast a ray to test if screenPosition is over any UI object in canvas. This is a replacement
    /// for IsPointerOverGameObject() which does not work on Android in 4.6.0f3
    /// 
    /// <para>MODIFIED</para>
    /// </summary>
    private GameObject IsPointerOverUIObject(/*Touch _touch,*/ Canvas canvas,Vector2 screenPosition, string _tag) {
        // Referencing this code for GraphicRaycaster https://gist.github.com/stramit/ead7ca1f432f3c0f181f
        // the ray cast appears to require only eventData.position.
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = screenPosition /*_touch.startScreenPosition*/;

        GraphicRaycaster uiRaycaster = canvas.gameObject.GetComponent<GraphicRaycaster>();
        List<RaycastResult> results = new List<RaycastResult>();
        //EventSystem.current.RaycastAll(eventDataCurrentPosition,results);
        uiRaycaster.Raycast(eventDataCurrentPosition,results);
        if(results.Count > 0) {
            foreach(var go in results) {
                if(go.gameObject.transform.CompareTag(_tag)) {
                    // Do something if such gameobject identified
                    return go.gameObject;
                }
            }
        }
        return null;
    }
    public void test(float _damage) {
        playerController.OnTakeDamage(_damage);
    }
    public void test2(float _damage) {
        playerController.OnRevive();
    }
}

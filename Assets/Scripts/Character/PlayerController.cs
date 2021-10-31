using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("_Movement Behaviour Section")]
    int jumpCounter;
    public int maxJumpCounters = 1;
    bool IsJumping = false;

    public float movementSpeed = 3f;
    public float movementSmoothingSpeed = 1f;
    Vector3 rawInputMovement;
    Vector3 smoothInputMovement;
    Vector3 movementDirection;

    public float turnspeed = 5f;
    Vector3 targetRotation;

    [Header("_Physics Section")]
    public Rigidbody rigidBody;

    [Header("_Input Section")]
    public PlayerInput playerInput;

    [Header("_Animator Section")]
    public Animator animator;
    int playerMovementAnimationID;
    int playerAttackAnimationID;

    private void Start() {
        jumpCounter = maxJumpCounters;
        targetRotation.y = 91;

        rigidBody = GetComponent<Rigidbody>();

        SetupAnimation();

        GameManager.Instance.AssignCurrentController(this);
    }
    void SetupAnimation() {
        animator = GetComponent<Animator>();
        playerMovementAnimationID =  Animator.StringToHash("Moving");
        playerAttackAnimationID =  Animator.StringToHash("Attacking");
    }
    public InputActionAsset GetInputActionAsset() {
        return playerInput.actions;
    }
    public void OnAttack() {
        // Attack
        Debug.Log("Attack");
        animator.SetTrigger(playerAttackAnimationID);
    }
    public void OnTogglePause(InputAction.CallbackContext ctx) {
        if(ctx.performed) {
            OnGamePause();
        }
    }
    public void OnGamePause() {
        Debug.Log("Toggle Pause");
    }
    public void EnablePauseMenuControls() {
        playerInput.SwitchCurrentActionMap("UI");
    }
    public void EnableGameplayControls() {
        playerInput.SwitchCurrentActionMap("Gameplay");
    }
    public void OnJump(InputAction.CallbackContext ctx) {
        if(ctx.started) {
            if (!EventSystem.current.IsPointerOverGameObject() && !GameManager.Instance.IsPaused) // Not clicking on UIs
                CharacterJump();
        }
    }
    public void OnMovement(InputAction.CallbackContext ctx) {
        if(ctx.started) {
            CharacterMove(ctx);
        }
        if(ctx.canceled) {
            CharacterStop();
        }
    }
    void CharacterJump() {
        if(!IsJumping && jumpCounter > 0) {
            animator.SetTrigger("Jumping");

            // Perform jump
            rigidBody.AddForce(new Vector3(0,5,0),ForceMode.Impulse);
        }
    }
    void CharacterMove(InputAction.CallbackContext ctx) {
        animator.SetBool(playerMovementAnimationID,true);

        // Move
        Vector2 inputMovement = ctx.ReadValue<Vector2>();
        rawInputMovement = new Vector3(inputMovement.x,0,0);
        if (inputMovement.x > 0) {
            targetRotation.y = 91f;
        }
        if (inputMovement.x < 0) {
            targetRotation.y = 269f;
        }
    }
    void CharacterStop() {
        animator.SetBool(playerMovementAnimationID,false);
    }


    private void OnTriggerStay(Collider other) {
        if(other.transform.CompareTag("Floor")) {
            IsJumping = false;
            jumpCounter = maxJumpCounters;
        }        
    }
    private void OnTriggerExit(Collider other) {
        if(other.transform.CompareTag("Floor")) {
            IsJumping = true;
            jumpCounter--;
        }
    }


    //Update Loop - Used for calculating frame-based data
    void Update() {
        CalculateMovementInputSmoothing();
        UpdatePlayerMovement();
    }

    //Input's Axes values are raw
    void CalculateMovementInputSmoothing() {
        smoothInputMovement = Vector3.Lerp(smoothInputMovement,rawInputMovement,Time.deltaTime * movementSmoothingSpeed);
    }
    void UpdatePlayerMovement() {
        UpdateMovementData(smoothInputMovement);
    }
    public void UpdateMovementData(Vector3 newMovementDirection) {
        movementDirection = newMovementDirection;
    }

    void FixedUpdate() {
        MoveThePlayer();
        TurnThePlayer();
    }
    void MoveThePlayer() {
        if(!animator.GetBool("Moving")) {
            rawInputMovement = Vector3.zero;
            smoothInputMovement = Vector3.zero;
        }
        Vector3 movement = movementDirection * movementSpeed * Time.deltaTime;
        rigidBody.MovePosition(transform.position + movement);
    }
    void TurnThePlayer() {
        transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(targetRotation), Time.fixedDeltaTime * turnspeed);
    }
}

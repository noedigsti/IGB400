using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("_Character Gameplay State Section")]
    [SerializeField] bool IsAttacking = false;
    [SerializeField] bool IsDefending = false;
    [SerializeField] bool IsMoving = false;
    [SerializeField] bool IsRunning = false;
    [SerializeField] bool IsJumping = false;

    [Header("_Character Gameplay Components Section")]
    public GameObject m_BarrierShield;
    [SerializeField] bool BarrierShieldEnabled = false;
    public GameObject m_SwordVFX_Glow;
    [SerializeField] bool SwordGlowEnabled = false;
    public GameObject m_SwordVFX_Trail;
    [SerializeField] bool SwordTrailEnabled = false;

    [Header("_Movement Behaviour Section")]
    int jumpCounter;
    public int maxJumpCounters = 1;
    public float jumpForce = 6f;
    public float jumpVelocity = 6f;

    public float movementSpeed = 3f;
    public float movementSpeedDefault = 3f;
    public float movementSmoothingSpeed = 1f;
    public float runSpeed = 3f;
    public float runDelayTimer = .85f;
    Vector3 rawInputMovement;
    Vector3 smoothInputMovement;
    Vector3 movementDirection;

    public float turnspeed = 5f;
    Vector3 targetRotation;

    [Header("_Physics Section")]
    public LayerMask groundLayers;
    public Rigidbody rigidBody;
    public CapsuleCollider capCol;

    [Header("_Input Section")]
    public PlayerInput playerInput;
    public NewInputActions inputActions;

    [Header("_Animator Section")]
    public Animator animator;
    int playerMovementAnimationID;
    int playerRunAnimationID;
    int playerJumpAnimationID;
    int playerAttackAnimationID;
    int playerDefendAnimationID;

    private void Start() {
        jumpCounter = maxJumpCounters;
        targetRotation.y = 91;

        capCol = GetComponent<CapsuleCollider>();
        rigidBody = GetComponent<Rigidbody>();

        SetupCharacterInput();

        SetupDefaultCharacterGameplayComponents(true);

        SetupAnimations();
    }
    void SetupDefaultCharacterGameplayComponents(bool _b) {
        m_BarrierShield.SetActive(_b);
        m_SwordVFX_Trail.SetActive(_b);
        m_SwordVFX_Glow.SetActive(_b);
    }
    public void GetCurrentController() {
        GameManager.Instance.AssignCurrentController(this);
    }
    void SetupAnimations() {
        animator = GetComponent<Animator>();
        playerMovementAnimationID =  Animator.StringToHash("Moving");
        playerRunAnimationID =  Animator.StringToHash("Running");
        playerJumpAnimationID =  Animator.StringToHash("Jumping");
        playerAttackAnimationID =  Animator.StringToHash("Attacking");
        playerDefendAnimationID =  Animator.StringToHash("Defending");
    }

    ////////////////////////////////////////////////////////////////////
    void SetupCharacterInput() {
        playerInput = GetComponent<PlayerInput>();
        playerInput.ActivateInput();
        inputActions = new NewInputActions();
        inputActions.Enable();
        inputActions.UI.Click.started += DebugLog;
        inputActions.UI.Click.Enable();
    }
    public void DebugLog(InputAction.CallbackContext ctx) {
        if(ctx.started) {

        }
    }
    public InputActionAsset GetInputActionAsset() {
        return playerInput.actions;
    }
    public void OnAttack() {
        CharacterStop();
        animator.SetTrigger(playerAttackAnimationID);
        SwordTrailEnabled = true;
    }
    public void OnDefend() {
        CharacterStop();
        animator.SetBool(playerDefendAnimationID, true);
    }
    public void OnNotDefend() {
        animator.SetBool(playerDefendAnimationID, false);
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
        //playerInput.SwitchCurrentActionMap("UI");
    }
    public void EnableGameplayControls() {
        //playerInput.SwitchCurrentActionMap("Player");
    }
    public void OnMoveLeft(bool _isRunning) {
        animator.SetBool(playerMovementAnimationID,true);
        rawInputMovement = new Vector3(-1,0,0);
        targetRotation.y = 269f;
        if(_isRunning) {
            OnRunnning();
        }
    }
    public void OnMoveRight(bool _isRunning) {
        animator.SetBool(playerMovementAnimationID,true);
        rawInputMovement = new Vector3(1,0,0);
        targetRotation.y = 91f;
        if(_isRunning) {
            OnRunnning();
        }
    }
    public void OnRunnning() {
        if(IsMoving) {
            movementSpeed = runSpeed;
            animator.SetBool(playerRunAnimationID,true);
        }
    }
    public void OnCharacterJump() {
        if(!IsJumping && jumpCounter > 0) {
            animator.SetTrigger(playerJumpAnimationID);
            jumpCounter--;
            IsJumping = true;
        }
    }
    public void CharacterStop() {
        animator.SetBool(playerMovementAnimationID,false);
        animator.SetBool(playerRunAnimationID,false);
        //animator.SetBool(playerAttackAnimationID,false); // trail vfx depends on this
        animator.SetBool(playerDefendAnimationID,false);
        movementSpeed = movementSpeedDefault;
    }

    ////////////////////////////////////////////////////////////////////

    //Update Loop - Used for calculating frame-based data
    void Update() {
        if(!animator.GetBool(playerMovementAnimationID) || animator.GetBool(playerDefendAnimationID)) {
            rawInputMovement = Vector3.zero;
            smoothInputMovement = Vector3.zero;
        }
        
        if(IsJumping && IsGrounded()) {
            rigidBody.AddForce(Vector3.up * jumpForce,ForceMode.Impulse);
            IsJumping = false;
        }
        CalculateMovementInputSmoothing();
        UpdatePlayerMovement();

        MoveThePlayer();
        TurnThePlayer();

        UpdateCharacterAnimationStates();

        UpdateCharacterGameplayComponents();
        UpdateCharacterVFX();
    }


    ////////////////////////////////////////////////////////////////////

    void UpdateCharacterAnimationStates() {
        if(animator.GetAnimatorTransitionInfo(0).IsUserName("IdleToAttack") || IsAttacking) {
            IsAttacking = true;
            if(!animator.IsInTransition(0)
                && animator.GetCurrentAnimatorStateInfo(0).IsName("Idle_Battle")) {
                IsAttacking = false;
            }
        }
        if(animator.GetBool("Defending")) {
            IsDefending = true;
        } else {
            IsDefending = false;
        }
        if(animator.GetBool("Moving")) {
            IsMoving = true;
        } else {
            IsMoving = false;
        }
        if(animator.GetBool("Running")) {
            IsRunning = true;
        } else {
            IsRunning = false;
        }
    }

    ////////////////////////////////////////////////////////////////////
    void UpdateCharacterGameplayComponents() {
        if(BarrierShieldEnabled) {
            m_BarrierShield.SetActive(true);
        } else {
            m_BarrierShield.SetActive(false);
        }
    }

    ////////////////////////////////////////////////////////////////////

    void UpdateCharacterVFX() {
        // Enable when upgraded
        if(SwordGlowEnabled) {
            if(!m_SwordVFX_Glow.activeSelf) {
                m_SwordVFX_Glow.SetActive(true);
                m_SwordVFX_Glow.GetComponent<ParticleSystem>().Play(); 
            }
        } else {
            m_SwordVFX_Glow.GetComponent<ParticleSystem>().Stop();
            m_SwordVFX_Glow.SetActive(false);
        }

        // Enable when attacking
        if(IsAttacking || SwordTrailEnabled) {
            SwordTrailEnabled = true;
            if(!m_SwordVFX_Trail.activeSelf) {
                m_SwordVFX_Trail.SetActive(true);
                m_SwordVFX_Trail.GetComponent<ParticleSystem>().Play();
            }
            if(animator.GetAnimatorTransitionInfo(0).IsUserName("AttackToIdle") || !IsAttacking) {
                SwordTrailEnabled = false;
                m_SwordVFX_Trail.SetActive(false);
                m_SwordVFX_Trail.GetComponent<ParticleSystem>().Stop();
            }
        } else {
            SwordTrailEnabled = false;
            m_SwordVFX_Trail.SetActive(false);
            m_SwordVFX_Trail.GetComponent<ParticleSystem>().Stop(); 
        }

        if(BarrierShieldEnabled || IsDefending) {
            BarrierShieldEnabled = true;
            if(!m_BarrierShield.activeSelf) {
                m_BarrierShield.SetActive(true);
            }
            if(!IsDefending) {
                BarrierShieldEnabled = false;
                m_BarrierShield.SetActive(false);
            }
        }
    }

    ////////////////////////////////////////////////////////////////////

    private void OnTriggerEnter(Collider other) {
        if(other.transform.CompareTag("Floor")) {
            if(jumpCounter <= 0) {

                IsJumping = false;
                jumpCounter = maxJumpCounters;
            }
        }
    }
    bool IsGrounded() {
        // Check if on the ground;
        return Physics.CheckCapsule(capCol.bounds.center, new Vector3(capCol.bounds.center.x, capCol.bounds.min.y, capCol.bounds.center.z), capCol.radius * 0.9f, groundLayers);
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
    void MoveThePlayer() {
        Vector3 movement = movementDirection * movementSpeed * Time.deltaTime;
        rigidBody.MovePosition(transform.position + movement);
    }
    void TurnThePlayer() {
        transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(targetRotation), Time.deltaTime * turnspeed);
    }
}

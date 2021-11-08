using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum CharacterState
{
    Idle,
    IsAttacking,
    IsDefending,
    IsMoving,
    IsRunning,
    IsGrounded,
}
public class PlayerController : MonoBehaviour
{
    [Header("_Debug")]
    public bool DrawDebugGizmos = false;
    [SerializeField] float groundCheckColliderRadius = 0.5f;
    [SerializeField] float groundCheckColliderOffset = 0.5f;

    [Header("_Character Gameplay State Section")]
    [SerializeField] int score = 0;
    public float GetPlayerScore => score;

    [Range(0,100)]
    [SerializeField] int health = 100;
    [Range(0,100)]
    [SerializeField] float healthDecimal = 100f;
    public float GetPlayerCurrentHP => health;
    [SerializeField] float maxHealth = 100f;
    [SerializeField] float attackPower = 10f;
    [SerializeField] float attackPowerDefault = 10f;
    [SerializeField] float attackPowerCap = 20f;

    [SerializeField] bool IsDead = false;
    [SerializeField] bool DeathCheck = true;
    [Range(0,5)]
    [SerializeField] byte shieldCount = 1;
    [Range(0,5)]
    [SerializeField] byte maxAvailableShields = 3;

    public float GetPlayerMaxHP => maxHealth;
    [SerializeField] bool IsAttacking = false;
    [SerializeField] bool IsDefending = false;
    [SerializeField] bool IsGettingHit = false;
    [SerializeField] bool IsMoving = false;
    [SerializeField] bool IsRunning = false;
    [SerializeField] bool CanJump = false;
    [SerializeField] bool IsGrounded = false;
    [SerializeField] int KilledOnce = 0;
    [SerializeField] float KilledOnceTimer = 5f;
    public CharacterState CurrentCharacterState {
        get {
            if(IsAttacking) return CharacterState.IsAttacking;
            if(IsDefending) return CharacterState.IsDefending;
            if(IsMoving) return CharacterState.IsMoving;
            if(IsRunning) return CharacterState.IsRunning;
            return CharacterState.Idle;
        }
    }

    [Header("_Character Gameplay Components Section")]
    public CapsuleCollider attackHitbox;

    public GameObject m_BarrierShield;
    [SerializeField] bool BarrierShieldEnabled = false;
    public SphereCollider shieldHitbox;

    [SerializeField] bool SwordGlowEnabled = false;
    public GameObject m_SwordVFX_Glow;
    public GameObject m_SwordVFX_GlowEnrage;
    public GameObject m_SwordVFX_Trail;
    [SerializeField] bool SwordTrailEnabled = false;

    [Header("_Movement Behaviour Section")]
    int jumpCounter;
    int maxJumpCounter = 1;
    [SerializeField] float jumpForce = 6f;

    [SerializeField] float movementSpeed = 3f;
    [SerializeField] float movementSpeedDefault = 3f;
    [SerializeField] float movementSmoothingSpeed = 1f;
    [SerializeField] float turnSpeed = 5f;
    [SerializeField] float runSpeed = 3f;
    [SerializeField] float runDelayTimer = .85f;
    public float RunDelayTimer => runDelayTimer;

    Vector3 rawInputMovement;
    Vector3 smoothInputMovement;
    Vector3 movementDirection;

    Vector3 targetRotation;

    [Header("_Physics Section")]
    public LayerMask groundLayers;
    public Rigidbody rigidBody;
    public CapsuleCollider capsuleCollider;

    [Header("_Input Section")]
    public PlayerInput playerInput;
    public NewInputActions inputActions;

    [Header("_Animator Section")]
    public Animator animator;
    int playerMovementAnimationPID;
    int playerRunAnimationPID;
    int playerJumpAnimationPID;
    int playerAttackAnimationPID;
    int playerAttack2AnimationPID;
    int playerDefendAnimationPID;
    int playerGotHitAnimationPID;
    int playerStunnedAnimationPID;
    int playerDeadAnimationPID;
    int playerReviveAnimationPID;

    ////////////////////////////////////////////////////////////////////

    private void Start() {
        jumpCounter = maxJumpCounter;
        targetRotation.y = 91;

        capsuleCollider = GetComponent<CapsuleCollider>();
        rigidBody = GetComponent<Rigidbody>();


        SetupDefaultCharacterGameplayComponents(true);

        SetupAnimations();
    }
    private void OnEnable() {
        SetupPlayerStats();
        SetupCharacterInput();
    }
    private void OnDisable() {

        inputActions.UI.Click.started -= DebugLog;
        inputActions.UI.Click.Disable();
        inputActions.Disable();
    }

    ////////////////////////////////////////////////////////////////////

    public void GetCurrentController() {
        GameManager.Instance.AssignCurrentController(this);
    }
    void SetupPlayerStats() {
        healthDecimal = maxHealth;
        health = Mathf.RoundToInt(healthDecimal);
        IsDead = false;
        shieldCount = maxAvailableShields;
        DeathCheck = true;
        attackPower = attackPowerDefault;
    }
    void SetupDefaultCharacterGameplayComponents(bool _b) {
        m_BarrierShield.SetActive(_b);
        shieldHitbox = m_BarrierShield.GetComponent<SphereCollider>();
        m_SwordVFX_Trail.SetActive(_b);
        m_SwordVFX_Glow.SetActive(_b);
        attackHitbox.GetComponent<HitboxTrigger>().TargetTagname = "Enemy";
    }
    void SetupAnimations() {
        animator = GetComponent<Animator>();
        playerMovementAnimationPID =  Animator.StringToHash("bMoving");
        playerRunAnimationPID =  Animator.StringToHash("bRunning");
        playerJumpAnimationPID =  Animator.StringToHash("Jumping");
        playerAttackAnimationPID =  Animator.StringToHash("Attacking");
        playerAttack2AnimationPID =  Animator.StringToHash("Attacking 2");
        playerDefendAnimationPID =  Animator.StringToHash("bDefending");
        playerGotHitAnimationPID =  Animator.StringToHash("GettingHit");
        playerStunnedAnimationPID =  Animator.StringToHash("Stunned");
        playerDeadAnimationPID =  Animator.StringToHash("bDead");
        playerReviveAnimationPID =  Animator.StringToHash("Reviving");
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
    public void EnablePauseMenuControls() {
        //playerInput.SwitchCurrentActionMap("UI");
    }
    public void EnableGameplayControls() {
        //playerInput.SwitchCurrentActionMap("Player");
    }
    public void OnAttack() {
        CharacterStop();

        // Random switching between 2 attack animation states/ clips
        if (Random.Range(1,3) == 1) {
            animator.SetTrigger(playerAttackAnimationPID);
        } else {
            animator.SetTrigger(playerAttack2AnimationPID);
        }
        SwordTrailEnabled = true;
    }
    public void OnDefend() {
        CharacterStop();
        if (shieldCount > 0) {
            //shieldCount--; // # of Available shields left
            animator.SetBool(playerDefendAnimationPID, true);
        }
    }
    public void OnNotDefend() {
        animator.SetBool(playerDefendAnimationPID, false);
    }
    //public void OnTakeDamage(float _damage) {
    //    animator.SetTrigger(playerGotHitAnimationPID);
    //    OnGetStunned();
    //}
    public void OnGetStunned() {
        animator.SetTrigger(playerStunnedAnimationPID);
    }
    public void OnDead() {
        CharacterStop();
        animator.SetBool(playerDeadAnimationPID,true);
        StartCoroutine(DieSequence());
    }
    IEnumerator DieSequence() {
        IsDead = true;
        animator.Play("Die");
        yield return new WaitUntil(() =>
            animator.GetCurrentAnimatorStateInfo(0).IsName("Die")
            && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1
        );
        UIManager.Instance.uiBehaviour.RestartLevel();
    }
    public void OnRevive() {
        IsDead = false;
        healthDecimal = maxHealth;
        animator.SetBool(playerDeadAnimationPID,false);
        animator.SetTrigger(playerReviveAnimationPID);
    }
    public void OnMoveLeft(bool _isRunning) {
        animator.SetBool(playerMovementAnimationPID,true);
        rawInputMovement = new Vector3(-1,0,0);
        targetRotation.y = 269f;
        if(_isRunning) {
            OnRunnning();
        }
    }
    public void OnMoveRight(bool _isRunning) {
        animator.SetBool(playerMovementAnimationPID,true);
        rawInputMovement = new Vector3(1,0,0);
        targetRotation.y = 91f;
        if(_isRunning) {
            OnRunnning();
        }
    }
    public void OnRunnning() {
        if(IsMoving) {
            movementSpeed = runSpeed;
            animator.SetBool(playerRunAnimationPID,true);
        }
    }
    public void OnCharacterJump() {
        jumpCounter--;
        if (jumpCounter < maxJumpCounter
            && jumpCounter >= 0) {
            animator.SetTrigger(playerJumpAnimationPID);
            CanJump = true;
        }
    }
    public void CharacterStop() {
        animator.ResetTrigger(playerAttackAnimationPID);
        animator.ResetTrigger(playerAttack2AnimationPID);
        animator.SetBool(playerMovementAnimationPID,false);
        animator.SetBool(playerRunAnimationPID,false);
        //animator.SetBool(playerAttackAnimationPID,false); // trail vfx depends on this
        animator.SetBool(playerDefendAnimationPID,false);
        movementSpeed = movementSpeedDefault;
    }

    ////////////////////////////////////////////////////////////////////

    //Update Loop - Used for calculating frame-based data
    void Update() {
        //UpdateCharacterGameplayStates();

        if(UpdateCharacterGameplayStates()) {

            UpdateCharacterAnimationStates();

            UpdateCharacterGameplayComponents();

            UpdateAftermath();

            UpdateCharacterVFX(); 
        }

        //Debug.Log(CurrentCharacterState.ToString());
    }
    
    ////////////////////////////////////////////////////////////////////

    void UpdateAftermath() {
        
    }

    ////////////////////////////////////////////////////////////////////
    public void TakeDamage(float _damage) {
        if(!IsDefending) {
            healthDecimal -= _damage;
            health = Mathf.RoundToInt(healthDecimal);
            CharacterStop();
            animator.Play("GetHit");
            FindObjectOfType<AudioManager>().Play("PlayerHit");
        }
    }
    public void AttackTarget(GameObject _obj) {
        EnemyController ctl = _obj.GetComponent<EnemyController>();
        ctl.TakeDamage(attackPower);
        if(ctl.IsDead) {
            Debug.Log("Killed " + _obj.name);
            StartCoroutine(ToggleSwordGlow());

            score += 100; // SCORE
        } 
    }

    IEnumerator ToggleSwordGlow() {
        KilledOnce++;
        attackPower = attackPowerCap;
        yield return new WaitForSeconds(KilledOnceTimer); // Timer
        KilledOnce = 0;
        attackPower = attackPowerDefault;
    }

    ////////////////////////////////////////////////////////////////////

    bool UpdateCharacterGameplayStates() {
        // Stats and state checks
        if(!IsDead && DeathCheck) {
            health = Mathf.RoundToInt(healthDecimal);
            if(health > 1) {

                // TEST HP
                //healthDecimal -= Time.deltaTime;
                //
                // Jump
                if(jumpCounter > 0
                    && CanJump
                    ) {
                    rigidBody.AddForce(Vector3.up * jumpForce,ForceMode.Impulse);
                    animator.ResetTrigger(playerJumpAnimationPID);
                    CanJump = false;
                }
                CheckGrounded();
                return true;

            } else {
                OnDead();
                return false;
            }
        } else {
            return false;
        }
    }

    ////////////////////////////////////////////////////////////////////

    void UpdateCharacterAnimationStates() {
        if(animator.GetAnimatorTransitionInfo(0).IsUserName("IdleToAttack") 
            || animator.GetCurrentAnimatorStateInfo(0).IsName("Attack01")
            || animator.GetCurrentAnimatorStateInfo(0).IsName("Attack02")
            || IsAttacking) {
            IsAttacking = true;
            if(!animator.IsInTransition(0)
                && animator.GetCurrentAnimatorStateInfo(0).IsName("Idle_Battle")) {
                IsAttacking = false;
            }
        }
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Defend") 
            || IsDefending) {
            IsDefending = true;
            if(animator.GetAnimatorTransitionInfo(0).IsUserName("DefendToIdle")){
                IsDefending = false;
            }
        }
        if(animator.GetBool("bMoving")) {
            IsMoving = true;
        } else {
            IsMoving = false;
        }
        if(animator.GetBool("bRunning")) {
            IsRunning = true;
        } else {
            IsRunning = false;
        }
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("GetHit")
           || IsGettingHit) {
            IsGettingHit = true;
            if(animator.GetAnimatorTransitionInfo(0).IsUserName("GetHitToIdle")) {
                IsGettingHit = false;
            }
        }

        if(!animator.GetBool(playerMovementAnimationPID)
            || animator.GetBool(playerDefendAnimationPID)) {
            rawInputMovement = Vector3.zero;
            smoothInputMovement = Vector3.zero;
        }

        if(!IsAttacking && !IsDefending && !IsGettingHit) {

            CalculateMovementInputSmoothing();
            UpdatePlayerMovement();

            MoveThePlayer(); 
        }

        TurnThePlayer();
    }

    //Input's Axes values are raw
    void CalculateMovementInputSmoothing() {
        smoothInputMovement = Vector3.Lerp(
            smoothInputMovement,
            rawInputMovement,
            Time.deltaTime * movementSmoothingSpeed);
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
        transform.localRotation = Quaternion.Lerp(
            transform.localRotation,
            Quaternion.Euler(targetRotation),
            Time.deltaTime * turnSpeed);
    }

    ////////////////////////////////////////////////////////////////////
    void UpdateCharacterGameplayComponents() {
        if(IsAttacking) {
            if(!attackHitbox.gameObject.activeSelf) {
                attackHitbox.gameObject.SetActive(true);
            }
        } else {
            attackHitbox.gameObject.SetActive(false);
        }

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
            if(KilledOnce == 0) {
                m_SwordVFX_GlowEnrage.GetComponent<ParticleSystem>().Stop();
                m_SwordVFX_GlowEnrage.SetActive(false);

                if(!m_SwordVFX_Glow.activeSelf) {
                    m_SwordVFX_Glow.SetActive(true);
                    m_SwordVFX_Glow.GetComponent<ParticleSystem>().Play(); 
                }
            } else {
                m_SwordVFX_Glow.GetComponent<ParticleSystem>().Stop();
                m_SwordVFX_Glow.SetActive(false);

                if(!m_SwordVFX_GlowEnrage.activeSelf) {
                    m_SwordVFX_GlowEnrage.SetActive(true);
                    m_SwordVFX_GlowEnrage.GetComponent<ParticleSystem>().Play();
                }
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
            if(animator.GetAnimatorTransitionInfo(0).IsUserName("Attack1ToIdle") 
                || animator.GetAnimatorTransitionInfo(0).IsUserName("Attack2ToIdle")
                || !IsAttacking) {
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

    bool CheckGrounded() {
        // Check if on the ground;
        if (Physics.CheckSphere(
            new Vector3(
                capsuleCollider.bounds.center.x,
                capsuleCollider.bounds.min.y + groundCheckColliderOffset,
                capsuleCollider.bounds.center.z),
            capsuleCollider.radius * groundCheckColliderRadius,
            groundLayers)) {
            jumpCounter = maxJumpCounter;
            return IsGrounded = true;
        } else {
            CanJump = false;
            return IsGrounded = false;
        }
    }
    private void OnDrawGizmos() {
        if(DrawDebugGizmos) {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(
                new Vector3(
                    capsuleCollider.bounds.center.x,
                    capsuleCollider.bounds.min.y + groundCheckColliderOffset),
                capsuleCollider.radius * groundCheckColliderRadius);
        }
    }

    ////////////////////////////////////////////////////////////////////
    
    public Vector3 GetCurrentCharacterPosition() {
        return transform.position;
    }
    public void OnTriggerEnter(Collider other) {
        

    }
}

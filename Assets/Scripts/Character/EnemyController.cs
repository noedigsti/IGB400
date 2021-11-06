using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("_Character Gameplay State Section")]
    [Range(0,100)]
    [SerializeField] int health;
    [Range(0,100)]
    [SerializeField] float healthDecimal;
    public float GetCurrentHP => health;
    [SerializeField] float maxHealth = 100f;
    public float GetMaxHP => maxHealth;
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float turnSpeed = 5f;

    [SerializeField] float damage = 20f;

    [SerializeField] bool isDead = false;
    public bool IsDead => isDead;
    [SerializeField] bool DeathCheck = true;

    [SerializeField] bool isTakingHit = false;



    [Header("_Character Gameplay Components Section")]
    public UIEnemy ui;

    [Header("_Physics Section")]
    public SphereCollider hitboxTrigger;
    public SphereCollider hitboxRigid;
    public Rigidbody rigidBody;

    [Header("_Movement Behaviour Section")]
    public List<Transform> patrolWaypoints = new List<Transform>(2);
    public Transform nextWaypoint;
    byte index = 0;
    float towardRotation = 91f;
    bool towardRight = true;

    [Header("_Animator Section")]
    public Animator animator;
    int enemyDieAnimationPID;


    // Start is called before the first frame update
    void Start()
    {
        hitboxTrigger = GetComponent<SphereCollider>();
        ui = GetComponentInChildren<UIEnemy>();
        rigidBody = GetComponent<Rigidbody>();

        SetupAnimation();
        SetupWaypoints();
    }
    void SetupAnimation() {
        animator = GetComponent<Animator>();
        enemyDieAnimationPID = Animator.StringToHash("Die");
    }
    void SetupWaypoints() {
        index = 0;
        if (patrolWaypoints.Count > 0) {
            nextWaypoint = patrolWaypoints[index];
        }
    }

    private void OnEnable() {
        healthDecimal = maxHealth;
        health = Mathf.RoundToInt(healthDecimal);
    }
    public void TakeDamage(float _damage) {
        //Debug.Log("Took damage");
        if(!isDead && DeathCheck) {
            animator.Play("GetHit");
            healthDecimal -= _damage;
            health = Mathf.RoundToInt(healthDecimal);
            ui.DisplayDamageTaken(_damage);
        }
        if(health <= 0) {
            isDead = true;
            StartCoroutine(DieSequence());
        }
    }
    IEnumerator DieSequence() {
        animator.SetTrigger(enemyDieAnimationPID);
        hitboxRigid.enabled = false;
        yield return new WaitUntil(() =>
            animator.GetCurrentAnimatorStateInfo(0).IsName("Die")
            && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1
        );
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if(!isTakingHit && !isDead) {
            if (patrolWaypoints.Count >= 1) {
                MoveCharacter();
                TurnCharacter();
            }
        }
    }

    void MoveCharacter() {
        Vector3 movement = new Vector3(towardRight ? 1 : -1,0,0) * moveSpeed * Time.deltaTime;
        rigidBody.MovePosition(transform.position + movement);
    }
    void TurnCharacter() {
        transform.localRotation = Quaternion.Lerp(
            transform.localRotation,
            Quaternion.Euler(new Vector3(0,towardRotation,0)),
            Time.deltaTime * turnSpeed);
    }

    private void OnTriggerEnter(Collider other) {
        if(!isDead) {
            if(other.gameObject.CompareTag("Player")) {
                other.GetComponent<PlayerController>().TakeDamage(damage);
            }
            if(GameObject.ReferenceEquals(other.gameObject,nextWaypoint? nextWaypoint.gameObject:null)) {
                nextWaypoint = patrolWaypoints[towardRight ? 1 : 0];
                towardRight = !towardRight;
                towardRotation = towardRight ? 91f : 269f;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("_Character Gameplay State Section")]
    [Range(0,100)]
    [SerializeField] int health = 100;
    [Range(0,100)]
    [SerializeField] float healthDecimal = 100f;
    public float GetCurrentHP => health;
    [SerializeField] float maxHealth = 100f;
    [SerializeField] bool IsDead = false;
    [SerializeField] bool DeathCheck = true;


    [Header("_Character Gameplay Components Section")]
    public UIEnemy ui;
    public SphereCollider hitbox;

    [Header("_Movement Behaviour Section")]

    public List<Transform> patrolWaypoints;

    // Start is called before the first frame update
    void Start()
    {
        hitbox = GetComponent<SphereCollider>();
    }

    public void TakeDamage(float _damage) {
        healthDecimal -= _damage;
        health = Mathf.RoundToInt(healthDecimal);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

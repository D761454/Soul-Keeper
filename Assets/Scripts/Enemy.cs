using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected float health;
    [SerializeField] protected float recoilLength, recoilFactor;
    [SerializeField] protected bool isRecoiling = false;

    [SerializeField] protected PlayerController player;
    [SerializeField] protected float speed;

    [SerializeField] protected float damage;

    protected float recoilTimer;
    protected Rigidbody2D rb;

    protected bool hasTakenDamage = false;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        
    }

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = PlayerController.Instance;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        hasTakenDamage = false;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
        if(isRecoiling)
        {
            if(recoilTimer < recoilLength)
            {
                recoilTimer += Time.deltaTime;
            }
            else
            {
                isRecoiling = false;
                recoilTimer = 0;
            }
        }
    }

    public virtual void EnemyHit(float _damageDone, Vector2 _hitDirection, float _hitForce)
    {
        if (hasTakenDamage) return;
        health -= _damageDone;
        if (!isRecoiling)
        {
            rb.AddForce(-_hitForce * recoilFactor * _hitDirection); // recoil enemy in direction hit
        }
        hasTakenDamage = true;
    }
    
    protected void OnTriggerStay2D(Collider2D _other)
    {
        if (_other.CompareTag("Player") && !PlayerController.Instance.pState.invincible)
        {
            Attack();
            PlayerController.Instance.HitStopTime(0, 5, 0.5f);
        }
    }

    protected virtual void Attack()
    {
        PlayerController.Instance.TakeDamage(damage);
    }
}

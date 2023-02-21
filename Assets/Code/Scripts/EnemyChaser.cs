using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyChaser : EnemyController
{
    public float ExplosionRange;
    
    protected override void Awake()
    {
        base.Awake();

        base.CurrentHealth = 30;
    }
    
    private void FixedUpdate()
    {
        Movement();
    }

    private void Update()
    {
        if (!base.ChaseEnemy())
        {
            base.WanderAround();
        }
    }

    // Explodes the boat, damaging all boats around.
    protected override void Attack()
    {
        // Damages all the boats within the explosion range.
        RaycastHit2D[] explosionRaycast = Physics2D.CircleCastAll(
            transform.position,
            ExplosionRange,
            transform.forward,
            0.1f,
            1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Enemy"));
        
        if (explosionRaycast == null)
        {
            return;
        }

        foreach (RaycastHit2D boat in explosionRaycast)
        {
            try
            {
                boat.collider.GetComponent<BoatController>().TakeHit(23);
            }
            catch(Exception e)
            {Debug.LogException(e);}
        }

        // Instantiates the explosion particle effects and reduces its health to 0.
        Instantiate(transform.GetChild(2),transform.position,transform.rotation).gameObject.SetActive(true);
        TakeHit(base.CurrentHealth);
    }


    // Calls the explosion when collided with anoter boat.
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") ||
            other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Attack();
        }
    }

    public override void TakeHit(int damage)
    {
        base.TakeHit(damage);
        base.GameplayManager.UpdateScore(9);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        // Explosion area.
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, ExplosionRange);
    }
}

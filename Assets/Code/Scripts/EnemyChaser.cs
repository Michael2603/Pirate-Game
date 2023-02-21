using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyChaser : BoatController
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
        if (!ChasingEnemy())
        {
            WanderAround();
        }
    }

    // If the enemy is on sight, chase him.
    private bool ChasingEnemy()
    {
        RaycastHit2D raycast = Physics2D.CircleCast(transform.position, 6, transform.forward, .1f, 1 << LayerMask.NameToLayer("Player"));
        
        // If the enemy is within sight range, only chase him if this boat has direct sight to him.
        if (raycast)
        {
            RaycastHit2D visionObstructed = Physics2D.Linecast(
                transform.position,
                raycast.collider.transform.position,
                1 << LayerMask.NameToLayer("Island"));

            if (visionObstructed)
            {
                return false;
            }

    
            // Gets position and angle in relation to the enemy and turns the boat so that it is in attack position in relation to the enemy.
            Vector3 posRelativeToPlayer = transform.InverseTransformPoint(raycast.transform.position);
            float angleRelativeToPlayer = Mathf.Atan2(posRelativeToPlayer.y, posRelativeToPlayer.x) * Mathf.Rad2Deg;
            
            if (Mathf.Abs(angleRelativeToPlayer) < 85)
            {
                base.Rotate(1);
                base.MoveForward(0.1f);
            }
            else if (Mathf.Abs(angleRelativeToPlayer) > 95)
            {
                base.Rotate(-1);
                base.MoveForward(0.1f);
            }
            else
            {
                base.Rotate(0);
                base.MoveForward(0.3f);
            }

            return true;
        }
        else
        {
            return false;
        }

    }

    // Wander around the map avoiding colision with islands.
    private void WanderAround()
    {
        RaycastHit2D leftRaycast = Physics2D.Raycast(transform.position, (transform.up + -transform.right), 4, 1 << LayerMask.NameToLayer("Island"));
        RaycastHit2D middleRaycast = Physics2D.CircleCast(transform.position + transform.up * 2.2f, 1.3f,transform.forward, .1f, 1 << LayerMask.NameToLayer("Island"));
        RaycastHit2D rightRaycast = Physics2D.Raycast(transform.position, (transform.up + transform.right), 4, 1 << LayerMask.NameToLayer("Island"));

        // Keep straight.
        if (!middleRaycast)
        {
            Rotate(0);
            MoveForward(.3f);
            return;
        }
        
        // Rotate to the right.
        if (leftRaycast)
        {
            Rotate(.5f);
            MoveForward(.2f);
        }
        
        // Rotate to the left.
        if (rightRaycast)
        {
            Rotate(-.5f);
            MoveForward(.2f);
        }
    }

    void OnDrawGizmos()
    {
        // Sight area.
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(transform.position, 6f);

        // Explosion area.
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, ExplosionRange);

        Gizmos.color = Color.black;
        Gizmos.DrawRay(transform.position, (transform.up + -transform.right)  * 2.9f);
        Gizmos.DrawWireSphere(transform.position + transform.up * 2.2f, 1.3f);
        Gizmos.DrawRay(transform.position, (transform.up + transform.right) * 2.9f);
    }

    public override void TakeHit(int damage)
    {
        base.TakeHit(damage);
        base.GameplayManager.UpdateScore(9);
    }

    // Explode when collides with anoter boat.
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") || other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            // Damages all the boats within the explosion range.
            RaycastHit2D[] explosionRaycast = Physics2D.CircleCastAll(
                transform.position, ExplosionRange,
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
                {   }
            }

            // Instantiates the explosion particle effects and reduces its health to 0.
            Instantiate(transform.GetChild(2),transform.position,transform.rotation).gameObject.SetActive(true);
            TakeHit(base.CurrentHealth);
        }
    }
}

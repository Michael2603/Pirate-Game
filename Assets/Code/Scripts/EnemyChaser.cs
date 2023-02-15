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
        // Looks for the player and if found, rotate the boat to face him.
        RaycastHit2D raycast = Physics2D.CircleCast(transform.position, 5, transform.forward, .1f, 1 << LayerMask.NameToLayer("Player"));
        if (raycast)
        {
            Vector3 posRelativeToPlayer = transform.InverseTransformPoint(raycast.transform.position);
            float angleRelativeToPlayer = Mathf.Atan2(posRelativeToPlayer.y, posRelativeToPlayer.x) * Mathf.Rad2Deg;
            
            if (Mathf.Abs(angleRelativeToPlayer) < 85)
            {
                base.Rotate(1);
            }
            else if (Mathf.Abs(angleRelativeToPlayer) > 95)
            {
                base.Rotate(-1);
            }
            else
            {
                base.Rotate(0);
            }
        }
    }

    void OnDrawGizmos()
    {
        // Sight area.
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 5f);

        // Explosion area.
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, ExplosionRange);
    }

    public override void TakeHit(int damage)
    {
        base.TakeHit(damage);
        base._uiManager.UpdateScore(9);
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

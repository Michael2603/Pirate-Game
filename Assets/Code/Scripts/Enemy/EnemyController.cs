using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : BoatController
{
    public float AttackRange, SightRange;

    // Chase the enemy if on sight and attack him if in attack range.
    protected bool ChaseEnemy()
    {
        RaycastHit2D inAttackRangeRaycast = Physics2D.CircleCast(
            transform.position,
            AttackRange,
            transform.forward,
            .1f,
            1 << LayerMask.NameToLayer("Player"));
            
        RaycastHit2D inSightRangeRaycast = Physics2D.CircleCast(
            transform.position,
            SightRange,
            transform.forward,
            .1f,
            1 << LayerMask.NameToLayer("Player"));
        

        // If the enemy is within sight range, only chase him if this boat has direct sight to him.
        if (inSightRangeRaycast)
        {
            RaycastHit2D visionObstructed = Physics2D.Linecast(
                transform.position,
                inSightRangeRaycast.collider.transform.position,
                1 << LayerMask.NameToLayer("Island"));

            if (visionObstructed)
            {
                return false;
            }
        }
        
        // Attacks the enemy or Persues the enemy until he enters the attack range.
        if (inAttackRangeRaycast)
        {
            Attack();
            return true;
        }
        else if (inSightRangeRaycast && !inAttackRangeRaycast)
        {

            // Gets position and angle relative to the enemy and turns the boat so that it is in attack position relative to the enemy.
            Vector3 posRelativeToEnemy = transform.InverseTransformPoint(inSightRangeRaycast.transform.position);
            float angleRelativeToEnemy = Mathf.Atan2(posRelativeToEnemy.y, posRelativeToEnemy.x) * Mathf.Rad2Deg;

            if (Mathf.Abs(angleRelativeToEnemy) < 85)
            {
                base.Rotate(1);
                base.MoveForward(base.MovementSpeed * 0.25f);
            }
            else if (Mathf.Abs(angleRelativeToEnemy) > 95)
            {
                base.Rotate(-1);
                base.MoveForward(base.MovementSpeed * 0.25f);
            }
            else
            {
                base.Rotate(0);
                base.MoveForward(base.MovementSpeed);
            }

            return true;
        }
        return false;
    }

    // Wander around the map avoiding colision with islands.
    protected void WanderAround()
    {
        RaycastHit2D leftRaycast = Physics2D.Raycast(
            transform.position,
            (transform.up + -transform.right),
            4,
            1 << LayerMask.NameToLayer("Island"));

        RaycastHit2D rightRaycast = Physics2D.Raycast(transform.position, 
            (transform.up + transform.right), 
            4, 
            1 << LayerMask.NameToLayer("Island"));
        
        RaycastHit2D middleRaycast = Physics2D.CircleCast(
            transform.position + transform.up * 2.2f,
            1.3f,
            transform.forward, 
            .1f, 
            1 << LayerMask.NameToLayer("Island"));

        // Keep straight.
        if (!middleRaycast)
        {
            Rotate(0);
            MoveForward(base.MovementSpeed);
            return;
        }
        
        // Rotate to the right.
        if (leftRaycast)
        {
            Rotate(0.5f);
            MoveForward(base.MovementSpeed * 0.8f);
        }
        
        // Rotate to the left.
        if (rightRaycast)
        {
            Rotate(-0.5f);
            MoveForward(base.MovementSpeed * 0.8f);
        }
    }

    // Will be overrited by each enemy type inside its class.
    protected virtual void Attack()
    {
        
    }

    // Draws the areas and rays on gizmos to help maintenance.
    protected virtual void OnDrawGizmos()
    {
        // Sight area.
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, SightRange);

        // Attack range.
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, AttackRange);

        Gizmos.color = Color.black;
        Gizmos.DrawRay(transform.position, (transform.up + -transform.right)  * 2.9f);
        Gizmos.DrawWireSphere(transform.position + transform.up * 2.2f, 1.3f);
        Gizmos.DrawRay(transform.position, (transform.up + transform.right) * 2.9f);
    }
}

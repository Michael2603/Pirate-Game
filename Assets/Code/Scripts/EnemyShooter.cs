using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : BoatController
{
    private void FixedUpdate()
    {
        Movement();

    }

    private void Update()
    {
        RaycastHit2D raycast = Physics2D.CircleCast(transform.position, 5, transform.forward, .1f, 1 << LayerMask.NameToLayer("Player"));
        if (raycast)
        {
            
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(transform.position, 5f);
    }

    public override void TakeHit(int damage)
    {
        base.TakeHit(damage);
        base._uiManager.UpdateScore(12);
    }

    private void Attack()
    {

    }
}

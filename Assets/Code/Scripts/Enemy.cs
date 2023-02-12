using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : BoatController
{
    void FixedUpdate()
    {
        Movement();
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

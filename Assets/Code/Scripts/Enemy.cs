using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : BoatController
{
    void FixedUpdate()
    {
        Movement();

        base.Rotate(1);
        base.MoveForward(1);

    }

    private void Attack()
    {

    }
}

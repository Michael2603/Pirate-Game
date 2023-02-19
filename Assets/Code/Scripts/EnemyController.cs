using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : BoatController
{

    private void ChaseEnemy()
    {
        base.MoveForward(1);
    }

    // Scans both sides and returns which one has an enemy within shooting range (45 degrees up or down on the sides).
    private int CheckLaterals()
    {
        RaycastHit2D raycast = Physics2D.CircleCast(transform.position, 5, transform.forward, .1f, 1 << LayerMask.NameToLayer("Player"));
        int direction = 0;

        if (raycast)
        {
            Vector3 posRelativeToPlayer = transform.InverseTransformPoint(raycast.transform.position);
            float angleRelativeToPlayer = Mathf.Atan2(posRelativeToPlayer.y, posRelativeToPlayer.x) * Mathf.Rad2Deg;

            // The player is within range if its position is 22.5 degrees up or down counting from the point perpendicular point to the boat.
            if (Mathf.Abs(angleRelativeToPlayer) - 22.5f < 22.5f)
            {
                // Right
                direction = -1;
            }
            else if (Mathf.Abs(angleRelativeToPlayer) - 22.5f > 112.5f)
            {
                // Left
                direction = 1;
            }
            else
            {
                direction = 0;
            }
        }

        return direction;
    }
}

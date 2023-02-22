using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : EnemyController
{
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

        // Constantly checks for ammunition.
        if (base._currentAmmunition < 3 && !base._isReloading)
        {
            base.StartCoroutine(ReloadAmmunition());
        }
        else if (base._currentAmmunition >= 3)
        {
            base._canShoot = true;
        }
    }

    protected override void Attack()
    {
        // Constantly rotates the boat to attack the enemy and if has enough ammunition, shoot.
        if (AimOnEnemy() != 0)
        {
            if (base._canShoot)
            {
                StartCoroutine(LateralShotPattern(AimOnEnemy()));
            }
        }
    }

    // Scans both sides and returns which one has an enemy within shooting range (30 degrees up or down on the sides).
    private int AimOnEnemy()
    {
        RaycastHit2D raycast = Physics2D.CircleCast(transform.position, 5, transform.forward, .1f, 1 << LayerMask.NameToLayer("Player"));
        int direction = 0;
        base.MoveForward(0);

        if (raycast)
        {
            // Gets position and angle in relation to the enemy and turns the boat so that it is in attack position in relation to the enemy.
            Vector3 posRelativeToEnemy = transform.InverseTransformPoint(raycast.transform.position);
            float angleRelativeToEnemy = Mathf.Atan2(posRelativeToEnemy.y, posRelativeToEnemy.x) * Mathf.Rad2Deg;
            
            if ( (angleRelativeToEnemy > 90 && angleRelativeToEnemy < 175) ||
                (angleRelativeToEnemy < -5 && angleRelativeToEnemy > -90))
            {
                base.Rotate(.5f);
            }
            else if ( (angleRelativeToEnemy > 5 && angleRelativeToEnemy < 90) ||
                (angleRelativeToEnemy > -175 && angleRelativeToEnemy < -90))
            {
                base.Rotate(-.5f);
            }
            else
            {
                base.Rotate(0);
            }

            // The player is within range if its position is 30 degrees up or down counting from the point perpendicular point to the boat.
            if (Mathf.Abs(angleRelativeToEnemy) - 15f < 15f)
            {
                // Right
                direction = -1;
            }
            else if (Mathf.Abs(angleRelativeToEnemy) - 15f > 120f)
            {
                // Left
                direction = 1;
            }
        }

        return direction;
    }

    // Fires 3 bullets to the side of the boat in a random pattern.
    private IEnumerator LateralShotPattern(int direction)
    {
        base._canShoot = false;
        base._currentAmmunition = 0;
        
        List<int> usedPositions = new List<int>();

        for(int i = 0; i < 3; i++)
        {
            int selectedPosition = Random.Range(-1,2);

            // Select a random position without repeat.
            while (true)
            {
                if (!usedPositions.Contains(selectedPosition))
                {
                    usedPositions.Add(selectedPosition);
                    break;
                }
                else
                {
                    selectedPosition = Random.Range(-1,2);
                }
            }

            // Combine the current boat rotation and more 7 degrees between each bullet.
            Quaternion newRotation = Quaternion.Euler(transform.forward * 7 * (selectedPosition * direction)) * transform.rotation;
            
            // Converts the rotation of the bullet so it can move to the left.
            if (direction == -1)
            {
                newRotation *= Quaternion.Euler(transform.forward * 180 * direction);
            }
            
            GameObject tempCannonBall = Instantiate(
                base.CannonBall,
                transform.position + (transform.up * (selectedPosition / 9f)),
                newRotation);
            
            Physics2D.IgnoreCollision(tempCannonBall.GetComponent<Collider2D>(), GetComponent<Collider2D>());
            tempCannonBall.GetComponent<Rigidbody2D>().AddForce(-tempCannonBall.transform.right * base._cannonFireStrength);
            tempCannonBall.GetComponent<CannonBallController>().PlaySound("Lateral");

            // Randomize a small time gap between each shot.
            float timer = Random.Range(0.05f, 0.21f);

            yield return new WaitForSeconds(timer);
        }
    }

    public override void TakeHit(int damage)
    {
        base.TakeHit(damage);
        base.GameplayManager.UpdateScore(12);
    }
}

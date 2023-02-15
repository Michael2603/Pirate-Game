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
        if (raycast && base._canShoot)
        {
            Vector3 posRelativeToPlayer = transform.InverseTransformPoint(raycast.transform.position);
            float angleRelativeToPlayer = Mathf.Atan2(posRelativeToPlayer.y, posRelativeToPlayer.x) * Mathf.Rad2Deg;
            if (Mathf.Abs(angleRelativeToPlayer) - 22.5f < 22.5f)
            {
                base._canShoot = false;
                StartCoroutine(LateralShotPattern(-1));
            }
        }
    }

    // Fires 3 bullets to the side of the boat in a random pattern.
    private IEnumerator LateralShotPattern(int direction)
    {
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
            Quaternion newRotation = Quaternion.Euler(transform.forward * 7 * (-selectedPosition * direction)) * transform.rotation;
            
            // Converts the rotation of the bullet so it can moveto the left.
            if (direction == -1)
            {
                newRotation *= Quaternion.Euler(transform.forward * 180 * direction);
            }
            
            GameObject tempCannonBall = Instantiate(
                base.CannonBall,
                transform.position + (transform.up * (selectedPosition / 9f)),
                newRotation);
            
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), tempCannonBall.GetComponent<Collider2D>());
            tempCannonBall.GetComponent<Rigidbody2D>().AddForce(-tempCannonBall.transform.right * base._cannonFireStrength);

            // Randomize a small time gap between each shot.
            float timer = Random.Range(0.05f, 0.21f);

            yield return new WaitForSeconds(timer);
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

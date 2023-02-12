using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBallController : MonoBehaviour
{
    private TrailRenderer _trailRenderer;

    private void Awake()
    {
        // Randomly changes the size of trails when instantiated.
        GetComponent<TrailRenderer>().widthMultiplier = Random.Range(.3f, 1.2f);
    }

    // Destroys itself and damages a boat if hit it.
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            other.gameObject.GetComponent<BoatController>().TakeHit(10);
        }

        Destroy(this.gameObject);
    }
}

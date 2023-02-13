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

            // Instantiate particles separately and they play on awake, so the ball can be destroyed immediately,
            Instantiate(transform.GetChild(1),transform.position, transform.rotation).gameObject.SetActive(true);            
        }

            Instantiate(transform.GetChild(0),transform.position, transform.rotation).gameObject.SetActive(true);
            Destroy(this.gameObject);
    }
}

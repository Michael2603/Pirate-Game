using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ----------------- Summary -----------------
//  This class is responsible for making the Cannon Ball interactions
//  in the game, such as colision, particles and sound triggers.
// -------------------------------------------

public class CannonBallController : MonoBehaviour
{
    private TrailRenderer _trailRenderer;
    private AudioSource _audioSource;

    public List<AudioClip> AudioClips = new List<AudioClip>();

    private void Awake()
    {
        // Randomly changes the size of trails when instantiated.
        GetComponent<TrailRenderer>().widthMultiplier = Random.Range(.3f, 1.2f);
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(string position)
    {
        switch(position)
        {
            case "Frontal":
                _audioSource.clip = AudioClips[0];
            break;
            case "Lateral":
                _audioSource.clip = AudioClips[Random.Range(1, 4)];
            break;
        }

        _audioSource.Play();
    }

    // Destroys itself and damages a boat if hit it.
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy") || other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            other.gameObject.GetComponent<BoatController>().TakeHit(10);

            // Instantiate particles separately and they play on awake, so the ball can be destroyed immediately,
            Instantiate(transform.GetChild(1),transform.position, transform.rotation).gameObject.SetActive(true);            
        }

            Instantiate(transform.GetChild(0),transform.position, transform.rotation).gameObject.SetActive(true);
            Destroy(this.gameObject);
    }
}

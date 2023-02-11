using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBallController : MonoBehaviour
{
    private TrailRenderer _trailRenderer;

    void Awake()
    {
        // Randomly changes the size of trails when instantiated.
        Random.InitState(System.DateTime.Now.Second);
        GetComponent<TrailRenderer>().widthMultiplier = Random.Range(.3f, 1.2f);
    }

    
}

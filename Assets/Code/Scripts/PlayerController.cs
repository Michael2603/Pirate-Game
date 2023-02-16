using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerController : BoatController
{
    private float _inputX;
    private float _inputY;
    private int _lateralShotDirection = 1;

    [SerializeField] private CinemachineVirtualCamera _camera;

    void Update()
    {
        base.Movement();
        base.Rotate(_inputX);
        base.MoveForward(_inputY);
    }

    public override void TakeHit(int damage)
    {
        base.TakeHit(damage);
        base._uiManager.UpdateScore(-12);

        StartCoroutine(ShakeCamera(.7f, 0.5f));
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

            tempCannonBall.GetComponent<Rigidbody2D>().AddForce(-tempCannonBall.transform.right * base._cannonFireStrength);

            // Randomize a small time gap between each shot and also shake the camera based on this time.
            float timer = Random.Range(0.05f, 0.21f);

            StartCoroutine(ShakeCamera(.8f, timer));
            yield return new WaitForSeconds(timer);
        }
    }

    // Uses the cinemachine to shake the camera and add some intensity to the shots and damage taken.
    private IEnumerator ShakeCamera(float intensity, float time)
    {   
        _camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = intensity;
        
        yield return new WaitForSeconds(time);

        _camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;
    }

    // ---------------------------------------- Callbacks for Input System ----------------------------------------------- \\

    // Takes the vertical and horizontal input and converts to separate variables, better handling movements separately.
    private void OnMove(InputValue value)
    {
        _inputY = value.Get<float>();
    }
    private void OnRotate(InputValue value)
    {
        _inputX = -value.Get<float>();
    }

    // Fires cannonballs forwards.
    private void OnFrontalShot()
    {
        GameObject tempCannonBall = Instantiate(
            base.CannonBall,
            transform.localPosition,
            transform.rotation);

        tempCannonBall.GetComponent<Rigidbody2D>().AddForce(-transform.up * base._cannonFireStrength);

        StartCoroutine(ShakeCamera(0.8f, 0.1f));
    }

    // Fires 3 cannonballs at the side of boat.
    private void OnLateralShot()
    {
        StartCoroutine(LateralShotPattern(_lateralShotDirection));
    }

    // Forces the lateral shot to change its direction.
    private void OnChangeLateralShotDirection()
    {
        if (_lateralShotDirection == 1)
        {
            _lateralShotDirection = -1;
        }
        else
        {
            _lateralShotDirection = 1;
        }

        // Emits arrows showing the current active side, also modifies particle attributes so arrows are facing the right direction.
        ParticleSystem indicatorParticles = transform.GetChild(2).GetComponent<ParticleSystem>();
        var particlesShape = indicatorParticles.shape;
        var particlesRenderer = indicatorParticles.GetComponent<ParticleSystemRenderer>();
        var particlesVelocity = indicatorParticles.velocityOverLifetime;
        var particlesRotation = indicatorParticles.main;

        particlesShape.scale = new Vector3(-_lateralShotDirection,1,1);
        particlesRenderer.flip = new Vector3(_lateralShotDirection,1,1);
        particlesVelocity.xMultiplier = -_lateralShotDirection * 0.3f;
        particlesRotation.startRotation = -transform.localEulerAngles.z * Mathf.Deg2Rad;

        indicatorParticles.Emit(1);
    }
}
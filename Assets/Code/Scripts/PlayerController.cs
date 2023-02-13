using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerController : BoatController
{
    private float _inputX;
    private float _inputY;
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
        List<GameObject> tempCannonBalls = new List<GameObject>();
        int shotPosition = -1;

        // Instantiate 3 bullets and store them inside a list.
        for (int i = 0; i < 3; i++, shotPosition++)
        {
            // The cannonballs are being created at 180 points in Z axis, making them shake for the camera noise
            tempCannonBalls.Add(Instantiate(
                base.CannonBall,
                transform.localPosition + transform.rotation.eulerAngles + Vector3.one * shotPosition,
                transform.rotation));

            tempCannonBalls[i].GetComponent<Rigidbody2D>().AddForce(-transform.right * base._cannonFireStrength);
        }

        StartCoroutine(ShakeCamera(1f, 0.2f));
    }
}
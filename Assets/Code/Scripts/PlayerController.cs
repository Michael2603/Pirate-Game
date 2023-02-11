using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float ForwardMovementSpeed;
    public float RotationSpeed;
    
    private Rigidbody2D _rigidbody2d;   
    private float _inputX;
    private float _inputY;
    private float _smoothMovementVelocity;
    private float _movementInputSmoothVelocity;

    public GameObject _cannonBall;


    private void Start()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Movement();
    }

    // Takes player input values and converts to ship movement.
    private void Movement()
    {
        // Smooth the ship departure and stop before sending the velocity to Rigidbody.
        if (_inputY > 0)
        {
            _smoothMovementVelocity = Mathf.SmoothDamp(_smoothMovementVelocity, _inputY, ref _movementInputSmoothVelocity, 0.55f);
        }
        else
        {
            _smoothMovementVelocity = Mathf.SmoothDamp(_smoothMovementVelocity, _inputY, ref _movementInputSmoothVelocity, 1f);
        }

        _rigidbody2d.velocity = -transform.up * _smoothMovementVelocity * ForwardMovementSpeed;


        // Rotates the ship based on its current rotation and the input.
        if (_inputX != 0)
        {
            float angle = _inputX * Mathf.Rad2Deg;
            transform.localRotation = transform.localRotation * Quaternion.Euler(Vector3.forward * angle * Time.deltaTime * RotationSpeed);
        }
    }

    // Takes the vertical and horizontal input and converts to separate variables, better handling movements separately.
    private void OnMove(InputValue value)
    {
        _inputY = value.Get<float>();
    }

    private void OnRotate(InputValue value)
    {
        _inputX = -value.Get<float>();
    }

    private void OnFrontalShot()
    {
        GameObject tempCannonBall = Instantiate(_cannonBall, transform.position, transform.rotation);
        tempCannonBall.GetComponent<Rigidbody2D>().AddForce(transform.right * 300);
    }
}
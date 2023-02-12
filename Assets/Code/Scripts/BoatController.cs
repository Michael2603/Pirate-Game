using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BoatController : MonoBehaviour
{
    public float ForwardMovementSpeed;
    public float RotationSpeed;
    [HideInInspector] public int CurrentHealth;
    [HideInInspector] public UIManager _uiManager;
    public GameObject CannonBall;
    public float CannonFireStrength;

    protected float _cannonFireStrength
    {
        get {return CannonFireStrength * 100; }
        set {CannonFireStrength = value; }
    }

    protected Rigidbody2D _rigidbody2d;
    private Animator _animator;
    private Slider _healthBar;

    private float _rotationDirection;
    private float _moveForwardCurrentForce;

    private float _smoothMovementVelocity;
    private float _movementInputSmoothVelocity;


    protected virtual void Awake()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _uiManager = GameObject.Find("Manager").GetComponent<UIManager>();
        _healthBar = transform.GetChild(0).GetChild(0).gameObject.GetComponent<Slider>();
        
        CurrentHealth = 100;
    }

    // Takes player input values and converts to ship movement.
    protected void Movement()
    {
        // Smooths ship's departure and stop before sending velocity to the Rigidbody.
        if (_moveForwardCurrentForce > 0)
        {
            _smoothMovementVelocity = Mathf.SmoothDamp(
                _smoothMovementVelocity,
                _moveForwardCurrentForce,
                ref _movementInputSmoothVelocity,
                0.55f);
        }
        else
        {
            _smoothMovementVelocity = Mathf.SmoothDamp(
                _smoothMovementVelocity,
                _moveForwardCurrentForce,
                ref _movementInputSmoothVelocity,
                1f);
        }

        _rigidbody2d.velocity = -transform.up * _smoothMovementVelocity * ForwardMovementSpeed;


        // Rotates the ship based on its current rotation and the input.
        if (_rotationDirection != 0)
        {
            float angle = _rotationDirection * Mathf.Rad2Deg;
            transform.localRotation *= Quaternion.Euler(Vector3.forward * angle * Time.deltaTime * RotationSpeed);
        }
    }

    // Damages the player and update its health.
    public virtual void TakeHit(int damage)
    {
        CurrentHealth -= damage;
        HealthHandler();
    }

    // Atualiza os dados referentes à vida do barco.
    private void HealthHandler()
    {
        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            _healthBar.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            _healthBar.value = CurrentHealth;
        }
        
        _animator.SetInteger("CurrentHealthAmount", CurrentHealth);
    }


    protected void Rotate(float direction)
    {
        _rotationDirection = direction;
    }

    protected void MoveForward(float currentSpeed)
    {
        _moveForwardCurrentForce = currentSpeed;
    }
}
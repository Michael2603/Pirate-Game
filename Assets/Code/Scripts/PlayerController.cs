using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float ForwardMovementSpeed;
    public float RotationSpeed;
    [HideInInspector] public int CurrentHealth;
    public GameObject CannonBall;
    public float CannonFireStrength;

    private float _cannonFireStrength
    {
        get {return CannonFireStrength * 100; }
        set {CannonFireStrength = value; }
    }

    private Rigidbody2D _rigidbody2d;
    private Animator _animator;
    private Slider _healthBar;
    private UIManager _uiManager;
    private float _inputX;
    private float _inputY;
    private float _smoothMovementVelocity;
    private float _movementInputSmoothVelocity;


    private void Awake()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _healthBar = transform.GetChild(0).GetChild(0).gameObject.GetComponent<Slider>();
        _uiManager = GameObject.Find("Manager").GetComponent<UIManager>();
        Random.InitState(System.DateTime.Now.Second);
    }

    private void Start()
    {
        CurrentHealth = 100;
    }

    void FixedUpdate()
    {
        Movement();
    }

    // Takes player input values and converts to ship movement.
    private void Movement()
    {
        // Smooths ship's departure and stop before sending velocity to the Rigidbody.
        if (_inputY > 0)
        {
            _smoothMovementVelocity = Mathf.SmoothDamp(
                _smoothMovementVelocity,
                _inputY,
                ref _movementInputSmoothVelocity,
                0.55f);
        }
        else
        {
            _smoothMovementVelocity = Mathf.SmoothDamp(
                _smoothMovementVelocity,
                _inputY,
                ref _movementInputSmoothVelocity,
                1f);
        }

        _rigidbody2d.velocity = -transform.up * _smoothMovementVelocity * ForwardMovementSpeed;


        // Rotates the ship based on its current rotation and the input.
        if (_inputX != 0)
        {
            float angle = _inputX * Mathf.Rad2Deg;
            transform.localRotation *= Quaternion.Euler(Vector3.forward * angle * Time.deltaTime * RotationSpeed);
        }
    }

    // Damages the player and update its health.
    public void TakeHit(int damage)
    {
        CurrentHealth -= damage;
        HealthHandler();
    }

    // Atualiza os dados referentes à vida do barco.
    private void HealthHandler()
    {
        _healthBar.value = CurrentHealth;
        _animator.SetInteger("CurrentHealthAmount", CurrentHealth);
    }



    // ---------------------------------------- Callbacks for the Input System ----------------------------------------------- \\

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
            CannonBall,
            transform.localPosition,
            transform.rotation,
            this.transform);

        tempCannonBall.GetComponent<Rigidbody2D>().AddForce(-transform.up * _cannonFireStrength);
    }

    // Fires 3 cannonballs at the side of boat.
    private void OnLateralShot()
    {
        List<GameObject> tempCannonBalls = new List<GameObject>();
        int shotPosition = -1;

        // Instantiate 3 bullets and store them inside a list.
        for (int i = 0; i < 3; i++, shotPosition++)
        {
            tempCannonBalls.Add(Instantiate(
                CannonBall,
                transform.localPosition + transform.rotation.eulerAngles + Vector3.one * shotPosition,
                transform.rotation,
                this.transform));

            tempCannonBalls[i].GetComponent<Rigidbody2D>().AddForce(-transform.right * _cannonFireStrength);
        }
    }
}
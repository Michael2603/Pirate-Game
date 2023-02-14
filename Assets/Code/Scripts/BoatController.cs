using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BoatController : MonoBehaviour
{
    [HideInInspector] public int CurrentHealth;
    [HideInInspector] public UIManager _uiManager;
    public float ForwardMovementSpeed;
    public float RotationSpeed;
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
    private Material _healthBarMaterial;

    private float _rotationDirection;
    private float _moveForwardCurrentForce;

    private float _smoothMovementVelocity;
    private float _movementInputSmoothVelocity;

    private Coroutine _healthBarCoroutine;


    protected virtual void Awake()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _uiManager = GameObject.Find("Manager").GetComponent<UIManager>();
        _healthBar = transform.GetChild(0).GetChild(0).gameObject.GetComponent<Slider>();

        // Creates a material with default shader and applies to the health bar so its alpha channel can be manipulated later.
        _healthBarMaterial = new Material(GetComponent<SpriteRenderer>().sharedMaterial.shader);
        foreach (Image image in _healthBar.transform.GetComponentsInChildren<Image>())
        {
            image.material = _healthBarMaterial;
        }

        _healthBarCoroutine = StartCoroutine(FadeHealthBar());
        
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

        // Displays the boat's health bar and then hide it again.
        StopCoroutine(_healthBarCoroutine);
        _healthBarCoroutine = StartCoroutine(FadeHealthBar());
    }

    // Updates the data related to boat's health points.
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

    // Briefly displays the health bar and then fades it out.
    private IEnumerator FadeHealthBar()
    {
        Color tempColor = Color.white;
        tempColor.a = _healthBarMaterial.color.a;

        // Fade in.
        while (tempColor.a < 1)
        {
            tempColor.a += .1f;
            _healthBarMaterial.color = tempColor;

            yield return new WaitForSeconds(.02f);
        }

        tempColor.a = 1;
        _healthBarMaterial.color = tempColor;
        
        yield return new WaitForSeconds(1f);

        // Fade Out.
        while (tempColor.a > 0f)
        {
            tempColor.a -= .02f;
            _healthBarMaterial.color = tempColor;

            yield return new WaitForSeconds(.01f);
        }

        tempColor.a = 0;
        _healthBarMaterial.color = tempColor;
    }

    // Used by the animations behaviour. Makes the boat emmmit more flames particles until its last visual state.
    [System.Obsolete]
    public void EmmitParticles()
    {
        if (transform.GetChild(1).GetComponent<ParticleSystem>().emissionRate >= 2)
        {
            transform.GetChild(1).GetComponent<ParticleSystem>().Stop();
        }
        else
        {
            transform.GetChild(1).GetComponent<ParticleSystem>().emissionRate++;
        }
    }


    // Takes the direction that the boat needs to rotate.
    protected void Rotate(float direction)
    {
        _rotationDirection = direction;
    }

    // Takes the current desired force to move the boat forward.
    protected void MoveForward(float currentSpeed)
    {
        if (currentSpeed < 0)
            return;
        
        _moveForwardCurrentForce = currentSpeed;
    }
}
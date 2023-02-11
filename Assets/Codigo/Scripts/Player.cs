using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float VelocidadeAvanco;
    
    private Rigidbody2D _rigidbody2d;   

    private void Start()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();

    }

    void Update()
    {
        
    }

    private void OnRotacionar(InputValue direcao)
    {
        _rigidbody2d.AddForce(transform.right * VelocidadeAvanco * direcao.Get<float>());

    }

    public void OnAvancar()
    {
        _rigidbody2d.AddForce(transform.up * VelocidadeAvanco);
    }
}

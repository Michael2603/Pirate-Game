using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : BoatController
{
    private float _inputX;
    private float _inputY;

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
            transform.rotation,
            this.transform);

        tempCannonBall.GetComponent<Rigidbody2D>().AddForce(-transform.up * base._cannonFireStrength);
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
                base.CannonBall,
                transform.localPosition + transform.rotation.eulerAngles + Vector3.one * shotPosition,
                transform.rotation,
                this.transform));

            tempCannonBalls[i].GetComponent<Rigidbody2D>().AddForce(-transform.right * base._cannonFireStrength);
        }
    }
}
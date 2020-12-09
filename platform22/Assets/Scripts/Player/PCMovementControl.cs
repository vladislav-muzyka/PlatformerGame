using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Movementcontroller))]
public class PCMovementControl : MonoBehaviour
{
    DateTime StrikeChargeTime;
    Movementcontroller movementcontroller;
    float move;
    bool jump;
    bool isJumping;
    bool crouching;
    float holdTime;


    void Start()
    {
        movementcontroller = GetComponent<Movementcontroller>();
    }

    void Update()
    {

        move = Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonDown("Jump"))
        {
            jump = true;

        }
        if (Input.GetKey(KeyCode.Space))
        {
            isJumping = true;
        }
        if (Input.GetButtonUp("Jump"))
        {
            isJumping = false;
        }
        if (Input.GetKey(KeyCode.E))
        {
            movementcontroller.StartFireThrowCasting();
        }

        if (Input.GetButtonDown("Fire1"))
        {
            StrikeChargeTime = DateTime.Now;
        }
        if (Input.GetButton("Fire1"))
        {
            holdTime = (float)(DateTime.Now - StrikeChargeTime).TotalSeconds;
            movementcontroller.StartMeleeAttacking(holdTime);
        }
        if (Input.GetButtonUp("Fire1"))
        {
            movementcontroller.ReleaseChargingStrike(holdTime);
        }

        crouching = Input.GetKey(KeyCode.C);
    }
    void FixedUpdate()
    {
        movementcontroller.Move(move, jump, isJumping, crouching);
        jump = false;
    }

}

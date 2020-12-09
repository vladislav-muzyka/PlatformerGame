using System;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class Movementcontroller : MonoBehaviour
{

    public event Action<bool> OnGetHurt = delegate { };

    [SerializeField] Animator playerAnimation;
    [SerializeField] Rigidbody2D playerRigidbody;
    [SerializeField] PlayerStanceController player;

    [Header("Horizontal movement")]
    [SerializeField] float speed;

    [Header("Jumping")]
    [SerializeField] float jumpPower;
    [SerializeField] bool Flyconrtoll;
    [SerializeField] float radius;
    [SerializeField] Transform GroundCheck;

    [Header("MultipleJumping")]
    [SerializeField] int ExtraJumps;
    int CurrentExtraJumps;

    [Header("Planing")]
    [SerializeField] int PlaningGravity;
    [SerializeField] int NormalGravity;

    [Header("Crouching")]
    [SerializeField] Transform CoverCheck;
    [SerializeField] LayerMask WhatIsGround;
    [SerializeField] Collider2D headCollider;
    [SerializeField] float crouchSpeed;

    [Header("Casting")]
    bool isCasting;
    [SerializeField] GameObject fireBall;
    [SerializeField] int castCost;
    [SerializeField] Transform firePoint;
    [SerializeField] float fireBallSpeed;

    [Header("MeleeAttack")]
    [SerializeField] Transform strikePoint;
    [SerializeField] float MeleeAtackRange;
    [SerializeField] int basicDamage;
    [SerializeField] LayerMask enemies;
    bool isStriking;

    [Header("PowerStrike")]
    [SerializeField] int chargedStrikeCost;
    [SerializeField] int chargedDamage;
    [SerializeField] float chargeTime;
    [SerializeField] float speedWhileMoving;
    int finiteDamage;


    [SerializeField] float pushForce;
    private float lastHurtTime;
    ///delete next  one
    [SerializeField] float CHARGETIME;

    bool slowDownAtk;
    bool grounded;
    bool canStand;
    bool canMove = true;
    bool faceRight;

    private void Start()
    {
        playerRigidbody.GetComponent<Rigidbody2D>();
        playerAnimation.GetComponent<Animator>();
        player.GetComponent<PlayerStanceController>();
    }

    private void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircle(GroundCheck.position, radius, WhatIsGround);
        if (playerAnimation.GetBool("TakeHit") && grounded && Time.time - lastHurtTime > 0.5f)
        {
            EndHurt();
        }
    }

    private void EndAllAnimation()
    {
        playerAnimation.SetBool("Casting", false);
        playerAnimation.SetBool("Strike", false);
        playerAnimation.SetBool("PowerStrike", false);
        playerAnimation.SetBool("ChargingAttack", false);

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(GroundCheck.position, radius);
        Gizmos.DrawWireSphere(CoverCheck.position, radius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(strikePoint.position, MeleeAtackRange);
    }

    void Turn()
    {
        faceRight = !faceRight;
        transform.Rotate(0, 180, 0);
    }

    public void Move(float move, bool jump, bool isJumping, bool crouching)
    {
        if (!canMove)
        {
            return;
        }

        //turn around 
        if (faceRight && move > 0)
        {
            Turn();
        }
        else if (!faceRight && move < 0)
        {
            Turn();
        }
        //move while jumping   
        if (move != 0 && (grounded || Flyconrtoll))
        {
            playerRigidbody.velocity = new Vector2(speed * move, playerRigidbody.velocity.y);
        }
        //single jump 

        if (jump && grounded)
        {
            playerRigidbody.gravityScale = NormalGravity;
            playerRigidbody.AddForce(Vector2.up * jumpPower);
            CurrentExtraJumps = ExtraJumps;
        }
        else if (jump && isJumping)
        {
            playerRigidbody.gravityScale = PlaningGravity;
        }
        if (!isJumping)
        {
            playerRigidbody.gravityScale = NormalGravity;
        }


        //else if (jump && CurrentExtraJumps > 0)
        //{
        //    playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, 0);
        //    playerRigidbody.AddForce(Vector2.up * jumpPower);
        //    CurrentExtraJumps--;
        //}

        //collider turn off while crouching 
        canStand = !Physics2D.OverlapCircle(CoverCheck.position, radius, WhatIsGround);
        if (crouching)
        {
            if (grounded)
            {
                playerRigidbody.velocity = new Vector2(crouchSpeed * move, playerRigidbody.velocity.y);
            }
            headCollider.enabled = false;
        }
        else if (!crouching && canStand)
        {
            headCollider.enabled = true;
        }

        playerAnimation.SetFloat("Speed", Mathf.Abs(move));
        playerAnimation.SetBool("Jump", !grounded);
        playerAnimation.SetBool("Crouch", !headCollider.enabled);

        //speed while attacking
        if (slowDownAtk)
        {
            playerRigidbody.velocity = new Vector2(speedWhileMoving * move, playerRigidbody.velocity.y);
        }


    }

    public void StartFireThrowCasting()
    {
        if (isCasting || !player.ChangeMP(-castCost))
        {
            return;
        }
        isCasting = true;
        playerAnimation.SetBool("Casting", true);
    }
    private void FireCasting()
    {
        GameObject fireBall = Instantiate(this.fireBall, firePoint.position, Quaternion.Euler(0, 0, 90));
        fireBall.GetComponent<Rigidbody2D>().velocity = transform.right * fireBallSpeed;
        fireBall.GetComponent<SpriteRenderer>().flipY = faceRight;
        fireBall.GetComponent<SpriteRenderer>().flipX = !faceRight;
        Destroy(fireBall, 3f);
    }
    private void EndFireThrowCasting()
    {
        isCasting = false;
        playerAnimation.SetBool("Casting", false);
    }

    public void StartMeleeAttacking(float holdTime)
    {
        playerAnimation.SetBool("ChargingAttack", false);
        if (holdTime >= chargeTime / 2)
        {
            playerAnimation.SetBool("PowerStrike", true);
            slowDownAtk = true;
        }
        CHARGETIME = holdTime;
    }

    public void ReleaseChargingStrike(float holdTime)
    {
        if (isStriking)
        {
            return;
        }

        slowDownAtk = false;
        playerAnimation.SetBool("PowerStrike", false);
        if (holdTime < chargeTime)
        {
            finiteDamage = basicDamage;
            playerAnimation.SetBool("Striking", true);
        }
        else if (!player.ChangeMP(-chargedStrikeCost))
        {
            playerAnimation.SetBool("PowerStrike", false);
            return;
        }
        else
        {
            finiteDamage = chargedDamage;
            playerAnimation.SetBool("ChargingAttack", true);
        }
        isStriking = true;
    }


    private void MeleeAttacking()
    {
        Collider2D[] enemiesArr = Physics2D.OverlapCircleAll(strikePoint.position, MeleeAtackRange, enemies);
        for (int i = 0; i < enemiesArr.Length; i++)
        {
            EnemyStanceController enemy = enemiesArr[i].GetComponent<EnemyStanceController>();
            if (enemy != null)
            {
                enemy.ChangeHP(finiteDamage);
            }
        }
    }
    private void EndMeleeAttack()
    {
        isStriking = false;
        playerAnimation.SetBool("Striking", false);
        playerAnimation.SetBool("ChargingAttack", false);
        slowDownAtk = false;
    }

    public void GetHurt(Vector2 enemyPosition)
    {
        lastHurtTime = Time.time;
        canMove = false;
        OnGetHurt(false);
        Vector2 pushDirection = new Vector2();
        pushDirection.x = enemyPosition.x > transform.position.x ? -1 : 1;
        pushDirection.y = 1;
        playerAnimation.SetBool("TakeHit", true);
        EndAllAnimation();
        playerRigidbody.AddForce(pushDirection * pushForce, ForceMode2D.Impulse);
    }

    private void EndHurt()
    {
        canMove = true;
        playerAnimation.SetBool("TakeHit", false);
        OnGetHurt(true);
    }




}

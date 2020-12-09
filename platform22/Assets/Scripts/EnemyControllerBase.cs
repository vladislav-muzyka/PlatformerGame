using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class EnemyControllerBase : MonoBehaviour
{

    protected Animator enemyAnimation;
    protected Rigidbody2D enemyRigidbody;
    protected Vector2 startPoint;
    protected EnemyState currentState;

    protected float lastStateChange;
    protected float timeToNextChange;
    [SerializeField] float maxStateTime;
    [SerializeField] float minStateTime;
    [SerializeField] EnemyState[] aviableState;
    [SerializeField] DamageType collisionDamageType;
    [SerializeField] protected int collisionDamage;
    [SerializeField] protected float collisionTimeDelay;
    private float lastDamageTime;

    [Header("Movement")]
    [SerializeField] float speed;
    [SerializeField] float range;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask whatIsGround;
    protected bool faceRight = true;




    protected virtual void Start()
    {
        startPoint = transform.position;
        enemyRigidbody = GetComponent<Rigidbody2D>();
        enemyAnimation = GetComponent<Animator>();
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        TryToDamage(collision.collider);
    }
    protected virtual void TryToDamage(Collider2D enemy)
    {
        if (Time.time - lastDamageTime < collisionTimeDelay)
        {
            return;
        }
        PlayerStanceController player = enemy.GetComponent<PlayerStanceController>();
        if (player != null)
        {
            player.TakeDamage(collisionDamage, collisionDamageType, transform);
        }
    }



    protected virtual void FixedUpdate()
    {
        if (IsGroundEnding())
        {
            Flip();
        }
        if (currentState == EnemyState.Move)
        {
            Move();
        }

    }

    protected virtual void Update()
    {
        if (Time.time - lastStateChange > timeToNextChange)
        {
            GetRandomState();
        }
    }

    protected virtual void Move()
    {
        enemyRigidbody.velocity = transform.right * new Vector2(speed, enemyRigidbody.velocity.y);
    }

    private bool IsGroundEnding()
    {
        return !Physics2D.OverlapPoint(groundCheck.position, whatIsGround);
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(range * 2, 0.5f, 0));
    }

    protected void Flip()
    {
        faceRight = !faceRight;
        transform.Rotate(0, -180, 0);
    }


    protected void GetRandomState()
    {
        int state = Random.Range(0, aviableState.Length);

        if (currentState == EnemyState.Idle && aviableState[state] == EnemyState.Idle)
        {
            GetRandomState();
        }
        timeToNextChange = Random.Range(minStateTime, maxStateTime);
        ChangeState(aviableState[state]);
    }


    protected virtual void ChangeState(EnemyState state)
    {
        if (currentState != EnemyState.Idle)
        {
            enemyAnimation.SetBool(currentState.ToString(), false);
        }
        if (state != EnemyState.Idle)
        {
            enemyAnimation.SetBool(state.ToString(), true);
        }
        currentState = state;
        lastStateChange = Time.time;

    }


}

public enum EnemyState
{
    Idle, Move, Shoot, Strike, PowerStrike
}
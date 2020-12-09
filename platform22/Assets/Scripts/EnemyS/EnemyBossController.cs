using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossController : EnemyPlantController
{
    [SerializeField] float idleTime;

    private bool inAtackRange;
    protected bool fightStarted;

    [Header("Strike")]
    [SerializeField] Transform strikePoint;
    [SerializeField] int damage;
    [SerializeField] float strikeRange;
    [SerializeField] LayerMask enemies;

    [Header("PowerStrike")]
    [SerializeField] Collider2D strikeCollaider;
    [SerializeField] int powerStrikeDamage;
    [SerializeField] float powerStrikeRange;
    [SerializeField] float powerStrikeSpeed;

    float currentStrikeRange;

    [SerializeField] float waitForTime;
    private EnemyState[] attackStates = { EnemyState.Strike, EnemyState.PowerStrike, EnemyState.Shoot };
    private EnemyState stateOnHold;


    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (attacking && currentState == EnemyState.Move)
        {
            TurnToPlayer();
            if (CanAttack())
            {
                ChangeState(stateOnHold);
            }
        }
    }

    protected override void TryToDamage(Collider2D enemy)
    { 
        if (currentState == EnemyState.Idle || currentState == EnemyState.Shoot)
        {
            return;
        }
        base.TryToDamage(enemy);
       
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(strikePoint.position, new Vector3(strikeRange, strikeRange, 0));
    }

    protected void Strike()
    {
        Collider2D player = Physics2D.OverlapBox(strikePoint.position, new Vector2(strikeRange, strikeRange), 0, enemies);
        if (player != null)
        {
            PlayerStanceController playerStance = player.GetComponent<PlayerStanceController>();
            if (playerStance != null)
            {
                playerStance.TakeDamage(damage);
            }
        }
    }


    protected void StrikeWithPower()
    {
        strikeCollaider.enabled = true;
        enemyRigidbody.velocity = transform.right * powerStrikeSpeed;
    }

    protected void EndPowerStrike()
    {
        strikeCollaider.enabled = false;
        enemyRigidbody.velocity = Vector2.zero;
    }


    protected override void CheckPlayerInRange()
    {
        if (player == null)
        {
            return;
        }
        if (Vector2.Distance(transform.position, player.transform.position) < angerRange)
        {
            isAngry = true;
            if (!fightStarted)
            {  
                StopCoroutine(ScanForPlayer());
                StartCoroutine(BeginNewCircle());
            }
        }
        else
        {
            isAngry = false;
        }
    }

    protected override void ChangeState(EnemyState state)
    {
        if (currentState == state)
        {
            return;
        }
        if (state == EnemyState.PowerStrike || state == EnemyState.Strike)
        {
            attacking = true;
            currentStrikeRange = state == EnemyState.Strike ? strikeRange : powerStrikeRange;
            enemyRigidbody.velocity = Vector2.zero;
            if (!CanAttack())
            {
                stateOnHold = state;
                state = EnemyState.Move;
            }
        }
        base.ChangeState(state);
    }

    private bool CanAttack()
    {
        return Vector2.Distance(transform.position, player.transform.position) < currentStrikeRange;

    }

    protected override void DoStateAction()
    {
        base.DoStateAction();
        switch (currentState)
        {
            case EnemyState.Strike:
                Strike();
                break;
            case EnemyState.PowerStrike:
                StrikeWithPower();
                break;

        }
    }

    protected override void EndState()
    {
        base.EndState();
        if (currentState == EnemyState.PowerStrike)
        {
            EndPowerStrike();
        }
        attacking = false;
        StartCoroutine(BeginNewCircle());
    }

    protected IEnumerator BeginNewCircle()
    {
        if (fightStarted)
        {
            ChangeState(EnemyState.Idle);
            CheckPlayerInRange();
            if (!isAngry)
            {
                fightStarted = false;
                StartCoroutine(ScanForPlayer());
                yield break;
            }
            yield return new WaitForSeconds(waitForTime);
        }
        fightStarted = true;
        TurnToPlayer();
        ChooseNextAttackState();
    }
    
    protected void ChooseNextAttackState()
    {
        int state = Random.Range(0, attackStates.Length);
        ChangeState(attackStates[state]);
    }












}

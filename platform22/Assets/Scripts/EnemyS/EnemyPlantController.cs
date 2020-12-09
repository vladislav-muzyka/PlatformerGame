using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlantController : OpposumController
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform shootPoint;
    [SerializeField] float seedSpeed;
    [SerializeField] protected float angerRange;

    protected bool isAngry;
    protected bool attacking;
    protected PlayerStanceController player;


    protected override void Start()
    {
        base.Start();
        player = FindObjectOfType<PlayerStanceController>();
        StartCoroutine(ScanForPlayer());
    }


    protected override void Update()
    {
        if (isAngry)
        {
            return;
        }
        base.Update();
    }

    protected void Shoot()
    {
        GameObject seed = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
        seed.GetComponent<Rigidbody2D>().velocity = transform.right * seedSpeed;
        seed.GetComponent<SpriteRenderer>().flipX = faceRight;
        Destroy(seed, 5f);

    }

    protected IEnumerator ScanForPlayer()
    {
        while (true)
        {
            CheckPlayerInRange();
            yield return new WaitForSeconds(1f);
        }
    }

    protected virtual void CheckPlayerInRange()
    {
        if (player == null || attacking)
        {
            return;
        }
        if (Vector2.Distance(transform.position, player.transform.position) < angerRange)
        {
            isAngry = true;
            TurnToPlayer();
            ChangeState(EnemyState.Shoot);
        }
        else
        {
            isAngry = false;
        }
    }

    protected void TurnToPlayer()
    {
        if (player.transform.position.x - transform.position.x > 0 && !faceRight)
        {
            Flip();
        }
        else if (player.transform.position.x - transform.position.x < 0 && faceRight)
        {
            Flip();
        }
    }

    protected override void ChangeState(EnemyState state)
    {
        base.ChangeState(state);
        switch (state)
        {
            case EnemyState.Shoot:
                attacking = true;
                enemyRigidbody.velocity = Vector2.zero;
                return;

        }
    }

    protected virtual void EndState()
    {
        switch (currentState)
        {
            case EnemyState.Shoot:
                attacking = false;
                return;

        }
        if (!isAngry)
        {
            ChangeState(EnemyState.Idle);
        }


    }

    protected virtual void DoStateAction()
    {
        switch (currentState)
        {
            case EnemyState.Shoot:
                Shoot();
                break;


        }

    }


}

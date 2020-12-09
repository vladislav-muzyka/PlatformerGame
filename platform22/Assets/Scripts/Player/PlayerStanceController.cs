using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStanceController : MonoBehaviour
{

    [SerializeField] Animator playerAnimation;
    Movementcontroller playerMovement;

    private bool canBeDamaged = true;

    [Header("HP")]
    [SerializeField] int MaxHP;
    int currentHP;

    [Header("MP")]
    [SerializeField] int MaxMP;
    int currentMP;



    void Start()
    {
        playerMovement = GetComponent<Movementcontroller>();
        playerMovement.OnGetHurt += OnGetHurt;
        currentHP += MaxHP;
        currentMP = +MaxMP;
    }


   
    public void TakeDamage(int damage, DamageType damageType = DamageType.Casual, Transform enemyLocation = null)
    {
        if (!canBeDamaged)
        {
            return;
        }
        currentHP -= damage;
        if (currentHP <= 0)
        {
            OnDeath();
        }
        switch (damageType)
        {
            case DamageType.PowerStrike:
                playerMovement.GetHurt(enemyLocation.position);
                break;
        }
        Debug.Log("damage value" + damage);
        Debug.Log("current hp" + currentHP);
    }

    private void OnGetHurt(bool _canBeDamaged)
    {
        _canBeDamaged = canBeDamaged;
    }

    public void RestoreHP(int hp)
    {
        currentHP += hp;
        if (currentHP > MaxHP)
        {
            currentHP = MaxHP;
        }
        Debug.Log("heal value" + hp);
        Debug.Log("current hp" + currentHP);

    }

    public bool ChangeMP(int value)
    {
        Debug.Log("mana value" + value);
        if (value < 0 && currentMP < Mathf.Abs(value))
        {
            return false;
        }
        currentMP += value;
        if (currentMP > MaxMP)
        {
            currentMP = MaxMP;
        }
        Debug.Log("current mp" + currentMP);
        return true;

    }

    

    void OnDeath()
    {
        Destroy(gameObject,2);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStanceController : MonoBehaviour
{
    [SerializeField] int hp;

    [SerializeField] Animator enemyAnimation;
    public void ChangeHP(int damage)
    {
        hp -= damage;
        enemyAnimation.SetBool("Hurt",true);
        if (hp<=0)
        {
            OnDeath();
        }
        enemyAnimation.SetBool("Hurt", false);
        Debug.Log("HP left = "+hp);
        Debug.Log("Damage dealt = " + damage);
    }
    

    public void OnDeath()
    {
        Debug.Log("he died ");
        Destroy(gameObject,2f);
    }
}

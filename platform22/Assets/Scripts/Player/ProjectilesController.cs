using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilesController : MonoBehaviour
{
    [SerializeField] int damage;

  
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        EnemyStanceController enemy = collision.GetComponent<EnemyStanceController>();

        if (enemy != null)
        {
            enemy.ChangeHP(damage);
            Destroy(gameObject);
        }
        Destroy(gameObject,2f);
    }
}

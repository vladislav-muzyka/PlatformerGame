using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileControlller : MonoBehaviour
{
    [SerializeField] int damage;


    private void OnTriggerEnter2D(Collider2D collision)
    {

        PlayerStanceController player = collision.GetComponent<PlayerStanceController>();

        if (player != null)
        {
            player.TakeDamage(damage);
            Destroy(gameObject);
        }
        Destroy(gameObject, 2f);
    }
}

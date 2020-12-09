using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] int Damage;
    [SerializeField] float DamageDelay;
    PlayerStanceController player;
    DateTime TicTimer;
    private void OnTriggerEnter2D(Collider2D TriggerPlayerName)
    {
        if ((DateTime.Now - TicTimer).TotalSeconds <= 0.1f)
        {
            return;
        }
        TicTimer = DateTime.Now;
        player = TriggerPlayerName.GetComponent<PlayerStanceController>();
        if (player != null)
        {
          player.TakeDamage(Damage);
        }
    }

    private void OnTriggerExit2D(Collider2D ExitPlayerName)
    {
        if (player == ExitPlayerName.GetComponent<PlayerStanceController>())
        {
            player = null;
        }

    }
    void Update()
    {
        if (player != null && (DateTime.Now - TicTimer).TotalSeconds > DamageDelay)
        {
            player.TakeDamage(Damage);
            TicTimer = DateTime.Now;
        }
    }

}

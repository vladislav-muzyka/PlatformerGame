using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPick : MonoBehaviour
{
    [SerializeField] int heal;

    private void OnTriggerEnter2D(Collider2D PlayerPickedName)
    {
        PlayerPickedName.GetComponent<PlayerStanceController>().RestoreHP(heal);
        Destroy(gameObject);
    }
  
}

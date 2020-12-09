using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObjects : MonoBehaviour
{
    new Rigidbody2D rigidbody;
   
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Player"))
        {
            rigidbody.isKinematic = false;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Equals("Player"))
        {
            Debug.LogWarning("Ouch!");
            Destroy(gameObject,0.5f);
        }
        else
        {
            Destroy(gameObject,1);
        }
    }



}

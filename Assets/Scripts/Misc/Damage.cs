using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class Damage : Health
{
    [SerializeField] float daño;
    Collision collision;


    private void OnCollisionEnter(Collision collision)
    {

    }
    private void OnTriggerEnter(Collider collision)
    {

        Health health;
        if (collision.TryGetComponent(out health))
        {
            health.RecibirDaño(daño);
        }
    }
}

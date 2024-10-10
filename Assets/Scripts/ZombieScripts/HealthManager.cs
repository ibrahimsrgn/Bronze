using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] private int maxHealth;


    private void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health < 0)
        {
            Die();
        }
    }
    private void Die()
    {
        //Düþman ölme animasyonu veya yok olma
        if (TryGetComponent<ZombieAi>(out ZombieAi zombieAi))
        {
            zombieAi.isDying = true;
        }
        Destroy(gameObject,5);
    }

}

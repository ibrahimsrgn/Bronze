using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private int currentHealth;
    [SerializeField] private int maxHealth;


    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
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
        //Player için farklı bir animasyon yapılabilir
        Destroy(gameObject,5);
    }

}

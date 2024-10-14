using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private int currentHealth;
    [SerializeField] private int maxHealth;
    private ZombieAi zombieAi;


    private void Start()
    {
        zombieAi = GetComponent<ZombieAi>();
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
        zombieAi.ragdollEnabler.animator.enabled = false;
        zombieAi.ragdollEnabler.agent.enabled = false;
        zombieAi.ragdollEnabler.EnableRagdoll();
        zombieAi.enabled = false;
        /*/Düþman ölme animasyonu veya yok olma
        if (TryGetComponent<ZombieAi>(out ZombieAi zombieAi))
        {
            zombieAi.isDying = true;
        }
        //Player için farklı bir animasyon yapılabilir*/
    }

}

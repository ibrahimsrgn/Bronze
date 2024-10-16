using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;


public class ZombieAi : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    public Transform player;
    [SerializeField] private LayerMask groundLayer, playerLayer;

    private Vector3 walkPoint;
    private bool walkPointSet;
    [SerializeField] private float walkPointRange;

    [SerializeField] private int attackDamage;
    [SerializeField] private float attackRate;
    private bool alreadyAttacked;

    [SerializeField] private float sightRange, attackRange;
    [SerializeField] private float sightAreaAngle;
    private bool playerInSight, playerInAttackRange;
    public bool isDying;
    [SerializeField] private Animator animator;

    [SerializeField] public RagdollEnabler ragdollEnabler;

    //Wait for attack ends
    private float waitTimerMax = 1f;
    private float waitTimer = 0;
    float fadeOutTimer = 1;
    private State state;
    private enum State
    {
        IdleAndPatrol,
        Chasing,
        Attacking,
        Dead
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    private void Update()
    {

            switch (state)
            {
                case State.IdleAndPatrol:
                    Patroling();
                    IsPlayerInSight();
                    break;
                case State.Chasing:
                    ChasePlayer();
                    IsPlayerAttackRange();
                    break;
                case State.Attacking:
                    AttackPlayer();
                    IsPlayerAttackRange();
                    break;
                case State.Dead: break;
            }
            Animate();

    }
    private void Animate()
    {
        animator.SetFloat("MoveSpeed", agent.velocity.magnitude);
    }
    private IEnumerator FadeOutCorpse()
    {
        yield return new WaitForSeconds(fadeOutTimer);
        ragdollEnabler.DisableAllRigidbody();
        while (fadeOutTimer > 0)
        {
            transform.position += Vector3.down * Time.deltaTime;
            fadeOutTimer -= Time.deltaTime;
        }

    }
    private void IsPlayerInSight()
    {
        //Oyuncu görüş alanındamı?
        playerInSight = Physics.CheckSphere(transform.position, sightRange, playerLayer) && Vector3.Angle(transform.forward, (player.position - transform.position).normalized) < sightAreaAngle / 2;
        state = State.Chasing;
    }
    private void IsPlayerAttackRange()
    {
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);
       state= playerInAttackRange ? State.Attacking : State.Chasing;
        animator.SetBool("IsAttacking", playerInAttackRange);
    }
    private void CheckState()
    {
        if (!playerInSight && !playerInAttackRange && !alreadyAttacked)
            state = State.IdleAndPatrol;
        if (playerInSight && !playerInAttackRange && !alreadyAttacked)
            state = State.Chasing;
        if (playerInSight && playerInAttackRange)
            state = State.Attacking;
        else
            animator.SetBool("IsAttacking", false);
    }

    private void Patroling()
    {
        Debug.Log("Patroling");
        if (!walkPointSet) SearchForWalkPoint();
        if (walkPointSet) agent.SetDestination(walkPoint);
        float distanceToWalkPoint = Vector3.Distance(transform.position, walkPoint);
        //Walk point reached
        if (distanceToWalkPoint < 1f) Invoke(nameof(ResetWalkPoint), 5);
    }
    private void ResetWalkPoint()
    {
        walkPointSet = false;
    }
    private void SearchForWalkPoint()
    {

        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 5f, groundLayer))
            walkPointSet = true;

    }
    private void ChasePlayer()
    {

        Debug.Log("Chasing");
        agent.SetDestination(player.position);

    }
    private void AttackPlayer()
    {
        Debug.Log("Attacking");
        waitTimer = waitTimerMax;
        agent.SetDestination(transform.position);
        Vector3 playerPos = player.position;
        playerPos.y = transform.position.y;
        transform.LookAt(playerPos);


        //oyuncu kaybolursa son lokasyonuna git
        walkPoint = player.position;
        walkPointSet = true;

        if (!alreadyAttacked)
        {
            animator.SetBool("IsAttacking", true);
            alreadyAttacked = true;
            //Invoke(nameof(ResetAttack), attackRate);
        }
    }
    /// <summary>
    /// Saldırı animasyonu ile tetikleniyor
    /// Animasyonda saldırının tam temas etme kısmını bekliyor
    /// </summary>
    private void Attack()
    {
        if (Vector3.Distance(transform.position, player.position) < 2f)
            player.gameObject.GetComponent<HealthManager>().TakeDamage(attackDamage);

    }
    private void ResetAttack()
    {
        Debug.Log("1");
        alreadyAttacked = false;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}

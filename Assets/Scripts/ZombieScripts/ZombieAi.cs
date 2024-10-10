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

    [SerializeField] private float attackRate;
    private bool alreadyAttacked;

    [SerializeField] private float sightRange, attackRange;
    [SerializeField] private float sightAreaAngle;
    private bool playerInSight, playerInAttackRange;

    [SerializeField] private Animator animator;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    private void Update()
    {
        CheckState();
        Animate();
    }
    private void Animate()
    {
        animator.SetFloat("MoveSpeed", agent.velocity.magnitude);
    }
    private void CheckState()
    {
        if (Physics.CheckSphere(transform.position, sightRange, playerLayer))
        {
            float angleToPlayer = Vector3.Angle(transform.position, player.position);
            if (angleToPlayer < sightAreaAngle / 2)
                playerInSight = true;
        }
        else playerInSight = false;
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);

        if (!playerInSight && !playerInAttackRange)
            Patroling();
        if (playerInSight && !playerInAttackRange)
            ChasePlayer();
        if (playerInSight && playerInAttackRange)
            AttackPlayer();
        else
            animator.SetBool("IsAttacking", false);

    }
    private void Patroling()
    {
        if (!walkPointSet) SearchForWalkPoint();
        if (walkPointSet) agent.SetDestination(walkPoint);
        Debug.Log("Patrolling");
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
        Debug.Log("Looking For Walk Point");

        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 5f, groundLayer))
            walkPointSet = true;

    }
    private void ChasePlayer()
    {
        Debug.Log("Chasing The Player");
        
        agent.SetDestination(player.position);

    }
    private void AttackPlayer()
    {
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
        Debug.Log("Attacking The Player");
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), attackRate);
        }
    }
    private void ResetAttack()
    {
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

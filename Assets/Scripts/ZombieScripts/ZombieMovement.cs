using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics.Internal;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class ZombieMovement : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator animator;
    [SerializeField] private EnemyState state;
    [SerializeField] private Transform target;
    [SerializeField] private float fieldOfView = 65;
    [SerializeField] private float lineOfSightDistance = 7f;
    [SerializeField] private float idleSpeedModifier = 0.25f;
    [SerializeField] private int walkPointRange;
    private float initialSpeed;
    private Vector3 targetLocation;


    public static NavMeshTriangulation triangulation;
    public enum EnemyState
    {
        Initial,
        Idle,
        Chasing,
        Attacking,
        Dead
    }
    private void Awake()
    {
        initialSpeed = agent.speed;
        if (triangulation.vertices == null || triangulation.vertices.Length == 0)
        {
            triangulation = NavMesh.CalculateTriangulation();
        }
    }
    private void GetAggressive()
    {
        if (state == EnemyState.Idle || state == EnemyState.Initial)
        {
            Debug.Log("Aggressive");

            state = EnemyState.Chasing;
            agent.speed = initialSpeed;

        }
    }
    private void Update()
    {
        Animate();
        if (agent.enabled)
        {
            switch (state)
            {
                case EnemyState.Initial:
                    state = EnemyState.Idle;
                    break;
                case EnemyState.Idle:
                    DoIdleMovement();
                    break;
                case EnemyState.Chasing:
                    DoTargetMovement();
                    break;
                case EnemyState.Attacking:
                    DoAttack();
                    break;
                case EnemyState.Dead:
                    break;
            }
        }
    }
    private void Animate()
    {
        animator.SetFloat("MoveSpeed", agent.velocity.magnitude);
    }
    private void DoIdleMovement()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        float fovRadians = Mathf.Cos(fieldOfView * Mathf.Deg2Rad);
        if (Vector3.Distance(transform.position, target.position) <= lineOfSightDistance && Vector3.Dot(transform.forward, direction) >= fovRadians)
        {
            Debug.Log("Target Found");
            GetAggressive();
        }
        else
        {
            agent.speed = initialSpeed * idleSpeedModifier;
            if (Vector3.Distance(transform.position, targetLocation) <= agent.stoppingDistance || targetLocation == Vector3.zero)
            {
                Vector3 triangle1 = triangulation.vertices[Random.Range(0, triangulation.vertices.Length)];
                Vector3 triangle2 = triangulation.vertices[Random.Range(0, triangulation.vertices.Length)];

                targetLocation = Vector3.Lerp(triangle1, triangle2, Random.value);
                agent.SetDestination(targetLocation);
            }
        }
    }
    private void DoTargetMovement()
    {
        if (Vector3.Distance(transform.position, target.position) > (agent.stoppingDistance + agent.radius) * 2)
        {
            Debug.Log("Chasing");
            agent.SetDestination(target.position);
        }
        else
        {
            state = EnemyState.Attacking;
        }
    }
    private void DoAttack()
    {
        if (Vector3.Distance(transform.position, target.position) > (agent.stoppingDistance + agent.radius) * 2)
        {
            Debug.Log("Attacking");

            animator.SetBool("IsAttacking", false);
            state = EnemyState.Chasing;
        }
        else
        {
            Quaternion lookRotation = Quaternion.LookRotation((target.position - transform.position).normalized);
            transform.rotation = Quaternion.Euler(0, lookRotation.eulerAngles.y, 0);
            animator.SetBool("IsAttacking", true);
        }
    }
}

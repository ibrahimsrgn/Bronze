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

    [SerializeField] private SpawnLoot spawnLoot;
    private GameObject loot;
    private bool GUIActivater = false;

    public ZombieSpawner zombieSpawner;

    //Wait for attack ends
    private float waitTimerMax = 1f;
    private float waitTimer = 0;
    float fadeOutTimer = 1;
    public State state;
    public enum State
    {
        IdleAndPatrol,
        Chasing,
        Attacking,
        Dead
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        ragdollEnabler = GetComponent<RagdollEnabler>();
    }

    private void Start()
    {
        //Checks if its on navmesh
        if (!agent.isOnNavMesh)
        {
            zombieSpawner.zombieCount--;
            Destroy(gameObject);
        }
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
                if (IsPlayerAttackRange())
                {
                    state = State.Attacking;
                }
                else
                {
                    ChasePlayer();
                }
                break;
            case State.Attacking:
                if (IsPlayerAttackRange())
                {
                    AttackPlayer();
                }
                else
                {
                    state = alreadyAttacked ? State.Attacking : State.Chasing;

                }
                break;
            case State.Dead:
                break;
        }
        Animate();

    }
    private void Animate()
    {
        animator.SetFloat("MoveSpeed", agent.velocity.magnitude);
    }
    private void IsPlayerInSight()
    {
        //Oyuncu görüş alanındamı?
        playerInSight = Physics.CheckSphere(transform.position, sightRange, playerLayer) && Vector3.Angle(transform.forward, (player.position - transform.position).normalized) < sightAreaAngle / 2;
        if (playerInSight)
        {
            state = State.Chasing;
        }
    }
    private bool IsPlayerAttackRange()
    {
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);
        state = playerInAttackRange ? State.Attacking : State.Chasing;
        return playerInAttackRange;
    }
    public void Dead()
    {
        loot = spawnLoot.SpawnLootBox();
        zombieSpawner.zombieCount--;
    }

    private void Patroling()
    {
        //Debug.Log("Patroling");
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

        //Debug.Log("Chasing");
        agent.SetDestination(player.position);

    }
    private void AttackPlayer()
    {
        // Debug.Log("Attacking");
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
    //Animasyon ile tetikleniyor
    private void ResetAttack()
    {
        alreadyAttacked = false;
        animator.SetBool("IsAttacking", false);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
    private void OnDestroy()
    {
        Destroy(loot);
    }

    public void OnRayHit(PlayerData playerData)
    {
        if (state != State.Dead) return;
        Debug.Log("1");
        if (playerData != null)
        {
            GUIActivater = true;
            if (Input.GetKeyDown(KeyCode.E))
            {
                UIManager.instance.UIListManager(loot);
            }
        }
    }

    void OnGUI()
    {
        if (GUIActivater)
        {
            GUI.Label(new Rect(Screen.width / 2 - 75, Screen.height - 100, 155, 60), "Press 'E' to loot body");
        }
    }
    private void OnMouseExit()
    {
        GUIActivater = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float distanceToPlayer;
    [SerializeField] private float minDistanceToPlayer;
    [SerializeField] private float viewDistance = 10;
    [SerializeField] private float viewAngle;
    [SerializeField] private Transform LookDir;

    private float currentSpeed = 0;
    public float moveSpeed = 5f;
    public float normalMoveSpeed = 5f;
    private float accelerationSpeed = 2f;

    [Header("Player Detection")]
    private bool playerDetected = false;
    private Vector3 playerLastKnownPosition;
    private bool movingToLastKnownPosition = false;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private LayerMask obstacleMask;


    private void Start()
    {
        moveSpeed = normalMoveSpeed;//Ýleride koþu hýzýný anlýk veya duruma göre yavaþlatmak gerektiðinde move speed i deðiþtir durum bitince bu satýrý tekrarla
    }
    private void Update()
    {
        playerDetected = DetectPlayer();
        if (playerDetected)
        {
            playerLastKnownPosition = player.transform.position;
            RunToPlayer();
            movingToLastKnownPosition = false;
        }
        else
        {
            if (!movingToLastKnownPosition)
            {
                movingToLastKnownPosition = true;
            }
            GoToPlayersLastKnownPlace();
        }
    }
    private bool DetectPlayer()
    {
        Vector3 dirToPlayer = player.transform.position - LookDir.position;
        float angleBetweenEnemyAndPlayer = Vector3.Angle(LookDir.forward, dirToPlayer);
        if (angleBetweenEnemyAndPlayer < viewAngle / 2)
        {
            Ray zombieLineOfSight = new Ray(LookDir.position, dirToPlayer);
            if (Physics.Raycast(zombieLineOfSight, out RaycastHit hitInfo, viewDistance))
            {
                if (hitInfo.collider != null && hitInfo.collider.CompareTag("Player"))
                {
                    return true;
                }
            }
        }
        return false;
    }
    private void RunToPlayer()
    {
        if (GetDistanceToPlayer() < viewDistance && GetDistanceToPlayer() > minDistanceToPlayer)
        {
            //Hýzý yavaþca arttýr
            currentSpeed = Mathf.Lerp(currentSpeed, moveSpeed, Time.deltaTime * accelerationSpeed);


            Vector3 dirToPlayer = player.transform.position - transform.position;
            dirToPlayer.y = 0;
            transform.position += dirToPlayer.normalized * currentSpeed * Time.deltaTime;

            Vector3 playerPos = player.transform.position;
            playerPos.y = 0;
            transform.LookAt(playerPos);
        }
    }
    private void GoToPlayersLastKnownPlace()
    {
        if (Vector3.Distance(transform.position, playerLastKnownPosition) > minDistanceToPlayer)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, moveSpeed, Time.deltaTime * accelerationSpeed);

            Vector3 dirToLastKnownPos = playerLastKnownPosition - transform.position;
            dirToLastKnownPos.y = 0;
            transform.position += dirToLastKnownPos.normalized * currentSpeed * Time.deltaTime;

            Quaternion targetRotation = Quaternion.LookRotation(dirToLastKnownPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * accelerationSpeed);
        }
        else
        {
            currentSpeed = Mathf.Lerp(currentSpeed, 0, Time.deltaTime * accelerationSpeed);

        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (playerDetected)
            Gizmos.DrawRay(LookDir.position, player.transform.position - LookDir.position);

        Vector3 forwardDir = LookDir.forward;
        forwardDir.y = 0;
        Vector3 leftBoundary = Quaternion.Euler(0, -viewAngle / 2, 0) * forwardDir * viewDistance;
        Vector3 rightBoundary = Quaternion.Euler(0, viewAngle / 2, 0) * forwardDir * viewDistance;

        Gizmos.color = playerDetected ? Color.green : Color.red;
        Gizmos.DrawRay(LookDir.position, leftBoundary);
        Gizmos.DrawRay(LookDir.position, rightBoundary);
    }
    private float GetDistanceToPlayer()
    {
        distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        return distanceToPlayer;
    }
}

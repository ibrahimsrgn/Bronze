using System.Collections;
using System.Collections.Generic;
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
    private Ray zombieLineOfSight;
    private bool playerDetected=false;
    
    private void Start()
    {
        moveSpeed = normalMoveSpeed;//Ýleride koþu hýzýný anlýk veya duruma göre yavaþlatmak gerektiðinde move speed i deðiþtir durum bitince bu satýrý tekrarla
    }
    private void Update()
    {
       playerDetected= DetectPlayer();
        if(playerDetected)
        {
            RunToPlayer();
        }
        else
        {
            currentSpeed = 0;
        }
    }
    private bool DetectPlayer()
    {
        Vector3 dirToPlayer=player.transform.position-LookDir.position;
        dirToPlayer.y = 0;
        float angleBetweenEnemyAndPlayer =Vector3.Angle(LookDir.forward, dirToPlayer);
        if(angleBetweenEnemyAndPlayer < viewAngle/2)
        {
            zombieLineOfSight = new Ray(LookDir.position, dirToPlayer);
            if (Physics.Raycast(zombieLineOfSight, out RaycastHit hitInfo, viewDistance))
            {
                if (hitInfo.collider != null&&hitInfo.collider.name=="Capsule") 
                {
                    return true;
                }
            }
        }
        return false;




    }
    private void RunToPlayer()
    {
        if (GetDistanceToPlayer()<viewDistance&&GetDistanceToPlayer()>minDistanceToPlayer)
        {
            //Hýzý yavaþca arttýr
            currentSpeed = Mathf.Lerp(currentSpeed, moveSpeed, Time.deltaTime * accelerationSpeed);


            Vector3 dirToPlayer = player.transform.position - transform.position;
            dirToPlayer.y = 0;
            transform.position += dirToPlayer.normalized * currentSpeed * Time.deltaTime;
            Debug.Log(currentSpeed);
            Vector3 playerPos = player.transform.position;
            playerPos.y = 0;
            transform.LookAt(playerPos);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if(DetectPlayer())
        Gizmos.DrawRay(LookDir.position,player.transform.position- LookDir.position );

        Vector3 forwardDir = LookDir.forward;
        forwardDir.y = 0;
        Vector3 leftBoundary=Quaternion.Euler(0,-viewAngle/2,0)* forwardDir * viewDistance;
        Vector3 rightBoundary=Quaternion.Euler(0,viewAngle/2,0)* forwardDir * viewDistance;

        Gizmos.color = Color.red;
        if (DetectPlayer())
        {
            Gizmos.color = Color.green;
        }
        Gizmos.DrawRay(LookDir.position, leftBoundary);
        Gizmos.DrawRay(LookDir.position, rightBoundary);
    }
    private float GetDistanceToPlayer()
    {
        distanceToPlayer= Vector3.Distance(player.transform.position, transform.position);
        return distanceToPlayer;
    }
}

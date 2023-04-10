using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [Header("----- Components -----")]

    [Header("-- Stats --")]
    [SerializeField] int hitPoints;
    [SerializeField] int turnSpeed;
    [SerializeField] int cameraAngle;
    float viewAngle;

    [Header("-- Variables --")]
    Vector3 identVec;
    bool playerInRange;
    float angleToPlayer;
    bool isShooting;
    float stopDistOrig;

    [Header("-- Objects --")]
    [SerializeField] Renderer model;
    [SerializeField] Transform headPos;
    [SerializeField] Transform gunPos;
    [SerializeField] NavMeshAgent navMeshA;


    void Start()
    {
        
    }

    void Update()
    {
        if(playerInRange)
        {
            FindPlayer();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
    void FindPlayer()
    {
        identVec = (gameManager.Instance.PlayerModel.transform.position - headPos.position);
        viewAngle = Vector3.Angle(new Vector3(identVec.x, 0, identVec.z), transform.forward);


        RaycastHit hit;
        if (Physics.Raycast(headPos.position, identVec, out hit))
        {
            if (hit.collider.CompareTag("Player") && viewAngle <= cameraAngle)
            {
                navMeshA.stoppingDistance = stopDistOrig;
                navMeshA.SetDestination(gameManager.Instance.PlayerModel.transform.position);

                if (navMeshA.remainingDistance <= navMeshA.stoppingDistance)
                {
                    FollowPlayer();
                }
            }
        }
    }
    void BlastPlayer()
    {
        
    }

    void FollowPlayer()
    {
        Quaternion enemyRotation = Quaternion.LookRotation(new Vector3(identVec.x, 0, identVec.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, enemyRotation, Time.deltaTime * turnSpeed);
    }

}

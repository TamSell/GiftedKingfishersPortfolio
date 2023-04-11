using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class EnemyMovement : MonoBehaviour , Damage
{
    [Header("----- Components -----")]

    [Header("-- Stats --")]
    [SerializeField] int hitPoints;
    [SerializeField] int turnSpeed;
    [SerializeField] int cameraAngle;
    [SerializeField] int stoppDist;
    float viewAngle;

    [Header("-- Variables --")]
    Vector3 identVec;
    bool playerInRange;
    float angleToPlayer;
    float stopDistOrig;

    [Header("-- Objects --")]
    [SerializeField] Renderer model;
    [SerializeField] Transform headPos;
    [SerializeField] Transform gunPos;
    [SerializeField] NavMeshAgent navMeshA;

    [Header("-- Gun Stats --")]
    [SerializeField] int ShootDamage;
    [SerializeField] float ShootRate;
    [SerializeField] int ShootDist;

    [SerializeField] bool isShooting;

    public GameObject bullet;
    public Transform gun;


    void Start()
    {
        stopDistOrig = stoppDist;
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

                if (!isShooting)
                {

                    StartCoroutine(shoot());
                }
            }
        }
    }

    void FollowPlayer()
    {
        Quaternion enemyRotation = Quaternion.LookRotation(new Vector3(identVec.x, 0, identVec.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, enemyRotation, Time.deltaTime * turnSpeed);
    }

    IEnumerator shoot()
    {
        isShooting = true;

        Instantiate(bullet, gun.position, gun.rotation);
        yield return new WaitForSeconds(ShootRate);
        isShooting = false;
    }

    IEnumerator flashColor()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = Color.white;
    }

    public void TakeDamage(int amountDamage)
    {
        hitPoints -= amountDamage;
        navMeshA.SetDestination(gameManager.Instance.PlayerModel.transform.position);
        navMeshA.stoppingDistance = 0;

      //  StartCoroutine(flashColor());

        if (hitPoints <= 0)
        {
            Destroy(gameObject);
        }
    }
}

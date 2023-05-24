using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.ProBuilder;

public class TestEnemy : MonoBehaviour, Damage
{

    [Header("--- Stats ---")]
    [SerializeField] int HealthPoints;
    [SerializeField] float fireRate;
    [SerializeField] int moveRadius;
    [SerializeField] bool isShooting = false;
    [SerializeField] bool isShimmy = false;
    Vector3 PlayerVel;
    Vector3 lookVector;

    [Header("--- Bodies ---")]
    [SerializeField] GameObject enemyFace;
    [SerializeField] GameObject playerForAI;

    [Header("--- AI ---")]
    [SerializeField] Vector3 playerDirection;
    [SerializeField] float turnSpeed;
    [SerializeField] float distanceToPlayer;
    [SerializeField] NavMeshAgent enemyNavMesh;
    Rigidbody playerBody;

    [Header("--- Components ---")]
    [SerializeField] GameObject enemyBullet;
    [SerializeField] Renderer model;

    void Start()
    {
        playerBody = playerForAI.GetComponent<Rigidbody>();
    }
    void Update()
    {
        lookVector = gameManager.Instance.playerController.PlayerBody.transform.position - enemyFace.transform.position;
        distanceToPlayer = Vector3.Distance(enemyFace.transform.position, playerForAI.GetComponent<Rigidbody>().position);
        //Debug.DrawRay(enemyFace.transform.position, lookVector);
        ActiveIntelligence();
    }


    void ActiveIntelligence()
    {
        FaceThePlayer();
        if (distanceToPlayer > moveRadius)
        {
            MoveTowardPlayer();
        }
        if (distanceToPlayer <= 20 && !isShooting)
        {
            StartCoroutine(ShootNormal());
        }
        if (distanceToPlayer < 4)
        {
            GivePlayerSpace();
        }
    }


    void FaceThePlayer()
    {
        Quaternion rot = Quaternion.LookRotation(lookVector);
        lookVector = rot.eulerAngles;
        lookVector.z = 0;
        lookVector.x = 0;
        rot.eulerAngles = lookVector;
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, 1);
    }

    void MoveTowardPlayer()
    {
        enemyNavMesh.SetDestination(transform.position + transform.forward);
    }

    void GivePlayerSpace()
    {
        enemyNavMesh.SetDestination(transform.position - transform.forward);
    }

    IEnumerator ShootNormal()
    {
        isShooting = true;
        Instantiate(enemyBullet, enemyFace.transform.position, enemyFace.transform.rotation);
        yield return new WaitForSeconds(fireRate);
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
        HealthPoints -= amountDamage;

        if (HealthPoints <= 0)
        {
            Destroy(gameObject);
            gameManager.Instance.updateGoal(50, -1);
        }
        else
        {
            StartCoroutine(flashColor());
        }
    }

}

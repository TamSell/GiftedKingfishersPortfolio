using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class EnemyMovement : MonoBehaviour, Damage
{
    [Header("----- Components -----")]

    [Header("-- Stats --")]
    [SerializeField] int hitPoints;
    [SerializeField] int turnSpeed;
    [SerializeField] int cameraAngle;
    [SerializeField] int stoppDist;
    [SerializeField] Animator animatorRanged;
    float viewAngle;

    [Header("-- Variables --")]
    Vector3 identVec;
    bool playerInRange;
    float angleToPlayer;
    float stopDistOrig;
    float speed;

    [Header("-- Objects --")]
    [SerializeField] Renderer model;
    [SerializeField] Transform headPos;
    [SerializeField] Transform gunPos;
    [SerializeField] NavMeshAgent navMeshA;

    [Header("-- Thrower Stats --")]
    [SerializeField] int ThrowWindup;
    [SerializeField] float ThrowRate;
    [SerializeField] int throwSpeed;
    [SerializeField] bool isThrowing;

    [Header("-- Effects --")]
    [SerializeField] GameObject TriggerEffect;
    bool IsEffecting;
    GameObject effect;

    float DistanceToPlayer;
    public GameObject bullet;
    public Transform gun;
    Vector3 playerDirection;


    void Start()
    {
        gameManager.Instance.updateGoal(1);
        stopDistOrig = stoppDist;
    }

    void Update()
    {
        if (navMeshA.isActiveAndEnabled)
        {
            speed = Mathf.Lerp(speed, navMeshA.velocity.normalized.magnitude, Time.deltaTime * 3);
            animatorRanged.SetFloat("Speed", speed);
            playerDirection = gameManager.Instance.PlayerModel.transform.position;
            if (playerInRange)
            {
                FindPlayer();
            }
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
        DistanceToPlayer = Vector3.Distance(gameManager.Instance.PlayerModel.transform.position, gunPos.position);
        identVec = (gameManager.Instance.PlayerModel.transform.position - headPos.position);
        viewAngle = Vector3.Angle(new Vector3(identVec.x, 0, identVec.z), transform.forward);
        Debug.DrawLine(headPos.position, gameManager.Instance.PlayerModel.transform.position);

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

                if (!isThrowing)
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
        isThrowing = true;
        animatorRanged.SetTrigger("Throw");
        yield return new WaitForSeconds(ThrowWindup);
        /* Old Thrower
        Instantiate(bullet, gun.position, gun.rotation);
        */
        GameObject temp = Instantiate(bullet, gun.position, Quaternion.identity);
        temp.transform.LookAt(playerDirection);
        Rigidbody tempRB = temp.GetComponent<Rigidbody>();
        tempRB.velocity = temp.transform.forward * throwSpeed;
        // tempRB.useGravity= true;

        yield return new WaitForSeconds(ThrowRate);
        isThrowing = false;
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
        StartCoroutine(hitEffect());
        effect = Instantiate(TriggerEffect, transform.position + new Vector3(0, 1.25f, 0), TriggerEffect.transform.rotation);

        Destroy(effect, 2);

        if (hitPoints <= 0)
        {
            StopAllCoroutines();
            gameManager.Instance.updateGoal(-1);
            animatorRanged.SetBool("Dead", true);
            GetComponent<CapsuleCollider>().enabled = false;
            navMeshA.enabled = false;
        }
        else
        {
            Vector3 lower = new Vector3(10.0f, 0.0f, 10.0f);
            animatorRanged.SetTrigger("Hit");
            navMeshA.SetDestination(gameManager.Instance.PlayerModel.transform.position - lower);
            navMeshA.stoppingDistance = 0;

            StartCoroutine(flashColor());
        }
    }
    IEnumerator hitEffect()
    {
        IsEffecting = true;
        TriggerEffect.SetActive(true);

        yield return new WaitForSeconds(1.0f);

        TriggerEffect.SetActive(false);

        // yield return new WaitForSeconds(0.5f);

        IsEffecting = false;

    }
}

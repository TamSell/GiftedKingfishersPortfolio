using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySwarmer : MonoBehaviour, Damage
{

    [Header("----- Top of Enemy -----")]


    [Header("--- Stats ---")]
    [SerializeField] int healthPoints;
    [SerializeField] float meleeRate;
    [SerializeField] float meleeWindUp;
    [SerializeField] float meleeSpeed;
    [SerializeField] float movementSpeed;
    [SerializeField] bool isMeleeing;

    [Header("--- AI Info ---")]
    [SerializeField] int turnSpeed;
    [SerializeField] Transform playerFinder;
    Vector3 dirOfPlayer;
    Vector3 playerShooter;
    bool playerInRange;
    float viewAngle;
    float distanceToPlayer;
    float fDistance;
    float fBaseCheckTime = 0.15f;
    float fTimePercheck = 0.05f;
    int iMaxIters = 100;
    private Tracker objectTracker;

    [Header("--- Components ---")]
    [SerializeField] GameObject playerDetector;
    [SerializeField] NavMeshAgent navMeshA;
    [SerializeField] Renderer model;
    [SerializeField] GameObject meleeSwipe;

    [Header("--- Effects ---")]
    [SerializeField] GameObject effect;

    void Start()
    {
        meleeSwipe.SetActive(false);
        navMeshA.stoppingDistance = 4;
        objectTracker = GetComponent<Tracker>();
        gameManager.Instance.updateGoal(0, 1);
    }


    void Update()
    {
        FindPlayer();
    }

    void FindPlayer()
    {
        dirOfPlayer = (gameManager.Instance.PlayerModel.transform.position - playerFinder.position);
        viewAngle = Vector3.Angle(new Vector3(dirOfPlayer.x, 0, dirOfPlayer.z), playerFinder.forward);
        distanceToPlayer = Vector3.Distance(transform.position, gameManager.Instance.PlayerModel.transform.position);
        Debug.DrawLine(playerFinder.position, gameManager.Instance.PlayerModel.transform.position);

        FollowPlayer();
        if (distanceToPlayer > 12)
        {
            PredictiveCutOff();
        }
        if (distanceToPlayer < 10)
        {
            //navMeshA.ResetPath();
            navMeshA.SetDestination(gameManager.Instance.PlayerModel.transform.position);
        }
        if (distanceToPlayer < 3 && !isMeleeing)
        {
            StartCoroutine(melee());
        }
    }

    void PredictiveCutOff()
    {
       // navMeshA.ResetPath();
        int iIterations = 0;
        float fCheckTime = fBaseCheckTime;
        Vector3 TargetPosition = objectTracker.ProjectedPosition(fBaseCheckTime);

        Vector3 ProjectidePosition = transform.position + ((TargetPosition - transform.position).normalized * movementSpeed * fCheckTime);
        fDistance = (TargetPosition - ProjectidePosition).magnitude;

        while (fDistance > 5f && iIterations < iMaxIters)
        {
            iIterations++;
            fCheckTime += fTimePercheck;
            TargetPosition = objectTracker.ProjectedPosition(fCheckTime);

            ProjectidePosition = transform.position + ((TargetPosition - transform.position).normalized * movementSpeed * fCheckTime);
            fDistance = (TargetPosition - ProjectidePosition).magnitude;
        }

        navMeshA.SetDestination(ProjectidePosition);
    }

    IEnumerator melee()
    {
        isMeleeing = true;
       // navMeshA.speed = 0;
        yield return new WaitForSeconds(meleeWindUp);
        meleeSwipe.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        meleeSwipe.SetActive(false);
        navMeshA.speed = movementSpeed;
        yield return new WaitForSeconds(meleeRate);
        isMeleeing = false;
    }

    void FollowPlayer()
    {
        Quaternion enemyRotation = Quaternion.LookRotation(new Vector3(dirOfPlayer.x, dirOfPlayer.y, dirOfPlayer.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, enemyRotation, Time.deltaTime * turnSpeed);
    }

    IEnumerator flashColor()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = Color.white;
    }

    public void TakeDamage(int amountDamage)
    {
        healthPoints -= amountDamage;
        //StartCoroutine(hitEffect());
        //effect = Instantiate(TriggerEffect, transform.position + new Vector3(0, 1.25f, 0), TriggerEffect.transform.rotation);

       // Destroy(effect, 2);

        if (healthPoints <= 0)
        {
            Destroy(gameObject);
            gameManager.Instance.updateGoal(20,-1);
        }
        else
        {
            //navMeshA.SetDestination(gameManager.Instance.PlayerModel.transform.position);
            //navMeshA.stoppingDistance = 0;
            StartCoroutine(flashColor());
        }

    }
}

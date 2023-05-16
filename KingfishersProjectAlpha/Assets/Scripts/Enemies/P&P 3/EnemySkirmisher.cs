using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.AI;

public class EnemySkirmisher : MonoBehaviour, Damage
{


    [Header("----- Top of Enemy -----")]
    [SerializeField] GameObject[] ItemToDrop;

    [Header("--- Stats ---")]
    [SerializeField] int healthPoints;
    [SerializeField] float fireRate;
    [SerializeField] float fireSpeed;
    [SerializeField] bool isShooting;
    [SerializeField] float movementSpeed;


    [Header("--- AI Info ---")]
    [SerializeField] int turnSpeed;
    [SerializeField] Transform playerFinder;
    [SerializeField] float fTimeTilTarget = 1.2f;
    Vector3 dirOfPlayer;
    Vector3 playerShooter;
    bool playerInRange;
    bool isShimmy;
    float viewAngle;
    float distanceToPlayer;
    float fDistance;
    float fBaseCheckTime = 0.15f;
    float fTimePercheck = 0.05f;
    float[] shimmyNums = {7, 5 , 9 };
    float timePassed;
    int iMaxIters = 100;
    private Tracker objectTracker;

    [Header("--- Components ---")]
    [SerializeField] GameObject playerDetector;
    [SerializeField] GameObject bullet;
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent navMeshA;
    [SerializeField] AudioSource aud;
    [SerializeField] Animator animatorSkirmisher;

    [Header("--- Audio ---")]
    [SerializeField] AudioClip[] audAmbience;
    [Range(0, 1)][SerializeField] float audAmbienceVol;
    [SerializeField] AudioClip[] audAttack;
    [Range(0, 1)][SerializeField] float audAttackVol;
    [SerializeField] AudioClip[] audHit;
    [Range(0, 1)][SerializeField] float audhitVol;
    [SerializeField] AudioClip[] audDeath;
    [Range(0, 1)][SerializeField] float auddeathVol;

    [Header("--- Effects ---")]
    [SerializeField] GameObject effect;
    // Start is called before the first frame update
    void Start()
    {
        navMeshA.stoppingDistance = 10;
        objectTracker = GetComponent<Tracker>();
        gameManager.Instance.updateGoal(0, 1);
    }

    // Update is called once per frame
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
        if (distanceToPlayer > 20)
        {
            navMeshA.SetDestination(gameManager.Instance.PlayerModel.transform.position);
        }
        if (10 < distanceToPlayer && distanceToPlayer < 20 && !isShimmy)
        {
            StartCoroutine(TangentShimmy());
        }
        if(distanceToPlayer < 10)
        {
            GivePlayerSpace();
        }
        //if (distanceToPlayer < 10)
        //{
        //    navMeshA.ResetPath();
        //    navMeshA.SetDestination(gameManager.Instance.PlayerModel.transform.position);
        //}
        if (distanceToPlayer < 15 && !isShooting)
        {
           StartCoroutine(ShootPlayer());
        }
    }

    void FollowPlayer()
    {
        Quaternion enemyRotation = Quaternion.LookRotation(new Vector3(dirOfPlayer.x, dirOfPlayer.y, dirOfPlayer.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, enemyRotation, Time.deltaTime * turnSpeed);
    }

    void GivePlayerSpace()
    {
        navMeshA.SetDestination(transform.position - transform.forward);
    }
    IEnumerator ShootPlayer()
    {
        int iIterations = 0;
        isShooting = true;
        float fCheckTime = fBaseCheckTime;
        yield return new WaitForSeconds(fireRate);
        animatorSkirmisher.GetComponent<Animator>().Play("demo_combat_shoot");
        BaseProjectile freshBullet = GameObject.Instantiate(bullet, playerFinder.transform.position, transform.rotation).GetComponent<BaseProjectile>();
        Vector3 TargetPosition = objectTracker.ProjectedPosition(fBaseCheckTime);

        Vector3 ProjectilePosition = playerFinder.position + ((TargetPosition - playerFinder.position).normalized * fireSpeed * fCheckTime);
        fDistance = (TargetPosition - ProjectilePosition).magnitude;

        while (fDistance > 3.5f && iIterations < iMaxIters)
        {
            iIterations++;
            fCheckTime += fTimePercheck;
            TargetPosition = objectTracker.ProjectedPosition(fCheckTime);

            ProjectilePosition = playerFinder.position + ((TargetPosition - playerFinder.position).normalized * fireSpeed * fCheckTime);
            fDistance = (TargetPosition - ProjectilePosition).magnitude;
        }

        Vector3 v3Velocity = TargetPosition - playerFinder.transform.position;
        freshBullet.Shoot(v3Velocity.normalized, fireSpeed);
        animatorSkirmisher.GetComponent<Animator>().Play("Blend Tree");
        isShooting = false;

    }

    IEnumerator TangentShimmy()
    {
        isShimmy = true;
        animatorSkirmisher.GetComponent<Animator>().Play("Blend Tree");
        navMeshA.speed = 30;
        int flipper = Random.Range(0, 2);
        navMeshA.stoppingDistance = 0;
        float shimVec = shimmyNums[Random.Range(0, shimmyNums.Length)];

        Vector3 randomPos = Random.insideUnitSphere * shimVec;
        randomPos += transform.position;

        NavMeshHit pos;
        NavMesh.SamplePosition(randomPos, out pos, shimVec, 1);

        navMeshA.SetDestination(pos.position);
        //if (flipper == 1)
        //{
        //    navMeshA.SetDestination(transform.position - transform.right);
        //}
        //if (flipper == 0)
        //{
        //    navMeshA.SetDestination(transform.position + transform.right);
        //}

        //Debug.Log("IT WORKE+S");
        yield return new WaitForSeconds(4);
        navMeshA.speed = movementSpeed;
        isShimmy = false;

        //yield return new WaitForSeconds(roamPauseTime);
        //destinationChosen = false;

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
            if (ItemToDrop.Length != 0)
            {
                ItemDrop();
            }

            Destroy(gameObject);
            gameManager.Instance.updateGoal(50, -1);
        }
        else
        {
            //navMeshA.SetDestination(gameManager.Instance.PlayerModel.transform.position);
            //navMeshA.stoppingDistance = 0;
            StartCoroutine(flashColor());
        }
        void ItemDrop()
        {
            int Item = Random.Range(0, 5);

            if (ItemToDrop.Length > Item)
            {
                Instantiate(ItemToDrop[Item], transform.position, transform.rotation);
            }
        }

    }
}

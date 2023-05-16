using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySwarmer : MonoBehaviour, Damage
{

    [Header("----- Top of Enemy -----")]
    [SerializeField] GameObject[] ItemToDrop;

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
    float speed;

    [Header("--- Components ---")]
    [SerializeField] GameObject playerDetector;
    [SerializeField] NavMeshAgent navMeshA;
    [SerializeField] Renderer model;
    [SerializeField] GameObject meleeSwipe;
    [SerializeField] AudioSource aud;
    [SerializeField] Animator animatorSwarmer;

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
        // animatorSwarmer.GetComponent<Animator>().Play("Blend Tree");
        aud.PlayOneShot(audAmbience[Random.Range(0, audAmbience.Length)], audAmbienceVol);
        speed = Mathf.Lerp(speed, navMeshA.velocity.normalized.magnitude, Time.deltaTime * 3);
        animatorSwarmer.SetFloat("Speed", speed);
    }

    void FindPlayer()
    {
        dirOfPlayer = (gameManager.Instance.PlayerModel.transform.position /*- playerFinder.position*/);
        viewAngle = Vector3.Angle(new Vector3(dirOfPlayer.x, 0, dirOfPlayer.z), playerFinder.forward);
        distanceToPlayer = Vector3.Distance(transform.position, gameManager.Instance.PlayerModel.transform.position);
        Debug.DrawLine(playerFinder.position, gameManager.Instance.PlayerModel.transform.position);

        FollowPlayer();
        //if (distanceToPlayer > 12)
        //{
        //    PredictiveCutOff();
        //}
        //if (distanceToPlayer < 10 && distanceToPlayer > 3)
        if( distanceToPlayer > 3)
        {
            //navMeshA.ResetPath();
            navMeshA.SetDestination(gameManager.Instance.PlayerModel.transform.position);
        }
        if (distanceToPlayer < 4 && !isMeleeing)
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
        aud.PlayOneShot(audAttack[Random.Range(0, audAttack.Length)], audAttackVol);
        // navMeshA.speed = 0;
        yield return new WaitForSeconds(meleeWindUp);
        meleeSwipe.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        animatorSwarmer.GetComponent<Animator>().Play("attack");
        meleeSwipe.SetActive(false);
        navMeshA.speed = movementSpeed;
        yield return new WaitForSeconds(meleeRate);
        animatorSwarmer.GetComponent<Animator>().Play("Blend Tree");
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
        aud.PlayOneShot(audHit[Random.Range(0, audHit.Length)], audhitVol);


        if (healthPoints <= 0)
        {
            if (ItemToDrop.Length != 0)
            {
                ItemDrop();
            }
            StopAllCoroutines();
            gameManager.Instance.updateGoal(20,-1);
           // animatorSwarmer.GetComponent<Animator>().Play("pounce");
           // aud.PlayOneShot(audDeath[Random.Range(0, audDeath.Length)], auddeathVol);
            GetComponent<CapsuleCollider>().enabled = false;
            navMeshA.enabled = false;
            Destroy(gameObject);
        }
        else
        {
            //navMeshA.SetDestination(gameManager.Instance.PlayerModel.transform.position);
            //navMeshA.stoppingDistance = 0;
           // animatorSwarmer.GetComponent<Animator>().Play("pounce");
          //  aud.PlayOneShot(audHit[Random.Range(0, audHit.Length)], audhitVol);
            navMeshA.SetDestination(gameManager.Instance.PlayerModel.transform.position);
            StartCoroutine(flashColor());
        }

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

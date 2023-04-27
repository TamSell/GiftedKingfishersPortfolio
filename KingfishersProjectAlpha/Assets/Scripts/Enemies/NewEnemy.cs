using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class NewEnemy : MonoBehaviour, Damage
{
    [Header("----- Components -----")]
    [SerializeField] Animator animator;
    [SerializeField] AudioSource aud;

    [Header("-- Stats --")]
    [SerializeField] int hitPoints;
    [SerializeField] int cameraTurnSpeed;
    [SerializeField] int cameraAngle;
    [SerializeField] int stoppDist;
    [SerializeField] int roamStopTime;
    [SerializeField] int roamDistance;
    [SerializeField] float animTransSpeed;
    float viewAngle;
    float originalSpeed;

    [Header("------ Audio ------")]
    [SerializeField] AudioClip[] audAmbience;
    [Range(0, 1)][SerializeField] float audAmbienceVol;
    [SerializeField] AudioClip[] audAttack;
    [Range(0, 1)][SerializeField] float audAttackVol;
    [SerializeField] AudioClip[] audHit;
    [Range(0, 1)][SerializeField] float audhitVol;
    [SerializeField] AudioClip[] audDeath;
    [Range(0, 1)][SerializeField] float auddeathVol;

    [Header("-- Variables --")]
    Vector3 identVec;
    bool playerInRange;
    float stopDistOrig;
    bool destinationChosen;
    Vector3 startingPos;

    [Header("-- Objects --")]
    [SerializeField] Renderer model;
    [SerializeField] Transform headPos;
    [SerializeField] NavMeshAgent navMeshA;

    [Header("-- Melee Stats --")]
    [SerializeField] float meleeWindUp;
    [SerializeField] float MeleeRate;

    [Header("-- Effects --")]
    [SerializeField] GameObject TriggerEffect;
    bool IsEffecting;
    GameObject effect;

    float distanceToPlayer;
    public GameObject meleeSwipe;
    bool isMeleeing;
    float speed;

    void Start()
    {
        gameManager.Instance.updateGoal(1);
        stopDistOrig = stoppDist;
        startingPos = transform.position;
        originalSpeed = navMeshA.speed;
    }

    void Update()
    {
        
        if (navMeshA.isActiveAndEnabled)
        {
            aud.PlayOneShot(audAmbience[Random.Range(0, audAmbience.Length)], audAmbienceVol);
            speed = Mathf.Lerp(speed, navMeshA.velocity.normalized.magnitude, Time.deltaTime * animTransSpeed);
            animator.SetFloat("Speed", speed);

            if (playerInRange && !FindPlayer())
            {
                StartCoroutine(roam());
            }
            else if (navMeshA.destination != gameManager.Instance.PlayerModel.transform.position)
            {
                StartCoroutine(roam());
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
    bool FindPlayer()
    {
        identVec = (gameManager.Instance.PlayerModel.transform.position - headPos.position);
        viewAngle = Vector3.Angle(new Vector3(identVec.x, 0, identVec.z), transform.forward);
        distanceToPlayer = Vector3.Distance(headPos.position, gameManager.Instance.PlayerModel.transform.position);

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

                if (!isMeleeing && distanceToPlayer < navMeshA.stoppingDistance)
                {

                    StartCoroutine(melee());
                }
                return true;
            }
        }
        return false;
    }

    void FollowPlayer()
    {
        Quaternion enemyRotation = Quaternion.LookRotation(new Vector3(identVec.x, 0, identVec.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, enemyRotation, Time.deltaTime * cameraTurnSpeed);
    }

    IEnumerator roam()
    {
            if (!destinationChosen && navMeshA.remainingDistance < 0.05)
            {
                destinationChosen = true;
                navMeshA.stoppingDistance = 0;

                yield return new WaitForSeconds(roamStopTime);

                Vector3 randomPos = Random.insideUnitSphere * roamDistance;
                randomPos += startingPos;

                NavMeshHit pos;
                NavMesh.SamplePosition(randomPos, out pos, roamDistance, 1);

                navMeshA.SetDestination(pos.position);
                destinationChosen = false;
            }
        }
    IEnumerator melee()
    {
        isMeleeing = true;
        GetComponent<Animator>().enabled = false;
        navMeshA.speed = 0;
        aud.PlayOneShot(audAttack[Random.Range(0, audAttack.Length)], audAttackVol);
        GetComponent<Animator>().enabled = true;
        animator.SetTrigger("Melee");
        yield return new WaitForSeconds(meleeWindUp);
        meleeSwipe.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        meleeSwipe.SetActive(false);
        navMeshA.speed = originalSpeed;
        yield return new WaitForSeconds(MeleeRate);
        isMeleeing = false;
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
            meleeSwipe.SetActive(false);
            aud.PlayOneShot(audDeath[Random.Range(0, audDeath.Length)], auddeathVol);
            gameManager.Instance.updateGoal(-1);
            animator.SetBool("Death", true);
            GetComponent<CapsuleCollider>().enabled = false;
            navMeshA.enabled = false;
        }
        else
        {
            aud.PlayOneShot(audHit[Random.Range(0, audHit.Length)], audhitVol);
            Vector3 lower = new Vector3(navMeshA.stoppingDistance, 0.0f, navMeshA.stoppingDistance);
            animator.SetTrigger("Damage");
            navMeshA.SetDestination(gameManager.Instance.PlayerModel.transform.position);
            navMeshA.stoppingDistance = 0;

            StartCoroutine(flashColor());
        }

    }

    IEnumerator hitEffect()
    {
        IsEffecting = true;
        TriggerEffect.SetActive(true);
        navMeshA.speed = 0;
        yield return new WaitForSeconds(1.0f);
        navMeshA.speed = originalSpeed;
        TriggerEffect.SetActive(false);

        // yield return new WaitForSeconds(0.5f);

        IsEffecting = false;

    }
}

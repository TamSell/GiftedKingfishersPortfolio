using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class NewEnemy : MonoBehaviour, Damage
{
    [Header("----- Components -----")]

    [Header("-- Stats --")]
    [SerializeField] int hitPoints;
    [SerializeField] int cameraTurnSpeed;
    [SerializeField] int cameraAngle;
    [SerializeField] int stoppDist;
    [SerializeField] int roamStopTime;
    [SerializeField] int roamDistance;
    float viewAngle;

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
    [SerializeField] int MeleeDamage;
    [SerializeField] float MeleeRate;

    float distanceToPlayer;
    public GameObject meleeSwipe;
    bool isMeleeing;


    void Start()
    {
        gameManager.Instance.updateGoal(1);
        stopDistOrig = stoppDist;
        startingPos = transform.position;
    }

    void Update()
    {
        if (navMeshA.isActiveAndEnabled)
        {
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

                if (!isMeleeing)
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
            meleeSwipe.SetActive(true);
            yield return new WaitForSeconds(MeleeRate);
            meleeSwipe.SetActive(false);
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
        navMeshA.SetDestination(gameManager.Instance.PlayerModel.transform.position);
        navMeshA.stoppingDistance = 0;

        StartCoroutine(flashColor());

        if (hitPoints <= 0)
        {
            gameManager.Instance.updateGoal(-1);
            Destroy(gameObject);
        }
    }
}

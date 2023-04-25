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
    [SerializeField] int runSpeed;
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
    [SerializeField] NavMeshAgent navMeshA;

    [Header("-- Melee Stats --")]
    [SerializeField] int MeleeDamage;
    [SerializeField] float MeleeRate;
    [SerializeField] int MeleeDist;
    [SerializeField] Transform swipePos;



    public GameObject meleeSwipe;

    [SerializeField] bool isMeleeing;


    void Start()
    {
        gameManager.Instance.updateGoal(1);
        stopDistOrig = stoppDist;
    }

    void Update()
    {
        if (playerInRange)
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

                if (!isMeleeing)
                {

                    StartCoroutine(melee());
                }
            }
        }
    }

    void FollowPlayer()
    {
        Quaternion enemyRotation = Quaternion.LookRotation(new Vector3(identVec.x, 0, identVec.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, enemyRotation, Time.deltaTime * runSpeed);
    }

    IEnumerator melee()
    {
        meleeSwipe.SetActive(true);
        yield return new WaitForSeconds(MeleeRate);
        meleeSwipe.SetActive(false);
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

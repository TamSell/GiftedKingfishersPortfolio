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
    [SerializeField] bool isMeleeing;
    [SerializeField] int moveRadius;
    Vector3 lookVector;


    [Header("--- AI Info ---")]
    [SerializeField] int turnSpeed;
    [SerializeField] GameObject enemyFace;
    [SerializeField] float distanceToPlayer;
    float speed;

    [Header("--- Components ---")]
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
        gameManager.Instance.updateGoal(0, 1);
    }


    void Update()
    {
            lookVector = gameManager.Instance.playerController.PlayerBody.transform.position - enemyFace.transform.position;
            distanceToPlayer = Vector3.Distance(enemyFace.transform.position, gameManager.Instance.playerController.PlayerBody.transform.position);
            ActiveIntelligence();
            speed = Mathf.Lerp(speed, navMeshA.velocity.normalized.magnitude, Time.deltaTime * 3);
            animatorSwarmer.SetFloat("Speed", speed);
    }

    void ActiveIntelligence()
    {
        FaceThePlayer();
        if (distanceToPlayer > moveRadius)
        {
            MoveTowardPlayer();
        }
        if (distanceToPlayer <= moveRadius && !isMeleeing)
        {
            StartCoroutine(melee());
        }
        if (distanceToPlayer < 2)
        {
            GivePlayerSpace();
        }
    }
    void MoveTowardPlayer()
    {
        navMeshA.SetDestination(transform.position + transform.forward);
    }

    void GivePlayerSpace()
    {
        navMeshA.SetDestination(transform.position - transform.forward);
    }
    void FaceThePlayer()
    {
        
        Quaternion rot = Quaternion.LookRotation(lookVector);
        lookVector = rot.eulerAngles;
        lookVector.z = 0;
        lookVector.x = 0;
        rot.eulerAngles = lookVector;
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, 1);
        animatorSwarmer.GetComponent<Animator>().Play("Blend Tree");
    }

    IEnumerator melee()
    {
        isMeleeing = true;
        animatorSwarmer.GetComponent<Animator>().StopPlayback();
        yield return new WaitForSeconds(meleeWindUp);
        meleeSwipe.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        animatorSwarmer.GetComponent<Animator>().Play("attack");
        meleeSwipe.SetActive(false);
        yield return new WaitForSeconds(meleeRate);
       // animatorSwarmer.GetComponent<Animator>().Play("Blend Tree");
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
        healthPoints -= amountDamage;

        if (healthPoints <= 0)
        {
            if (ItemToDrop.Length != 0)
            {
                ItemDrop();
            }
            StopAllCoroutines();
            gameManager.Instance.updateGoal(20,-1);
            GetComponent<CapsuleCollider>().enabled = false;
            navMeshA.enabled = false;
            Destroy(gameObject);
        }
        else
        {
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

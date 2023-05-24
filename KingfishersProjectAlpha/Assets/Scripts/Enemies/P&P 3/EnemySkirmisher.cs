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
    [SerializeField] bool isShooting;
    [SerializeField] int moveRadius = 20;
    Vector3 lookVector;


    [Header("--- AI Info ---")]
    [SerializeField] int turnSpeed;
    [SerializeField] GameObject playerFinder;
    float distanceToPlayer;
    float speed;

    [Header("--- Components ---")]
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
    void Start()
    {
        gameManager.Instance.updateGoal(0, 1);
    }
    void Update()
    {
        lookVector = gameManager.Instance.playerController.PlayerBody.transform.position - playerFinder.transform.position;
        distanceToPlayer = Vector3.Distance(playerFinder.transform.position, gameManager.Instance.playerController.PlayerBody.transform.position);
        ActiveIntelligence();
        speed = Mathf.Lerp(speed, navMeshA.velocity.normalized.magnitude, Time.deltaTime * 3);
        animatorSkirmisher.SetFloat("Speed", speed);
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
            StartCoroutine(ShootPlayer());
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
        animatorSkirmisher.GetComponent<Animator>().Play("Blend Tree");
    }

    void GivePlayerSpace()
    {
        navMeshA.SetDestination(transform.position - transform.forward);
    }

    void MoveTowardPlayer()
    {
        navMeshA.SetDestination(transform.position + transform.forward);
    }
    IEnumerator ShootPlayer()
    {
        isShooting = true;
        animatorSkirmisher.GetComponent<Animator>().Play("demo_combat_shoot");
        Instantiate(bullet, playerFinder.transform.position, playerFinder.transform.rotation);
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
        healthPoints -= amountDamage;

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class EnemyTurret : MonoBehaviour, Damage
{

    [Header("----- Top of Enemy -----")]
    [SerializeField] GameObject[] ItemToDrop;

    [Header("--- Stats ---")]
    [SerializeField] int healthPoints;
    [SerializeField] float fireRate;
    [SerializeField] float fireSpeed;
    [SerializeField] bool isShooting;


    [Header("--- AI Info ---")]
    [SerializeField] int turnSpeed;
    [SerializeField] Transform playerFinder;
    [SerializeField] float fTimeTilTarget = 1.2f;
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
    [SerializeField] GameObject bullet;
    [SerializeField] Renderer model;

    [Header("--- Effects ---")]
    [SerializeField] GameObject effect;

    // Start is called before the first frame update
    void Start()
    {
        objectTracker = GetComponent<Tracker>();
        gameManager.Instance.updateGoal(0, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange)
        {
            FindPlayer();
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
        {
            return;
        }
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
        dirOfPlayer = (gameManager.Instance.PlayerModel.transform.position - playerFinder.position);
        viewAngle = Vector3.Angle(new Vector3(dirOfPlayer.x, 0, dirOfPlayer.z), playerFinder.forward);
        distanceToPlayer = Vector3.Distance(playerFinder.position, gameManager.Instance.PlayerModel.transform.position);
        Debug.DrawLine(playerFinder.position, gameManager.Instance.PlayerModel.transform.position);

        FollowPlayer();
        if (!isShooting)
        {
           StartCoroutine(ShootPlayer());
        }
    }

    IEnumerator ShootPlayer()
    {
        int iIterations = 0;
        isShooting = true;
        float fCheckTime = fBaseCheckTime;
        yield return new WaitForSeconds(fireRate);
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
        isShooting = false;

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
          
            if (ItemToDrop.Length != 0)
            {
                ItemDrop();
            }
 
            gameManager.Instance.updateGoal(40, -1);
            Destroy(gameObject);
          //  gameManager.Instance.enemySpawner.EnemyCount--;
        }
        else
        { 
            StartCoroutine(flashColor());
        }

    }
    void ItemDrop()
    {
        int Item = Random.Range(0, 5);

        if(ItemToDrop.Length >Item)
        {
            Instantiate(ItemToDrop[Item], transform.position, transform.rotation);
        }
    }
}


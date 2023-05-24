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
    [SerializeField] bool isShooting;


    [Header("--- AI Info ---")]
    [SerializeField] int turnSpeed;
    [SerializeField] GameObject playerFinder;
    Vector3 dirOfPlayer;
    bool playerInRange;

    [Header("--- Components ---")]
    [SerializeField] GameObject bullet;
    [SerializeField] Renderer model;
    [SerializeField] AudioSource aud;


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
        dirOfPlayer = (gameManager.Instance.PlayerModel.transform.position - playerFinder.transform.position);
       // Debug.DrawLine(playerFinder.position, gameManager.Instance.PlayerModel.transform.position);

        FollowPlayer();
        if (!isShooting)
        {
           StartCoroutine(ShootPlayer());
        }
    }

    IEnumerator ShootPlayer()
    {
        isShooting = true;
        Instantiate(bullet, playerFinder.transform.position, playerFinder.transform.rotation);
        yield return new WaitForSeconds(fireRate);
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
        if (healthPoints <= 0)
        {

            if (ItemToDrop.Length != 0)
            {
                ItemDrop();
            }
 
            gameManager.Instance.updateGoal(40, -1);
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

        if(ItemToDrop.Length >Item)
        {
            Instantiate(ItemToDrop[Item], transform.position, transform.rotation);
        }
    }
}


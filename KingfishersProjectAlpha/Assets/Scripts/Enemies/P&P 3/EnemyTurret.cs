using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class EnemyTurret : MonoBehaviour
{

    [Header("----- Top of Enemy -----")]


    [Header("--- Stats ---")]
    [SerializeField] int healthPoints;
    [SerializeField] float fireRate;
    [SerializeField] float fireSpeed;
    [SerializeField] bool isShooting;


    [Header("--- AI Info ---")]
    [SerializeField] int turnSpeed;
    [SerializeField] Transform playerFinder;
    Vector3 dirOfPlayer;
    Vector3 playerShooter;
    Vector3 playerFuturePos;
    bool playerInRange;
    float playerFutureX = 0.0f;
    float playerFutureY = 0.0f;
    float playerFutureZ = 0.0f;
    float viewAngle;
    float distanceToPlayer;

    [Header("--- Components ---")]
    [SerializeField] GameObject playerDetector;
    [SerializeField] GameObject bullet;

    [Header("--- Effects ---")]
    [SerializeField] GameObject effect;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange)
        {
            StartCoroutine(calculateFuture());
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
        distanceToPlayer = Vector3.Distance(transform.position, gameManager.Instance.PlayerModel.transform.position);
        Debug.DrawLine(playerFinder.position, gameManager.Instance.PlayerModel.transform.position);

        FollowPlayer();
        if (!isShooting)
        {
            StartCoroutine(shootPlayer());
        }
    }


    IEnumerator shootPlayer()
    {
        isShooting = true;
        // playerShooter = gameManager.Instance.PlayerModel.transform.position;
        playerShooter = playerFuturePos;
        Vector3 playerDirection = new Vector3(playerShooter.x, playerShooter.y + 1, playerShooter.z);
        GameObject temp = Instantiate(bullet, playerFinder.position, Quaternion.identity);
        temp.transform.LookAt(playerDirection);
        Rigidbody tempRB = temp.GetComponent<Rigidbody>();
        tempRB.velocity = temp.transform.forward * fireSpeed;
        // tempRB.useGravity = true;

        yield return new WaitForSeconds(fireRate);
        isShooting = false;

    }

    IEnumerator calculateFuture()
    {
        playerFutureX = gameManager.Instance.playerController.transform.position.x;
        playerFutureY = gameManager.Instance.playerController.transform.position.y;
        playerFutureZ = gameManager.Instance.playerController.transform.position.z;
        yield return new WaitForSeconds(0.16f);
        float changeX = playerFutureX - gameManager.Instance.playerController.transform.position.x;
        float changeY = playerFutureY - gameManager.Instance.playerController.transform.position.y;
        float changeZ = playerFutureZ - gameManager.Instance.playerController.transform.position.z;
        float futureX = gameManager.Instance.playerController.transform.position.x + changeX;
        float futureY = gameManager.Instance.playerController.transform.position.y + changeY;
        float futureZ = gameManager.Instance.playerController.transform.position.y + changeZ;
        gameManager.Instance.
        playerFuturePos = new Vector3(futureX, futureY, futureZ);

    }
    void FollowPlayer()
    {
        Quaternion enemyRotation = Quaternion.LookRotation(new Vector3(dirOfPlayer.x, dirOfPlayer.y, dirOfPlayer.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, enemyRotation, Time.deltaTime * turnSpeed);
    }
}

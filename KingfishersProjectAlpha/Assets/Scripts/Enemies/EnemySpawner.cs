using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject[] prefab;
    [SerializeField] int spawnRate;
    [SerializeField] Transform[] spawnPos;
    [SerializeField] int prefabMaxNum;
    public List<GameObject> spawnList = new List<GameObject>();

    int prefabSpawncount;
    bool playerInRange;
    bool isSpawning;


    // Start is called before the first frame update
    void Start()
    {
        gameManager.Instance.updateGoal(prefabMaxNum);
    }

    // Update is called once per frame
    void Update()
    {
        if(playerInRange && !isSpawning && prefabSpawncount < prefabMaxNum)
        {
            StartCoroutine(spawn());
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }
    IEnumerator spawn()
    {
        isSpawning = true;
        GameObject Spawn = Instantiate(prefab[Random.Range(0,prefab.Length)], spawnPos[Random.Range(0, spawnPos.Length)].position, prefab[Random.Range(0, spawnPos.Length)].transform.rotation);
        spawnList.Add(Spawn);
        prefabSpawncount++;

        yield return new WaitForSeconds(spawnRate);

        isSpawning = false;

    }
}

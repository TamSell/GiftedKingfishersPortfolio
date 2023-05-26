using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePortalCode : MonoBehaviour
{
    [SerializeField] private GameObject BaseTeleporter = null;
    [SerializeField] private GameObject StartEffects = null;
    [SerializeField] private float timeBeforeLoad = 4f;
    [SerializeField] public int scoreNecessary;
    [SerializeField] int ScoreCurrent;
    [SerializeField] int levelSelect;

    private float timeElapsed;
    private bool hasEntered;
    private Collider Enter;

    private void Start()
    {
        Enter = GetComponent<Collider>();
        Enter.enabled = false;
    }

    void Update()
    {
        ScoreCurrent = gameManager.Instance.playerScore;
        if(gameManager.Instance.playerScore >= scoreNecessary)
        {
            if(BaseTeleporter)
                BaseTeleporter.SetActive(true);
            Enter.enabled = true;
            if (hasEntered && timeElapsed > timeBeforeLoad)
            {
                LoadScene();
            }
            else if(hasEntered)
            {
                timeElapsed += Time.deltaTime;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            hasEntered = true;
            if(StartEffects)
                StartEffects.SetActive(true);
        }
    }

    private void LoadScene()
    {
        if (levelSelect > -1)
        {
            SceneManager.LoadScene(levelSelect);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}

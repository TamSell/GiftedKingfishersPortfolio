using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePortalCode : MonoBehaviour
{
    [SerializeField] int scoreNecessary;
    [SerializeField] int ScoreCurrent;
    [SerializeField] int levelSelect;
    void Start()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.GetComponent<BoxCollider>().enabled = false;
    }
    void Update()
    {
        ScoreCurrent = gameManager.Instance.playerScore;
        if(gameManager.Instance.playerScore >= scoreNecessary)
        {
            gameObject.GetComponent<MeshRenderer>().enabled = true;
            gameObject.GetComponent<BoxCollider>().enabled = true;
            Debug.Log("Please");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(levelSelect > -1)
        {
            SceneManager.LoadScene(levelSelect);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}

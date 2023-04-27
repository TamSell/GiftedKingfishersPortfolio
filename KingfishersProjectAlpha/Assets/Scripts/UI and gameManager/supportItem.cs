using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class supportItem : MonoBehaviour
{
    public GameObject drop;
    public bool ammoorhealth;
    [SerializeField] int addIt;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(ammoorhealth)
            {
                gameManager.Instance.playerController.addHP(addIt);
            }
            else
            {
                gameManager.Instance.playerController.usingGun.totalAmmo += addIt;
            }
        }
    }
}

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
            if(!ammoorhealth)
            {
                if(gameManager.Instance.playerController.HPorig > gameManager.Instance.playerController.HP + addIt)
                    gameManager.Instance.playerController.addHP(addIt);
                else
                    gameManager.Instance.playerController.HP = gameManager.Instance.playerController.HPorig;
            }
            else
            {
                gameManager.Instance.playerController.usingGun.totalAmmo += addIt;
            }
            Destroy(gameObject);
        }
    }
}

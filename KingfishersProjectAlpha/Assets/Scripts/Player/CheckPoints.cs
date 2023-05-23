using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoints : MonoBehaviour
{
    [SerializeField] Renderer model;
    [SerializeField] GameObject triggerEffect;
    GameObject effect;

    private void OnTriggerEnter(Collider other)
    {  
       if (other.CompareTag("Player") && gameManager.Instance.playerSpawnPos.transform.position != transform.position)
        {
            gameManager.Instance.playerSpawnPos.transform.position = transform.position;
            if(triggerEffect)
            {
                StartCoroutine(fashColor());
              effect =  Instantiate(triggerEffect, transform.position, triggerEffect.transform.rotation);
             
                Destroy(effect,5);
            }           
        }
    }

    IEnumerator fashColor()
    {
        triggerEffect.SetActive(true);
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        model.material.color = Color.black;
        triggerEffect.SetActive(false);
       
  
    }

}

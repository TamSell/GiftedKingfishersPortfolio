using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    [Header("----Melee Settings----")]
    [SerializeField] int damage;
    [SerializeField] bool ChargeRunningWeapon;
    [Range(0,10)][SerializeField]float RunningTime;
    [Range(0.1f, 2.0f)][SerializeField] float ChargeTime;
    [SerializeField] GameObject TriggerEffect;
   
    [SerializeField] BoxCollider box;
   
    GameObject effect;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    private void Update()
    {
       
        
       if ( gameManager.Instance.playerController.isRunning==false)
        {
            RunningTime = 0;
        }
        else if(Input.GetButton("Run") && gameManager.Instance.playerController.isRunning)
        {
           
            RunningTime += Time.deltaTime;
        }
    }

    public void OnTriggerEnter(Collider other)
    {

        if (other.isTrigger)
        {
            return;
        }
        if(ChargeRunningWeapon == true)
        {
           // box.enabled = false;
            
            if(Input.GetButton("Run"))
            {

               // if(RunningTime > ChargeTime)
               if(gameManager.Instance.playerController.energyMax>11)
                {

                    Damage canDamage = other.GetComponent<Damage>();

                    if (canDamage != null)
                    {
                        //  box.enabled=true;
                        if (TriggerEffect)
                        {
                            StartCoroutine(hitEffect());
                            effect = Instantiate(TriggerEffect, transform.position, gameManager.Instance.PlayerModel.transform.rotation);

                            Destroy(effect, 5);
                        }
                        canDamage.TakeDamage(damage);
                    }
                }
             
            }
        }
     
    }
    IEnumerator hitEffect()
    {
        TriggerEffect.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        TriggerEffect.SetActive(false);


    }

}

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

    [SerializeField] BoxCollider box;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        
        if( gameManager.Instance.playerController.isrunning==false)
        {
            RunningTime = 0;
        }
        else if(Input.GetButton("Run") &&gameManager.Instance.playerController.isrunning==true)
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

                if(RunningTime > ChargeTime)
                {
                    Damage canDamage = other.GetComponent<Damage>();

                    if (canDamage != null)
                    {
                      //  box.enabled=true;
                        canDamage.TakeDamage(damage);
                    }
                }
             
            }
        }
     
    }
    }

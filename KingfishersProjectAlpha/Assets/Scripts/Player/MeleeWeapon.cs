using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] bool ChargeRunningWeapon;
    [Range(0,10)][SerializeField]float RunningTime;
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
            if(Input.GetButton("Run"))
            {
              
                if(RunningTime > 0.6)
                {
                    Damage canDamage = other.GetComponent<Damage>();

                    if (canDamage != null)
                    {
                        canDamage.TakeDamage(damage);
                    }
                }
             
            }
        }
     
    }
    }

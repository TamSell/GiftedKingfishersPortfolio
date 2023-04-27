using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Gun : MonoBehaviour
{
    [SerializeField] Transform cam;

    [Header("----Gun Basic Stats-----")]
    [Range(0.1f,2)][SerializeField] public float ShootRate;
    [SerializeField] public int realoadSpeed;
    [SerializeField] public bool reaload;

    [Header("-----RayTrace Gun Stats------")]
    [SerializeField] public float RayGunDist;
    [SerializeField] public int RayGunDamage;
    [SerializeField] public GameObject RayGunEffect;
  
    [Header("----- Ammo -----")]
    [Range(0, 30)][SerializeField] public int magSize;
    [Range(0, 300)][SerializeField] public int totalAmmo;



   
    
    public bool isShooting;
    public int currentMag;
    public BulletSpeed bulletVals;
    [SerializeField] public GameObject bullet;
    [SerializeField] public Transform Barrel;
    public bool RayCastWeapon;
  

    
   
    

    // Start is called before the first frame update
    void Start()
    {
        RealoadingLogic();
    }

    // Update is called once per frame
    void Update()
    {
            Reloading();
            if (reaload == true)
            {
            Invoke("shooting", realoadSpeed);
              if(RayCastWeapon)
              {
                RayGunEffect.SetActive(false);
              }
             
            }
            else
            {
            shooting();
            RayCastSetActive();
             
            }
      
        
    }

   
    public void shooting()
    {
      
            if (currentMag == 0)
            {
                return;
            }
            if (!isShooting && Input.GetButton("Shoot"))
            {
                reaload = false;
            if (gameManager.Instance.playerController.enabled == true)
            {
                StartCoroutine(shoot());
            }
            }
        
     
    }

    public void RayCastSetActive()
    {
        if (currentMag == 0)
        {
           
            return;
        }
        if(RayGunEffect)
        {
            if (RayCastWeapon && Input.GetButton("Shoot"))
            {
                RayGunEffect.SetActive(true);
            }
            else
            {
                RayGunEffect.SetActive(false);
            }
        }
    }
   


    IEnumerator shoot()
    {
        isShooting = true;
        if(RayCastWeapon)
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, RayGunDist))
            {
              

                Damage damage = hit.collider.GetComponent<Damage>();
                if (damage != null)
                {
                    damage.TakeDamage(RayGunDamage);
                }
            }
            yield return new WaitForSeconds(ShootRate);
            isShooting = false;
            CountOfBullets(-1);
            gameManager.Instance.loadText(totalAmmo, currentMag);
        }
       
        else
        {
            Instantiate(bullet, Barrel.position, cam.rotation);
            yield return new WaitForSeconds(ShootRate);
            isShooting = false;
            CountOfBullets(-1);
            gameManager.Instance.loadText(totalAmmo, currentMag);


        }
    }

    




    

    public void CountOfBullets(int ammount)
    {
        currentMag += ammount;
    }

    public void Reloading()
    {
        if(!isShooting)
        {
            if (Input.GetButtonDown("Reloading"))
            {
                reaload = true;
                RealoadingLogic();
            }
            else if (currentMag == 0 && totalAmmo > 0)
            {
                reaload = true;
                RealoadingLogic();
            }
        }
      
    }
    public void RealoadingLogic()
    {
       
        if(totalAmmo == 0)
        {
            return;
        }
        else
        {
            if(totalAmmo >= magSize)
            {
                totalAmmo = totalAmmo - (magSize - currentMag);
                currentMag = magSize;
                gameManager.Instance.loadText(totalAmmo, currentMag);
            }
            else
            {
                currentMag += totalAmmo;
                totalAmmo = 0;
                gameManager.Instance.loadText(totalAmmo, currentMag);
            }
        }
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Gun : MonoBehaviour
{
    [Header("----Gun Basic Stats-----")]
    [Range(0.1f,2)][SerializeField] float ShootRate;
    [SerializeField] int realoadSpeed;
    [SerializeField]bool reaload;
    [SerializeField] float RayGunDist;
    [SerializeField] int RayGunDamage;

    [Header("----- Ammo -----")]
    [Range(0, 30)][SerializeField] int magSize;
    [Range(0, 300)][SerializeField] int totalAmmo;




    float timePressed;
    public bool isShooting;
    public int currentMag;
    int ammoToInsert;
    public GameObject bullet ;
    public Transform Barrel;
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
            }
            else
            {
                shooting();
            }
      
        
    }

   
    public void shooting()
    {
        if(currentMag == 0)
        {
            return;
        }
        if (!isShooting && Input.GetButton("Shoot"))
        {
            reaload = false;
            StartCoroutine(shoot());   
        }
    }
   


    IEnumerator shoot()
    {
        isShooting = true;
        if(!RayCastWeapon)
        {
            Instantiate(bullet, Barrel.position, Barrel.rotation);
            yield return new WaitForSeconds(ShootRate);
            isShooting = false;
            CountOfBullets(-1);
            gameManager.Instance.loadText(totalAmmo, currentMag);
        }
        else
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, RayGunDist))
            {
                Damage damage = hit.collider.GetComponent<Damage>();
                if(damage != null)
                {
                    damage.TakeDamage(RayGunDamage);
                }
            }
            yield return new WaitForSeconds(ShootRate);
            isShooting = false;
            CountOfBullets(-1);
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

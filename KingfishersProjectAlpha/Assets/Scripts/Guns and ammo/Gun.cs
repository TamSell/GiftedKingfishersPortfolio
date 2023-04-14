using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UIElements;

public class Gun : MonoBehaviour
{
   
    [Range(0.1f,2)][SerializeField] float ShootRate;
    [Range(0,30)][SerializeField] int MagazineInGun;
    [Range(0,60)][SerializeField] int TotalAmmo;
    [SerializeField] int realoadSpeed;


    [SerializeField]bool reaload;

  
    int MagTotalAmmo;
    public bool isShooting;
    int TempAmmo;
    int ammoToInsert;
    public GameObject bullet ;
    public Transform gun;

   
    float timer;

    

    // Start is called before the first frame update
    void Start()
    {
        MagTotalAmmo = MagazineInGun;
       
    }

    // Update is called once per frame
    void Update()
    {
        if(MagazineInGun>MagTotalAmmo)
        {
            MagazineInGun = MagTotalAmmo;
        }
        timer += Time.deltaTime;
      

    
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
        if(MagazineInGun ==0)
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
        Instantiate(bullet, gun.position,gun.rotation);
    
        yield return new WaitForSeconds(ShootRate);
        isShooting = false;
        CountOfBullets(-1);

    }

    public void CountOfBullets(int ammount)
    {
        MagazineInGun+= ammount;
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
            else if (MagazineInGun == 0 && TotalAmmo > 0)
            {
                reaload = true;
                RealoadingLogic();

            }
        }
      
    }
    public void RealoadingLogic()
    {
        if(TotalAmmo == 0)
        {
            return;
        }
         if (TotalAmmo >= MagTotalAmmo)
        {
            ammoToInsert = MagTotalAmmo - MagazineInGun;
            TotalAmmo -= ammoToInsert;
            MagazineInGun += ammoToInsert;
        }
        else if(MagTotalAmmo >= TotalAmmo)
        {
            ammoToInsert = MagTotalAmmo  - MagazineInGun;
            TempAmmo = TotalAmmo;
            TotalAmmo -= ammoToInsert;
            if(TotalAmmo <=0)
            {
                TotalAmmo = TempAmmo;
                MagazineInGun += TotalAmmo;
                TotalAmmo -= TotalAmmo;
                return;
            }
            MagazineInGun += ammoToInsert;
        }
        else if(MagazineInGun == 0)
        {
            ammoToInsert = TotalAmmo;
            TotalAmmo -= ammoToInsert;
            MagazineInGun += ammoToInsert;
        }
    }

    IEnumerator IsRealoading()
    {
       
        yield return new WaitForSeconds(1);
        Reloading();

    }


}

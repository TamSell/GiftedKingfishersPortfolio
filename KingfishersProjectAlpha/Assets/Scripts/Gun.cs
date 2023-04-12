using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Gun : MonoBehaviour
{
   
    [Range(0.1f,2)][SerializeField] float ShootRate;
    [Range(0,30)][SerializeField] int MagazineInGun;
    [Range(0,60)][SerializeField] int TotalAmmo;
    [SerializeField]bool reaload;

    float time = 1f;
    float timer;
    int GunTotalAmmo;
    bool isShooting;

    int ammoToInsert;
    public GameObject bullet ;
    public Transform gun;
  


    

    // Start is called before the first frame update
    void Start()
    {
        timer = Time.time;
        GunTotalAmmo = MagazineInGun;
    }

    // Update is called once per frame
    void Update()
    {
        if(MagazineInGun>GunTotalAmmo)
        {
            MagazineInGun = GunTotalAmmo;
        }

        Reloading();
  
        if (reaload == true)
        {
            Invoke("shooting", 2);
          
        }
        else
        {
            shooting();
          
        }
     

    }

    public void shooting()
    {
        if (!isShooting && Input.GetButton("Shoot"))
        {

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
        if (Input.GetButtonDown("Reloading"))
        {
            reaload = true;
            RealoadingLogic();
            IsReloading();
            reaload = false;
        }
        else if (MagazineInGun==0 && TotalAmmo>0)
        {
            reaload = true;
            RealoadingLogic();
            IsReloading();
            reaload=false;
         
        }
       
    }
    public void RealoadingLogic()
    {
        if (TotalAmmo > GunTotalAmmo)
        {
            ammoToInsert = GunTotalAmmo - MagazineInGun;
            TotalAmmo -= ammoToInsert;
            MagazineInGun += ammoToInsert;
        }
        else if(MagazineInGun > TotalAmmo)
        {
            ammoToInsert = MagazineInGun - TotalAmmo;
            TotalAmmo -= TotalAmmo;
            MagazineInGun += ammoToInsert;
        }
        else if(MagazineInGun == 0)
        {
            ammoToInsert = TotalAmmo;
            TotalAmmo -= TotalAmmo;
            MagazineInGun += ammoToInsert;
        }
    }

    IEnumerator IsReloading()
    {
        reaload = true;
        yield return new WaitForSeconds(2);
        reaload = false;

    }



}

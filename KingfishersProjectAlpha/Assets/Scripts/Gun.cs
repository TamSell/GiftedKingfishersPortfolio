using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Gun : MonoBehaviour
{
   
    [Range(0.1f,2)][SerializeField] float ShootRate;
    [Range(0,30)][SerializeField] int MagazineInGun;
    [Range(0,60)][SerializeField] int ammoCount;
    
    
    
    int GunTotalAmmo;
    [SerializeField]bool isShooting;

    int ammoToInsert;
    public GameObject bullet ;
    public Transform gun;

    

    // Start is called before the first frame update
    void Start()
    {
        GunTotalAmmo = MagazineInGun;
    }

    // Update is called once per frame
    void Update()
    {
      Reloading();
        if (MagazineInGun == 0)
        {
            return;
        }
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
            if (ammoCount >= GunTotalAmmo)
            {
                ammoToInsert = GunTotalAmmo - MagazineInGun;
                ammoCount -= ammoToInsert;
                MagazineInGun += ammoToInsert;
            }
            else
            {
                ammoToInsert = ammoCount;
                ammoCount -= ammoCount;
                MagazineInGun += ammoToInsert;
            }
        }

    }





}

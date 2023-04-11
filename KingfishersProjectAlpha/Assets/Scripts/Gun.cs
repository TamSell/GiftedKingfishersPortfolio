using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] int ShootDamage;
    [SerializeField] float ShootRate;
    [SerializeField] int ShootDist;
    [Range(0,30)][SerializeField] int bulletCount;
    [SerializeField]bool isShooting;

    public GameObject bullet ;
    public Transform gun;

    

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (bulletCount == 0)
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
        bulletCount+= ammount;
    }

}

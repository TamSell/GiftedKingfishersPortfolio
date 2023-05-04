using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Gun : MonoBehaviour
{
    [SerializeField] Transform cam;
    [SerializeField] AudioSource aud;
    [Header("----Gun Basic Stats-----")]
    [Range(0, 2)][SerializeField] public float ShootRate;
    [SerializeField] public int realoadSpeed;
    [SerializeField] public bool reaload;
    [SerializeField] GameObject hitEffect;

    [Header("-----Sniper Stats------")]
    [SerializeField] public float RayGunDist;
    [SerializeField] public int RayGunDamage;
    [SerializeField] public GameObject RayGunEffect;

    [Header("---ShotGun-----")]
    [SerializeField] public float ShotGunDist;
    [SerializeField] public int ShotGunDamage;
    [SerializeField] public bool shotgun;
    [SerializeField] public int bulletPerShot;

    [Header("---Impulse Setting----")]
    [SerializeField] public float ImpulseTime;
    [SerializeField] public int ImpulseSpeed;


    [Header("----- Ammo -----")]
    [Range(0, 30)][SerializeField] public int magSize;
    [Range(0, 300)][SerializeField] public int totalAmmo;

    [Header("----Audio Clip----")]
    [SerializeField] public AudioClip GunShot;
    [SerializeField] public float gunShotVol;


    public bool isShooting;
    public int currentMag;
    public BulletSpeed bulletVals;
    [SerializeField] public GameObject bullet;
    [SerializeField] public Transform Barrel;
    public bool Sniper;
    GameObject DestroyEffect;
    




    private void Awake()
    {
        cam = Camera.main.transform;
    }

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
            if (Sniper)
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
           // if (gameManager.Instance.playerController.enabled == true)
         //   {
                StartCoroutine(shoot());
         //  }
        }


    }

    public void RayCastSetActive()
    {
        if (currentMag == 0)
        {

            return;
        }
        if (RayGunEffect)
        {
            if (Sniper && Input.GetButton("Shoot"))
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
        if (Sniper)
        {
            CountOfBullets(-1);
            gameManager.Instance.loadText(totalAmmo, currentMag);
            aud.PlayOneShot(GunShot, gunShotVol);
            ShootImpulse();
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, RayGunDist))
            {
                if (hit.collider.CompareTag("Player"))
                {
                   yield return null;   
                }
                else
                {
                    if (hitEffect)
                    {
                        DestroyEffect = Instantiate(hitEffect, hit.point, transform.rotation);
                        Destroy(DestroyEffect, 2);
                    }

                    Damage damage = hit.collider.GetComponent<Damage>();
                    if (damage != null)
                    {
                        damage.TakeDamage(RayGunDamage);
                    }
                }

               
            }
            yield return new WaitForSeconds(ShootRate);
       
            isShooting = false;
           
          
        }
        else if (shotgun)
        {
            CountOfBullets(-1);
            gameManager.Instance.loadText(totalAmmo, currentMag);
            ShootImpulse();
            for (int i = 0; i < bulletPerShot; i++)
            {
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(UnityEngine.Random.Range(0.5f, 0.6f), UnityEngine.Random.Range(0.5f, 0.6f))), out hit, ShotGunDist))
                {

                    if(hit.collider.CompareTag("Player"))
                    {
                        break;
                    }
                    if (hitEffect)
                    {
                        DestroyEffect = Instantiate(hitEffect, hit.point, transform.rotation);
                        Destroy(DestroyEffect, 2);
                    }


                    Damage damage = hit.collider.GetComponent<Damage>();
                    if (damage != null)
                    {
                        damage.TakeDamage(ShotGunDamage);
                    }
                }
            }
            yield return new WaitForSeconds(ShootRate);
            
            isShooting = false;
          
           
        }
        else
        {

            aud.PlayOneShot(GunShot, gunShotVol);
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
        if (!isShooting)
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

        if (totalAmmo == 0)
        {
            return;
        }
        else
        {
            if (totalAmmo >= magSize)
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

    public void AddAmmo(int ammount)
    {
        totalAmmo += ammount;
        gameManager.Instance.loadText(totalAmmo, currentMag);
    }




    void ShootImpulse()
    {
       
        if (isShooting)
        {

            StartCoroutine(Impulse());

        }

    }
    IEnumerator Impulse()
    {
     
        float startTime = Time.time;

        while (Time.time < startTime + ImpulseTime)
        {
            gameManager.Instance.playerController.controller.Move(-cam.forward * Time.deltaTime * ImpulseSpeed);

            yield return new WaitForEndOfFrame();
        }
     
    }

}
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] int ShootDamage;
    [SerializeField] float ShootRate;
    [SerializeField] int ShootDist;

    [SerializeField]bool isShooting;

    public GameObject bullet ;
    public Transform gun;
    public int selectedWeapon = 0;

    // Start is called before the first frame update
    void Start()
    {
        SelectWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        int previousSelectedWeapon = selectedWeapon;

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedWeapon = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedWeapon = 1;
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


     //   RaycastHit hit;

      //  if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, ShootDist))
    //    {
      //      Damage TakeHit = hit.collider.GetComponent<Damage>();

        //    if (TakeHit != null)
        //    {
        //        TakeHit.TakeDamage(ShootDamage);
        //    }
        //}
        yield return new WaitForSeconds(ShootRate);
        isShooting = false;
    }

    void SelectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if(i == selectedWeapon)
                weapon.gameObject.SetActive(true);
            else
                weapon.gameObject.SetActive(false);
            i++;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] int ShootDamage;
    [SerializeField] float ShootRate;
    [SerializeField] int ShootDist;

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
        if (!isShooting && Input.GetButton("Shoot"))
        {
            
            StartCoroutine(shoot());
        }
    }

    IEnumerator shoot()
    {
        isShooting = true;

        Instantiate(bullet, gun.position,gun.rotation);


        RaycastHit hit;

        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, ShootDist))
        {
            Damage TakeHit = hit.collider.GetComponent<Damage>();

            if (TakeHit != null)
            {
                TakeHit.TakeDamage(ShootDamage);
            }
        }
        yield return new WaitForSeconds(ShootRate);
        isShooting = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDamage : MonoBehaviour
{
    [SerializeField] int damage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    //private void OnTriggerEnter(Collider other)
    //{
    //    if(other.isTrigger)
    //    {
    //        return;
    //    }

    //    Damage canDamage = other.GetComponent<Damage>();

    //    if (canDamage != null)
    //    {
    //        canDamage.TakeDamage(damage);
    //    }
    //}
}

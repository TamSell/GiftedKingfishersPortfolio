using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    [Header("----Melee Settings----")]
    [SerializeField] int damage;

    [SerializeField] CapsuleCollider meleeZone;

    GameObject effect;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void OnTriggerEnter(Collider other)
    {

        if (other.isTrigger)
        {
            return;
        }
 
       Damage canDamage = other.GetComponent<Damage>();

        if (canDamage != null)
        {
            canDamage.TakeDamage(damage);
        }

    }
}

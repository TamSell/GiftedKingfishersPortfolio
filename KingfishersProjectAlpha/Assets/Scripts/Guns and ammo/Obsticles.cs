using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obsticles : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] Collider box;

    private void OnTriggerEnter(Collider other)
    {
        Damage dam = other.GetComponent<Damage>();
        dam.TakeDamage(damage);

    }

}

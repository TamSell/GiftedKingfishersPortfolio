using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBullet : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] float projSpeed;
    private void Start()
    {
        Destroy(this.gameObject, 5);
    }
    void Update()
    {
        this.transform.position += transform.forward * projSpeed * Time.deltaTime;
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
        Destroy(gameObject);
    }
}

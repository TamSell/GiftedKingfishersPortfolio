using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyThrow : MonoBehaviour
{
    [Header("----Bullet Settigns-----")]
    [SerializeField] float timer;
    [SerializeField] int damage;
    [SerializeField] GameObject TriggerEffect;


    [SerializeField] int spin;
    [SerializeField] bool IsSpinning;
    [SerializeField] GameObject model;
    GameObject effect;
    float timeToDestroy;


    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, timer);
    }

    // Update is called once per frame

    public void Update()
    {
        if (IsSpinning)
        {
            transform.Rotate(0, 0, Time.deltaTime * spin);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
            Destroy(gameObject);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            Destroy(gameObject);

    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
        {
            return;
        }
        if (TriggerEffect)
        {
            StartCoroutine(hitEffect());
            effect = Instantiate(TriggerEffect, transform.position, TriggerEffect.transform.rotation);

            Destroy(effect, 5);
        }


        Damage canDamage = other.GetComponent<Damage>();

        if (canDamage != null)
        {
            canDamage.TakeDamage(damage);
            Debug.Log("Tumama");
        }
        Destroy(gameObject);
    }


    IEnumerator hitEffect()
    {
        TriggerEffect.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        TriggerEffect.SetActive(false);


    }
}

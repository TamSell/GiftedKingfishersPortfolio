using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyThrow : MonoBehaviour
{
    [Header("----Bullet Settigns-----")]
    [SerializeField] float timer;
    [SerializeField] int damage;
    [SerializeField] GameObject TriggerEffect;
    bool IsEffecting;
    GameObject effect;
    float timeToDestroy;
    bool IsFollow;


    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, timer);
    }

    // Update is called once per frame

    public void Update()
    {
        //if (!IsFollow)
        //{
        //    StartCoroutine(bulletTracker());
        //}
        if (!IsEffecting)
        {
            StartCoroutine(hitEffect());
            effect = Instantiate(TriggerEffect, transform.position, TriggerEffect.transform.rotation);
            Destroy(effect, 0.25f);
        }

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

    IEnumerator bulletTracker()
    {
        IsFollow = true;
        transform.LookAt(gameManager.Instance.playerController.transform.position);
        yield return new WaitForSeconds(0.1f);
        IsFollow= false;
    }

    IEnumerator hitEffect()
    {
        IsEffecting = true;
        TriggerEffect.SetActive(true);

        yield return new WaitForSeconds(0.05f);

        TriggerEffect.SetActive(false);

       // yield return new WaitForSeconds(0.5f);

        IsEffecting = false;

    }
}

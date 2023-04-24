using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpeed : MonoBehaviour
{
    [Header("----Bullet Settigns-----")]
    [SerializeField] float speed;
    [SerializeField] float timer;
    [SerializeField] int damage;
    [SerializeField] GameObject TriggerEffect;

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
        this.transform.Translate(0, 0, Time.deltaTime * speed);

        if (Input.GetKeyDown(KeyCode.Alpha1))
            Destroy(gameObject);
        else if(Input.GetKeyDown(KeyCode.Alpha2))
            Destroy(gameObject);

    }
    public void OnTriggerEnter(Collider other)
    {
        if(other.isTrigger)
        {
            return;
        }
        if (TriggerEffect)
        {
            StartCoroutine(hitEffect());
            effect = Instantiate(TriggerEffect, transform.position, gameManager.Instance.PlayerModel.transform.rotation);

            Destroy(effect, 5);
        }


        Damage canDamage = other.GetComponent<Damage>();

        if (canDamage != null)
        {
            canDamage.TakeDamage(damage);
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

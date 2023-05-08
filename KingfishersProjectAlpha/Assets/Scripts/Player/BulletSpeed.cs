using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpeed : MonoBehaviour
{
    [Header("----Bullet Settigns-----")]
    [SerializeField] float speed;
    [SerializeField] float timer;
    float Energy
    {
        get => gameManager.Instance.playerController.Enery;
        set => gameManager.Instance.playerController.Enery = value;
    }

    [SerializeField] public int BasicDamage;
    [SerializeField] public int LowDamage;
    [SerializeField] public int MediumDamage;
    [SerializeField] public int HighDamage;
    [SerializeField] GameObject TriggerEffect;


    [SerializeField] int spin;
    [SerializeField]  bool IsSpinning;
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
        if(IsSpinning)
        {
            transform.Rotate(0, 0, Time.deltaTime * spin);  
        }

        transform.Translate(0, 0, Time.deltaTime * speed);

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
            effect = Instantiate(TriggerEffect, transform.position, TriggerEffect.transform.rotation);

            Destroy(effect, 5);
        }


        Damage canDamage = other.GetComponent<Damage>();

        if (canDamage != null)
        {
           
            canDamage.TakeDamage(DamageDependingOnEnergy());
            
        }
        Destroy(gameObject);
    }

    int DamageDependingOnEnergy()
    {
       
        if (Energy >= 75)
        {
            return HighDamage;
        }
        else if (Energy >= 50)
        {
            return MediumDamage;
        }
        else if (Energy >= 25)

        {
            return HighDamage;
        }
        else
        {
            return BasicDamage;
        }
    }



    IEnumerator hitEffect()
    {
        TriggerEffect.SetActive(true);
        
        yield return new WaitForSeconds(0.5f);
       
        TriggerEffect.SetActive(false);
    }

}

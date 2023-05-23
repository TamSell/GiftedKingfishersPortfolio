using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class supportItem : MonoBehaviour
{
    public GameObject drop;
    public bool Health;
    [SerializeField] int addIt;
    [SerializeField] GameObject ItemEffect;
    [SerializeField] AudioSource aud;
    [SerializeField] int DestroyTime;
    GameObject DestroyEffect;
    [SerializeField] MeshRenderer mesh;
    

    [Header("-----Sound Effect-----")]
    [SerializeField] AudioClip SoundEffect;
    [Range(0,1)][SerializeField] float Volume;
    


    private void Update()
    {
        transform.Rotate(0,1,0,Space.Self);
        Destroy(gameObject, DestroyTime);
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(Health)
            {
                Effect();
                Instantiate(SoundEffect, transform.position, drop.transform.rotation);
                aud.PlayOneShot(SoundEffect, Volume);
           
                if(gameManager.Instance.playerController.origHP > gameManager.Instance.playerController.HP + addIt)
                    gameManager.Instance.playerController.HP += addIt;
                else
                    gameManager.Instance.playerController.HP = gameManager.Instance.playerController.origHP;
            }
            else
            {
                Effect();
                aud.PlayOneShot(SoundEffect, Volume);
               
                gameManager.Instance.playerController.currentGun.totalAmmo += addIt;
            }
            drop.GetComponentInChildren<Renderer>().enabled = false;
            
            Destroy(gameObject,3);
        }
    }
    void Effect()
    {
        if(ItemEffect)
        {
            DestroyEffect = Instantiate(ItemEffect, drop.transform.position, transform.rotation);
            Destroy(DestroyEffect, 1);
        }
    }
}

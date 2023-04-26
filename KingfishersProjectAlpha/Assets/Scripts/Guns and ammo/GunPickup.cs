using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPickup : MonoBehaviour
{
    [SerializeField] Gun gun;
    [SerializeField] MeshFilter model;
    [SerializeField] MeshRenderer mat;


    // Start is called before the first frame update
    void Start()
    {
        model.mesh = gun.GetComponent<MeshFilter>().sharedMesh;
        mat.material = gun.GetComponent<MeshRenderer>().sharedMaterial;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.Instance.playerController.gunPickup(gun);
            Destroy(gameObject);
        }
    }
}

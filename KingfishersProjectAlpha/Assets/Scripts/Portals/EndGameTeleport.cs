using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameTeleport : MonoBehaviour
{
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {

        }
    }
}

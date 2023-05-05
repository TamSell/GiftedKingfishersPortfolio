using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triggercamera : MonoBehaviour
{
    [SerializeField] Portal_camera portalCam;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            portalCam.playerHere();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            portalCam.playerHere();
        }
    }
}

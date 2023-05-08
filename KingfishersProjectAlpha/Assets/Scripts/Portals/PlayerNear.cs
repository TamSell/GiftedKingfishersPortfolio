using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNear : MonoBehaviour
{
    [SerializeField] MirrorPlayerPosition Positioner;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Positioner.PlayerNear();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Positioner.PlayerNear();
        }
    }
}

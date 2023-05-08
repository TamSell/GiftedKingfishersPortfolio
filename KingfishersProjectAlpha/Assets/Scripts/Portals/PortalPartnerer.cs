using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalPartnerer : MonoBehaviour
{
    [SerializeField] MirrorPlayerPosition PortalA;
    [SerializeField] MirrorPlayerPosition PortalB;

    private void Update()
    {
        if(PortalA.playerNear)
        {
            PortalB.playerNear = false;
        }
        if(PortalA.playerNear)
        {
            PortalA.playerNear = false;
        }
    }
}

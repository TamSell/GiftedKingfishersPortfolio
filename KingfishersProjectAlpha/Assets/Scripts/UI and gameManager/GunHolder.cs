using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunHolder : MonoBehaviour
{
    public GunStats2 gunHeld;

    private void Awake()
    {
        gunHeld = gameManager.Instance.currentGunAspects;
    }
}

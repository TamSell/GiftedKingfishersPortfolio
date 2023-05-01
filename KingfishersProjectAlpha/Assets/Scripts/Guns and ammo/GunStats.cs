using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class GunStats : ScriptableObject
{
    public float ShootRate;
    public int realoadSpeed;
    public bool reaload;
    public int magSize;
    public int totalAmmo;
    public GameObject model;
}

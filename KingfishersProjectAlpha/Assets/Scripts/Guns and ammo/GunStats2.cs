using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class GunStats2 : Item
{
    public float ShootRate;
    public float shootRange;
    public float recoil;
    public int realoadSpeed;
    public bool reaload;
    public int magSize;
    public int totalAmmo;
    public Item[] Attachments = new Item[4];
    public GameObject Muzzle;
    public GameObject Sight;
    public GameObject Magazine;
    public GameObject Stock;
    public GameObject Body;
}

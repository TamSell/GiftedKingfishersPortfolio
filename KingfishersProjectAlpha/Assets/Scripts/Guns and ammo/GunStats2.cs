using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class GunStats2 : Item
{
    public float damage;
    public float shootRate;
    public float shootRange;
    public float recoil;
    public float realoadSpeed;
    public bool reaload;
    public float magSize;
    public float totalAmmo;
    public Attachment[] Attachments = new Attachment[4];
    public GameObject Muzzle;
    public GameObject Sight;
    public GameObject Magazine;
    public GameObject Stock;
    public GameObject Body;
}

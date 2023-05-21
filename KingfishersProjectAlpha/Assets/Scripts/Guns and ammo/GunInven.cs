using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class GunInven : ScriptableObject
{
    [SerializeField] public GunStats2[] primaryGuns;
    [SerializeField] public GunStats2[] secondaryGuns;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Attachment : Item
{
    [Header("---ModifiedStats---")]
    /*
     * 0 - Damage
     * 1 - Range
     * 2 - ReloadSpeed
     * 3 - MagazineSize
     * 4 - Recoil
     * 5 - Rate of Fire
     * 6 - Sway?
    */
    public float[] modifiedStats = new float[7];
    [Header("---Where it is Attached---")]
    public int position;
    public bool equipped = false;
}

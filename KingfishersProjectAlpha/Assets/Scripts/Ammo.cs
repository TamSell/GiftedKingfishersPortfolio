using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Ammo : MonoBehaviour
{
    [Header("----- Ammo -----")]
    public int totalAmmunition;
    public int magSize;

    [Header("----- Ammo Display -----")]
    public TextMeshProUGUI mag;
    public TextMeshProUGUI reserve;

    public int currentMag;

    void Start()
    {
        reload();
    }

    void reload()
    {
        currentMag = magSize;
        totalAmmunition = totalAmmunition - (magSize-currentMag);
        reserve.text = totalAmmunition.ToString("F0");
        mag.text = currentMag.ToString("F0");
    }
}

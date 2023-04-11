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

    private int currentMag;

    void Start()
    {
        reload();
    }

    void reload()
    {
        currentMag = magSize;
        totalAmmunition -= magSize;
        reserve.text = totalAmmunition.ToString("0F");
        mag.text = currentMag.ToString("0F");
    }
}

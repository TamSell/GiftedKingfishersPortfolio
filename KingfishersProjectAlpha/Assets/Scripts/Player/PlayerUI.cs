using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] PlayerController2 player;
    public void PlayerUpdateUI()
    {
        gameManager.Instance.HPbar.fillAmount = (float)player.HP / player.origHP;
        gameManager.Instance.EnergyBar.fillAmount = player.currentEnergy / player.energyMax;
    }
}

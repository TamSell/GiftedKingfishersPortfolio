using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] PlayerController2 player;
    public void PlayerUpdateUI()
    {
        gameManager.Instance.SBar.fillAmount = player.stamina / player.origStamina;
        gameManager.Instance.HPbar.fillAmount = (float)player.HP / player.origHP;
        gameManager.Instance.Speedbar.fillAmount = player.currentEnergy / player.energyMax;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("------ Player ------")]
    GameObject PlayerModel;
    PlayerController playerController;
    

    [Header("------ UI Elements ------")]
    GameObject LostMenu;
    GameObject WinMenu;
    GameObject PauseMenu;

    private void Awake()
    {
        playerController = PlayerModel.GetComponent<PlayerController>();
    }
}

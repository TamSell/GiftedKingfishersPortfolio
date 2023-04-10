using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class gameManager : MonoBehaviour
{
    public static gameManager Instance;

    [Header("------ Player ------")]
    public GameObject PlayerModel;
    public PlayerController playerController;


    [Header("------ UI Elements ------")]
    private GameObject activeMenu;
    public GameObject LostMenu;
    public GameObject WinMenu;
    public GameObject PauseMenu;
    public Image HPbar;
    public TextMeshProUGUI enemyCount;

    public bool inMenu;
    int enemyRemain;
    float timeScaleO;

    void Awake()
    {
        Instance = this;
        PlayerModel = GameObject.FindGameObjectWithTag("Player");
        playerController = PlayerModel.GetComponent<PlayerController>();
        timeScaleO = Time.timeScale;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel") && activeMenu == null)
        {
            inMenu = !inMenu;
            setMenu(PauseMenu);
            if (inMenu)
            {
                pause();
            }
            else
            {
                unpause();
            }

        }
    }
    public void pause()
    {
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void unpause()
    {
        Time.timeScale = timeScaleO;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        activeMenu.SetActive(false);
        activeMenu = null;
    }

    public void updateGoal(int amount)
    {
        enemyRemain += amount;
        enemyCount.text = enemyRemain.ToString("0F");
        if (enemyRemain <= 0)
        {
            setMenu(WinMenu);
            pause();
        }
    }

    public void death()
    {
        pause();
        setMenu(LostMenu);
    }

    private void setMenu(GameObject menu)
    {
        activeMenu = menu;
        activeMenu.SetActive(true);

    }
}
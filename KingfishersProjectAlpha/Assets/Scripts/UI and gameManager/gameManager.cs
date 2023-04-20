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
    public GameObject playerSpawnPos;


    [Header("------ UI Elements ------")]
    private GameObject activeMenu;
    public GameObject LostMenu;
    public GameObject WinMenu;
    public GameObject PauseMenu;
    public Image HPbar;
    public Image SBar;
    public TextMeshProUGUI enemyCount;
    public GameObject reticle;
    public TextMeshProUGUI mag;
    public TextMeshProUGUI reserve;

    public bool inMenu;
    int enemyRemain;
    float timeScaleO;

    void Awake()
    {
        Instance = this;
        PlayerModel = GameObject.FindGameObjectWithTag("Player");
        playerController = PlayerModel.GetComponent<PlayerController>();
        playerSpawnPos = GameObject.FindGameObjectWithTag("Player Spawn Pos");
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
        reticle.SetActive(false);
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void unpause()
    {
        reticle.SetActive(true);
        Time.timeScale = timeScaleO;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        activeMenu.SetActive(false);
        activeMenu = null;
    }

    public void updateGoal(int amount)
    {
        enemyRemain += amount;
        enemyCount.text = enemyRemain.ToString("F0");
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

    public void loadText(int totalAmmo, int currentMag)
    {
        reserve.text = totalAmmo.ToString("F0");
        mag.text = currentMag.ToString("F0");
    }
}
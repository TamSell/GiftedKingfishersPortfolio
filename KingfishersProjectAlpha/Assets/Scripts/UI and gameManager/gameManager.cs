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
    [SerializeField] GameObject Inventory;
    [SerializeField] GameObject CraftIt;
    public Inventory inven;
    public TextMeshProUGUI invenDesc;
    public TextMeshProUGUI invenName;
    public Image invenIcon;
    private GameObject activeMenu;
    public GameObject LostMenu;
    public GameObject WinMenu;
    public GameObject PauseMenu;
    public GameObject MainMenu;
    public Image HPbar;
    public Image HPbarBack;
    public Image SBar;
    public Image Speedbar;
    public Image SpeedbarBack;
    public TextMeshProUGUI enemyCountTitle;
    public TextMeshProUGUI enemyCount;
    public GameObject reticle;
    public TextMeshProUGUI magDivReserve;
    public TextMeshProUGUI mag;
    public TextMeshProUGUI reserve;

    public bool isNear;
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
        if (Input.GetButtonDown("Cancel") && (activeMenu == null || activeMenu == PauseMenu))
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
        if (Input.GetButtonDown("Inventory") && (activeMenu == null || activeMenu == Inventory))
        {
            inMenu = !inMenu;
            setMenu(Inventory);
            if (inMenu)
            {
                pause();
            }
            else
            {
                unpause();
            }
        }
        if(Input.GetButtonDown("Interact") && (activeMenu==null || activeMenu == CraftIt) && isNear)
        {
            inMenu = !inMenu;
            setMenu(CraftIt);
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
        turnOffUI();
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void unpause()
    {
        turnOnUI();
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

    public Item SelectItem(int index)
    {
        return inven.InvenSelect(index);
    }

    public void addGun(Gun gun)
    {
        Item gunItem = null;
        gunItem.name = gun.name;
        gunItem.amount = 1;
        if (gun.RayGunDamage != 0)
        {
            gunItem.description = "Damage: " + gun.RayGunDamage.ToString() + "\n";
        }
        else
        {
            gunItem.description = "Damage: " + gun.bulletVals.BasicDamage + "\n";
        }
        gunItem.description += "Magazine Size: " + gun.magSize.ToString() + "\n" + "Reserve Ammo: " + gun.totalAmmo.ToString();
        inven.InvenAdd(gunItem);
    }

    public void DisplayItem(Item _item)
    {
        invenName.text = _item.name;
        invenDesc.text = _item.description;
        invenIcon = _item.icon;
    }

    public void turnOffUI()
    {
        SBar.enabled = false;
        HPbar.enabled = false;
        HPbarBack.enabled = false;
        Speedbar.enabled = false;
        SpeedbarBack.enabled = false;
        reticle.SetActive(false);
        enemyCountTitle.enabled = false;
        enemyCount.enabled = false;
        magDivReserve.enabled = false;
        mag.enabled = false;
        reserve.enabled = false;
    }

    public void turnOnUI()
    {
        SBar.enabled = true;
        HPbar.enabled = true;
        HPbarBack.enabled = true;
        Speedbar.enabled = true;
        SpeedbarBack.enabled = true;
        reticle.SetActive(true);
        enemyCountTitle.enabled = true;
        enemyCount.enabled = true;
        magDivReserve.enabled = true;
        mag.enabled = true;
        reserve.enabled = true;
    }

    IEnumerator Wait(float numSec)
    {
        yield return new WaitForSeconds(numSec);
    }
}
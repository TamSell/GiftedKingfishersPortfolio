using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class gameManager : MonoBehaviour
{
    public static gameManager Instance;

    [Header("------ Player ------")]
    public GameObject PlayerModel;
    public FinalPlayerController playerController;
    public Rigidbody rb;
    public GameObject playerSpawnPos;
    [SerializeField] public List<GunStats2> gunAspects;
    public GunStats2 currentGunAspects;
    public int currentGunIndex = 0;
    [SerializeField] public Crafting modify;


    [Header("------ UI Elements ------")]
    //[SerializeField] GameObject Inventory;
    [SerializeField] GameObject WeaponSwap;
    public Inventory inven;
    public TextMeshProUGUI invenDesc;
    public TextMeshProUGUI invenName;
    public Sprite invenIcon;
    private GameObject activeMenu;
    public GameObject LostMenu;
    public GameObject WinMenu;
    public GameObject PauseMenu;
    public GameObject MainMenu;
    public GameObject Settings;
    public AudioSource LevelMusic;
    public Image HPbar;
    public Image EnergyBar;
    public Image BarBack;
    public Image MomentumOverlay;
    public Slider ReloadBar;
    public TextMeshProUGUI enemyCountTitle;
    public TextMeshProUGUI enemyCount;
    public TextMeshProUGUI finalScore;
    public TextMeshProUGUI finalScoreWin;
    public TextMeshProUGUI enemiesEnded;
    public GameObject reticle;
    public TextMeshProUGUI magDivReserve;
    public TextMeshProUGUI mag;
    public TextMeshProUGUI reserve;

    public bool isNear;
    public bool inMenu;
    public int playerScore;
    public int enemyRemaining;
    public int enemyTotal;
    float timeScaleO;
    public GameObject pauseMenuButt, settingsMenuButt;
    private string Pause;

    void Awake()
    {
        Instance = this;
        PlayerModel = GameObject.FindGameObjectWithTag("Player");
        playerController = PlayerModel.GetComponent<FinalPlayerController>();
        playerSpawnPos = GameObject.FindGameObjectWithTag("Player Spawn Pos");
        timeScaleO = Time.timeScale;
        if (Application.platform != RuntimePlatform.WebGLPlayer)
        {
            Pause = "Cancel";
        }
        else
        {
            Pause = "WebGLPause";
        }
    }

    private void Start()
    {
        currentGunAspects = playerController.currentGun;
        modify.result.gunHeld = currentGunAspects;
        modify.ResetGun();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown(Pause) && (activeMenu == null || activeMenu == PauseMenu))
        {
            Settings.SetActive(false);
            inMenu = !inMenu;
           // LevelMusic.enabled = false;
            setMenu(PauseMenu);
            if (inMenu)
            {
                pause();
                LevelMusic.enabled = false;
            }
            else
            {
                unpause();
                LevelMusic.enabled = true;
            }
        }
        //if (Input.GetButtonDown("Inventory") && (activeMenu == null || activeMenu == Inventory))
        //{
        //    inMenu = !inMenu;
        //    setMenu(Inventory);
        //    if (inMenu)
        //    {
        //        pause();
        //    }
        //    else
        //    {
        //        unpause();
        //    }
        //}
        if(Input.GetButtonDown("Interact") && (activeMenu==null || activeMenu == WeaponSwap) && isNear)
        {
            inMenu = !inMenu;
            setMenu(WeaponSwap);
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
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(pauseMenuButt);
    }

    public void unpause()
    {
        turnOnUI();
        Time.timeScale = timeScaleO;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        activeMenu.SetActive(false);
        activeMenu = null;
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void updateGoal(int amount, int enemies)
    {
        playerScore += amount;
        enemyRemaining += enemies;
        if (enemies > 0)
        {
            enemyTotal += enemies;
        }
        enemyCount.text = playerScore.ToString("F0");
        enemiesEnded.text = enemyTotal.ToString("F0");
        finalScoreWin.text = playerScore.ToString("F0");
        if (enemyRemaining <= 0)
        {
            setMenu(WinMenu);
            enemyCount.enabled = false;
            enemyCountTitle.enabled = false;
        }
    }

    public void death()
    {
        pause();
        setMenu(LostMenu);
        LevelMusic.enabled = false;
        Time.timeScale = 0;
    }

    private void setMenu(GameObject menu)
    {
        activeMenu = menu;
        activeMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(settingsMenuButt);

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
        playerController.gotHitOverlay.SetActive(false);
        MomentumOverlay.enabled = false;
        HPbar.enabled = false;
        EnergyBar.enabled = false;
        BarBack.enabled = false;
        reticle.SetActive(false);
        enemyCountTitle.enabled = false;
        enemyCount.enabled = false;
        magDivReserve.enabled = false;
        mag.enabled = false;
        reserve.enabled = false;
    }

    public void turnOnUI()
    {
        HPbar.enabled = true;
        EnergyBar.enabled = true;
        BarBack.enabled = true;
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
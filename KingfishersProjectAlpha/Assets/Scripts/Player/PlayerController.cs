using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, Damage
{

    [Header("----- Components-----")]
    [SerializeField] CharacterController controller;

    [Header("----- Player Stats -----")]
    [Range(3, 8)][SerializeField] float PlayerSpeed;
    [Range(3, 30)][SerializeField] float jumpHeight;
    [Range(3, 25)][SerializeField] float gravityValue;
    [Range(1, 4)][SerializeField] int jumpMax;
    [Range(1, 20)][SerializeField] int HP;

    [Range(1, 200)][SerializeField] float FOVorg;
    [Range(1, 200)][SerializeField] float RunFOV;

    public List<Gun> gunList = new List<Gun>();

    [Header("-------Player Movement--------")]
    [Range(0, 10)][SerializeField] float RunSpeed;
    [Range(5, 250)][SerializeField] float MaxDashSpeed;
    [Range(5, 205)][SerializeField] float MidDashSpeed;
    [Range(5, 205)][SerializeField] float LowDashSpeed;
    [Range(0,30)][SerializeField] float MaxStamina;
    [SerializeField] float Stamina;

    public Gun usingGun;
    public MeshRenderer gunMaterial;
    public MeshFilter gunModel;




    public int selectedWeapon = 0;
    int jumpTimes;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float StaminaOrig;
    public Vector3 move;
    int HPorig;
    public bool isrunning;
    public bool isDashing;
    
    int dash;
   

    // Start is called before the first frame update
    void Start()
    {
       
        HPorig = HP;
        StaminaOrig = Stamina;
        PLayerUpdateUI();
        FOVorg = Camera.main.fieldOfView;
        respawnPlayer();
      
    }

    // Update is called once per frame
    void Update()
    {
        Dash();
        movement();
        selectGun();
    }

    void movement()
    {
        groundedPlayer = controller.isGrounded;
        if(groundedPlayer && playerVelocity.y<0)
        {
            playerVelocity.y = 0f;
            jumpTimes = 0;
        }

        Run();

        move = (transform.right * Input.GetAxis("Horizontal")) + (transform.forward * Input.GetAxis("Vertical"));
        controller.Move(move * Time.deltaTime * PlayerSpeed);

        Jump();
      
        playerVelocity.y -= gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        PLayerUpdateUI();
    }
    void Jump()
    {
        if (Input.GetButtonDown("Jump") && jumpTimes < jumpMax)
        {
            gameManager.Instance.SBar.enabled = true;
            jumpTimes++;
            playerVelocity.y = jumpHeight;
            Stamina -= 5;
        }
    }
    void Run()
    {
        if (move.x != 0 && move.z != 0)
        {
            if (Input.GetButton("Run"))
            {
                gameManager.Instance.SBar.enabled = true;
                isrunning = true;
                if (Stamina > 0 && isrunning)
                {
                    Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, RunFOV, Time.deltaTime * 2.5f);
                    controller.Move(move * Time.deltaTime * (PlayerSpeed + RunSpeed));
                    Stamina -= 3 * Time.deltaTime;

                }
                else if (Stamina <= 0)
                {
                    isrunning = false;
                    Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, FOVorg, Time.deltaTime * 2.5f);
                }

            }
            else
            {

                isrunning = false;
                Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, FOVorg, Time.deltaTime * 2.5f);
                controller.Move(move * Time.deltaTime * PlayerSpeed);
                StaminaRecovery();
            }
        }
        else
        {
            StaminaRecovery();
        }
    }

    void Dash()
    {
       if(move.x==0&&move.z==0)
        {
            return;
        }
        if(Input.GetButtonDown("Dash"))
        {
            gameManager.Instance.SBar.enabled = true;
            if (Stamina >=8)
            {
               
                controller.Move(move * Time.deltaTime * (PlayerSpeed + MaxDashSpeed));
                Stamina -= 12;
            }
            else if(Stamina >=5)
            {
                controller.Move(move * Time.deltaTime * (PlayerSpeed + MidDashSpeed));
                Stamina -= 7;
            }
            else if(Stamina >=3)
            {
                controller.Move(move * Time.deltaTime * (PlayerSpeed + LowDashSpeed));
                Stamina -= 5;
            }
        }

    }
    void StaminaRecovery()
    {
        if (Stamina < MaxStamina)
        {
            Stamina += 1 * Time.deltaTime;
        }
        else
        {
            gameManager.Instance.SBar.enabled = false;
        }
    }

    public void TakeDamage(int amount)
    {
        HP -= amount;
        PLayerUpdateUI();
        if(HP <= 0)
        {
            gameManager.Instance.death();
        }
    }
    
    void PLayerUpdateUI()
    {
        gameManager.Instance.SBar.fillAmount = Stamina / StaminaOrig;
        gameManager.Instance.HPbar.fillAmount = (float)HP / HPorig;
    }

    public void respawnPlayer()
    {
        HP = HPorig;
        PLayerUpdateUI();
        controller.enabled = false;
        transform.position = gameManager.Instance.playerSpawnPos.transform.position;
        controller.enabled = true;
    }

    public void gunPickup(Gun gun)
    {
        gunList.Add(gun);

        usingGun.currentMag = gun.currentMag;
        usingGun.bullet = gun.bullet;
        usingGun.Barrel = gun.Barrel;

        usingGun.magSize = gun.magSize;
        usingGun.totalAmmo = gun.totalAmmo;

        usingGun.RayGunDist = gun.RayGunDist;
        usingGun.RayGunDamage = gun.RayGunDamage;
        usingGun.RayGunEffect = gun.RayGunEffect;

        usingGun.ShootRate = gun.ShootRate;
        usingGun.realoadSpeed = gun.realoadSpeed;
        usingGun.reaload = gun.reaload;

        usingGun.RayCastWeapon = gun.RayCastWeapon;

        gunModel.mesh = gun.GetComponent<MeshFilter>().sharedMesh;
        gunMaterial.material = gun.GetComponent<MeshRenderer>().sharedMaterial;
    }

    void selectGun()
    {
        int previousSelectedWeapon = selectedWeapon;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedWeapon = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedWeapon = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            selectedWeapon = 2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            selectedWeapon = 3;
        }

        if (previousSelectedWeapon != selectedWeapon)
            SelectGun();
        //Invoke("SelectWeapon", 0.5f);  

        if (Input.GetAxis("Mouse ScrollWheel") > 0 && selectedWeapon < gunList.Count - 1)
        {
            selectedWeapon++;
            changeGun();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && selectedWeapon > 0)
        {
            selectedWeapon--;
            changeGun();
        }
    }
    void SelectGun()
    {
        int i = 0;
        foreach (Gun gun in gunList)
        {
            if (i == selectedWeapon)
                gunList[i].gameObject.SetActive(true);
            else
                gunList[i].gameObject.SetActive(false);
            i++;
        }
    }

    void changeGun()
    {
        usingGun.currentMag = gunList[selectedWeapon].currentMag;
        usingGun.bullet = gunList[selectedWeapon].bullet;
        usingGun.Barrel = gunList[selectedWeapon].Barrel;

        usingGun.magSize = gunList[selectedWeapon].magSize;
        usingGun.totalAmmo = gunList[selectedWeapon].totalAmmo;

        usingGun.RayGunDist = gunList[selectedWeapon].RayGunDist;
        usingGun.RayGunDamage = gunList[selectedWeapon].RayGunDamage;
        usingGun.RayGunEffect = gunList[selectedWeapon].RayGunEffect;

        usingGun.ShootRate = gunList[selectedWeapon].ShootRate;
        usingGun.realoadSpeed = gunList[selectedWeapon].realoadSpeed;
        usingGun.reaload = gunList[selectedWeapon].reaload;

        usingGun.RayCastWeapon = gunList[selectedWeapon].RayCastWeapon;

        gunModel.mesh = gunList[selectedWeapon].GetComponent<MeshFilter>().sharedMesh;
        gunMaterial.material = gunList[selectedWeapon].GetComponent<MeshRenderer>().sharedMaterial;
    }
}

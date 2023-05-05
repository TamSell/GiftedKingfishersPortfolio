using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, Damage
{

    [Header("----- Components-----")]
    [SerializeField] public CharacterController controller;
    [SerializeField] AudioSource aud;

    [Header("----- Player Stats -----")]
    [Range(3, 8)][SerializeField] float PlayerSpeed;
    [Range(3, 30)][SerializeField] float jumpHeight;
    [Range(3, 25)][SerializeField] float gravityValue;
    [Range(1, 4)][SerializeField] int jumpMax;
    [Range(1, 20)][SerializeField] public int HP;

    [Range(1, 200)][SerializeField] float FOVorg;
    [Range(1, 200)][SerializeField] float RunFOV;

    public List<Gun> gunList = new List<Gun>();

    [Header("-------Player Movement--------")]
    [Range(0, 10)][SerializeField] float RunSpeed;
    [Range(5, 250)][SerializeField] float DashSpeed;
    [Range(0, 1)][SerializeField] float DashTime;
    [Range(0,30)][SerializeField] float MaxStamina;
    [SerializeField] float Stamina;
    [SerializeField] public float speed;
    [SerializeField] public float Enery;
    [SerializeField] float maxEnergy;
   

    [Header("------ Audio ------")]
    [SerializeField] AudioClip[] audSteps;
    [Range(0, 1)][SerializeField] float audStepsVol;
    [SerializeField] AudioClip[] audJump;
    [Range(0, 1)][SerializeField] float audJumpVol;
    [SerializeField] AudioClip[] auddamage;
    [Range(0, 1)][SerializeField] float auddamageVol;

    [Header("----GUN PICK UP-----")]
    public Gun usingGun;
    public MeshRenderer gunMaterial;
    public MeshFilter gunModel;

    [Header("---- CoolDown----")]
    [SerializeField] bool DashReady;
    [SerializeField] public float DashCD;
    [SerializeField]float maxCD;


    public int selectedWeapon = 0;
    bool isPlaying;
    bool isPlayingSteps;
    int jumpTimes;
    public Vector3 playerVelocity;
    private bool groundedPlayer;
    private float StaminaOrig;
    public Vector3 move;
    public int HPorig;
    public bool isrunning;
    public bool isDashing;
   
    
   

    // Start is called before the first frame update
    void Start()
    {
        DashCD = 6;
        DashReady = true;
        StartCoroutine(CalculateSpeed());
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
        EnergyBuildUp();
        CD(isDashing);
    }

    void CD(bool ability)
    {
        if(ability ==false && DashCD < maxCD)
        {
            DashCD += 1 * Time.deltaTime;
        }
    }


    void movement()
    {
        groundedPlayer = controller.isGrounded;
        if(groundedPlayer)
        {
            if(!isPlayingSteps && move.normalized.magnitude >0.5)
            {
                StartCoroutine(MoveSound());
            }
            if (playerVelocity.y < 0)
            {
                playerVelocity.y = 0f;
                jumpTimes = 0;
            }
        }

       

        Run();

        move = (transform.right * Input.GetAxis("Horizontal")) + (transform.forward * Input.GetAxis("Vertical"));
        controller.Move(move * Time.deltaTime * PlayerSpeed);

        Jump();
      
        playerVelocity.y -= gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        PLayerUpdateUI();
    }

    IEnumerator MoveSound()
    {
        isPlayingSteps = true;

        aud.PlayOneShot(audSteps[Random.Range(0, audSteps.Length)], audStepsVol);
      
        if(isrunning)
        {
           yield return new  WaitForSeconds(0.3f);
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
        }

        isPlayingSteps = false;
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && jumpTimes < jumpMax)
        {
            aud.PlayOneShot(audJump[Random.Range(0, audJump.Length)], audJumpVol);
            gameManager.Instance.SBar.enabled = true;
            jumpTimes++;
            playerVelocity.y = jumpHeight;
      
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
        if(DashCD >5)
        {
            DashReady = true;
        }
       if(move.x==0&&move.z==0)
        {
            return;
        }
        if(Input.GetButtonDown("Dash"))
        {
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, RunFOV, Time.deltaTime * 0.5f);
            gameManager.Instance.SBar.enabled = true;
            if (DashReady)
            {
               
              StartCoroutine(Dashing());
               
            }
        
        }

    }
    IEnumerator Dashing()
    {
        isDashing = true;
        float startTime = Time.time;
        DashCD = 0;
        DashReady = false;
        while( Time.time< startTime + DashTime)
        {
            controller.Move(move * Time.deltaTime * DashSpeed);
        
           yield return null;
        }
        isDashing=false;
        
       
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
        aud.PlayOneShot(auddamage[Random.Range(0, auddamage.Length)], auddamageVol);
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
       
       usingGun.Sniper = gun.Sniper;
       
       usingGun = gun;
       
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
       
       usingGun.Sniper = gunList[selectedWeapon].Sniper;
       
       usingGun.GunShot = gunList[selectedWeapon].GunShot;
       usingGun.gunShotVol = gunList[selectedWeapon].gunShotVol;
       
       usingGun = gunList[selectedWeapon];
       
       gunModel.mesh = gunList[selectedWeapon].GetComponent<MeshFilter>().sharedMesh;
       gunMaterial.material = gunList[selectedWeapon].GetComponent<MeshRenderer>().sharedMaterial;
        
    
    }

   public void addHP(int amount)
    {
        HP += amount;
    }

    IEnumerator CalculateSpeed()
    {
        isPlaying = true;
        while (isPlaying)
        {
            Vector3 prevPos = transform.position;

            yield return new WaitForFixedUpdate();
            speed = Mathf.RoundToInt(Vector3.Distance(transform.position, prevPos) / Time.fixedDeltaTime);
        }
    }

    public float EnergyBuildUp()
    {
        
        if(Enery < maxEnergy)
        {
            if(speed > 12)
            {
                Enery += 3 * Time.deltaTime;
            }
            else if(speed > 20)
            {
                Enery += 5 * Time.deltaTime;

            }
            else if(speed > 40)
            {
                Enery += 10 * Time.deltaTime;
            }
            else if(Enery>0)
            {
                
                Enery -= 1 * Time.deltaTime;
            }
        }
        return Enery;
    }
}

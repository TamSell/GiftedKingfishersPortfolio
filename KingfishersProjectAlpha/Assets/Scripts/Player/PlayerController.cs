using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour, Damage
{

    [Header("----- Components-----")]
    [SerializeField] AudioSource aud;
    [SerializeField] Slider silderSensitivity;
    [SerializeField] public Rigidbody rb;

    [Header("----- Player Stats -----")]
    [SerializeField] public float interactDist;
    [Range(3, 8)][SerializeField] float PlayerSpeed;
    [Range(3, 30)][SerializeField] float jumpHeight;
    [Range(3, 25)][SerializeField] float gravityValue;
    [Range(1, 4)][SerializeField] int jumpMax;
    [Range(1, 20)][SerializeField] public int HP;
    [SerializeField] Animator animator;
    [Range(1, 200)][SerializeField] float FOVorg;
    [Range(1, 200)][SerializeField] float RunFOV;

    public List<Gun> gunList = new List<Gun>();

    [Header("-------Player Movement--------")]
    [Range(0, 10)][SerializeField] float RunSpeed;
    [Range(5, 250)][SerializeField] public float DashSpeed;
    [Range(0, 1)][SerializeField] float DashTime;
    [Range(0,30)][SerializeField] float MaxStamina;
    [SerializeField] float Stamina;
    [SerializeField] public float speed;
    [SerializeField] public float Enery;
    [SerializeField] float maxEnergy;
    [SerializeField] float jumpButtonGraceperiod;
    [SerializeField] float? lastGoundedTime;
    [SerializeField] float? jumpButtonPressedTime;
    float originalStopOffset;
    [SerializeField] float baseAccel;
   

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
    private float EneryOrig;
    public Vector3 move;
    public int HPorig;
    public bool isrunning;
    public bool isDashing;
    public float velocity;
    public bool inMomentum;
    private bool isGrounded;




    // Start is called before the first frame update
    void Start()
    {
        DashCD = 6;
        DashReady = true;
        StartCoroutine(CalculateSpeed());
        HPorig = 50;
        StaminaOrig = Stamina;
        EneryOrig = Enery;
        PLayerUpdateUI();
        FOVorg = Camera.main.fieldOfView;
        respawnPlayer();
       // originalStopOffset = rb.stepOffset;

    }

    // Update is called once per frame
    void Update()
    {
        //if(!isDead)
        
            Dash();
            movement();
            //selectGun();
            EnergyBuildUp();
            canInteract();
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
        if(isGrounded)
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

       ActMomentumState();

        Run();

        move = (transform.right * Input.GetAxis("Horizontal")) + (transform.forward * Input.GetAxis("Vertical"));
        rb.transform.position += (move * Time.deltaTime * PlayerSpeed);

        Jump();
      
        playerVelocity.y -= gravityValue * Time.deltaTime;
        rb.transform.position += (playerVelocity * Time.deltaTime);

        PLayerUpdateUI();
    }

    private void ActMomentumState()
    {
        if(Input.GetButtonDown("Momentum"))
        {
            inMomentum = !inMomentum;
        }
        
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
        if(isGrounded)
        {
            lastGoundedTime = Time.time;
        }
        if(Input.GetButtonDown("Jump"))
        {
            jumpButtonPressedTime=Time.time;
        }


        if (Time.time - lastGoundedTime<= jumpButtonGraceperiod || jumpTimes < jumpMax)
        {
            //controller.stepOffset = originalStopOffset;
          
            if(Time.time - jumpButtonPressedTime <= jumpButtonGraceperiod)
            {
                aud.PlayOneShot(audJump[Random.Range(0, audJump.Length)], audJumpVol);
                jumpTimes++;
                playerVelocity.y = jumpHeight;
                jumpButtonPressedTime = null;
                lastGoundedTime = null;
            }
        }
    }
    void Run()
    {
        if (move.x != 0 && move.z != 0)
        {
            if (Input.GetButton("Run"))
            {
                isrunning = true;
                if (Stamina > 0 && isrunning)
                {
                    Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, RunFOV, Time.deltaTime * 2.5f);
                    rb.gameObject.transform.position += (move * Time.deltaTime * (PlayerSpeed + RunSpeed));
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
                rb.gameObject.transform.position += (move * Time.deltaTime * PlayerSpeed);
                //StaminaRecovery();
            }
        }
        else
        {
            //StaminaRecovery();
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
            rb.gameObject.transform.position += (move * Time.deltaTime * DashSpeed);
        
           yield return new WaitForEndOfFrame();
        }
        isDashing=false;
        
       
    }
  
    IEnumerator Accelerate(float amount)
    {
        if(inMomentum)
        {

        }
        else
        {
            
        }
        yield return new Null();
    }

    //void StaminaRecovery()
    //{
    //    if (Stamina < MaxStamina)
    //    {
    //        Stamina += 1 * Time.deltaTime;
    //    }
    //    else
    //    {
    //        gameManager.Instance.SBar.enabled = false;
    //    }
    //}
    public bool isDead
    {
        get
        {
            return HP == 0;
        }
    }
    public void goDie()
    {
        gameManager.Instance.death();
    }

    public void TakeDamage(int amount)
    {
        aud.PlayOneShot(auddamage[Random.Range(0, auddamage.Length)], auddamageVol);
        HP -= amount;
        PLayerUpdateUI();
        if(isDead)
        {
            animator.SetTrigger("Death");
        }
    }
    
    void PLayerUpdateUI()
    {
        gameManager.Instance.HPbar.fillAmount = (float)HP / HPorig;
        gameManager.Instance.EnergyBar.fillAmount = (Enery / 100) / 2;
    }

    public void respawnPlayer()
    {
        HP = HPorig;
        PLayerUpdateUI();
        rb.isKinematic = false;
        transform.position = gameManager.Instance.playerSpawnPos.transform.position;
        rb.isKinematic = true;
    }

    //public void gunPickup(Gun gun)
    //{
    //   gunList.Add(gun);

    //   usingGun.currentMag = gun.currentMag;
    //   usingGun.bullet = gun.bullet;
    //   usingGun.Barrel = gun.Barrel;
       
    //   usingGun.magSize = gun.magSize;
    //   usingGun.totalAmmo = gun.totalAmmo;
       
    //   usingGun.RayGunDist = gun.RayGunDist;
    //   usingGun.RayGunDamage = gun.RayGunDamage;
    //   usingGun.RayGunEffect = gun.RayGunEffect;
       
    //   usingGun.ShootRate = gun.ShootRate;
    //   usingGun.realoadSpeed = gun.realoadSpeed;
    //   usingGun.reaload = gun.reaload;
       
    //   usingGun.Sniper = gun.Sniper;
       
    //   usingGun = gun;
       
    //   gunModel.mesh = gun.GetComponent<MeshFilter>().sharedMesh;
    //   gunMaterial.material = gun.GetComponent<MeshRenderer>().sharedMaterial;
    //}

     //void selectGun()
     //{
     //    int previousSelectedWeapon = selectedWeapon;
     
     //    if (Input.GetKeyDown(KeyCode.Alpha1))
     //    {
     //        selectedWeapon = 0;
     //    }
     //    else if (Input.GetKeyDown(KeyCode.Alpha2))
     //    {
     //        selectedWeapon = 1;
     //    }
     //    else if (Input.GetKeyDown(KeyCode.Alpha3))
     //    {
     //        selectedWeapon = 2;
     //    }
     //    else if (Input.GetKeyDown(KeyCode.Alpha4))
     //    {
     //        selectedWeapon = 3;
     //    }
     
     //    if (previousSelectedWeapon != selectedWeapon)
     //        SelectGun();
     //    //Invoke("SelectWeapon", 0.5f);  
     
     //    if (Input.GetAxis("Mouse ScrollWheel") > 0 && selectedWeapon < gunList.Count - 1)
     //    {
     //        selectedWeapon++;
     //        changeGun();
     //    }
     //    else if (Input.GetAxis("Mouse ScrollWheel") < 0 && selectedWeapon > 0)
     //    {
     //        selectedWeapon--;
     //        changeGun();
     //    }
     //}
     //void SelectGun()
     //{
     //    int i = 0;
     //    foreach (Gun gun in gunList)
     //    {
     //        if (i == selectedWeapon)
     //            gunList[i].gameObject.SetActive(true);
     //        else
     //            gunList[i].gameObject.SetActive(false);
     //        i++;
     //    }
     //}

    //void changeGun()
    //{
    //   usingGun.currentMag = gunList[selectedWeapon].currentMag;
    //   usingGun.bullet = gunList[selectedWeapon].bullet;
    //   usingGun.Barrel = gunList[selectedWeapon].Barrel;
       
    //   usingGun.magSize = gunList[selectedWeapon].magSize;
    //   usingGun.totalAmmo = gunList[selectedWeapon].totalAmmo;
       
    //   usingGun.RayGunDist = gunList[selectedWeapon].RayGunDist;
    //   usingGun.RayGunDamage = gunList[selectedWeapon].RayGunDamage;
    //   usingGun.RayGunEffect = gunList[selectedWeapon].RayGunEffect;
       
    //   usingGun.ShootRate = gunList[selectedWeapon].ShootRate;
    //   usingGun.realoadSpeed = gunList[selectedWeapon].realoadSpeed;
    //   usingGun.reaload = gunList[selectedWeapon].reaload;
       
    //   usingGun.Sniper = gunList[selectedWeapon].Sniper;
       
    //   usingGun.GunShot = gunList[selectedWeapon].GunShot;
    //   usingGun.gunShotVol = gunList[selectedWeapon].gunShotVol;
       
    //   usingGun = gunList[selectedWeapon];
       
    //   gunModel.mesh = gunList[selectedWeapon].GetComponent<MeshFilter>().sharedMesh;
    //   gunMaterial.material = gunList[selectedWeapon].GetComponent<MeshRenderer>().sharedMaterial;
    //}

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
            speed = Vector3.Distance(transform.position, prevPos) / Time.fixedDeltaTime;
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
            else if(Enery > 0)
            {
                
                Enery -= 1 * Time.deltaTime;
            }
        }
        return Enery;
    }

    void canInteract()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, interactDist))
        {
            if (hit.collider.CompareTag("CraftBench"))
            {
                gameManager.Instance.isNear = true;
            }
        }
        else
        {
            gameManager.Instance.isNear = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isGrounded = true;
        }
    }
}

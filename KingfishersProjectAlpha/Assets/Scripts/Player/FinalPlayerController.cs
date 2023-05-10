using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D;
using UnityEngine;

public class FinalPlayerController : MonoBehaviour
{

    [SerializeField] private LayerMask Floor;
    [SerializeField] private Transform Feet;
    [SerializeField] private Transform Camera;
    [SerializeField] public Rigidbody PlayerBody;
    [SerializeField] Animator animator;
    [SerializeField] AudioSource audio;
    [Space]
    [SerializeField] private float PlayerSpeed;
    [SerializeField] private float sensitivity;
    [SerializeField] private float jumpForce;


    [Header("---Stats---")]
    [SerializeField] public float HP;
    [SerializeField] int speed;
    [SerializeField] public float energyMax;
    [SerializeField, Range(0f, 50f)] float interactDist;
    [SerializeField] int jumpMax;
    [SerializeField] int jumptimes;

    [Header("-----Runing stats-----")]
    [SerializeField] public float RunFov;
    [SerializeField] float runSpeed;
    [SerializeField] float FovOrg;

    [Header("------Dash Stats------")]
    [SerializeField] float DashSpeed;
    [SerializeField] float Dashtime;
    [SerializeField] float DashCD;
    [SerializeField] float DashMaxCD;
    public bool DashReady;
    public bool isDashing;

    [Header("----Audio -----")]
    [Range(0, 1)][SerializeField] float audJumpVol;
    [SerializeField] AudioClip[] audJump;
    [Range(0,1)][SerializeField] float audDamageVol;
    [SerializeField] AudioClip[] audDamage;


    [Header("---Gun---")]
    [SerializeField] public GunStats2 currentGun;
    [Space]
    
    public Vector3 MoveVector;
    public Vector3 PlayerMovementInput;
    public Vector3 PlayerMovementAddition;
    private Vector2 PlayerMouse;
    private float xRotation;
    public bool isRunning;
   // private bool isDead = false;
    public float origHP;
    public float currentEnergy = 0;
    public float origStamina;
    [SerializeField] float CurrentSpeed;
    bool isPlaying;
    private void Start()
    {

        origHP = HP; 
       // playerUpdateUI();
        StartCoroutine(CalculateSpeed());
       // respawnPlayer();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        FovOrg = UnityEngine.Camera.main.fieldOfView;
        DashCD = 5;
    }
    // Update is called once per frame
    void Update()
    {
        playerUpdateUI();
        PlayerMovementInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        PlayerMouse = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Dash();
        MovePlayer();
        MouseMove();
        CD(isDashing, ref DashCD, DashMaxCD);
        EneryBuildUP();
        canInteract();
    }

   
    void CD(bool ability, ref float abilityCD, float maxCD)
    {
        if (abilityCD > maxCD)
        {
            abilityCD = maxCD;
        }

        else if (ability == false && abilityCD < maxCD)
        {
            abilityCD += 1 * Time.deltaTime;
        }
    }


    private void MovePlayer()
    {
        if (PlayerBody.velocity.y < 0)
        {
            jumptimes = 0;
        }
        MoveVector = transform.TransformDirection(PlayerMovementInput + PlayerMovementAddition) * PlayerSpeed;
        PlayerBody.velocity = new Vector3(MoveVector.x, PlayerBody.velocity.y, MoveVector.z);
        if (Input.GetButtonDown("Jump"))
        {
            if (Physics.CheckSphere(Feet.position, 0.1f, Floor) )
            {
               
                audio.PlayOneShot(audJump[Random.Range(0, audJump.Length)], audJumpVol);
                PlayerBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                jumptimes++;
            }
        }
        Run();
    }

    private void MouseMove()
    {
        xRotation -= PlayerMouse.y * sensitivity;
        transform.Rotate(0f, PlayerMouse.x * sensitivity, 0f);
        xRotation = Mathf.Clamp(xRotation, -90, 90);
        Camera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    void Run()
    {

        if (Input.GetButton("Run"))
        {
            gameManager.Instance.SBar.enabled = true;
            isRunning = true;
            UnityEngine.Camera.main.fieldOfView = Mathf.Lerp(UnityEngine.Camera.main.fieldOfView, RunFov, Time.deltaTime * 2.5f);
            MoveVector = transform.TransformDirection(PlayerMovementInput) * PlayerSpeed * runSpeed;
            PlayerBody.velocity = new Vector3(MoveVector.x, PlayerBody.velocity.y, MoveVector.z);
        }
        else
        {
            isRunning = false;
            UnityEngine.Camera.main.fieldOfView = Mathf.Lerp(UnityEngine.Camera.main.fieldOfView, FovOrg, Time.deltaTime * 2.5f);

        }
    }

    void Dash()
    {
        if (DashCD > DashMaxCD)
        {
            DashReady = true;
        }
        if (PlayerMovementInput.x == 0 && PlayerMovementInput.z == 0)
        {
            return;
        }
        if (Input.GetButtonDown("Dash"))
        {
            UnityEngine.Camera.main.fieldOfView = Mathf.Lerp(UnityEngine.Camera.main.fieldOfView, RunFov, Time.deltaTime * 1f);
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
        while (Time.time < startTime + Dashtime)
        {
            MoveVector = transform.TransformDirection(PlayerMovementInput) * PlayerSpeed * DashSpeed;
            PlayerBody.velocity = new Vector3(MoveVector.x, PlayerBody.velocity.y, MoveVector.z);
            yield return new WaitForEndOfFrame();

        }
        isDashing = false;
    }
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
       // audio.PlayOneShot(audDamage[Random.Range(0, audDamage.Length)], audDamageVol);
        HP -= amount;
        playerUpdateUI();
        if (isDead)
        {
            animator.SetTrigger("Death");
        }
    }
    void playerUpdateUI()
    {
        gameManager.Instance.HPbar.fillAmount = (float)HP / origHP;
        gameManager.Instance.Speedbar.fillAmount = currentEnergy / 100;
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
            CurrentSpeed = Vector3.Distance(transform.position, prevPos) / Time.fixedDeltaTime;
        }
        isPlaying = false;
    }

    public float EneryBuildUP()
    {
        if (currentEnergy < energyMax)
        {
            if (CurrentSpeed > 30)
            {
                currentEnergy += 15 * Time.deltaTime;
            }
            else if (CurrentSpeed > 20)
            {
                currentEnergy += 12 * Time.deltaTime;
            }
            else if (CurrentSpeed > 12)
            {
                currentEnergy += 10 * Time.deltaTime;
            }

        }
        if (currentEnergy > 0)
        {
            currentEnergy -= 1 * Time.deltaTime;
        }
        return currentEnergy;
    }
    public void respawnPlayer()
    {
        HP = origHP;
        //playerUpdateUI();
        PlayerBody.isKinematic = true;
        transform.position = gameManager.Instance.playerSpawnPos.transform.position;
        PlayerBody.isKinematic = false;
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
}

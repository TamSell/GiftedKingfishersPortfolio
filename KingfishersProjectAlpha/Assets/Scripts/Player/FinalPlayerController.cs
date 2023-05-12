using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalPlayerController : MonoBehaviour, Damage
{

    [SerializeField] private LayerMask Floor;
    [SerializeField] private Transform Feet;
    [SerializeField] private Transform Camera;
    [SerializeField] public Rigidbody PlayerBody;
    [SerializeField] public PlayerMomentum1 Momentum;
    [SerializeField] Animator animator;
    [SerializeField] AudioSource audioPl;
    [Space]
    [SerializeField] public float walkSpeedBase;
    [SerializeField] public float airSpeedBase;
    [SerializeField] public float runSpeedBase;
    [SerializeField] public float walkSpeedMomentum;
    [SerializeField] public float airSpeedMomentum;
    [SerializeField] public float runSpeedMomentum;
    [SerializeField] private float sensitivity;
    [SerializeField] private float jumpForce;


    [Header("---Stats---")]
    [SerializeField] public float HP;
    [SerializeField] public float stamina;
    [SerializeField] public float energyMax;
    [SerializeField, Range(0f, 50f)] float interactDist;
    [SerializeField] float fallMult;
    [SerializeField] public float MomentumDrag;
    [SerializeField] public float StandardDrag;
    [SerializeField] int jumpMax;
    [SerializeField] int jumptimes;

    [Header("-----Runing stats-----")]
    [SerializeField] public float RunFov;
    [SerializeField] public float runSpeed;
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
    [Range(0, 1)][SerializeField] float audDamageVol;
    [SerializeField] AudioClip[] audDamage;


    [Header("---Gun---")]
    [SerializeField] public GunStats2 currentGun;

    [Space]
    PlayerAudio auido;
    public float PlayerSpeed;
    public float CurrentSpeed;
    public float currWalkSpeed;
    public float currAirSpeed;
    public float currRunSpeed;
    public Vector3 MoveVector;
    public Vector3 PlayerMovementInput;
    public Vector3 PlayerMovementAddition;
    private Vector2 PlayerMouse;
    private float xRotation;
    private int energyfallOff;
    public bool isRunning;
    public bool isPlaying;
    public float origHP;
    public float currentEnergy = 0;
    public float origStamina;
    public float speed;
    public bool isGrounded;
    public bool isTeleporting;
    public float minXLock;
    public float maxXLock;

    private void Start()
    {
        playerUpdateUI();
        FovOrg = UnityEngine.Camera.main.fieldOfView;
        DashCD = 6;
        DashReady = true;
        StartCoroutine(CalculateSpeed());
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        origHP = HP;
        origStamina = stamina;
        currentEnergy = 0;
        Momentum.MomentumChange();
        currRunSpeed = runSpeedBase;
        currWalkSpeed = walkSpeedBase;
        currAirSpeed = airSpeedBase;
        energyfallOff = 5;
    }
    // Update is called once per frame
    void Update()
    {
        if (gameManager.Instance.inMenu)
        {
            return;
        }
        //Check if Grounded
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.05f, Floor);
        //PlayerInput
        PlayerInput();
        MouseMove();
        EneryBuildUP();
        canInteract();
        playerUpdateUI();
    }

    public void PlayerInput()
    {
        //Obtain Input from keyboard
        PlayerMovementInput = PlayerBody.transform.forward * Input.GetAxis("Vertical") + PlayerBody.transform.right * Input.GetAxis("Horizontal");
        //Obtain Mouse Input
        PlayerMouse = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        //Check Run
        Run();
        //MomentumState
        if (Input.GetButtonDown("MoveChange"))
        {
            Momentum.MomentumState();
        }
        //Jump
        if (Input.GetButtonDown("Jump") && (isGrounded || jumptimes < jumpMax))
        {
            Jump();
        }
        //Dash
        CD(isDashing, ref DashCD, DashMaxCD);
        Dash();
    }

    private void FixedUpdate()
    {
        Momentum.SecondaryMovement(PlayerMovementInput);
        if (!isGrounded)
        {
            PlayerBody.velocity += fallMult * Physics.gravity;
            PlayerBody.drag = PlayerBody.drag * 0.05f;
        }
        else
            if (Momentum.inMomentum)
            PlayerBody.drag = MomentumDrag;
        else
            PlayerBody.drag = StandardDrag;

    }

    private void MouseMove()
    {
        xRotation -= PlayerMouse.y * sensitivity;
        xRotation = Mathf.Clamp(xRotation, minXLock, maxXLock);
        transform.Rotate(0f, PlayerMouse.x * sensitivity, 0f);
        Camera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    private void Jump()
    {
        if (isGrounded)
        {
            jumptimes = 0;
        }
        PlayerBody.velocity = new Vector3(PlayerBody.velocity.x, 0f, PlayerBody.velocity.z);
        PlayerBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        jumptimes++;
        audioPl.PlayOneShot(audJump[Random.Range(0, audJump.Length - 1)], audJumpVol);
    }

    void Run()
    {

        if (Input.GetButton("Run"))
        {
            gameManager.Instance.SBar.enabled = true;
            isRunning = true;
            UnityEngine.Camera.main.fieldOfView = Mathf.Lerp(UnityEngine.Camera.main.fieldOfView, RunFov, Time.deltaTime * 2.5f);
        }
        else
        {
            isRunning = false;
            UnityEngine.Camera.main.fieldOfView = Mathf.Lerp(UnityEngine.Camera.main.fieldOfView, FovOrg, Time.deltaTime * 2.5f);

        }
    }

    void Dashs()
    {
        if (DashCD > DashMaxCD)
        {
            DashReady = true;
        }
        //if (PlayerMovementInput.x == 0 && PlayerMovementInput.z == 0)
        //{
        //    return;
        //}
        if (Input.GetButtonDown("Dash"))
        {
            UnityEngine.Camera.main.fieldOfView = Mathf.Lerp(UnityEngine.Camera.main.fieldOfView, RunFov, Time.deltaTime * 1f);

            // MoveVector = transform.TransformDirection(PlayerMovementInput) * DashSpeed;
            //  PlayerBody.velocity = new Vector3(MoveVector.x, PlayerBody.velocity.y, MoveVector.z);
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
           // MoveVector = transform.TransformDirection(PlayerMovementInput) * DashSpeed;
           // PlayerBody.velocity = new Vector3(MoveVector.x, PlayerBody.velocity.y, MoveVector.z);
            MoveVector = transform.TransformDirection(PlayerMovementInput) * DashSpeed;
            PlayerBody.velocity = new Vector3(MoveVector.x, PlayerBody.velocity.y, MoveVector.z);

            yield return new WaitForEndOfFrame();

        }
        isDashing = false;
    }

    private void CD(bool ability, ref float abilityCD, float maxCD)
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
            goDie();
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
                currentEnergy += 15 * Time.deltaTime * 2f;
            }
            else if (CurrentSpeed > 20)
            {
                currentEnergy += 12 * Time.deltaTime * 2f;
            }
            else if (CurrentSpeed > 12)
            {
                currentEnergy += 8 * Time.deltaTime * 2f;
            }

        }
        if (currentEnergy > 0)
        {
            currentEnergy -= (1 * energyfallOff) * Time.deltaTime * 5f;
        }
        return currentEnergy;
    }

    public void Recoil()
    {
        Vector3 direction = Camera.forward;
        PlayerBody.AddForce(-direction.normalized * currentGun.recoil, ForceMode.Impulse);
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

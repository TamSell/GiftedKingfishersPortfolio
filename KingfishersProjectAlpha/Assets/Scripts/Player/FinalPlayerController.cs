using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalPlayerController : MonoBehaviour
{
    [SerializeField] private LayerMask Floor;
    [SerializeField] private Transform Feet;
    [SerializeField] private Transform Camera;
    [SerializeField] public Rigidbody PlayerBody;
    [SerializeField] private PlayerMomentum1 Momentum;
    [Space]
    [SerializeField] public float walkSpeed;
    [SerializeField] public float runSpeed;
    [SerializeField] public float airSpeed;
    [SerializeField] private float sensitivity;
    [SerializeField] private float jumpForce;

    [Header("---Stats---")]
    [SerializeField] public float HP;
    [SerializeField] public float stamina;
    [SerializeField] public float energyMax;
    [SerializeField, Range(0f, 50f)] float interactDist;

    [Header("---Gun---")]
    [SerializeField] public GunStats2 currentGun;

    private Vector3 PlayerMovementInput;
    private Vector2 PlayerMouse;
    private float xRotation;
    public bool isRunning;
    private bool isDead = false;
    public float origHP;
    public float currentEnergy = 0;
    public float origStamina;
    public float speed;
    public bool isGrounded;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        origHP = HP;
        origStamina = stamina;
        currentEnergy = energyMax;
    }
    // Update is called once per frame
    void Update()
    {
        PlayerMovementInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        PlayerMouse = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        
        if(Input.GetButtonDown("MoveChange"))
        {
            Momentum.MomentumState();
        }
        MouseMove();
    }

    private void FixedUpdate()
    {
        if(Momentum.inMomentum)
        {
            Momentum.SecondaryMovement();
        }
        else
            MovePlayer();
    }

    private void MovePlayer()
    {
        Vector3 MoveVector = transform.TransformDirection(PlayerMovementInput) * walkSpeed;
        PlayerBody.velocity = new Vector3(MoveVector.x, PlayerBody.velocity.y, MoveVector.z);

        if (Physics.CheckSphere(Feet.position, 0.1f))
        {
            isGrounded = true;
            if (Input.GetButtonDown("Jump"))
            {
                PlayerBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                isGrounded = false;
            }
        }
    }

    private void MouseMove()
    {
        xRotation -= PlayerMouse.y * sensitivity;
        transform.Rotate(0f, PlayerMouse.x*sensitivity, 0f);
        Camera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}

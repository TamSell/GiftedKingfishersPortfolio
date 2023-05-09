using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2 : MonoBehaviour
{
    [Header("---Componenets---")]
    [SerializeField] private Transform feet;
    [SerializeField] private LayerMask floor;
    [SerializeField] private PlayerMomentum1 momentum;
    [SerializeField] public Rigidbody PlayerBody;
    [SerializeField] Transform Camera;
    [SerializeField] PlayerAnimator animator;
    [SerializeField] PlayerUI UI;
    [SerializeField] public GameObject playerSpawn;
    [Space]
    [SerializeField] public float speed;
    [SerializeField] public float sensitiivity;
    [SerializeField] public float jumpForce;

    [Header("---Stats---")]
    [SerializeField] public float HP;
    [SerializeField] public float stamina;
    [SerializeField] public float energyMax;
    [SerializeField, Range(0f, 50f)] float interactDist;

    [Header("---Gun---")]
    [SerializeField] public GunStats2 currentGun;


    [Header("---Audio---")]
    [SerializeField] PlayerAudio audioPlayer;
    [Range(0, 1)][SerializeField] public float audStepsVol;
    [Range(0, 1)][SerializeField] public float audJumpVol;
    [Range(0, 1)][SerializeField] public float auddamageVol;

    private Vector3 PlayerMovementInput;
    private Vector2 PlayerMouse;
    private float xRot;
    public float camLockMax;
    public float camLockMin;
    public bool isRunning;
    private bool isDead = false;
    public float origHP;
    public float currentEnergy = 0;
    public float origStamina;

    private void Start()
    {

    }
    void Update()
    {
        PlayerMovementInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        PlayerMouse = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        //momentum.Momentum();
        Movement();
        MouseMove();

    }

    private void Movement()
    {
        Vector3 MoveVector = transform.TransformDirection(PlayerMovementInput) * speed;
        PlayerBody.velocity = new Vector3(MoveVector.x, PlayerBody.velocity.y, MoveVector.z);

        if(Input.GetButtonDown("Jump"))
        {
            if(Physics.CheckSphere(feet.position, 0.1f, floor))
            {
                PlayerBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
    }

    private void MouseMove()
    {
        xRot -= PlayerMouse.y * sensitiivity;
        xRot = Mathf.Clamp(xRot, camLockMin, camLockMax);
        transform.Rotate(0f, PlayerMouse.x * sensitiivity, 0f);
        Camera.transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);
    }

}

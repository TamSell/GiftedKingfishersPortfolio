using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalPlayerController : MonoBehaviour
{
    [SerializeField] private LayerMask Floor;
    [SerializeField] private Transform Feet;
    [SerializeField] private Transform Camera;
    [SerializeField] private Rigidbody PlayerBody;
    [Space]
    [SerializeField] private float Speed;
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
    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    // Update is called once per frame
    void Update()
    {
        PlayerMovementInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        PlayerMouse = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        MovePlayer();
        MouseMove();
    }

    private void MovePlayer()
    {
        Vector3 MoveVector = transform.TransformDirection(PlayerMovementInput) * Speed;
        PlayerBody.velocity = new Vector3(MoveVector.x, PlayerBody.velocity.y, MoveVector.z);

        if(Input.GetButtonDown("Jump"))
        {
            if(Physics.CheckSphere(Feet.position, 0.1f))
            {
                PlayerBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
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

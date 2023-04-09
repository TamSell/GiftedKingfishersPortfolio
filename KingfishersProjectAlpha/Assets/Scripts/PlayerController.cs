using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Header("----- Components-----")]
    [SerializeField] CharacterController controller;

    [Header("----- Player Stats -----")]
    [Range(3,8)] [SerializeField] float PlayerSpeed;
    [Range(3, 30)][SerializeField] float jumpHeight;
    [Range(3, 25)][SerializeField] float gravityValue;
    [Range(1, 4)][SerializeField] int jumpMax;
    [Range(1, 10)][SerializeField] int HP;

    int jumpTimes;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    Vector3 move;
    int HPorig;

    // Start is called before the first frame update
    void Start()
    {
        HPorig = HP;

    }

    // Update is called once per frame
    void Update()
    {
        movement();
    }

    void movement()
    {
        groundedPlayer = controller.isGrounded;
        if(groundedPlayer && playerVelocity.y<0)
        {
            playerVelocity.y = 0f;
            jumpTimes = 0;
        }

        move = (transform.right * Input.GetAxis("Horizontal")) + (transform.forward * Input.GetAxis("Vertical"));

        controller.Move(move * Time.deltaTime * PlayerSpeed);

        if(Input.GetButtonDown("Jump") && jumpTimes<jumpMax)
        {
            jumpTimes++;
            playerVelocity.y = jumpHeight;
        }
        playerVelocity.y -= gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime); 
    }
}

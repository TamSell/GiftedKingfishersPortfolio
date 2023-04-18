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
    [Range(1, 200)][SerializeField] float RunFOV;
    [Range(1, 200)][SerializeField] float DashFOV;
    [Range(0, 10)][SerializeField] float RunSpeed;
    [Range(5, 205)][SerializeField] float DashSpeed;
    [Range(0,10)] [SerializeField] float Stamina;


    float FOVorg;
    
 
    int jumpTimes;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float StaminaOrig;
    Vector3 move;
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

        if(Input.GetButton("Run"))
        {

            isrunning = true;
            if(Stamina > 0 && isrunning)
            {
                Camera.main.fieldOfView = RunFOV;
                controller.Move(move * Time.deltaTime * (PlayerSpeed + RunSpeed));
                Stamina -= 3 * Time.deltaTime;
               
            }
            else if(Stamina<=0)
            {
                isrunning=false;
                Camera.main.fieldOfView = FOVorg;
            }
          
        }
        else
        {
          
            isrunning = false;
                Camera.main.fieldOfView = FOVorg;
                controller.Move(move * Time.deltaTime * PlayerSpeed);
                if (Stamina <= 10)
                {
                    Stamina += 1 * Time.deltaTime;
                }        
        }
        controller.Move(move * Time.deltaTime * PlayerSpeed);


        if (Input.GetButtonDown("Jump") && jumpTimes<jumpMax)
        {
            jumpTimes++;
            playerVelocity.y = jumpHeight;
        }
        playerVelocity.y -= gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
        PLayerUpdateUI();
    }

    void Dash()
    {
       
        if(Input.GetButtonDown("Dash"))
        {
            
            if (Stamina > 3)
            {
                Camera.main.fieldOfView = DashFOV;
                controller.Move(move * Time.deltaTime * (PlayerSpeed + DashSpeed));
                Stamina -= 3;
            }
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

}

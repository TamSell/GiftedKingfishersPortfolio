using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    [Header("----Sway Setting------")]
    [SerializeField]  float smooth;
    [SerializeField]  float WalkingSway;
    [SerializeField] float RunningSway;

   
    float mouseX;
    float mouseY;

    private void Update()
    {
      
        if(gameManager.Instance.playerController.isrunning==false)
        {
             mouseX = Input.GetAxisRaw("Mouse X") * WalkingSway;
             mouseY = Input.GetAxisRaw("Mouse Y") * WalkingSway;
        }
        else
        {
             mouseX = Input.GetAxisRaw("Mouse X") * RunningSway;
             mouseY = Input.GetAxisRaw("Mouse Y") * RunningSway;
        }

      

        Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

        Quaternion targetRottaion = rotationX * rotationY;

        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRottaion, smooth * Time.deltaTime);
    }

}

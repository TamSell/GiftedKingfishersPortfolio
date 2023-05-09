using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class Camara : MonoBehaviour
{
    [SerializeField] public float sensHor;
    [SerializeField] public float sensVert;

    [SerializeField] int lockVerMin;
    [SerializeField] int lockVerMax;

    //[SerializeField] Slider xSlider;
    //[SerializeField] Slider ySlider;

    float xRotation;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //  sensVert = xSlider.value;
        //  sensHor = ySlider.value;
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensHor;
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sensVert;

          xRotation -= mouseY;

        ////  yRotation -= mouseX;

          xRotation = Mathf.Clamp(xRotation, lockVerMin, lockVerMax);


         transform.localRotation = Quaternion.Euler(xRotation, 0, 0);


        transform.parent.Rotate(0,mouseX,0);
        //transform.parent.Rotate(Vector3.up * mouseX);

    }
}

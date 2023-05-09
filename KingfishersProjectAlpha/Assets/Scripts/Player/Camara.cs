using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class Camara : MonoBehaviour
{
    [SerializeField] int sensHor;
    [SerializeField] int sensVert;

    [SerializeField] int lockVerMin;
    [SerializeField] int lockVerMax;

    float xRotation;
  //  float yRotation;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensVert;
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sensHor;

        xRotation -= mouseY;
      //  yRotation -= mouseX;

        xRotation = Mathf.Clamp(xRotation, lockVerMin, lockVerMax);

        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        transform.parent.Rotate(Vector3.up * mouseX);

    }
}

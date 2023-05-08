using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Camara : MonoBehaviour
{
    [SerializeField] public int sensHor;
    [SerializeField] public int sensVert;
    [SerializeField] int lockverMin;
    [SerializeField] int lockverMax;
    [SerializeField] Slider XSlider;
    [SerializeField] Slider YSlider;

    float xRotation;
    // Start is called before the first frame update
    void Start()
    {
        XSlider.value = sensVert;
        YSlider.value = sensHor;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        sensVert = (int)XSlider.value;
        sensHor = (int)YSlider.value;
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensHor;
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sensVert;

        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, lockverMin, lockverMax);

        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        transform.parent.Rotate(Vector3.up * mouseX);
    }
}

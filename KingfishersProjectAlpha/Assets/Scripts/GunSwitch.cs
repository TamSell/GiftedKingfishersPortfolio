using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSwitch : MonoBehaviour
{
    public int selectedWeapon = 0;

    private bool isSwitching;


    // Start is called before the first frame update
    void Start()
    {
        SelectWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        int previousSelectedWeapon = selectedWeapon;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            
            selectedWeapon = 0;
            StartCoroutine(Switch());
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            
            selectedWeapon = 1;
            StartCoroutine(Switch());
        }

        if (previousSelectedWeapon != selectedWeapon)
            SelectWeapon();
    }

    void SelectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == selectedWeapon)
                weapon.gameObject.SetActive(true);
            else
                weapon.gameObject.SetActive(false);
            i++;
        }
    }

    private IEnumerator Switch()
    {
        isSwitching = true;

        yield return new WaitForSeconds(2.5f);
        isSwitching = false;
    }
}

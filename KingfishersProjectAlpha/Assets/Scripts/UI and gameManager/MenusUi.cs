using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MenusUi : MonoBehaviour
{
    public static MenusUi menus;

    [Range(0, 1)][SerializeField] float audClickVol = 0.5f;
    [SerializeField] AudioClip MouseClip;
    [SerializeField] AudioSource MouseSource;
    [SerializeField] public GameObject MainMenu;
    [SerializeField] public GameObject OptionMenu;
    [SerializeField] public GameObject LevelSMenu;

    private void Awake()
    {
        menus = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButton("Shoot"))
        {
            MouseSource.PlayOneShot(MouseClip, audClickVol);
        }
    }
}

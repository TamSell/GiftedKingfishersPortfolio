using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MenusUi : MonoBehaviour
{
    public static MenusUi menus;

    [Range(0, 1)][SerializeField] float audClickVol = 0.7f;
    [SerializeField] AudioClip MouseClip;
    [SerializeField] AudioSource MouseSource;
    [SerializeField] AudioSource MenuMusic;
    [SerializeField] public GameObject MainMenu;
    [SerializeField] public GameObject OptionMenu;
    [SerializeField] public GameObject LevelSMenu;

    [Header("----- Volume Control -----")]
    [SerializeField] public AudioSource MenuMusicSource;
    [SerializeField] public AudioSource MenuSFXSource;
    [SerializeField] public AudioMixer MenuMixer;
    [SerializeField] public Slider Musicvalue;
    [SerializeField] public Slider SFXvalue;


    private void Awake()
    {
        menus = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Shoot"))
        {
            StartCoroutine(Click());
        }
        //if(Input.GetButtonUp("shoot"))
        //    return;
            
        if(OptionMenu.activeSelf == true)
        {
            gameObject.GetComponent<AudioSource>().enabled = false;
        }
        else
            gameObject.GetComponent<AudioSource>().enabled = true;
        UpdateVolume();
    }

    void UpdateVolume()
    {
        MenuMixer.SetFloat("SFXVolume", Mathf.Log10(SFXvalue.value) * 30);
        MenuMixer.SetFloat("MusicVolume", Mathf.Log10(Musicvalue.value) * 30);
    }

    IEnumerator Click()
    {
        yield return new WaitForSeconds(0.1f);
        MouseSource.PlayOneShot(MouseClip, audClickVol);
    }
}

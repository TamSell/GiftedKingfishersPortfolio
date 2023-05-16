using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
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
    [SerializeField] public GameObject CreditsMenu;

    [Header("----- Game Volume Control -----")]
    [SerializeField] public AudioSource GameMusicSource;
    [SerializeField] public AudioSource GameSFXSource;
    [SerializeField] public AudioMixer GameMixer;
    [SerializeField] public Slider GMusicvalue;
    [SerializeField] public Slider GSFXvalue;


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
        GameMixer.SetFloat("SFXVolume", Mathf.Log10(GSFXvalue.value) * 30);
        GameMixer.SetFloat("MusicVolume", Mathf.Log10(GMusicvalue.value) * 30);
    }

    IEnumerator Click()
    {
        yield return new WaitForSeconds(0.1f);
        MouseSource.PlayOneShot(MouseClip, audClickVol);
    }
}

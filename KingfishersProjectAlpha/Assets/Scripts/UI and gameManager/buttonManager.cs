using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class buttonManager : MonoBehaviour
{
    //Camara camera;
    int place;
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip button;
    [Range(0, 1)][SerializeField] float audButtonVol;

    public void resume()
    {
        aud.PlayOneShot(button, audButtonVol);
        gameManager.Instance.unpause();
        gameManager.Instance.inMenu = !gameManager.Instance.inMenu;
        
    }

    public void restart()
    {
        aud.PlayOneShot(button, audButtonVol);
        gameManager.Instance.unpause();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Save()
    {
        aud.PlayOneShot(button, audButtonVol);
        saveLoadManager.SaveGame();
    }
    public void load()
    {
        saveLoadManager.LoadGame();
    }

    public void quit()
    {
        aud.PlayOneShot(button, audButtonVol);
        Application.Quit();
    }
    public void Respawnplayer()
    {
        gameManager.Instance.unpause();
        gameManager.Instance.playerController.respawnPlayer();
    }

    public void itemClick()
    {
        string name = EventSystem.current.currentSelectedGameObject.name;
        int.TryParse(name.Substring(name.Length-2,name.Length-1), out place);
        Item _item = gameManager.Instance.SelectItem(place);
        gameManager.Instance.DisplayItem(_item);
    }

    public void Settings()
    {
        gameManager.Instance.Settings.SetActive(true);
        gameManager.Instance.PauseMenu.SetActive(false);
    }
    public void Back()
    {
        gameManager.Instance.Settings.SetActive(false);
        gameManager.Instance.PauseMenu.SetActive(true);
    }


    public void nextWeapon()
    {
        int index = gameManager.Instance.currentGunIndex + 1;
        if (index < gameManager.Instance.gunAspects.Count)
        {
            gameManager.Instance.currentGunAspects = gameManager.Instance.gunAspects[index];
        }
        gameManager.Instance.modify.NextGun();
    }

    public void prevWeapon()
    {
        int index = gameManager.Instance.currentGunIndex - 1;
        if (index < gameManager.Instance.gunAspects.Count)
        {
            gameManager.Instance.currentGunAspects = gameManager.Instance.gunAspects[index];
        }
        gameManager.Instance.modify.NextGun();
    }


}

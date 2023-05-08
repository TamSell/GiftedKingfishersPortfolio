using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class buttonManager : MonoBehaviour
{
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
        saveLoadManager.Save();
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
}

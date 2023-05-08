using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class buttonManager : MonoBehaviour
{
    int place;

    public void resume()
    {
        gameManager.Instance.unpause();
        gameManager.Instance.inMenu = !gameManager.Instance.inMenu;
    }

    public void restart()
    {
        gameManager.Instance.unpause();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Save()
    {
        saveLoadManager.SaveGame();
    }
    public void load()
    {
        saveLoadManager.LoadGame();
    }

    public void quit()
    {
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

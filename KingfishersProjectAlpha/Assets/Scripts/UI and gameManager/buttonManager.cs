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
}

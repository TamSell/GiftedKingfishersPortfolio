using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class buttonManager : MonoBehaviour
{
    Camara cameraChange;
    int place;

    bool isPlayingM;
    bool isPlayingMSFX;
    bool isPlayingMGame;
    bool isPlayingGameSFX;

    public void resume()
    {
        gameManager.Instance.unpause();
        gameManager.Instance.inMenu = !gameManager.Instance.inMenu;
    }

    public void restart()
    {
        gameManager.Instance.unpause();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale= 1.0f;
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
    public void MainMenuPress()
    {
        SceneManager.LoadScene(0);
    }
    public void Respawnplayer()
    {
        gameManager.Instance.unpause();
        //gameManager.Instance.playerController.respawnPlayer();
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

    public void OptionMenu()
    {
        MenusUi.menus.OptionMenu.SetActive(true);
        MenusUi.menus.MainMenu.SetActive(false);
    }
    public void LevelSelect()
    {
        MenusUi.menus.LevelSMenu.SetActive(true);
        MenusUi.menus.MainMenu.SetActive(false);
    }

    public void Back()
    {
        gameManager.Instance.Settings.SetActive(false);
        gameManager.Instance.PauseMenu.SetActive(true);
    }

    public void BackMenufromOp()
    {
        VolumeControl._volInstance.UpdateVolume();
        MenusUi.menus.OptionMenu.SetActive(false);
        MenusUi.menus.MainMenu.SetActive(true);
    }

    public void BackMenufromLevelS()
    {
        MenusUi.menus.LevelSMenu.SetActive(false);
        MenusUi.menus.MainMenu.SetActive(true);
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

    public void MenuMusicButton()
    {
        isPlayingM = !isPlayingM;
        if (isPlayingM)
            MenusUi.menus.MenuMusicSource.enabled = true;
        else
            MenusUi.menus.MenuMusicSource.enabled = false;
    }

    public void GameMusicButton()
    {
        isPlayingMGame = !isPlayingMGame;
        if (isPlayingMGame)
            MenusUi.menus.GameMusicSource.enabled = true;
        else
            MenusUi.menus.GameMusicSource.enabled = false;
    }

    public void MenuSFXButton()
    {
        isPlayingMSFX = !isPlayingMSFX;
        if (isPlayingMSFX)
            MenusUi.menus.MenuSFXSource.enabled = true;
        else
            MenusUi.menus.MenuSFXSource.enabled = false;
    }

    public void GameSFXButton()
    {
        isPlayingGameSFX = !isPlayingGameSFX;
        if (isPlayingGameSFX)
            MenusUi.menus.GameSFXSource.enabled = true;
        else
            MenusUi.menus.GameSFXSource.enabled = false;
    }
}

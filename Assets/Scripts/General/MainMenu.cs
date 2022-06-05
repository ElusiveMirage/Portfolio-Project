using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject settingsMenuCanvas;
    [SerializeField] private AudioClip menuBGM;

    private void Start()
    {
        SoundManager.Instance.PlayBGM(menuBGM);
    }

    public void StartPlatformerGame()
    {
        SoundManager.Instance.PlaySound(Resources.Load<AudioClip>("SFX/SFX_UI_Confirm"));
        SceneLoader.Instance.LoadScene("Platformer_Level_1_1");
    }

    public void StartTowerDefenseGame()
    {
        SoundManager.Instance.PlaySound(Resources.Load<AudioClip>("SFX/SFX_UI_Confirm"));
        SceneLoader.Instance.LoadScene("TD_LevelSelect");
    }

    public void OpenSettings()
    {
        SoundManager.Instance.PlaySound(Resources.Load<AudioClip>("SFX/SFX_UI_OpenMenu"));
        settingsMenuCanvas.SetActive(true);
    }

    public void CloseSettings()
    {
        SoundManager.Instance.PlaySound(Resources.Load<AudioClip>("SFX/SFX_UI_CloseMenu"));
        settingsMenuCanvas.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

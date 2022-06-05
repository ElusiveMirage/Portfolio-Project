using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TD_LevelSelect : MonoBehaviour
{
    public GameObject tutorialScreen;

    public void StartLevel1()
    {
        SoundManager.Instance.PlaySound(Resources.Load<AudioClip>("SFX/SFX_UI_Confirm"));
        SceneLoader.Instance.LoadScene("TD_Level_1_1");
    }

    public void Return()
    {
        SoundManager.Instance.PlaySound(Resources.Load<AudioClip>("SFX/SFX_UI_Exit"));
        SceneLoader.Instance.LoadScene("MainMenuScene");
    }

    public void OpenTutorial()
    {
        tutorialScreen.SetActive(true);
    }

    public void CloseTutorial()
    {
        tutorialScreen.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public GameObject volumeSlider;
    public GameObject difficultySlider;

    
    void Start()
    {
        // Set the game volume on startup
        AudioListener.volume = PlayerPrefs.GetFloat("Volume Level");
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }

    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void SetDifficulty()
    {
        PlayerPrefs.SetInt("Difficulty Level", (int)difficultySlider.GetComponent<Slider>().value);
    }

    public void SetVolume()
    {
        AudioListener.volume = volumeSlider.GetComponent<Slider>().value;
        PlayerPrefs.SetFloat("Volume Level", volumeSlider.GetComponent<Slider>().value);
    }
}

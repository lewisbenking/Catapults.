using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

///<summary> Handles user input and displays pause menu canvas </summary>
///<remarks> Attatched to PauseMenu GameObject</remarks>
///<author> Lewis King </author>

public class PauseMenuScript : MonoBehaviour {

    public bool isPaused; // stores true if paused, false otherwise
    public bool isMuted; // stores true if clicked
    public GameObject pauseMenuCanvas; // game object
    [SerializeField]
    Text muteText;

    void Start()
    {
    }

    /// <summary> If user presses p key switch value of isPaused </summary>
    void Update()
    {
        // If isPaused is true, set the PauseMenuCanvas gameObject to active
        // Also, set the timeScale to 0f so the user can't move around in the background
        if (isPaused)
        {
            //pauseMenuCanvas.SetActive(true);
            pauseMenuCanvas.GetComponent<Canvas>().enabled = true;
            Time.timeScale = 0f;
            
        }
        // Else set the PauseMenuCanvas gameObject to inactive
        // Also, set the timeScale back to 1f so the user can resume moving their catapult
        else
        {
            //pauseMenuCanvas.SetActive(false);
            pauseMenuCanvas.GetComponent<Canvas>().enabled = false;
            Time.timeScale = 1f;
        }

        // If user presses P key, switch values of isPaused
        if (Input.GetKeyDown((KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Pause", "P"))))
        {
            isPaused = !isPaused;
            if(Cursor.lockState == CursorLockMode.None)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            else if(Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
            }
        }

        if (isMuted)
        {
            AudioListener.volume = 0;
            muteText.text = "Unmute Audio";
        }
        else
        {
            AudioListener.volume = 1;
            muteText.text = "Mute Audio";
        }
    }

    /// <summary> Allows user to mute the background audio </summary>
    public void MuteAudio()
    {
        isMuted = !isMuted;
    }

    /// <summary> Simply sets isPaused to false </summary>
	public void Resume()
    {
        isPaused = false;
    }

    /// <summary> Simply loads the first scene in the build settings, i.e. the main menu </summary>
    public void Quit()
    {
        SceneManager.LoadScene(0);
    }
}
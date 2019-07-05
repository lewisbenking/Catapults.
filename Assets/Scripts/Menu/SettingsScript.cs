using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SettingsScript : MonoBehaviour {

    public bool isSettings;
    public bool isMuted = false;
    [SerializeField]
    Text muteText;
    public GameObject settingsCanvas;
    public GameObject mainCanvas;

    // Use this for initialization
    void Start () {
        GetComponent<AudioSource>().Play();
    }

    void update()
    {
        if (isSettings)
        {
            settingsCanvas.GetComponent<Canvas>().enabled = true;
            mainCanvas.GetComponent<Canvas>().enabled = false;
        }
        else
        {
            settingsCanvas.GetComponent<Canvas>().enabled = false;
            mainCanvas.GetComponent<Canvas>().enabled = true;
        }
        

        if (isMuted)
        {
            AudioListener.volume = 0;
            muteText.text = "Unmute Audio";
            Debug.Log("Muted Audio");
        }
        else
        {
            AudioListener.volume = 1;
            muteText.text = "Mute Audio";
            Debug.Log("Unmuted Audio");
        }
    }

    /// <summary> Allows user to mute the background audio </summary>
    public void MuteAudio()
    {
        //Debug.Log("Has clicked Mute Audio button");
        isMuted = !isMuted;
        if (isMuted)
        {
            AudioListener.volume = 0;
            muteText.text = "Unmute Audio";
            Debug.Log("Muted Audio");
        }
        else
        {
            AudioListener.volume = 1;
            muteText.text = "Mute Audio";
            Debug.Log("Unmuted Audio");
        }
    }
}
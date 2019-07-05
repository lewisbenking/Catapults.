using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

///<summary> Loads scene based on user input </summary>
///<remarks> Had to import UnityEngine.SceneManagement as previous way of doing this is now obsolete </remarks>
///<author> Lewis King </author>

public class MenuCtrl : MonoBehaviour
{
    private Text text;
    private bool secret;

    public void Start()
    {
        //GetComponent<AudioSource>().Play();
        //GameObject.Find("Audio Source").GetComponent<AudioSource>().Play();
    }

    public void FixedUpdate()
    {
        if (GameObject.Find("SecretEmitter").GetComponent<ParticleSystem>().particleCount > 0)
        {
            try
            {
                //Text secretText = GameObject.Find("MainCanvas").transform.Find("SecretText").GetComponent<Text>();
                Text secretText = GameObject.Find("SecretText").GetComponent<Text>();
                secretText.enabled = true;
                secretText.CrossFadeAlpha(1.0f, 0.1f, false);
                secret = true;
            }
            catch
            {
                secret = true;
            }
        }
        else if (secret == true)
        {
            try
            {
                //GameObject.Find("MainCanvas").transform.Find("SecretText").GetComponent<Text>().CrossFadeAlpha(0.0f, 0.1f, true);
                GameObject.Find("SecretText").GetComponent<Text>().CrossFadeAlpha(0.0f, 0.1f, true);
                secret = false;
            }
            catch
            {
                secret = false;
            }
        }
    }

    ///<summary> Loads scene based on user input </summary>
    ///<param name="scene"> User specifies the scene to change to </param>
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
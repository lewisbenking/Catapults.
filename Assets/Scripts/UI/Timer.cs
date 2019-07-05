using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour {

    private Text timerText;
    private float currentTime;
    private bool timerOn;
    private bool completed;

	// Use this for initialization
	void Start () {
        timerText = GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
        if (timerOn)
        {
            //calculate change in time
            currentTime -= Time.deltaTime;

            //get minutes number as a string
            string mins = (currentTime / 60f).ToString();
            char digit1 = mins[0];
            char digit2 = mins[1];
            if (digit2.ToString().Equals(".")){ //add leading zero for 1 single digit number
                digit2 = digit1;
                digit1 = '0';
            }

            //set Timer text value
            timerText.text = (digit1.ToString() + digit2.ToString()).PadLeft(2,'0') + ":" + string.Format("{0:00}", currentTime % 60);
            
            if (currentTime <= 0)
            {
                currentTime = 0;
                completed = true;
                timerOn = false;
            }

            if (currentTime < 10)
            {
                timerText.color = new Color(1, 0, 0);
            }
            else
            {
                if(SceneManager.GetActiveScene().name == "Level_Night")
                {
                    timerText.color = new Color(1, 1, 1);
                }else
                {
                    timerText.color = new Color(1, 1, 1);
                }
                
                timerText.CrossFadeAlpha(0.8f, 1f, true);
            }
        }
	}

    public void startCountdown(int minutes,int seconds)
    {
        currentTime = seconds + (minutes * 60);
        
        timerOn = true;
        completed = false;
    }

    public bool isCompleted()
    {
        return completed;
    }

    public void stop()
    {
        timerOn = false;
    }
}

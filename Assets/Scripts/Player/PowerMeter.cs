using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PowerMeter : MonoBehaviour {

    private bool firing;
    private bool currentTurn;

    private Transform playerController;
    private Animator anim;

	// Use this for initialization
	void Start () {
        playerController = transform.parent.parent.parent; //get PlayerController Object
        anim = playerController.transform.Find("Player").GetComponent<Animator>(); //get Player's Animator Object
        if(SceneManager.GetActiveScene().name == "Level_Sunset")
        {
            GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("PowerBar_Empty_White");
            transform.parent.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("PowerBar_Empty_White");
        }
    }
	
	// Update is called once per frame
	void Update () {

        if (playerController.GetComponent<CharacterController>().isCurrentTurn())
        {
            //If firing sequence started
            if (firing == true)
            {
                //Increment x & y scale values of PowerMeterFull by 0.01
                Vector3 meterScale = transform.localScale;
                meterScale.x = (float)(meterScale.x + 0.01);
                meterScale.y = (float)(meterScale.y + 0.01);
                transform.localScale = meterScale;

                //If the player releases space or PowerMeterFull is the same scale as PowerMeter
                if ((Input.GetKeyUp((KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Fire", "Space")))
                     || (transform.localScale.x >= 1)))
                {
                    //Set firePower value in CharacterController using current x scale as a percentage
                    playerController.GetComponent<CharacterController>().setFirePower(transform.localScale.x * 100);

                    //Halt player's keyboard input
                    playerController.GetComponent<CharacterController>().setCurrentTurnState(false);

                    //Trigger fire animation
                    anim.SetTrigger("fire");

                    //Reset
                    firing = false;
                    transform.localScale = new Vector3(0, 0, 0);
                }
            }
        }
    }

    public void startFire()
    {
        firing = true;
    }

    public bool isFiring()
    {
        return firing;
    }
}

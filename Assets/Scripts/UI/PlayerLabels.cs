using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerLabels : MonoBehaviour {

    private CharacterController characterController;
    private Text healthLabel;
    private Text nameLabel;
    private Color playerColor;
    
    void Start () {
        //initialise field objects
        characterController = gameObject.transform.parent.GetComponent<CharacterController>();
        healthLabel = gameObject.transform.Find("HealthLabel").GetComponent<Text>(); //get HealthLabel text box
        nameLabel = gameObject.transform.Find("NameLabel").GetComponent<Text>(); //get NameLabel text box
        playerColor = characterController.getPlayerColor();

        //set label colours to the player's colour field - with a fixed alhpa (transparency) value of 150
        healthLabel.color = new Color(playerColor.r, playerColor.g, playerColor.b, 150);
        nameLabel.color = new Color(playerColor.r, playerColor.g, playerColor.b, 150);

        //update player name label
        nameLabel.text = characterController.getPlayerName();
    }
	
	// Update is called once per frame
	void Update () {
        healthLabel.text = characterController.getCurrentHealth().ToString(); //update health label with the currentHealth field of the player
    }
}

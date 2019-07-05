using UnityEngine;
using System.Collections;
using UnityEngine.UI;

///<summary> Retrieves user input into field and creates an entry in playerPrefs so it can be called later </summary>
///<remarks> Attatched to the button that initiates the game. Imported UnityEngine.UI to access Text </remarks>
///<author> Lewis King </author>

public class GetPlayerPrefs : MonoBehaviour {

    private Text playerName; // This will link to the text field that displays the player's name

    void Awake ()
    {
        playerName = gameObject.transform.Find("PlayerLabel").GetComponent<Text>(); // Retrieve the object that will store the name
        playerName.text = "Player: " + PlayerPrefs.GetString("Player1", ""); // Update the text in that object to contain the PlayerPrefs value
    }
}
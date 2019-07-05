using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

///<summary> Retrieves user input into field and creates an entry in playerPrefs so it can be called later </summary>
///<remarks> Attatched to the button that initiates the game </remarks>
///<author> Lewis King </author>

public class SavePlayerPrefs : MonoBehaviour {

    public InputField playerName1; // Input field that asks for the user's name
    public InputField playerName2;
    public Text t1p1Text; // The text in the input field
    public Text t2p1Text; // The text in the input field
    public Color player1Color;
    public Color player2Color;

    public void SaveName()
    {
        // find the objects
        t1p1Text = GameObject.Find("Team1Player1").GetComponent<InputField>().textComponent;
        t2p1Text = GameObject.Find("Team2Player1").GetComponent<InputField>().textComponent;
        // set playerPrefs based on value
        PlayerPrefs.SetString("Player0", t1p1Text.text);
        PlayerPrefs.SetString("Player1", t2p1Text.text);

        string[] playerNames = new string[2];
        playerNames[0] = t1p1Text.text;
        playerNames[1] = t2p1Text.text;
        PlayerPrefsX.SetStringArray("PlayerNames", playerNames);
    }

    public void SaveColour()
    {
        // 03/02/2017 -> Lewis -> Updated code so now the colours are stored in a color array rather individual strings.
        // I have commented the original code in for now, so if we need to revert back to the old way we can do it quickly.

        // find the objects
        player1Color = GameObject.Find("Player1Text").GetComponent<Text>().color;
        player2Color = GameObject.Find("Player2Text").GetComponent<Text>().color;
        // set playerPrefs based on value toString()
        //PlayerPrefs.SetString("Team0Colour", player1Color.ToString());
        //PlayerPrefs.SetString("Team1Colour", player2Color.ToString());

        Color[] playerColors = new Color[2];
        playerColors[0] = player1Color;
        playerColors[1] = player2Color;
        PlayerPrefsX.SetColorArray("PlayerColors", playerColors);        
    }
}
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

///<summary> Allows the user to customise their own controls </summary>
///<remarks> Attached to the KeyBindManager gameobject </remarks>
///<author> Lewis King </author>

public class KeyBinding : MonoBehaviour {

    // Store the key codes in a dictionary variable called keys
    private Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();
    // Text variables for the button text
    public Text fire, up, left, down, right, pause, inventory;
    // Current key game object
    private GameObject currentKey;

	// Use this for initialization
	void Start () {
        // Sets up the keys variable, parses the value in PlayerPrefs to a KeyCode
        keys.Add("Up", (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Up", "UpArrow")));
        keys.Add("Down", (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Down", "DownArrow")));
        keys.Add("Left", (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Left", "LeftArrow")));
        keys.Add("Right", (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Right", "RightArrow")));
        keys.Add("Fire", (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Fire", "Space")));
        keys.Add("Pause", (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Pause", "P")));
        keys.Add("Inventory", (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Inventory", "I")));

        // Sets the text of the buttons to the corresponding value in keys
        up.text = keys["Up"].ToString();
        down.text = keys["Down"].ToString();
        left.text = keys["Left"].ToString();
        right.text = keys["Right"].ToString();
        fire.text = keys["Fire"].ToString();
        pause.text = keys["Pause"].ToString();
        inventory.text = keys["Inventory"].ToString();
    }

    // This is used because it is called more frequently than Update()
    void OnGUI()
    {
        if (currentKey != null)
        {
            // If the current event is a key
            Event e = Event.current;
            if (e.isKey)
            {
                // change the text of the button to the current key, reset currentKey value.
                keys[currentKey.name] = e.keyCode;
                currentKey.transform.GetChild(0).GetComponent<Text>().text = e.keyCode.ToString();
                currentKey = null;
            }
        }
    }

    public void ChangeKey(GameObject clicked)
    {
        currentKey = clicked;
    }

    public void SaveKeys()
    {
        // for each key in keys, set PlayerPrefs string to the key and it's value
        foreach (var key in keys)
        {
            PlayerPrefs.SetString(key.Key, key.Value.ToString());
        }
    }

    public void ResetKeys()
    {
        // Reset keys, give it default values. Also resets the button text values.
        keys = null;
        keys = new Dictionary<string, KeyCode>();

        keys.Add("Up", KeyCode.UpArrow);
        keys.Add("Down", KeyCode.DownArrow);
        keys.Add("Left", KeyCode.LeftArrow);
        keys.Add("Right", KeyCode.RightArrow);
        keys.Add("Fire", KeyCode.Space);
        keys.Add("Pause", KeyCode.P);
        keys.Add("Inventory", KeyCode.I);

        up.text = keys["Up"].ToString();
        down.text = keys["Down"].ToString();
        left.text = keys["Left"].ToString();
        right.text = keys["Right"].ToString();
        fire.text = keys["Fire"].ToString();
        pause.text = keys["Pause"].ToString();
        inventory.text = keys["Inventory"].ToString();
    }
}
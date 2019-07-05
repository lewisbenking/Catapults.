using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamHealthBar : MonoBehaviour
{

    private Slider healthBar;
    private int playerNumber;
    private GameController gameController;
    private Color playerColor;
    

    // Use this for initialization
    void Start()
    {
        gameController = GameObject.Find("_GameManager").GetComponent<GameController>();
        healthBar = gameObject.GetComponent<Slider>();
        healthBar.value = 100;
        playerNumber = System.Int32.Parse(gameObject.name);
        playerColor = gameController.getPlayerColor(playerNumber);
        transform.Find("Fill Area").transform.Find("Fill").GetComponent<Image>().color = playerColor;

     }

    // Update is called per second
    void Update()
    {
        healthBar.value = Mathf.Lerp(healthBar.value,gameController.getPlayerHealth(playerNumber),0.2f);
    }
}


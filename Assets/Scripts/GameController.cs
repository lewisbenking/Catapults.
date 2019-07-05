using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    private int numPlayers;
    private int numTeams;
    private int currentPlayer;
    private bool inIntermediateState; // true if neither player's turn i.e. mid projectile throw
    private bool showInventory;
    private bool gameOver;
    private bool turnEnding; //used by character controllers to end turn on non-projectile based moves

    private Transform spawnPosition;
    private GameObject[] players;
    private Transform[] spawnPositions;
    private GameObject screenUI;
    private Transform gameTimer;
    private Transform turnTimer;
    private InventoryManager inventoryManager;

    // Use this for initialization
    void Start()
    {
        screenUI = GameObject.Find("Screen UI");
        spawnPosition = GameObject.Find("SpawnPositions").transform; //get SpawnPositions GameObject
        numPlayers = spawnPosition.childCount; //set number of players to number of SpawnPos gameObjects
        gameTimer = screenUI.transform.Find("TimerPanel").Find("GameTimer");
        gameTimer.GetComponent<Timer>().startCountdown(3, 30); //set game timer to arbitrary value (temporary) mins must be <100
        turnTimer = screenUI.transform.Find("TimerPanel").Find("TurnTimer");
        inventoryManager = screenUI.transform.Find("Inventory Panel").GetComponent<InventoryManager>();

        players = new GameObject[numPlayers]; //max 2 players for now
        spawnPositions = new Transform[numPlayers]; //set number of spawn positions to number of spawn positions in game world
        inventoryManager.initialiseInventories(numPlayers); //initialises the starting inventories of all players to default state

        for (int j = 0; j < numPlayers; j++) //set the spawnPositions array to contain the transform components all SpawnPos GameObjects
        {
            spawnPositions[j] = spawnPosition.GetChild(j).transform;
        }
        
        for (int i = 0; i < numPlayers; i++) //instantiate player controllers in positions of SpawnPos GameObject positions
        {
            GameObject newPlayer = (GameObject)Instantiate(Resources.Load("Player Controller"), spawnPositions[i].position, Quaternion.identity);
            string playerNumber = "Player" + i;
            newPlayer.name = PlayerPrefs.GetString(playerNumber, "");
            newPlayer.GetComponent<CharacterController>().setPlayerName(newPlayer.name);
            newPlayer.GetComponent<CharacterController>().setPlayerColor(setTeamColourUI(i));
            players[i] = newPlayer;
        }

        currentPlayer = 0; //set currentPlayer field to player 1 (0)

        players[currentPlayer].GetComponent<CharacterController>().setCurrentTurnState(true); //set player 1's current turn to true
        setPlayerLabel(players[currentPlayer].GetComponent<CharacterController>().getPlayerName());
        showInventory = false;

        turnTimer.GetComponent<Timer>().startCountdown(0, 30);

        setTeamColourUI(currentPlayer);
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameOver)
        {
            //Temporary (dev)
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                switchToNextPlayer();
            }

            //when user clicks on game screen
            if (Input.GetMouseButtonDown(0))
            {
                //if inventory isn't up & mouse is free & pause menu isn't up
                if (showInventory == false && Cursor.lockState == CursorLockMode.None && GameObject.Find("PauseMenu").transform.Find("Canvas").GetComponent<Canvas>().enabled == false)
                {
                    Cursor.lockState = CursorLockMode.Locked;
                }
            }

            //show controls ui
            if (Input.GetKeyDown("c"))
            {
                screenUI.transform.Find("Controls").GetComponent<Text>().CrossFadeAlpha(1f, 0.1f, true);
            }

            //hide controls ui
            if (Input.GetKeyUp("c"))
            {
                screenUI.transform.Find("Controls").GetComponent<Text>().CrossFadeAlpha(0f, 0.1f, false);
            }

            //toggle inventory, change cursor state & toggle camera enabled
            if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Inventory", "I"))))
            {

                //if inventory needs to be closed
                if (showInventory == true)
                {
                    showInventory = false;
                    screenUI.transform.Find("Inventory Panel").GetComponent<InventoryManager>().gameObject.SetActive(false);
                    //lock & hide mouse
                    Cursor.lockState = CursorLockMode.Locked;
                    Camera.main.GetComponent<CameraFollow>().enabled = true;
                }
                else //if inventory needs to be opened
                {
                    showInventory = true;
                    screenUI.transform.Find("Inventory Panel").GetComponent<InventoryManager>().gameObject.SetActive(true);
                    //unlock & show mouse
                    Cursor.lockState = CursorLockMode.None;
                    Camera.main.GetComponent<CameraFollow>().enabled = false;
                }
            }

            
            if(turnEnding == true) //If player has triggered end of turn e.g. place an object
            {
                if (GameObject.FindWithTag("Projectile") != null) //If a projectile object exists in game
                {
                    Camera.main.GetComponent<CameraFollow>().setCameraMode(1);
                }

                inIntermediateState = true;
                turnTimer.GetComponent<Timer>().stop();
                turnEnding = false;
            }
            else if (inIntermediateState && GameObject.FindWithTag("Projectile") == null) //If turn has ended and switching to next turn & no projectile is found in the game
            {
                //Switch to next player and tell camera to move to their position
                switchToNextPlayer();
                Camera.main.GetComponent<CameraFollow>().setPlayerToFollow(players[currentPlayer]);
                Camera.main.GetComponent<CameraFollow>().setCameraMode(2);
                inIntermediateState = false;
            }

            //Check if gametimer has ended ## LOGIC ONLY WORKS FOR ONE PERSON PER TEAM
            if (gameTimer.GetComponent<Timer>().isCompleted())
            {
                gameOver = true;

                //calculate player with most health
                int largestHealth = 0;
                int bestPlayer = 0;
                for(int i = 0; i < numPlayers; i++)
                {
                    if (players[i].GetComponent<CharacterController>().getCurrentHealth() > largestHealth)
                    {
                        bestPlayer = i;
                        largestHealth = players[i].GetComponent<CharacterController>().getCurrentHealth();
                    }
                }

                currentPlayer = bestPlayer;
                playerWins();
            }

            if (turnTimer.GetComponent<Timer>().isCompleted())
            {
                switchToNextPlayer();
                Camera.main.GetComponent<CameraFollow>().setPlayerToFollow(players[currentPlayer]);
                Camera.main.GetComponent<CameraFollow>().setCameraMode(2);
                turnTimer.GetComponent<Timer>().startCountdown(0, 30);
            }
        }
        else
        {
            playerWinsLabel();

            if (Input.GetKeyDown("m"))
            {
                Cursor.lockState = CursorLockMode.None;
                SceneManager.LoadScene("Menu");
            }
        }
    }

    //switch character
    public void switchToNextPlayer()
    {
        //check whether a player has won
        for (int i = 0; i < numPlayers; i++)
        {
            if (players[i].GetComponent<CharacterController>().getCurrentHealth() <= 0)
            {
                playerWins();
            }
        }
        players[currentPlayer].GetComponent<CharacterController>().setPowerMeterVisibility(false);
        players[currentPlayer].GetComponent<CharacterController>().setCurrentTurnState(false);
        currentPlayer = (currentPlayer + 1) % numPlayers;
        players[currentPlayer].GetComponent<CharacterController>().setCurrentTurnState(true);
        players[currentPlayer].GetComponent<CharacterController>().setPowerMeterVisibility(true);
        screenUI.transform.Find("Inventory Panel").GetComponent<InventoryManager>().setCurrentItem(0); //set current item to rock at the start of every turn
        screenUI.transform.Find("Inventory Panel").GetComponent<InventoryManager>().setCurrentPlayer(currentPlayer);
        players[currentPlayer].GetComponent<CharacterController>().takePoison();

        //set current player label ui object to the current player's name
        setPlayerLabel(players[currentPlayer].GetComponent<CharacterController>().getPlayerName());

        setTeamColourUI(currentPlayer);
        turnTimer.GetComponent<Timer>().startCountdown(0, 30);
    }

    //sets playerlabel gameobject text to the a given string(player's name)
    private void setPlayerLabel(string currentPlayerName)
    {
        screenUI.transform.Find("PlayerLabel").GetComponent<Text>().text = currentPlayerName + "'s turn";
    }

    public void playerWins()
    {
        gameOver = true;
        Destroy(players[(currentPlayer + 1) % numPlayers]);
        print("PlayerWins");
        screenUI.transform.Find("PlayerWinsText").GetComponent<Text>().enabled = true;
        screenUI.transform.Find("PlayerWinsText").GetComponent<Text>().text = players[currentPlayer].GetComponent<CharacterController>().getPlayerName() + " Wins!";
        screenUI.transform.Find("Inventory Panel").gameObject.SetActive(false);
        screenUI.transform.Find("CurrentItem").gameObject.SetActive(false);
        screenUI.transform.Find("TeamHealthBar").gameObject.SetActive(false);
        screenUI.transform.Find("TimerPanel").gameObject.SetActive(false);
        screenUI.transform.Find("PlayerLabel").gameObject.SetActive(false);
        playerWinsLabel();
        Time.timeScale = 0f;
    }

    public void otherPlayerWins()
    {
        currentPlayer = (currentPlayer + 1) % numPlayers;
        playerWins();
    }

    private void playerWinsLabel()
    {
        if (screenUI.transform.Find("PlayerWinsText").GetComponent<Text>().fontSize < 92)
        {
            screenUI.transform.Find("PlayerWinsText").GetComponent<Text>().fontSize++;
        }
        else
        {
            screenUI.transform.Find("PlayerWinsText").transform.Find("PressMText").GetComponent<Text>().enabled = true;
        }
    }

    private Color setTeamColourUI(int playerNum)
    {
        // If any exceptions happen, it will return 0,0,0,0 for the rgba values.

        //change current team colour ui
        try
        {
            Color[] c = PlayerPrefsX.GetColorArray("PlayerColors");
            return c[playerNum];
        }
        catch
        {
            return new Color(0, 0, 0, 0);
        }
    }

    public GameObject getCurrentPlayer()
    {
        return players[currentPlayer];
    }

    public Transform getNextPlayer()
    {
        return players[(currentPlayer + 1) % numPlayers].transform;
    }

    public void setTurnEnding(bool newBool)
    {
        turnEnding = newBool;
    }

    public int getPlayerHealth(int playerNum)
    {
        return players[playerNum].GetComponent<CharacterController>().getCurrentHealth();
    }

    public Color getPlayerColor(int playerNum)
    {
        return players[playerNum].GetComponent<CharacterController>().getPlayerColor();
    }

}

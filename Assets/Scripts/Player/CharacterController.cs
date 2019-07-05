using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

///<summary> Handles player movement & Actions </summary>
///<remarks> Attatched to Player Controller GameObject </remarks>
///<author> Charlie Postgate </author>

public class CharacterController : MonoBehaviour {

    
    private GameObject player; 
    private Animator anim;
    private Transform powerMeter;
    private Transform powerMeterFull;
    private Rigidbody2D body;
    private Transform throwPoint;
    private Vector3 throwPos, aimPoint;
    private ItemDatabase itemDatabase;
    private InventoryManager inventoryManager;
    private GameController gameController;
    private bool facingRight = true;
    private float firePower, fireAngle, move, maxSpeed, playerAngle, relativeAngle;
    private bool placingObject;
    private int turnsPoisoned = 0;
    
    
    //arbitrary initial values are used
    private bool currentTurn = false;
    private string playerName="defaultName";
    private int playerTeam = 0;
    private int currentHealth=100;
    private Color playerColor=Color.black;

    void Start () {
        //Assign objects & components to fields
        player = gameObject.transform.Find("Player").gameObject;
        anim = player.GetComponent<Animator>();
        powerMeter = player.transform.Find("PowerMeter");
        powerMeterFull = powerMeter.Find("PowerMeterFull");
        body = GetComponent<Rigidbody2D>();
        throwPoint = player.transform.Find("ThrowPoint");
        itemDatabase = GameObject.Find("ItemDatabase").GetComponent<ItemDatabase>();
        powerMeter.gameObject.SetActive(false);
        inventoryManager = GameObject.Find("Screen UI").transform.Find("Inventory Panel").GetComponent<InventoryManager>();
        gameController = GameObject.Find("_GameManager").GetComponent<GameController>();

        //Initialise to arbitary values
        firePower = 0;
        fireAngle = 20;
        maxSpeed = 1f;

        //Face player toward game world origin
        if (gameObject.transform.position.x > 0)
        {
            Flip();
        }

        if(SceneManager.GetActiveScene().name == "Level_Sunset")
        {
            print("sunset colour");
            player.GetComponent<SpriteRenderer>().color = new Color(0.5882f, 0.3058f, 0.59607f, 1);
        }
    }

    ///<summary> Handles aiming GUI </summary>
    ///<remarks> Called once every frame - not physics based </remarks>
    void Update()
    {
        if (currentTurn) //if it's this player object's current turn
        {
            //render the powerMeter if it's disabled
            if(powerMeter.gameObject.activeInHierarchy == false)
            {
                powerMeter.gameObject.SetActive(true);
            }

            //Rotate PowerMeter to current aiming angle (offset graphical inaccuracy by 3 degrees)
            powerMeter.transform.rotation = Quaternion.Euler(0, 0, relativeAngle+3); 
        }
    }
    
    ///<summary> Handles user input & firing GUI </summary>
    ///<remarks> Called once every physics tick - not frame </remarks>
    void FixedUpdate () {
        if (currentTurn) //if it's this player object's current turn
        {
            //Update movement
            move = Input.GetAxis("Horizontal");
            anim.SetFloat("speed", System.Math.Abs(move));
            body.velocity = new Vector2(move * maxSpeed, body.velocity.y);

            //Update sprite direction
            if ((move > 0 && !facingRight) || move < 0 && facingRight)
            { Flip(); }

            // If fire key is pressed, default to space if not changed in menu
            if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Fire", "Space"))))
            {
                //if currently placing an object:
                if (placingObject)
                {
                    placingObject = false; //object to be placed will detect change in placingObject & will place
                    inventoryManager.removeItem(inventoryManager.getCurrentItem());
                    currentTurn = false; //lock player controls
                    gameController.setTurnEnding(true);
                }
                else if (itemDatabase.getItem(inventoryManager.getCurrentItem()).getName() == "Repair")
                {
                    addHealth(30);
                    gameController.setTurnEnding(true);
                    currentTurn = false; //lock character controls
                }
                else if (itemDatabase.getItem(inventoryManager.getCurrentItem()).getName() == "Lightning Strike")
                {
                    GameObject lightning = Resources.Load<GameObject>("Items/Lightning");
                    GameObject lightningStrike = Instantiate(lightning, transform.position, Quaternion.Euler(new Vector3(0, 0, 10))) as GameObject;
                    lightningStrike.GetComponent<Lightning>().setTarget(gameController.getNextPlayer());
                    gameController.setTurnEnding(true);
                    currentTurn = false; //lock character controls
                }
                else if (!powerMeterFull.GetComponent<PowerMeter>().isFiring())
                {
                    powerMeterFull.GetComponent<PowerMeter>().startFire();
                }
            } 

            //Update projectile angle
            float angleInput = Input.GetAxisRaw("Vertical");
            if (!facingRight) //inverts up/down input if facing left
            { angleInput *= -1; }

            //Calculate change in firePower value
            float newAngle = fireAngle + (angleInput / 4); //newangle = fireangle +/- 0.25

            //Check angle constraints
            if (facingRight)
            {
                if (newAngle >= -10 && newAngle <= 90) //ensures angle doesn't go >90 or <-10 when facing right
                { fireAngle = newAngle; }
            }
            else //(if facing left)
            {
                if (newAngle <= 190 && newAngle >= 90) //ensures angle doesn't go <190 or >90 when facing left
                { fireAngle = newAngle; }
            }

            //Get player's z rotation
            playerAngle = gameObject.transform.localEulerAngles.z;

            //set relativeAngle to fireAngle in relation to player's current rotation
            if (playerAngle > 0 && playerAngle < 180) //if on upslope
            {
                relativeAngle = fireAngle + playerAngle;
            }
            else //if on downslope
            {
                relativeAngle = fireAngle + (playerAngle - 360);
            }
        }
    }

    ///<summary> Flips the world to simulate sprite turning </summary>
    ///<remarks> More efficient than flipping character sprite as it halves no. of animations </remarks>
    void Flip() 
    {
        facingRight = !facingRight;

        //invert player object's x scale
        Vector3 playerScale = player.transform.localScale;
        playerScale.x *= -1;
        player.transform.localScale = playerScale;

        //invert PowerMeter object's x scale
        Vector3 powerMeterScale = powerMeter.transform.localScale;
        powerMeterScale.x *= -1;
        powerMeter.transform.localScale = powerMeterScale;

        if (!facingRight)
        { fireAngle = 90 + (90 - fireAngle);}
        else
        {fireAngle = (180 - fireAngle);}
    }
    
    ///<summary> Fires Projectile </summary>
    ///<remarks> Activated by fire animation completion </remarks>
    public void triggerFire() 
    {
        if(itemDatabase.getItem(inventoryManager.getCurrentItem()).getIsProjectile() == true) //if currentItem is a weapon
        {
            throwPoint.GetComponent<AimInput>().Throw(inventoryManager.removeItem(inventoryManager.getCurrentItem()).getGameObject(), firePower, relativeAngle); //call Throw method in AimInput Script & pass fireAngle relative to player rotation
        }
        gameController.setTurnEnding(true);
        currentTurn = false; //lock character controls
    }

    public void setPowerMeterVisibility(bool isVisible)
    {
        powerMeter.GetComponent<Renderer>().enabled = isVisible;
    }

    ///<summary> Accessor method to get the current fire angle </summary>
    public float getFireAngle()
    {
        return fireAngle;
    }

    ///<summary> Accessor method to get the current fire power </summary>
    public float getFirePower()
    {
        return firePower;
    }

    ///<summary> Sets firePower to specified value </summary>
    ///<remarks> Used by PowerMeter Script </remarks>
    public void setFirePower(float firePower)
    {
        this.firePower = firePower;
    }

    ///<summary> Accessor method to get whether it's this player object's current turn </summary>
    public bool isCurrentTurn()
    {
        return currentTurn;
    }

    ///<summary> Set whether it's this player's turn </summary>
    public void setCurrentTurnState(bool state)
    {
        currentTurn = state;

        //toggle powerMeteter rendering
        if(powerMeter != null)
        {
            if (currentTurn == false)
            {
                powerMeter.gameObject.SetActive(false);
            }
            else
            {
                powerMeter.gameObject.SetActive(true);
            }
        }
    }

    ///<summary> Set the playerName value for this character </summary>
    public void setPlayerName(string name)
    {
        playerName = name;
    }

    ///<summary> Accessor method to get this player's Name </summary>
    public string getPlayerName()
    {
        return playerName;
    }

    ///<summary> Set the team of this Player </summary>
    public void setPlayerTeam(int teamNum)
    {
        playerTeam = teamNum;
    }

    ///<summary> Accessor method to get this player's team </summary>
    public int getPlayerTeam()
    {
        return playerTeam;
    }

    ///<summary> Set the health of the player </summary>
    ///<remarks> Is used at the creation of the object to set the starting health </remarks>
    public void setHealth(int newHealth)
    {
        currentHealth = newHealth;
    }

    ///<summary> Add an integer health value to the current health </summary>
    ///<remarks> Used by pickups to increase player's health </remarks>
    public void addHealth(float healthQuantity)
    {
        currentHealth += (int) healthQuantity;

        GameObject healthPopup = (GameObject) Resources.Load("DamagePopup");
        healthPopup.transform.localPosition = gameObject.transform.localPosition;

        if (healthQuantity < 0)
        {
            healthPopup.transform.Find("DamagePopupText").GetComponent<HealthPopup>().initiatePopup(Color.red, ((int)healthQuantity).ToString());
        }
        else
        {
            healthPopup.transform.Find("DamagePopupText").GetComponent<HealthPopup>().initiatePopup(Color.green, "+" + ((int) healthQuantity).ToString());
        }

        Instantiate(healthPopup);
    }

    ///<summary> Accessor method to get this player's current health value </summary>
    public int getCurrentHealth()
    {
        return currentHealth;
    }

    ///<summary> Set the Player's colour </summary>
    ///<remarks> Is used at the creation of the object to set the player's colour to its team's colour </remarks>
    ///<remarks> Used with the player's corresponding UI labels </remarks>
    public void setPlayerColor(Color newColor)
    {
        playerColor = newColor;
    }

    ///<summary> Accessor method to get the player's color value (RGBA format) </summary>
    public Color getPlayerColor()
    {
        return playerColor;
    }

    /// <summary> Accessor method to get the player's current facing direction </summary>
    public bool getFacingRight()
    {
        return facingRight;
    }

    public void setPlacingObject(bool newBool)
    {
        if(newBool == true && placingObject == false) //if new placingObject bool is true and player isn't already placing
        {
            setPowerMeterVisibility(false);
        }
        placingObject = newBool;
    }

    public bool isPlacingObject()
    {
        return placingObject;
    }

    public void addPoison(int numTurns)
    {
        turnsPoisoned += numTurns;
    }

    public void takePoison()
    {
        if (turnsPoisoned > 0)
        {
            turnsPoisoned--;
            addHealth(-10);
        }
    }
}

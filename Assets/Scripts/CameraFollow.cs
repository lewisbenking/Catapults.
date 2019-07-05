using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

///<summary> Handles the camera controls & boundaries </summary>
///<remarks> Attatched to a Camera GameObject</remarks>
///<author> Charlie Postgate </author>

public class CameraFollow : MonoBehaviour {

    private Vector2 velocity; //used by camera smoothing calculations
    private GameObject player; //used if camera set to follow Player
    private float camOrthoSize; //main camera's orthographic size
    private float posX; //current cam x position
    private float posY; //current cam y position

    //camera movement smoothing values
    private float smoothTimeY = 0.5f;
    private float smoothTimeX = 0.5f;

    //True if boundaries are required for camera
    private bool bounds = true;
    private EdgeCollider2D LevelBounds; //bounds of the level
    private Vector3 minCameraPos;
    private Vector3 maxCameraPos;

    //speed multiplier for camera movement
    private float camSpeed = 200f;
    private float zoomSpeed = 2f;
    private float zoomSmoothSpeed = 2f;

    //Set min/max camera sizes
    private float minOrthoSize = 2f;
    private float maxOrthoSize;

    //Following values
    private int cameraMode;
    private float timeSincePanStart;
    private float playerCamLockTime = 1f;

    ///<summary> Sets the cursor state on startup & initialises the cameras boundaries </summary>
    ///<remarks> Camera boundaries are based opon the GameObject 'LevelBoundary's' Edge Collider </remarks>
    void Start() {
        Scene scene = SceneManager.GetActiveScene();

        if (scene.name == "Level_Basic")
        {
            maxOrthoSize = 10f;
        }
        else if(scene.name == "Level_Castle" || scene.name == "Level_Night" || scene.name == "Level_Sunset")
        {
            maxOrthoSize = 15f;
        }

        //Lock cursor to center of game window and hide
        Cursor.lockState = CursorLockMode.Locked;

        //Get camera's initial orthographic size
        camOrthoSize = Camera.main.orthographicSize;

        //Set camera to free mode
        cameraMode = 0;

        if (bounds) //if boundaries have been set for the level
        {
            //Get box collider of LevelBoundary GameObject
            LevelBounds = GameObject.Find("LevelBoundary").GetComponent<EdgeCollider2D>();

            //Set min/max values of bounds
            minCameraPos = LevelBounds.bounds.min;
            maxCameraPos = LevelBounds.bounds.max;
        }
        else
        {
            //Get random player game object to follow
            player = GameObject.Find("Player");
            cameraMode = 2;
            print("Level Bounds not found - please set level bounds EdgeCollider2D for this level");
        }
    }

    ///<summary> Sets the camera position based on relative mouse position to the last frame </summary>
    ///<remarks> Method called once per frame </remarks>
    void FixedUpdate() {

        //Initialise new camera pos values
        posX = transform.position.x;
        posY = transform.position.y;

        if (cameraMode == 0) //if the camera is free & controlled by user
        {
            freeCam();
        }
        else if (cameraMode == 1 && GameObject.FindWithTag("Projectile") != null) //if the camera is following a projectile & a projectile exists
        {
            followProjectile();
        }
        else if (cameraMode == 2) //if the camera is following a player
        {
            timeSincePanStart += Time.deltaTime;
            //if camera has reached the player's position
            if (timeSincePanStart >= playerCamLockTime) 
            {
                //set cameraMode to freeCam
                cameraMode = 0;
                timeSincePanStart = 0;
            }
            else
            {
                followPlayer();
            }
        }


        //Set camera to new position
        transform.position = new Vector3(posX, posY, transform.position.z);

        //Constrain camera to defined level boundaries
        if (bounds)
        {
            float cameraHalfWidth = camOrthoSize * ((float)Screen.width / Screen.height);

            // lock the camera to the right or left bound if we are touching it
            posX = Mathf.Clamp(posX, minCameraPos.x + cameraHalfWidth, maxCameraPos.x - cameraHalfWidth);

            // lock the camera to the top or bottom bound if we are touching it
            posY = Mathf.Clamp(posY, minCameraPos.y + camOrthoSize, maxCameraPos.y - camOrthoSize);

            transform.position = new Vector3(posX, posY, transform.position.z);
        }
    }

    /// <summary> Follow the game world projectile. If there's more than one it follows earliest created. <summary>
    /// <remarks> Is a mode of camera functionality </remarks>
    /// <remarks> Move towards the projectile smoothly using Mathf.Lerp & a slow camSpeed</remarks>
    private void followProjectile()
    {
        //Find a projectile object in the game world
        GameObject projectile = GameObject.FindWithTag("Projectile");

        //Move cam towards projectile position
        posX = Mathf.Lerp(transform.position.x, projectile.transform.position.x, Time.deltaTime * camSpeed / 100);
        posY = Mathf.Lerp(transform.position.y, projectile.transform.position.y, Time.deltaTime * camSpeed / 100);
    }

    /// <summary> Follow nothing & use user mouse input to translate & zoom <summary>
    /// <remarks> Is a mode of camera functionality </remarks>
    /// <remarks> Move towards the projectile smoothly using Mathf.Lerp & a slow camSpeed </remarks>
    private void freeCam()
    {
        //Change camera size by scrollwheel value
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0.0f)
        {
            camOrthoSize -= scroll * zoomSpeed;
            camOrthoSize = Mathf.Clamp(camOrthoSize, minOrthoSize, maxOrthoSize);
        }

        //Set main camera to new zoomed position
        Camera.main.orthographicSize = Mathf.MoveTowards(Camera.main.orthographicSize, camOrthoSize, zoomSmoothSpeed * Time.deltaTime);

        //Get smoothed position based on current camera position + position in the direction of mouse movement
        posX = Mathf.SmoothDamp(transform.position.x, transform.position.x + Input.GetAxis("Mouse X") * Time.deltaTime * camSpeed, ref velocity.x, smoothTimeX);
        posY = Mathf.SmoothDamp(transform.position.y, transform.position.y + Input.GetAxis("Mouse Y") * Time.deltaTime * camSpeed, ref velocity.y, smoothTimeY);
    }

    /// <summary> Follow the current active player <summary>
    /// <remarks> Is a mode of camera functionality </remarks>
    /// <remarks> Move towards the player smoothly using Mathf.Lerp & a slow camSpeed </remarks>
    /// <remarks> Used when moving to next player after one player has taken their turn </remarks>
    private void followPlayer()
    {
        //Move cam towards projectile position
        posX = Mathf.Lerp(transform.position.x, player.transform.position.x, Time.deltaTime * camSpeed / 100);
        posY = Mathf.Lerp(transform.position.y, player.transform.position.y, Time.deltaTime * camSpeed / 100);
    }

    /// <summary> Set whether the camera attempts to follow a projectile in the game world <summary>
    /// <param name="isFollowing"> Sets state of followingProjectile boolean field </param>
    public void setCameraMode(int camMode)
    {
        cameraMode = camMode;
    }

    /// <summary> Set the player that the camera will follow when set to cameraMode = 2</summary>
    /// <remarks> Used by GameController class after a turn has been taken </remarks>
    /// <remarks> Must be set before cameraMode is set to 2, otherwise camera will follow previously set player (or none at all) </remarks>
    /// <param name="player"> Player gameobject to follow </param>
    public void setPlayerToFollow(GameObject player)
    {
        this.player = player;
    }
}

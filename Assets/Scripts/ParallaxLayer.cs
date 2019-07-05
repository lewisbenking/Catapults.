using System.Collections;
using UnityEngine;

/// <summary>Parallax layer manager</summary>
/// <author>Marks Paskannijs</author>
[ExecuteInEditMode]
public class ParallaxLayer : MonoBehaviour {

    public float speedX;
    public float speedY;
    public bool moveInOppositeDirection;

    private Transform cameraTransform;
    private Vector3 previousCameraPosition;
    private bool previousMoveParallax;
    private ParallaxOption options;

    // When enabled, find the main camera
    private void OnEnable()
    {
        GameObject gameCamera = Camera.main.gameObject;
        options = gameCamera.GetComponent<ParallaxOption>();
        cameraTransform = gameCamera.transform;
        previousCameraPosition = cameraTransform.position;
    }

    // Update Parallax layers and their positions)
    void Update () {
        // If it was moved and not the same position as before, update the position
        if (options.moveParallax && !previousMoveParallax)
            previousCameraPosition = cameraTransform.position;

        // Save the move
        previousMoveParallax = options.moveParallax;

        // A return statement with no expression (void). I know, don't question my methods. 
        if (!Application.isPlaying && !options.moveParallax)
            return;

        // Setup the distance for movement
        Vector3 distance = cameraTransform.position - previousCameraPosition;
        // And its direction
        float direction = (moveInOppositeDirection) ? -1f : 1f; // if true, return -1f, otherwise 1f
        // Now scale it!
        transform.position += Vector3.Scale(distance, new Vector3(speedX, speedY)) * direction;

        // Overwrite the position
        previousCameraPosition = cameraTransform.position;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour {

    private SpriteRenderer spriteRenderer;
    private PolygonCollider2D polyCollider;
    private Transform owner;
    private CharacterController ownerController;
    private bool initialised;
    private bool facingRight;
    private bool placed;

	// Use this for initialization
	void Start () {
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
        polyCollider = transform.GetComponent<PolygonCollider2D>();

        spriteRenderer.color = new Color(0,255,0,50);
	}
	
	// Update is called once per frame
	void Update () {
        if (!placed && initialised) //if owner has been set
        {
            facingRight = ownerController.getFacingRight();

            if (ownerController.isPlacingObject() == false) //if owner has stopped placing object
            {
                place();
            }
            if (facingRight)
            {
                transform.localPosition = new Vector3(owner.position.x + 2, owner.position.y + 1.6f, owner.position.z);
                spriteRenderer.flipX = false;
            }
            else
            {
                transform.localPosition = new Vector3(owner.position.x - 2, owner.position.y + 1.6f, owner.position.z);
                spriteRenderer.flipX = true;
            }

            //If the player's turn has ended
            if (ownerController.isCurrentTurn() == false)
            {
                place(); //place object
                ownerController.setPlacingObject(false); // 
            }
        }
	}

    public void place()
    {
        polyCollider.enabled = true;
        spriteRenderer.color = new Color(255, 255, 255, 255);
        placed = true;
    }

    public void setPlayerOwner(Transform owner)
    {
        this.owner = owner;
        ownerController = owner.GetComponent<CharacterController>();
        ownerController.setPlacingObject(true);
        initialised = true;
    }
}

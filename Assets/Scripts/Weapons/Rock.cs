using UnityEngine;
using System.Collections;

public class Rock : MonoBehaviour {

    private float currentLifeTime; //Rock has a lifetime of lifeLength
    private float maxLifeLength = 5;
    private ParticleSystem crumbleEffect;
    private bool collided;
    private Rigidbody2D rigidBody;

	// Use this for initialization
	void Start () {
        currentLifeTime = 0;
        crumbleEffect = GetComponent<ParticleSystem>();
        rigidBody = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        currentLifeTime += Time.deltaTime;
        //If the object has existed for the length of maxLifeLength
        if (currentLifeTime >= maxLifeLength)
        {
            Destroy(gameObject);
        }

        if(collided == true){
            rigidBody.constraints = RigidbodyConstraints2D.FreezeAll;

            //Delete this gameObject
            if (!crumbleEffect.IsAlive())
            {
                Destroy(gameObject);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        float hitPower = coll.relativeVelocity.magnitude;
        
        //If collision is with a player and it's powerful enough
        if (coll.gameObject.tag == "Player" && coll.relativeVelocity.magnitude > 5)
        {
            coll.transform.GetComponent<CharacterController>().addHealth(-1 * (int)Mathf.Round(hitPower)); //change player's health
        }

        //TODO: CRUMBLING EFFECTS
        crumbleEffect.Play();
        collided = true;
        GetComponent<Renderer>().enabled = false;

        //stop registering any further collisions from this game object
        GetComponent<Collider2D>().isTrigger = false;
    }
}

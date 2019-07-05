using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeBall : MonoBehaviour {

    private float currentLifeTime; //Rock has a lifetime of lifeLength
    private float maxLifeLength = 5;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        currentLifeTime += Time.deltaTime;
        //If the object has existed for the length of maxLifeLength
        if (currentLifeTime >= maxLifeLength)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        float hitPower = coll.relativeVelocity.magnitude;

        //If collision is with a player and it's powerful enough
        if (coll.gameObject.tag == "Player" && coll.relativeVelocity.magnitude > 5)
        {
            coll.transform.GetComponent<CharacterController>().addHealth(-5); //change player's health
        }

        if(coll.gameObject.tag != "Projectile" && currentLifeTime > 0.1)
        {
            print("SpikeBall collided with " + coll.gameObject.name);
            Destroy(gameObject);
        }
    }

}

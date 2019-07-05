using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonVial : MonoBehaviour
{
    
    private ParticleSystem poisonTrail;
    private ParticleSystem poisonBurst;
    private Rigidbody2D rigidBody;
    private float currentLifeTime; //Rock has a lifetime of lifeLength
    private float maxLifeLength = 5;
    private bool collided;

    // Use this for initialization
    void Start()
    {
        poisonBurst = transform.Find("PoisonBurst").GetComponent<ParticleSystem>();
        poisonTrail = transform.Find("PoisonTrail").GetComponent<ParticleSystem>();
        rigidBody = GetComponent<Rigidbody2D>();
        poisonTrail.Play();
        currentLifeTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        currentLifeTime += Time.deltaTime;
        //If the object has existed for the length of maxLifeLength
        if (currentLifeTime >= maxLifeLength)
        {
            Destroy(gameObject);
        }

        if (collided == true)
        {
            rigidBody.constraints = RigidbodyConstraints2D.FreezeAll;

            //if collision effects have finished
            if (!poisonBurst.IsAlive())
            {
                poisonTrail.transform.SetParent(null); //detatch trail from gameobject to allow particle persitence
                Destroy(poisonTrail, 3); //destroy trail in 3 seconds
                Destroy(gameObject); //destroy gameObject
            }
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        GetComponent<CircleCollider2D>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;

        if (coll.gameObject.tag == "Player") //if collision object was a player
        {
            coll.gameObject.GetComponent<CharacterController>().addPoison(2); // add 2 turns worth of poison to player
        }

        //CRUMBLING EFFECTS
        poisonBurst.Play();
        collided = true;
        GetComponent<Renderer>().enabled = false;
        poisonTrail.Stop(); //stop particle emission

        //stop registering any further collisions from this game object
        GetComponent<Collider2D>().isTrigger = false;

    }
}
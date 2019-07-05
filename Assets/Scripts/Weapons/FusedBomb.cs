using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FusedBomb : MonoBehaviour {

    private float currentLifeTime;
    private float explodeTime = 5;
    private float damageModifier = 30f;
    private bool exploded;
    private ParticleSystem explosionBurst;
    private ParticleSystem flameTrail;
    private Rigidbody2D rigidBody;

    // Use this for initialization
    void Start () {
        explosionBurst = transform.Find("ExplosionBurst").GetComponent<ParticleSystem>();
        flameTrail = transform.Find("FlameTrail").GetComponent<ParticleSystem>();
        rigidBody = GetComponent<Rigidbody2D>();

        flameTrail.Play();
        currentLifeTime = 0;
    }
	
	// Update is called once per frame
	void Update () {
        currentLifeTime += Time.deltaTime;
        //If the object has existed for the length of maxLifeLength
        if (currentLifeTime >= explodeTime && !exploded)
        {
            rigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
            Explode();
            exploded = true;
        }

        if (exploded == true)
        {
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<CircleCollider2D>().enabled = false;
            flameTrail.Stop(); //stop particle emission

            //if collision effects have finished
            if (!explosionBurst.IsAlive())
            {
                flameTrail.transform.SetParent(null); //detatch flameTrail from gameobject to allow particle persitence
                Destroy(flameTrail, 3); //destroy flameTrail in 3 seconds
                Destroy(gameObject); //destiroy gameObject
            }
        }
    }

    void Explode()
    {
        explosionBurst.Play();

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player"); //get all players

        foreach (GameObject player in players)
        {
            //testing whether enemy is behind an object:
            bool exposed = false;

            //detect the collider which is nearest the explosion between the explosion and the player
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y));

            if (hit.transform.tag == "Player")// if the hit was a player
            {
                exposed = true;
                print("raycast hit on" + hit.transform.name + "(distance " + hit.distance + ")");
            }
            else // explosion was blocked
            {
                print("raycast block by " + hit.transform.name + "(distance " + hit.distance + ")"); ;
            }

            if (exposed) //if the payer is in line of site of the explosion
            {
                //calculate distance away
                float xDistance = player.transform.position.x - transform.position.x;
                float yDistance = player.transform.position.y - transform.position.y;
                float distance = Mathf.Sqrt(Mathf.Pow(xDistance, 2) + Mathf.Pow(yDistance, 2)); //distance = sqrt(xDistance^2 + yDistance^2)

                //if distance from player is < 10
                if (distance < 3)
                {
                    player.GetComponent<CharacterController>().addHealth(-1f * ((3f - distance) * damageModifier)); //remove health from player (optimal is -45 health)
                }
            }
        }
    }
}

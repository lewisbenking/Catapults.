using UnityEngine;
using System.Collections;

public class FlamingRock : MonoBehaviour {

    private ParticleSystem rockBurst;
    private ParticleSystem emberBurst;
    private ParticleSystem flameTrail;
    private Rigidbody2D rigidBody;
    private Transform explosionSprite;
    private float damageModifier = 15f;
    //private Animator explosionAnim;

    private float currentLifeTime; //Rock has a lifetime of lifeLength
    private float maxLifeLength = 5;
    private bool collided;
    

    // Use this for initialization
    void Start()
    {
        rockBurst = transform.Find("RockBurst").GetComponent<ParticleSystem>();
        emberBurst = transform.Find("EmberBurst").GetComponent<ParticleSystem>();
        flameTrail = transform.Find("FlameTrail").GetComponent<ParticleSystem>();
        //explosionAnim = transform.Find("explosion_sprite").GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();

        flameTrail.Play();
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
            if (!rockBurst.IsAlive()&& !emberBurst.IsAlive())
            {
                flameTrail.transform.SetParent(null); //detatch flameTrail from gameobject to allow particle persitence
                flameTrail.Stop(); //stop particle emission
                Destroy(flameTrail, 3); //destroy flameTrail in 3 seconds
                Destroy(gameObject); //destiroy gameObject
            }
        }
    }

    public void setDamageMod(float modifier) //sets the damage modifier for the explosion damage effect
    {
        damageModifier = modifier;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        //explosionAnim.SetBool("exploding", true);

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player"); //get all players

        foreach (GameObject player in players)
        {
            //testing whether enemy is behind an object:
            bool exposed = false;

            //detect the collider which is neerest the explosion between the explosion and the player
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(coll.transform.position.x, coll.transform.position.y), new Vector2(player.transform.position.x - coll.transform.position.x, player.transform.position.y - coll.transform.position.y));

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

        //CRUMBLING EFFECTS
        rockBurst.Play();
        emberBurst.Play();
        collided = true;
        //explosionSprite.SetParent(null);
        GetComponent<Renderer>().enabled = false;

        //stop registering any further collisions from this game object
        GetComponent<Collider2D>().isTrigger = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour {
    
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter2D(Collision2D coll)
    {
        if(coll.gameObject.tag == "Player")
        {
            coll.transform.GetComponent<CharacterController>().setHealth(0);
            GameObject.Find("_GameManager").GetComponent<GameController>().otherPlayerWins();
            GetComponent<BoxCollider2D>().enabled = false;
        }
        transform.Find("SplashParticles").GetComponent<ParticleSystem>().Play();
    }
}

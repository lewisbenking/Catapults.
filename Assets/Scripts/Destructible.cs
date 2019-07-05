using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour {

    public int currentHealth;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (currentHealth <= 0)
        {
            if (transform.GetComponent<Explodable>())
            {
                transform.GetComponent<Explodable>().explode();
            }else
            {
                Destroy(gameObject);
            }

            GameObject healthPopup = (GameObject)Resources.Load("DamagePopup");
            healthPopup.transform.localPosition = gameObject.transform.localPosition;
            healthPopup.transform.Find("DamagePopupText").GetComponent<HealthPopup>().initiatePopup(Color.red, "Destroyed!");
            Instantiate(healthPopup);

        }
	}

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Projectile")
        {
            currentHealth--;
        }
        
    }
}

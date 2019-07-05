using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScatterBomb : MonoBehaviour {

    private float currentLifeTime; //Scatterbomb has a lifetime of lifeLength
    private float maxLifeLength = 5;
    private Rigidbody2D rigidBody;

    // Use this for initialization
    void Start () {
        currentLifeTime = 0;
        rigidBody = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update () {
        currentLifeTime += Time.deltaTime;
        //If the object has existed for the length of maxLifeLength
        if (currentLifeTime >= maxLifeLength)
        {
            Destroy(gameObject);
        }

        if (Input.GetKeyDown(KeyCode.Space)) //if player presses spacebar
        {
            detonate();
        }
	}

    void OnCollisionEnter2D(Collision2D coll)
    {
        detonate();
    }

    void detonate()
    {
        rigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
        GameObject flamingRock = (GameObject)Resources.Load("Items/FlamingRock");

        GameObject rock1 = Instantiate(flamingRock, new Vector3(transform.position.x-0.2f, transform.position.y-0.1f, transform.position.z), Quaternion.Euler(new Vector3(0, 0, 0)));
        rock1.transform.localScale -= new Vector3(0.15f, 0.15f, 0.15f);
        rock1.GetComponent<FlamingRock>().setDamageMod(7f);
        Rigidbody2D rigidbody = rock1.GetComponent<Rigidbody2D>(); //get rock's rigidBody
        rigidbody.AddForce(new Vector2(-2,2), ForceMode2D.Force); //apply force

        GameObject rock2 = Instantiate(flamingRock, new Vector3(transform.position.x-0.2f, transform.position.y+0.1f, transform.position.z), Quaternion.Euler(new Vector3(0, 0, 0)));
        rock2.transform.localScale -= new Vector3(0.15f, 0.15f, 0.15f);
        rock2.GetComponent<FlamingRock>().setDamageMod(7f);
        Rigidbody2D rigidbody2 = rock2.GetComponent<Rigidbody2D>(); //get rock's rigidBody
        rigidbody2.AddForce(new Vector2(-1f, 3), ForceMode2D.Force); //apply force

        GameObject rock3 = Instantiate(flamingRock, new Vector3(transform.position.x, transform.position.y+0.2f, transform.position.z), Quaternion.Euler(new Vector3(0, 0, 0)));
        rock3.transform.localScale -= new Vector3(0.15f, 0.15f, 0.15f);
        rock3.GetComponent<FlamingRock>().setDamageMod(7f);
        Rigidbody2D rigidbody3 = rock3.GetComponent<Rigidbody2D>(); //get rock's rigidBody
        rigidbody3.AddForce(new Vector2(0, 3), ForceMode2D.Force); //apply force

        GameObject rock4 = Instantiate(flamingRock, new Vector3(transform.position.x + 0.2f, transform.position.y+0.1f, transform.position.z), Quaternion.Euler(new Vector3(0, 0, 0)));
        rock4.transform.localScale -= new Vector3(0.15f, 0.15f, 0.15f);
        rock4.GetComponent<FlamingRock>().setDamageMod(7f);
        Rigidbody2D rigidbody4 = rock4.GetComponent<Rigidbody2D>(); //get rock's rigidBody
        rigidbody4.AddForce(new Vector2(1f, 3), ForceMode2D.Force); //apply force

        GameObject rock5 = Instantiate(flamingRock, new Vector3(transform.position.x + 0.2f, transform.position.y-0.1f, transform.position.z), Quaternion.Euler(new Vector3(0, 0, 0)));
        rock5.transform.localScale -= new Vector3(0.15f, 0.15f, 0.15f);
        rock5.GetComponent<FlamingRock>().setDamageMod(7f);
        Rigidbody2D rigidbody5 = rock5.GetComponent<Rigidbody2D>(); //get rock's rigidBody
        rigidbody5.AddForce(new Vector2(2, 4), ForceMode2D.Force); //apply force

        Destroy(gameObject);
    }
}

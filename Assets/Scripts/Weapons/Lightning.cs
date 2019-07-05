using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour {

    SpriteRenderer lightningRenderer;
    SpriteRenderer darknessRenderer;
    Transform target;
    bool strikeStarted;
    float lifeTime = 0;
    float darknessAlpha = 0.9f;
    float darknessSpeed = 0.05f;

    // Use this for initialization
    void Start() {
        lightningRenderer = GetComponent<SpriteRenderer>();
        darknessRenderer = GameObject.Find("ForegroundLayer").transform.Find("DarknessFilter").GetComponent<SpriteRenderer>();
        lightningRenderer.color = new Color(0,0,0,0);
        Vector3 newPos = new Vector3(target.position.x, target.position.y + 9.5f, target.position.z); //get position above target player
        transform.position = newPos;
    }

    // Update is called once per frame
    void Update() {
        if(Mathf.Abs(darknessAlpha - darknessRenderer.color.a) > 0.01 && strikeStarted == false) //if darkness has reachest darkest state
        {
            darknessRenderer.color = Color.Lerp(darknessRenderer.color, new Color(0, 0, 0, darknessAlpha), darknessSpeed); //make darkness fitler more dark
        }
        else
        {
            if(strikeStarted == false)
            {
                Strike();
            }

            strikeStarted = true;

            lifeTime += Time.deltaTime;

            //after 2 seconds destroy object
            if (lifeTime >= 2)
            {
                CancelInvoke("StrikeStep");
                CancelInvoke("damageTarget");
                lightningRenderer.enabled = false; // hide lightning
                darknessRenderer.color = Color.Lerp(darknessRenderer.color, new Color(0, 0, 0, 0), darknessSpeed/2);

                if (Mathf.Abs(0 - darknessRenderer.color.a) < 0.01)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    //move the lightining above the enemy
    public void Strike()
    {
        lightningRenderer.color = new Color(255, 255, 255, 255);
        InvokeRepeating("StrikeStep", Random.Range(0.01f,0.1f), Random.Range(0.01f, 0.1f)); //repeatedly invoke toggleflash once every 0.01 seconds
        InvokeRepeating("damageTarget", 0.2f, 0.2f); //damage target every 0.1 seconds
    }

    public void setTarget(Transform target)
    {
        this.target = target;
    }

    private void damageTarget()
    {
        target.GetComponent<CharacterController>().addHealth(-5f);
    }

    //toggle visibility of lighting sprite to simulate flashing lightning
    private void StrikeStep()
    {
        if (lightningRenderer.color.a == 1f || lightningRenderer.color.a == 255)
        {
            lightningRenderer.color = new Color(255, 255, 255, 0);
        }
        else
        {
            lightningRenderer.color = new Color(255, 255, 255, 255);
        }
    }
}

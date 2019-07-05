using UnityEngine;
using System.Collections;

///<summary> Handles projectile calculations & creation </summary>
///<remarks> Attatched to ThrowPoint GameObject</remarks>
///<author> Charlie Postgate </author>

public class AimInput : MonoBehaviour {
    
    private Transform throwPoint; //throwpoint location

    void Start () {
        throwPoint = gameObject.transform; //set throwpoint to the position of gameObject
    }

    ///<summary> Calculate and return the throw's 'target' </summary>
    ///<param name="angleDeg"> Aiming angle from Throw method in degrees</param>
    ///<param name="power"> Power value from Throw method </param>
    private Vector2 setArc(float power,float angleDeg)
    {
        float angle = angleDeg * 0.0174533f; //convert to radians
        
        //find X & Y positions using trigonometric rules
        float targetX = throwPoint.position.x + (power * Mathf.Cos(angle));
        float targetY = throwPoint.position.y + (power * Mathf.Sin(angle)); 

        return new Vector2(targetX, targetY);
    }

    ///<summary> Instantiate a ball gameObject and apply force to it with calculated values </summary>
    ///<param name="angle"> Aiming angle from CharacterController in degrees</param>
    ///<param name="power"> Power value from CharacterController </param>
    public void Throw(GameObject weapon,float power, float angle)
    {
        

        if (weapon.name == "SpikeBallClump") //if the weapon is a spike scatter, apply force to all balls
        {
            GameObject bulletInstance = Instantiate(weapon, throwPoint.position, Quaternion.Euler(new Vector3(0, 0, 10))) as GameObject; //create ball gameObject at throwPoint
            Rigidbody2D[] bodies;
            bodies = bulletInstance.GetComponentsInChildren<Rigidbody2D>();
            foreach(Rigidbody2D body in bodies)
            {
                Vector2 arc = setArc(power + Random.Range(-2f, 2f), angle); //get projectile 'target' values
                body.AddForce(arc, ForceMode2D.Force); //apply force towards 'target' position
            }
        }
        else
        {
            Vector2 arc = setArc(power, angle); //get projectile 'target' values
            GameObject bulletInstance = Instantiate(weapon, throwPoint.position, Quaternion.Euler(new Vector3(0, 0, 10))) as GameObject; //create ball gameObject at throwPoint
            Rigidbody2D rigidbody = bulletInstance.GetComponent<Rigidbody2D>(); //get ball's rigidBody
            rigidbody.AddForce(arc, ForceMode2D.Force); //apply force towards 'target' position
        }
    }
}

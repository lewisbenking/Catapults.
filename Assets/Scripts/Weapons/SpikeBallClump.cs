using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeBallClump : MonoBehaviour {

    float waiting = 5f;

	// Use this for initialization
	void Start () {
        
    }

    void FixedUpdate()
    {
        if(transform.childCount == 0)
        {
            Destroy(gameObject);
        }
    }

    void detatch()
    {
    }
}

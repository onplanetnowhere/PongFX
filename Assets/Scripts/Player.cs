using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public float movementSpeed;
    private Vector3 startPosition;
	
	// Update is called once per frame
	void Update () {
        // Player controls
        bool moveLeft = (Input.GetKey("up") || Input.GetKey("w"));
        bool moveRight = (Input.GetKey("down") || Input.GetKey("s"));
        if (moveLeft && !moveRight)
        {
            this.GetComponent<Rigidbody>().velocity = -this.transform.up * movementSpeed;
        }
        else if (!moveLeft && moveRight)
        {
            this.GetComponent<Rigidbody>().velocity = this.transform.up * movementSpeed;
        }
        else
        {
            this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }
}

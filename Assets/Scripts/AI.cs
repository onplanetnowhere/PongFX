using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    public float movementSpeedStart;
    public float movementSpeedIncrement;
    public float movementSpeedMax;
    public GameObject puck;
    private float movementSpeed;
    private Vector3 startPosition;

    // Use this for initialization
    void Start ()
    {
        ResetDifficulty();
    }
	
	// Update is called once per frame
	void Update ()
    {
        // Move the AI towards the ball faster if it is further
        bool moveLeft = false;
        bool moveRight = false;
        float puckOffset = Mathf.Abs(this.transform.position.z - puck.transform.position.z);
        if (puck.transform.position.z < this.transform.position.z - 0.5f)
        {
            moveLeft = true;
        }
        else if (puck.transform.position.z > this.transform.position.z + 0.5f)
        {
            moveRight = true;
        }
        if (moveLeft)
        {
            this.GetComponent<Rigidbody>().velocity = -Vector3.ClampMagnitude(this.transform.up * movementSpeed * puckOffset, movementSpeed);
        }
        else if (moveRight)
        {
            this.GetComponent<Rigidbody>().velocity = Vector3.ClampMagnitude(this.transform.up * movementSpeed * puckOffset, movementSpeed);
        }
        else
        {
            this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }

    public void IncrementDifficulty()
    {
        movementSpeed += movementSpeedIncrement;
        if (movementSpeed > movementSpeedMax)
        {
            movementSpeed = movementSpeedMax;
        }
    }

    public void ResetDifficulty()
    {
        this.movementSpeed = movementSpeedStart;
    }
}

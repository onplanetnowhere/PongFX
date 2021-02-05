using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puck : MonoBehaviour
{
    public GameController gameController;
    public float startVelocity;
    public float puckVelocity;
    public float velocityIncrement;
    public float velocityHitScale;
    public float maxVelocity;
    public float minVelocity;
    public float paddleDeflectScale;
    public float paddleDeflectAngleLimit;
    public AudioClip pongHitRed;
    public AudioClip pongHitBlue;
    public AudioClip pongHitWall;
    public GameObject ripplePrefab;
    private AudioSource audioSource;

    // Use this for initialization
    void Start ()
    {
        ResetVelocity();
        this.audioSource = this.GetComponent<AudioSource>();
    }

    //Detect collisions between the GameObjects with Colliders attached
    void OnCollisionEnter(Collision collision)
    {
        // Ripple
        GameObject cube = Instantiate(ripplePrefab);
        cube.transform.position = this.transform.position + new Vector3(0.0f, 2.0f, 0.0f);

        Vector3 velocity = this.GetComponent<Rigidbody>().velocity;
        if (collision.gameObject.tag == "PlayerPaddle")
        {
            // Adjust ball velocity on paddle collision
            velocity = GetPuckVelocity(velocity, collision);
            this.audioSource.PlayOneShot(pongHitBlue);
        }
        else if (collision.gameObject.tag == "AIPaddle")
        {
            // Adjust ball velocity on paddle collision
            velocity = GetPuckVelocity(velocity, collision);
            this.audioSource.PlayOneShot(pongHitRed);
        }
        else if (collision.gameObject.tag == "LeftWall" || collision.gameObject.tag == "RightWall")
        {
            // Correct ball reflection on left and right wall collision to prevent Unity physics flatline
            velocity = CorrectPuckVelocity(velocity, collision);
            this.audioSource.PlayOneShot(pongHitWall);
        }
        else if (collision.gameObject.tag == "PlayerAIGoal")
        {
            // Update score on goal
            gameController.IncrementPlayerScore();
            velocity = Vector3.zero;
            this.ResetGameState();
        }
        else if (collision.gameObject.tag == "PlayerGoal")
        {
            // Update score on goal
            gameController.IncrementAIScore();
            velocity = Vector3.zero;
            this.ResetGameState();
        }

        this.GetComponent<Rigidbody>().velocity = velocity;
    }

    public Vector3 ClampMagnitude(Vector3 v, float max, float min)
    {
        double sm = v.sqrMagnitude;
        if (sm > (double)max * (double)max) return v.normalized * max;
        else if (sm < (double)min * (double)min) return v.normalized * min;
        return v;
    }

    Vector3 GetPuckVelocity(Vector3 velocity, Collision collision)
    {
        // Scale velocity after each hit
        velocity *= velocityHitScale;

        // Clamp maximum velocity
        velocity = this.ClampMagnitude(velocity, maxVelocity, minVelocity);

        // Offset deflection angle by the position it hit the paddle
        float deflectOffset = 0.0f;
        if (collision.gameObject.tag == "PlayerPaddle")
        {
            deflectOffset = this.transform.position.z - collision.gameObject.transform.position.z;
        }
        else if (collision.gameObject.tag == "AIPaddle")
        {
            deflectOffset = collision.gameObject.transform.position.z - this.transform.position.z;
        }
        deflectOffset *= paddleDeflectScale;
        velocity = Quaternion.Euler(0, deflectOffset, 0) * velocity;

        // Clamp paddle deflection angle
        Vector3 generalDirectionVector = Vector3.zero;
        if (collision.gameObject.tag == "PlayerPaddle")
        {
            generalDirectionVector = -collision.transform.right;
        }
        else if (collision.gameObject.tag == "AIPaddle")
        {
            generalDirectionVector = collision.transform.right;
        }
        float puckAngle = Vector3.SignedAngle(velocity, generalDirectionVector, Vector3.up);
        if (puckAngle > paddleDeflectAngleLimit)
        {
            velocity = Quaternion.Euler(0, puckAngle - paddleDeflectAngleLimit, 0) * velocity;
        }
        else if (puckAngle < -paddleDeflectAngleLimit)
        {
            velocity = Quaternion.Euler(0, puckAngle + paddleDeflectAngleLimit, 0) * velocity;
        }
        return velocity;
    }

    Vector3 CorrectPuckVelocity(Vector3 velocity, Collision collision)
    {
        if (collision.gameObject.tag == "LeftWall" && velocity.z < 0.0f)
        {
            velocity.z *= -1;
        }
        else if (collision.gameObject.tag == "RightWall" && velocity.z > 0.0f)
        {
            velocity.z *= -1;
        }
        return velocity;
    }

    public void SetStartVelocity()
    {
        float startAngle = Random.Range(-Mathf.PI / 6.0f, Mathf.PI / 6.0f) - Mathf.PI / 2.0f;
        this.GetComponent<Rigidbody>().velocity = new Vector3(Mathf.Sin(startAngle), 0.0f, Mathf.Cos(startAngle)) * puckVelocity;
    }

    void ResetGameState()
    {
        // Reset game object positions and apply starting velocity to ball
        puckVelocity += velocityIncrement;
        gameController.ResetGameState();
    }
    
    public void ResetVelocity()
    {
        this.puckVelocity = this.startVelocity;
    }
}

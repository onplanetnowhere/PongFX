using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuckCollision : MonoBehaviour {
    public int secondsToDestroy;

    void Update()
    {
        Destroy(this.gameObject, secondsToDestroy);
    }
}

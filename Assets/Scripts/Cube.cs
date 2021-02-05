using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour {
    public int band;
    public float startScale;
    public float scaleMultiplier;

    // Use this for initialization
    void Start ()
    {
        
    }
	
	// Update is called once per frame
	void Update ()
    {
        float audioBand = SpectrumAnalyzer.audioBands[band];
        if (audioBand == 0)
        {
            this.transform.localScale = new Vector3(this.transform.localScale.x, startScale, this.transform.localScale.z);
        }
        else
        {
            this.transform.localScale = new Vector3(this.transform.localScale.x, SpectrumAnalyzer.audioBands[band] * scaleMultiplier, this.transform.localScale.z);
        }
    }
}

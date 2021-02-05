using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    Material m_Material;
    public int band;
    public float colorStart;
    public float colorScale;
    public float colorRate;
    public float brightness;
    public float alpha;
    private float colorBeat;

    public Color[] colors;
    public float duration = 3.0f;

    private int index = 0;
    private float timer = 0.0f;
    private Color currentColor;
    private Color startColor;

    // Use this for initialization
    void Start ()
    {
        m_Material = GetComponent<Renderer>().material;
        AudioProcessor processor = FindObjectOfType<AudioProcessor>();
        processor.onBeat.AddListener(onBeat);
        processor.onSpectrum.AddListener(onSpectrum);
        colorBeat = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        //m_Material.color = new Color(colorStart + colorBeat, 0.0f, 0.0f);
        modulateColor();
    }

    void modulateColor()
    {
        if (colorBeat > 0.0f)
        {
            colorBeat *= colorRate;
        }

        currentColor = Color.Lerp(startColor, colors[index], timer);

        timer += Time.deltaTime / duration;
        if (timer > 1.0f)
        {
            timer -= 1.0f;
            index++;
            if (index >= colors.Length)
                index = 0;
            startColor = currentColor;
        }
        //m_Material.color = ;
        m_Material.color = currentColor;
        m_Material.color = new Color(m_Material.color.r, m_Material.color.g, m_Material.color.b, alpha) + new Color(colorBeat, colorBeat, colorBeat);
        m_Material.color *= brightness;
    }

    // Update is called once per frame
    void onBeat()
    {
        float audioBand = SpectrumAnalyzer.audioBands[band];
        colorBeat = audioBand / colorScale;
    }

    // Update is called once per frame
    void onSpectrum(float[] spectrum)
    {
        /*
        for (int i = 0; i < spectrum.Length; ++i)
        {
            Vector3 start = new Vector3(i, 0, 0);
            Vector3 end = new Vector3(i, spectrum[i], 0);
            Debug.DrawLine(start, end);
        }*/
    }
}

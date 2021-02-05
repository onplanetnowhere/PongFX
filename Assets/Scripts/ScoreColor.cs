using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreColor : MonoBehaviour
{
    Color m_Color;
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
        m_Color = GetComponent<ScoreUI>().textColor;
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
        //m_Color = ;
        m_Color = currentColor;
        m_Color = new Color(m_Color.r, m_Color.g, m_Color.b, alpha) + new Color(colorBeat, colorBeat, colorBeat);
        m_Color *= brightness;
        GetComponent<ScoreUI>().textColor = m_Color;
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

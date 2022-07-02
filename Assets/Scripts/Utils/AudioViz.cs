using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioViz : MonoBehaviour
{
    IGameManager srvGManager;
    float _clampSpectrum;
    private void Start()
    {
        srvGManager = ServicesLocator.GetService<IGameManager>();
    }
    void Update()
    {
        float[] spectrum = new float[256];

        AudioListener.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);

        if(PlayerPrefs.GetInt("quality") == 1)
        {
            _clampSpectrum = spectrum[2] * 1.2f;

            if (_clampSpectrum > 0.15)
            {
                _clampSpectrum = 0.15f;
            }
            srvGManager.ChangeBloom(_clampSpectrum);
        }
        
    }
}
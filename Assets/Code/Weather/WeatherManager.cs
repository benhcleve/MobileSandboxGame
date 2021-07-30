using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherManager : MonoBehaviour
{
    public enum Weather { Sunny, Cloudy, LightRain, Rain, HeavyRain }
    public Weather currentWeather;
    public Rain rain;



    private static WeatherManager _instance;
    public static WeatherManager instance { get { return _instance; } }
    void CreateInstance() //Make this an instance (Or destroy if already exists)
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;
    }
    public void Awake() => CreateInstance();


    void Update()
    {
        SwitchWeather();
    }

    void SwitchWeather()
    {
        switch (currentWeather)
        {
            case Weather.Sunny:
                RainOn(false);
                break;
            case Weather.Cloudy:
                RainOn(false);
                break;
            case Weather.LightRain:
                RainOn(true);
                rain.rainIntensity = 1;
                break;
            case Weather.Rain:
                RainOn(true);
                rain.rainIntensity = 5;
                break;
            case Weather.HeavyRain:
                RainOn(true);
                rain.rainIntensity = 10;
                break;
        }
    }

    void RainOn(bool isRainOn)
    {
        if (!isRainOn)
        {
            rain.enabled = false;
            rain.gameObject.GetComponent<ParticleSystem>().Stop();
        }
        else if (isRainOn)
        {
            rain.enabled = true;
            rain.gameObject.GetComponent<ParticleSystem>().Play();
        }

    }
}

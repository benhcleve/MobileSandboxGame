using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherManager : MonoBehaviour
{
    public GlobalWeatherManager.Weather currentWeather;
    public Rain rain;

    private void Start() => SwitchWeather();
    void Update()
    {
        if (currentWeather != GlobalWeatherManager.instance.currentWeather)
            SwitchWeather();
    }

    void SwitchWeather()
    {
        currentWeather = GlobalWeatherManager.instance.currentWeather;

        switch (currentWeather)
        {
            case GlobalWeatherManager.Weather.Sunny:
                RainOn(false);
                break;
            case GlobalWeatherManager.Weather.Cloudy:
                RainOn(false);
                break;
            case GlobalWeatherManager.Weather.LightRain:
                RainOn(true);
                rain.rainIntensity = 1;
                break;
            case GlobalWeatherManager.Weather.Rain:
                RainOn(true);
                rain.rainIntensity = 5;
                break;
            case GlobalWeatherManager.Weather.HeavyRain:
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

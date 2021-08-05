using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class WeatherChange
{
    public DateTime dateTime;
    public GlobalWeatherManager.Weather weather;

    public WeatherChange(DateTime dateTime, GlobalWeatherManager.Weather weather)
    {
        this.dateTime = dateTime;
        this.weather = weather;
    }
}


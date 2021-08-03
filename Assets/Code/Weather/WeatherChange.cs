using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class WeatherChange
{
    public int gameTime;
    public GlobalWeatherManager.Weather weather;

    public WeatherChange(int gameTime, GlobalWeatherManager.Weather weather)
    {
        this.gameTime = gameTime;
        this.weather = weather;
    }
}


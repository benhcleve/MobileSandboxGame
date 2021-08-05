using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;


public class WeatherChannel : MonoBehaviour
{
    public WeatherCard[] WeatherCards;
    public Sprite[] weatherIcons;

    private void Update()
    {
        UpdateWeatherUI();
    }

    void UpdateWeatherUI()
    {
        for (int i = 0; i < WeatherCards.Length; i++)
        {
            int cardTime = WeatherCards[i].dayTime + GlobalWeatherManager.instance.lastWeatherUpdateTime;
            WeatherChange cardWeather;


            if (GlobalWeatherManager.instance.weatherChanges.Count > 0)
            {
                WeatherChange closestWeather = (WeatherChange)GlobalWeatherManager.instance.weatherChanges.OrderBy(item => Mathf.Abs((WeatherCards[i].dayTime + GameTime.instance.gameTime) - GameTime.instance.DateTimeToGametime(item.dateTime))).First();
                cardWeather = closestWeather;

                WeatherCards[i].weatherIcon.sprite = SetWeatherIcon(cardWeather);
            }
        }
    }

    Sprite SetWeatherIcon(WeatherChange cardWeather)
    {
        Sprite weatherSprite = null;

        switch (cardWeather.weather)
        {
            case GlobalWeatherManager.Weather.Sunny:
                weatherSprite = weatherIcons[0];
                break;
            case GlobalWeatherManager.Weather.Cloudy:
                weatherSprite = weatherIcons[1];
                break;
            case GlobalWeatherManager.Weather.LightRain:
                weatherSprite = weatherIcons[2];
                break;
            case GlobalWeatherManager.Weather.Rain:
                weatherSprite = weatherIcons[3];
                break;
            case GlobalWeatherManager.Weather.HeavyRain:
                weatherSprite = weatherIcons[4];
                break;
            default:
                weatherSprite = weatherIcons[0];
                break;
        }
        return weatherSprite;
    }
}

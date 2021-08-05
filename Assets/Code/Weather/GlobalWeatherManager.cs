using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GlobalWeatherManager : MonoBehaviour
{
    public enum Weather { Sunny, Cloudy, LightRain, Rain, HeavyRain }
    public Weather currentWeather;
    public int lastWeatherUpdateTime = 0;
    int minutesInDay = 1440;
    public List<WeatherChange> weatherChanges = new List<WeatherChange>();




    private static GlobalWeatherManager _instance;
    public static GlobalWeatherManager instance { get { return _instance; } }
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
        GenerateWeather();
        ChangeWeather();
    }


    void GenerateWeather()
    {
        if (GameTime.instance.gameTime % minutesInDay == 0 && GameTime.instance.gameTime != lastWeatherUpdateTime)
        {
            lastWeatherUpdateTime = GameTime.instance.gameTime;

            int weatherChangeCount = Random.Range(1, 5); //Random times weather changes next day

            //Add random time weather changes between beginning and end of next day
            for (int x = 0; x < weatherChangeCount; x++)
            {

                int newTime = Random.Range(GameTime.instance.gameTime + minutesInDay, GameTime.instance.gameTime + (minutesInDay * 2));
                int newWeatherInt = Random.Range(0, 5);
                Weather newWeather = (Weather)newWeatherInt;
                weatherChanges.Add(new WeatherChange(GameTime.instance.GameTimeToDateTime(newTime), newWeather));
            }

            weatherChanges.Sort(SortByTime);
        }
    }

    static int SortByTime(WeatherChange w1, WeatherChange w2)
    {
        return GameTime.instance.DateTimeToGametime(w1.dateTime).CompareTo(GameTime.instance.DateTimeToGametime(w2.dateTime));
    }

    void ChangeWeather()
    {
        if (weatherChanges.Count() > 1)
        {
            for (int i = 0; i < weatherChanges.Count(); i++)
            {
                if (GameTime.instance.DateTimeToGametime(weatherChanges[i].dateTime) == GameTime.instance.gameTime) //If weather change time == gameTime
                    currentWeather = weatherChanges[i].weather;

                if (weatherChanges[i].dateTime.days < GameTime.instance.day - 2) //Remove weather changes that are 2 days old
                    weatherChanges.Remove(weatherChanges[i]);
            }
        }
    }


}

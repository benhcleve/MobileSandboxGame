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

        DebugTimeSpeed();
    }

    void DebugTimeSpeed()
    {
        if (Input.GetKey(KeyCode.Keypad1))
            Time.timeScale = 1;
        if (Input.GetKey(KeyCode.Keypad2))
            Time.timeScale = 10;
        if (Input.GetKey(KeyCode.Keypad3))
            Time.timeScale = 20;
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
                int newWeatherInt = Random.Range(0, 4);
                Weather newWeather = (Weather)newWeatherInt;
                weatherChanges.Add(new WeatherChange(newTime, newWeather));
            }

            weatherChanges.Sort(SortByTime);
        }
    }

    static int SortByTime(WeatherChange w1, WeatherChange w2)
    {
        return w1.gameTime.CompareTo(w2.gameTime);
    }

    void ChangeWeather()
    {
        if (weatherChanges.Count() > 1)
            if (GameTime.instance.gameTime == weatherChanges[0].gameTime)
            {
                currentWeather = weatherChanges[0].weather;

                weatherChanges.Remove(weatherChanges[0]);
            }
    }


}

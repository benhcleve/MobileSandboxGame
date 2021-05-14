using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTime : MonoBehaviour
{
    public enum Season { Spring, Summer, Fall, Winter }
    public enum Day { Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday };

    public Season season = Season.Spring;
    public Day day = Day.Monday;
    public int hour = 12;
    public int minute = 0;


    private static GameTime _instance;
    public static GameTime instance { get { return _instance; } }

    void CreateInstance() //Make this an instance (Or destroy if already exists)
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;
    }

    void Awake()
    {
        CreateInstance();
        StartCoroutine(UpdateTime());
    }

    IEnumerator UpdateTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            minute += 1;

            if (minute >= 60) //If hour has passed
            {
                hour += 1;
                minute = 0;

                if (hour >= 24) //If day has passed
                {
                    hour = 0;
                    UpdateDay();
                    UpdateSeason();
                }

            }
        }
    }

    void UpdateDay()
    {
        switch (day)
        {
            case Day.Monday:
                day = Day.Tuesday;
                break;
            case Day.Tuesday:
                day = Day.Wednesday;
                break;
            case Day.Wednesday:
                day = Day.Thursday;
                break;
            case Day.Thursday:
                day = Day.Friday;
                break;
            case Day.Friday:
                day = Day.Saturday;
                break;
            case Day.Saturday:
                day = Day.Sunday;
                break;
            case Day.Sunday:
                day = Day.Monday;
                break;
        }
    }

    void UpdateSeason()
    {
        if (day == Day.Monday)
        {
            switch (season)
            {
                case Season.Spring:
                    season = Season.Summer;
                    break;
                case Season.Summer:
                    season = Season.Fall;
                    break;
                case Season.Fall:
                    season = Season.Winter;
                    break;
                case Season.Winter:
                    season = Season.Spring;
                    break;
            }
        }

    }
}

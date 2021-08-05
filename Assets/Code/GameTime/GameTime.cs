using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTime : MonoBehaviour
{
    public int gameTime;
    public string[] seasons = new string[] { "Spring", "Summer", "Fall", "Winter" };
    public string[] days = new string[] { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
    public int year;
    public int season;
    public int day;
    public int dayOfWeek;
    public int hour;
    public int minute;


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

    private void Update() => DebugTimeSpeed();

    void DebugTimeSpeed()
    {
        if (Input.GetKey(KeyCode.Keypad1))
            Time.timeScale = 1;
        if (Input.GetKey(KeyCode.Keypad2))
            Time.timeScale = 10;
        if (Input.GetKey(KeyCode.Keypad3))
            Time.timeScale = 20;
    }

    IEnumerator UpdateTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            gameTime++;

            //There are 40320 minutes in 28 Days. 4 Seasons (7 days per seasons) in a game year.
            year = (int)Mathf.Floor(gameTime / 40320);
            //Current year's minutes
            var yearMinutes = gameTime - (year * 40320);



            //There are 1440 minutes in a day
            day = (int)Mathf.Floor((yearMinutes / 1440));
            //Current days's minutes
            var dayMinutes = yearMinutes - (day * 1440);

            hour = (int)Mathf.Floor(dayMinutes / 60);
            minute = dayMinutes - (hour * 60);

            //There are 7 days in a season
            season = (int)Mathf.Floor((day / 7));

            dayOfWeek = day - (season * 7);
        }
    }

    public int DateTimeToGametime(DateTime dateTime)
    {
        int gameTimedateTime = dateTime.minutes + (dateTime.hours * 60) + (dateTime.days * 1440) + (dateTime.years * 40320);
        return gameTimedateTime;
    }

    public DateTime GameTimeToDateTime(int gameTime)
    {
        DateTime dateTime = new DateTime(0, 0, 0, 0);
        //There are 40320 minutes in 28 Days. 4 Seasons (7 days per seasons) in a game year.
        dateTime.years = (int)Mathf.Floor(gameTime / 40320);
        //Current year's minutes
        var yearMinutes = gameTime - (dateTime.years * 40320);

        //There are 1440 minutes in a day
        dateTime.days = (int)Mathf.Floor((yearMinutes / 1440));
        //Current days's minutes
        var dayMinutes = yearMinutes - (dateTime.days * 1440);

        dateTime.hours = (int)Mathf.Floor(dayMinutes / 60);
        dateTime.minutes = dayMinutes - (dateTime.hours * 60);

        return dateTime;
    }



}

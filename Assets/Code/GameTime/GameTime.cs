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

            //There are 10080 minutes in a week
            season = (int)Mathf.Floor((day / 7));






        }
    }

}

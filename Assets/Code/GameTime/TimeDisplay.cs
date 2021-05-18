using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeDisplay : MonoBehaviour
{
    public enum DisplayStyle { AmPm, Military };
    public DisplayStyle currentDisplayStyle;
    public TextMeshProUGUI timeDisplay;
    public TextMeshProUGUI dateDisplay;

    void Update()
    {
        if (GameTime.instance != null)
            UpdateTimeDisplay();
    }

    void UpdateTimeDisplay()
    {
        string hour;
        string minute;
        string period;

        if (currentDisplayStyle == DisplayStyle.AmPm)
        {
            //HOUR
            hour = GameTime.instance.hour.ToString();
            int periodHour; //instead of military time, adjust hour to period
            if (GameTime.instance.hour > 12)
                periodHour = GameTime.instance.hour - 12;
            else if (GameTime.instance.hour == 0)
                periodHour = 12;
            else periodHour = GameTime.instance.hour;

            //MINUTE
            if (GameTime.instance.minute < 10)
                minute = "0" + GameTime.instance.minute;
            else minute = GameTime.instance.minute.ToString();

            //PERIOD
            if (GameTime.instance.hour >= 12)
                period = "pm";
            else period = "am";

            timeDisplay.text = periodHour + ":" + minute + " " + period;
        }
        else if (currentDisplayStyle == DisplayStyle.Military)
        {
            //HOUR
            hour = GameTime.instance.hour.ToString();

            //MINUTE
            if (GameTime.instance.minute < 10)
                minute = "0" + GameTime.instance.minute;
            else minute = GameTime.instance.minute.ToString();

            timeDisplay.text = hour + ":" + minute;


        }

        dateDisplay.text = GameTime.instance.days[GameTime.instance.dayOfWeek] + ", " + GameTime.instance.seasons[GameTime.instance.season];

    }
}

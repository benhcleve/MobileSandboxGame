using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DateTime
{
    public int minutes;
    public int hours;
    public int days;
    public int years;

    public DateTime(int _minutes, int _hours, int _days, int _years)
    {
        minutes = _minutes;
        hours = _hours;
        days = _days;
        years = _years;
    }

}

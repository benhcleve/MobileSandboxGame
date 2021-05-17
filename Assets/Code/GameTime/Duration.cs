using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Duration
{
    public int minutes;
    public int hours;
    public int days;
    public int years;

    public Duration(int _minutes, int _hours, int _days, int _years)
    {
        minutes = _minutes;
        hours = _hours;
        days = _days;
        years = _years;
    }

}

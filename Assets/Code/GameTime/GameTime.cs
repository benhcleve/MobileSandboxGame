using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTime : MonoBehaviour
{
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
                    hour = 0;


            }
        }
    }
}

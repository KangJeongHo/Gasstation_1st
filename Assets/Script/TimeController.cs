using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{
    private double day = 0;
    private int hour = 0;
    private float minute = 0;
    public Text GameTimeText;
    private int a;
    private string b;
    private string[] week = {"일","월","화","수","목","금","토"};
    // Start is called before the first frame update
    void Start()
    {
    }


    // Update is called once per frame
    void Update()
    {
        
        minute += Time.deltaTime;
        if (minute >= 60)
        {
            hour++;
            minute = 0;
            if (hour == 24)
            {
                day++;
                hour = 0;
            }
        }

        a = (int)(day % 7);
        b = week[a];
        
        if (hour == 0)
        {
            GameTimeText.text = (int)minute + " 분  " + b + "요일";
        }
        else
        {
            GameTimeText.text = (int)hour + " 시 " +(int)minute + " 분  " + b + "요일";
        }

    }
}

// 유니티 시간 관련 자료 : https://www.engedi.kr/unity/?q=YToxOntzOjEyOiJrZXl3b3JkX3R5cGUiO3M6MzoiYWxsIjt9&bmode=view&idx=3709777&t=board

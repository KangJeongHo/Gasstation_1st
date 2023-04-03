using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// 플레이어 움직임 관련 / 오브젝트를 움직이기 위한 대본

//내가보기엔 나중에 게임 매니저에 넣어야 할듯.
public class TimeController : MonoBehaviour
{ 
    public double day = 0;
    public int hour = 0;
    public float minute = 0;
    public Text GameTimeText;
    private int a;
    private string b;
    private string[] week = {"일","월","화","수","목","금","토"};

    public static TimeController instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            {
                if(instance != this)
                    Destroy(this.gameObject);
            }
        }
    }
    
    
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

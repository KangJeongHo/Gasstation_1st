﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;



public class PlayerScript : MonoBehaviour
{
    private static PlayerScript _instance;  //싱글톤
    public static PlayerScript Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(PlayerScript)) as PlayerScript;

                if (_instance == null)
                {
                    Debug.Log("싱글톤 오브젝트 없음");
                }
            }

            return _instance;
        }
    }  //싱글톤

    // 가진 돈
    public double m_Money = 1000000;
    [HideInInspector]
    public Grounds[] m_Grounds = null;

    
    // Station 에서 부지 정보 가져옴
    void Get_Grounds_Info_From_Station()
    {
        m_Grounds = new Grounds[Station.Grounds.Length];
        m_Grounds = Station.Grounds;
    }
    void SingleTone()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    } // 싱글톤
    private void Awake()
    {
        SingleTone();
        Get_Grounds_Info_From_Station();
    }
}

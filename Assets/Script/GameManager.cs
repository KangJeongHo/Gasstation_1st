using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Authentication;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// 게임 규칙 및 정보 담김
public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(GameManager)) as GameManager;

                if (_instance == null)
                {
                    Debug.Log("싱글톤 오브젝트 없음");
                }
            }

            return _instance;
        }
    }

    private void Awake()
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
    }

    internal int lubCount0 = 0;

    public void LubCountAdd()
    {
        lubCount0++;
        Debug.Log("(1이면 활성화)현재 주유기 설치 상태 활성화 : " + lubCount0);
    }
    
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
 
    }
    
    #region Dictionary

    void lubDictionary()
    {
        var lubDatas = new Dictionary<string, Dictionary<string, string>>();

        var lub1 = new Dictionary<string, string>();
        lub1.Add("주유소 이름", "주유소1");
        lub1.Add("기름 종류", "경유");
        lub1.Add("주유 충전 속도", "1");
        lub1.Add("주유소 그림", "lub_image1");

        var lub2 = new Dictionary<string, string>();
        lub2.Add("주유소 이름", "주유소2");
        lub2.Add("기름 종류", "경유");
        lub2.Add("주유 충전 속도", "2");
        lub2.Add("주유소 그림", "lub_image2");

        var lub3 = new Dictionary<string, string>();
        lub3.Add("주유소 이름", "주유소3");
        lub3.Add("기름 종류", "경유");
        lub3.Add("주유 충전 속도", "3");
        lub3.Add("주유소 그림", "lub_image3");

        lubDatas.Add("주유기", lub1);
        lubDatas.Add("주유기", lub2);
        lubDatas.Add("주유기", lub3);
    }

    /*
     * 자동차 세부사항
     * 1. 자동차 이름
     * 2. 자동차 기름 종류
     * 3. 자동차 이미지
     * 4. 종류에 따른 차량의 주유 요구 량
     */
    void carDictionary()
    {
        var carDatas = new Dictionary<string, Dictionary<string, string>>();

        var car1 = new Dictionary<string, string>();
        car1.Add("자동차 이름", "빨간 자동차");
        car1.Add("기름 종류", "경유");
        car1.Add("주유 주유 요구 량", "1~10");
        car1.Add("자동차 그림", "RedCar");

        var car2 = new Dictionary<string, string>();
        car2.Add("자동차 이름", "파란 자동차");
        car2.Add("기름 종류", "경유");
        car2.Add("주유 주유 요구 량", "1~20");
        car2.Add("자동차 그림", "BlueCar");

        var car3 = new Dictionary<string, string>();
        car3.Add("자동차 이름", "노란 자동차");
        car3.Add("기름 종류", "경유");
        car3.Add("주유 주유 요구 량", "5~15");
        car3.Add("자동차 그림", "YellowCar");

        var car4 = new Dictionary<string, string>();
        car4.Add("자동차 이름", "초록 자동차");
        car4.Add("기름 종류", "경유");
        car4.Add("주유 주유 요구 량", "15~30");
        car4.Add("자동차 그림", "GreenCar");

        carDatas.Add("자동차", car1);
        carDatas.Add("자동차", car2);
        carDatas.Add("자동차", car3);
        carDatas.Add("자동차", car4);
    }

    #endregion
}


// 딕셔너리 사용법1 : https://engineer-mole.tistory.com/174
// 네임스페이스 설명 : https://coderzero.tistory.com/entry/%EC%9C%A0%EB%8B%88%ED%8B%B0-C-%EA%B0%95%EC%A2%8C-12-%EB%84%A4%EC%9E%84%EC%8A%A4%ED%8E%98%EC%9D%B4%EC%8A%A4Namespaces-using
// 싱글턴 : https://art-life.tistory.com/130
using System;
using UnityEngine;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEditor;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using System.Text;
using Mono.Cecil.Cil;
using System.Runtime.CompilerServices;
using UnityEditor.Experimental;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
using Ramdon = System.Random;


/// 차량 규칙 및 정보 담김
public enum CarNames //차량 이름 그룹
{
    RedCar, BlueCar, GreenCar, YellowCar, FuckCar
}

public class CarInfo //차량에 대한 정보 틀 형성
{
    public CarNames CarNames { get; }
    public string MCarGasType { get; set; }

    public int MCarGasAmountMin { get; set; }
    public int MCarGasAmountMax { get; set; }
    public int MCarLimitTimeMin { get; set; }
    public int MCarLimitTimeMax { get; set; }
    public int MCarSatisfactionMin { get; set; }
    public int MCarSatisfactionMax { get; set; }
    public int MCarZenProbability { get; set; }
    public int MCarOpenLevel { get; set; }
    public bool MIsActive { get; set; }

    public CarInfo(CarNames carNames, string gastype,
        int gamin, int gamax,
        int ltmin, int ltmax,
        int satmin, int satmax,
        int zen, int level,
        bool caractive)
    //차량 정보를 CarInfo에 저장하게 해줌.
    {
        this.CarNames = carNames;
        this.MCarGasType = gastype;
        this.MCarGasAmountMin = gamin;
        this.MCarGasAmountMax = gamax;
        this.MCarLimitTimeMin = ltmin;
        this.MCarLimitTimeMax = ltmax;
        this.MCarSatisfactionMin = satmin;
        this.MCarSatisfactionMax = satmax;
        this.MCarZenProbability = zen;
        this.MCarOpenLevel = level;
        this.MIsActive = caractive;
    }

    public CarInfo(){}
    // 객체 생성 틀

    public CarInfo SetUnitValue(CarNames carNames)
    // 이름 값을 받아서 객채 생성값 반환
    {
        CarInfo carInfo = null;

        switch (carNames)
        {
            case CarNames.RedCar:
                {
                    carInfo = new CarInfo(carNames, "Gasoline", 5, 10, 5, 10, 1, 1, 1, 1, true);
                    break;
                }
            case CarNames.GreenCar:
                {
                    carInfo = new CarInfo(carNames, "Gasoline", 10, 15, 7, 15, 1, 1, 1, 1, true);
                    break;
                }
            case CarNames.BlueCar:
                {
                    carInfo = new CarInfo(carNames, "Gasoline", 10, 20, 5, 10, 1, 1, 1, 1, true);
                    break;
                }
            case CarNames.YellowCar:
                {
                    carInfo = new CarInfo(carNames, "Gasoline", 15, 26, 10, 15, 1, 3, 1, 1, true);
                    break;
                }
            case CarNames.FuckCar:
                {
                    carInfo = new CarInfo(carNames, "Gasoline", 5, 10, 5, 10, 1, 1, 1, 1, false);
                    break;
                }
        }

        return carInfo;
    }
}


public class CarManager : MonoBehaviour
{
    
    
    #region SingleTon_In_CarManager

    private static CarManager _instance;

    public static CarManager Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(CarManager)) as CarManager;

                if (_instance == null)
                {
                    Debug.Log("싱글톤 오브젝트 없음");
                }
            }

            return _instance;
        }
    }

    #endregion

    public CarNames carNames;
    public CarInfo carInfo;
    
    
    private void Awake()
    {
        #region Awake내부 싱글턴 파트
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        #endregion
        
        carInfo = new CarInfo();
        carInfo = carInfo.SetUnitValue(carNames);
    }
    
    void Start()
    {
        //var carNamesArray = Enum.GetValues(typeof(CarNames)); // 차이름에 해당하는 값을 문자로 변경한 값으로 배열만들어서 담아줌.
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

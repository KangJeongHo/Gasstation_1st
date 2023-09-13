using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using Random = UnityEngine.Random;

public class CarPlayer : MonoBehaviour
{ 
    private CarNames _carPlayerName;
    private CarInfo _carPlayerInfo;
    
    // Start is called before the first frame update
    void Start()
    {
        _carPlayerName = CarManager.Instance.carNames; //차량의 정보를 가져와 담아준다. CarManager -> CarGenerator -> CarPlayer 이동 완료.
        _carPlayerInfo = CarManager.Instance.carInfo;
        //var carNamesArray = Enum.GetValues(typeof(CarNames)); // 차이름에 해당하는 값을 문자로 변경한 값으로 배열만들어서 담아줌.
        GetCarInfo();
    }

    void GetCarInfo() // 차량 값 받아주는 변수
    {
        var carArray = Enum.GetValues(typeof(CarNames));                                            // 차이름에 해당하는 값을 문자로 변경한 값으로 배열만들어서 담아줌.
        var chooseRandomIndex = Random.Range(0, carArray.Length);                                // 차를 랜덤으로 골라주기 위해 차량 배열의 수만큼을 가지고 랜덤 숫자를 뽑음.
        _carPlayerName = (CarNames)Enum.Parse(typeof(CarNames), chooseRandomIndex.ToString());
        //gasAmountRand = Random.Range(_carPlayerInfo.MCarGasAmountMin, _carPlayerInfo.MCarGasAmountMax);
        Debug.Log("곧 주유할 지정된 차량 : " + _carPlayerName);
    }
    void ReadyToCharge()
    {
        var chargingTime = 0;
        var randomTimeSpawn = Random.Range(5, 15);
        Debug.Log("차량이 오는데 걸리는 시간 : ");
        
    }
    
    
    
}

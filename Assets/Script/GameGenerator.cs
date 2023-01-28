using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    /// 플레이어 움직임에 따른 구름 생성과 움직임 조작
    /// 플레이어 이동, 시간 경과 후 출현하는 오브젝트
    
//[System.Serializable]
public class Car
{
    public string Name;
    public string GasType;
    public int ChargeTime;
    public int WaitPatience;
    public int Satisfaction; // 만족도
    public int ZenProbability; // 차들끼리의 등장 확률 (이거를 주유기에서 조절할지 차 클래스에서 조절할지 고민해봐야 할듯.)
    public List<int> KindOf = new List<int>();

    public Car(string name, string gasType, int chargeTime, int waitPatience, int satisfaction, int zenProbability)
    {
    }
}

//[System.Serializable]
public class CarKindOf
{
    public string Name;
    public string GasType;
    public int ChargeTime;
    public int WaitPatience;
    public int Satisfaction; // 만족도
    public int ZenProbability; // 차들끼리의 등장 확률 (이거를 주유기에서 조절할지 차 클래스에서 조절할지 고민해봐야 할듯.)

    public CarKindOf(string name, string gasType, int chargeTime, int waitPatience, int satisfaction,
        int zenProbability)
    {
    }
}

    public class GameGenerator : MonoBehaviour
    {
        public List<Car> CarList = new List<Car>();


        // IEnumerator Start()
        // {
        //     
        // }

    // Update is called once per frame
    void Update()
    {
        
    }
}

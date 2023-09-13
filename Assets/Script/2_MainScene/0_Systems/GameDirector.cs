using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;
    /// 점수 계산 및 게임 진행 재시작 및 오버 여부
    /// ui 조작하거나 진행 상황을 판단
public class GameDirector : MonoBehaviour
{
    #region 싱글턴
    private static GameDirector _instance;
    public static GameDirector Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(GameDirector)) as GameDirector;

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
    #endregion

    [SerializeField] private CarGenerator carGenerator;
    private bool _carPayCheck = false;
    
    private CarInfo _ccCarInfo;
    
    
    private bool _chargeStart = false;
    private bool _carTouchCheck = false;
    
    void Start()
    {
        _ccCarInfo = CarManager.Instance.carInfo;
        
        var lubCheckCoroutine = LubCheck(); // 주유기 설치 되어있는지 확인하는 코루틴 작동 시작.  
        StartCoroutine(lubCheckCoroutine);
    }
    
    
    IEnumerator LubCheck()                                                              // 주유기 설치 유무 체크 코루틴
    { 
        while (GameManager.Instance.lubCount0 == 0) // 해당 내용을 주유기 리스트의 항목들 중 여부를 따로 파악해야 할 것 같다.
        {
            Debug.Log("차량 주유기 설치 인식 대기중..");                                // 주유기가 설치되어있는지 확인함. 카 스폰 코루틴 시작함.
            yield return new WaitForSeconds(1f);
        }
        var carSpawnCoroutine = carGenerator.CarSpawn();  //메모리를 덜 먹어서 사용한 방법.
        StartCoroutine(carSpawnCoroutine);
    }
    

    internal void 방문차량정산()
    { // 플레이어스크립트나 게임디렉터,게임제너레이터,게임매니저 등 다른데로 옮겨야할 내용.
        var mSatis = Random.Range(_ccCarInfo.MCarSatisfactionMin,_ccCarInfo.MCarSatisfactionMax);
        mSatis = mSatis + (int)GameManager.Instance.satisfaction;
        SoundController.Instance.소리재생("만족도상승");

        var mgasAmountRand = carGenerator.gasAmountRand;
        var mMoney = (int)GameManager.Instance.m_Money;
        if (_ccCarInfo.MCarGasType == "Gasoline")
        {
            mMoney = mMoney + (int)(mgasAmountRand * 1000);
        }
        else if (_ccCarInfo.MCarGasType == "Diesel")
        {
            mMoney = mMoney + (int)(mgasAmountRand * 1000);
        }
        else if (_ccCarInfo.MCarGasType == "Bio")
        {
            mMoney = mMoney + (int)(mgasAmountRand * 1000);
        }
        else if (_ccCarInfo.MCarGasType == "Lpg")
        {
            mMoney = mMoney + (int)(mgasAmountRand * 1000);
        }
        else if (_ccCarInfo.MCarGasType == "Lpg")
        {
            mMoney = mMoney + (int)(mgasAmountRand * 1000);
        }
        GameManager.Instance.m_Money = mMoney;
        GameManager.Instance.Ui글자초기화();
        SoundController.Instance.소리재생("돈받음");
        _carPayCheck = false;
    }
    
    internal void 방문차량파괴()
    {
        if (_carPayCheck == true)
        {
            Debug.Log("파괴 내부 정산 작동.");
            var mSatis = Random.Range(_ccCarInfo.MCarSatisfactionMin,_ccCarInfo.MCarSatisfactionMax);
            //랜던값을 만들어놓고 불러와봤는데 해당 값을 고정한 상태로 변하지 않아서 값을 가져와서 랜덤을 돌리기로 했음.
            mSatis = mSatis - (int)GameManager.Instance.satisfaction;
            //만족도 감소시키려고 만든 체크문
            SoundController.Instance.소리재생("만족도하락");
            방문차량정산();
        }
        Debug.Log("차량 인스턴스 파괴.");
        Destroy(carGenerator.carInstance1);
        carGenerator.carInstance1 = null;
        carGenerator._randomCars = null;
        _chargeStart = false;
        _carTouchCheck = false;                                                                      // 터치인식 켜져있을수도 있으니 초기화
        _carPayCheck = false;
        carGenerator._carGageBarImage.GetComponentsInParent<Image>()[1].color = Color.clear;
        Destroy(carGenerator._carGageBar);
        SoundController.Instance.소리재생("차량떠남");
    }
    
}
    /// 데이터 저장 방법  * https://devruby7777.tistory.com/entry/Unity-%EC%9C%A0%EB%8B%88%ED%8B%B0%EC%9D%98-%EB%8D%B0%EC%9D%B4%ED%84%B0-%EC%A0%80%EC%9E%A5-%EB%B0%A9%EB%B2%95%EB%93%A4%EA%B3%BC-%EA%B7%B8-%EA%B2%BD%EB%A1%9C
    /// 1. 데이터베이스에 연결해 저장
    /// 2. 유니티에서 제공하는 PlayerPrefs 이용
    /// 3. Json, Xml과 같은 파일에 저장


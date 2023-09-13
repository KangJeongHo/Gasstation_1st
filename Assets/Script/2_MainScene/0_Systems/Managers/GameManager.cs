using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Authentication;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Net.Mime;
using Random = UnityEngine.Random;

/// 게임 규칙 및 정보 담김
public class GameManager : MonoBehaviour
{
    #region SingleTon_In_GameManager

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
    #endregion

    internal int lubCount0 = 0;
    public void LubCountAdd()
    {
        lubCount0++;
        Debug.Log("(1이면 활성화)현재 주유기 설치 상태 활성화 : " + lubCount0);
    }

    /// Manager
    public PrefabManager prefabManager;
    public ItemManager itemManager;
    public CarManager carManager;
    
    /// Controller
    public ShopController shopController;
    public SoundController soundController;
    public CarGenerator carGenerator;
    public PlayerController playerController;
    

    // 가진 돈
    public double m_Money = 1000000;
    public double satisfaction = 0;
    public double awareness = 0;
    public double gas = 1000;

    
    void Start()
    {
        오브젝트체킹();
        Ui글자초기화(); //게임이 실행되고 나서 화면에 표현해야 할 글자들을 갱신해줌.
    }

    internal Text MoneyText;
    internal Text SatisText;
    internal Text AwareText;
    internal Text GasText;

    void 오브젝트체킹()
    {
        MoneyText = GameObject.Find("Money").GetComponent<Text>();
        SatisText = GameObject.Find("Satisfaction").GetComponent<Text>();
        AwareText = GameObject.Find("Awareness").GetComponent<Text>();
        GasText = GameObject.Find("Gas").GetComponent<Text>();
    }
    public void Ui글자초기화()
    {
        MoneyText.text = m_Money.ToString() + " 원";
        SatisText.text = satisfaction.ToString() + " 점";
        AwareText.text = awareness.ToString() + " 점";
        GasText.text = gas.ToString() + " L";
    }

    void Update()
    {
        //GasText.text = gas.ToString() + " L";
    }
}
    


    #region 제이슨 데이터 관리 사용법

/*
    public CarsData carsData; // 차에 대한 데이터를 담고 있는 그릇.

    [ContextMenu("제이슨 데이터로 저장하기")]
    //제이슨 데이터로 저장할 수 있게끔 유니티 안에서 버튼을 만들어줌.
    void SaveCarsDataToJson()
    {
        string jsonData = JsonUtility.ToJson(carsData, true);
        // prettyprint가 true 로 바뀌면 내용 정리가 깔끔하게 저장된다고 한다.
        // true로 안해주면 기본값으로 false가 들어간다고 한다.
        // carsData로 받아준 값을 제이슨 파일로 바꿔주도록 JsonUtility 기능이 도와줌.
        //그래서 그 제이슨 파일로 받아줄 값을 문자열 string으로 jsonData 값으로 받아줌.
        string path = Path.Combine(Application.dataPath + "/Resources/Jsons/carsData.json");
        // Application.dataPath라는 건 이 유니티 프로젝트가 저장되는 곳을 뜻한다.
        // Path.Combine을 쓰면 윈도우랑 맥이 주소가 다르게 저장되는 문제를 해결해준다.
        File.WriteAllText(path, jsonData);
        // 파일을 저장할건데 저장할 경로랑 저장할 문자열을 적어준다.
    }

    [ContextMenu("제이슨 데이터를 불러오기")]
    void LoadCarDataToJson()
    {
        string path = Path.Combine(Application.dataPath + "/Resources/Jsons/CarDatas.json");
        string jsonData = File.ReadAllText(path); //경로에 있는 제이슨 데이터를 불러와서 jsonData에 담아준다.
        carsData = JsonUtility.FromJson<CarsData>(jsonData);
        TextAsset carsAsset = Resources.Load<TextAsset>("Jsons/CarDatas");
        // JsonUtility.FromJson을 통해서 제이슨으로부터 오브젝트를 역 직렬화 시켜주려고 하는 상황.
        // 오브젝트로 복구시키고 싶은 대상을 정의해주어야한다.
    }
}


[System.Serializable]
public class CarsData
{
    public string mCarName;

    public enum MCarGasType { Gasoline, Diesel, Lpg, Bio, Electronic, ox }

    public int mCarGasAmountMin,
        mCarGasAmountMax,
        mCarGasAmountRand,
        mCarLimitTimeMin,
        mCarLimitTimeMax,
        mCarLimitTimeRand,
        mCarSatisfactionMin,
        mCarSatisfactionMax,
        mCarSatisfactionRand,
        mCarZenProbability,
        mCarOpenLevel;

    public string mCarImage;

    public bool mIsActive;
}
*/
        

    #endregion

/*
// 딕셔너리 사용법1 : https://engineer-mole.tistory.com/174
// 네임스페이스 설명 : https://coderzero.tistory.com/entry/%EC%9C%A0%EB%8B%88%ED%8B%B0-C-%EA%B0%95%EC%A2%8C-12-%EB%84%A4%EC%9E%84%EC%8A%A4%ED%8E%98%EC%9D%B4%EC%8A%A4Namespaces-using
// 싱글턴 : https://art-life.tistory.com/130
*/
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

/// 첫번째 항목 : 차량의 움직임 관련 / 오브젝트를 움직이기 위한 대본
public enum CarNames
{
    RedCar, BlueCar, GreenCar, YellowCar, FuckCar
}

[System.Serializable]
public class CarInfo
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

    public CarInfo()
    {
    }

    public CarInfo SetUnitValue(CarNames carNames)
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

public class CarController : MonoBehaviour
{
    #region 선언(public/private)
    public CarNames carNames;
    public CarInfo carInfo;
    public Transform lub_cube_0_transform;
    private GameObject carInstance1;
    private GameObject _randomCars;
    private int gasAmountRand = 0;
    private bool touchCheck = false;
    private bool chargeStart = false;
    private bool payCheck = false;
    private int carLimitRand = 0;
    
    public GameObject carGageBarPrefab;
    public Vector3 carGageBarOffset = new Vector3(0, 0.2f, 0);
    private Canvas _gageCanvas;
    private Image _carGageBarImage;
    private Image _carWaitBarImage;
    private Image _carWaitBarImage2;
    private GameObject _carGageBar;
    #endregion

    private void Awake()
    {
        carInfo = new CarInfo();
        carInfo = carInfo.SetUnitValue(carNames);
    }

    void Start()
    {
        //var carNamesArray = Enum.GetValues(typeof(CarNames)); // 차이름에 해당하는 값을 문자로 변경한 값으로 배열만들어서 담아줌.
        var lubCheckCoroutine = LubCheck(); // 주유기 설치 되어있는지 확인하는 코루틴 작동 시작.
        StartCoroutine(lubCheckCoroutine);
    }

    IEnumerator LubCheck()                                                                   // 주유기 설치 유무 체크 코루틴
    { //주유기 체크는 나중에 게임 제너레이터나 디렉터로 가야할듯.
        while (GameManager.Instance.lubCount0 == 0/*GameObject.Find("GameManager").GetComponent<GameManager>().lubCount0 == 0*/)
        {
            Debug.Log("차량 주유기 설치 인식 대기중..");                                     // 주유기가 설치되어있는지 확인함. 확인이 되면 아래 카 스폰 코루틴 시작함.
            yield return new WaitForSeconds(1f);
        }
        var carSpawnCoroutine = CarSpawn();
        StartCoroutine(carSpawnCoroutine);
    }

            
    
    IEnumerator CarSpawn()
    {
        Debug.Log("CarSpawn 코루틴 진입");
        ReapeatCarSpawn:
        GetCarInfo();
        var chargingTime = 0;                                                                // 기름을 지금까지 얼마나 충전했는지를 담는 값.
        var randomTimeSpawn = Random.Range(5, 15);                                        // 다음 차량이 오는데 걸리는 시간 
        if (_randomCars == null)
        {                                                                                    // 주유하러 올 차가 인스턴스화 되기전에 값이 안들어있으면 값을 넣어줌.
            _randomCars = Resources.Load<GameObject>(carNames.ToString());
        }
        Debug.Log("차량이 오기까지 걸리는 시간 : " + randomTimeSpawn + "초");
        Debug.Log("차량이 완충까지 걸리는 시간 : " + gasAmountRand + "초");
        yield return new WaitForSeconds(randomTimeSpawn);                                    // 차량이 다시 오는데까지 기다림.
        while (GameManager.Instance.lubCount0 == 1/*GameObject.Find("GameManager").GetComponent<GameManager>().lubCount0 == 1*/)
        {                                                                                    // 주유기가 설치되어있다면 작동하는 내용.
            if (carInstance1 == null)
            {                                                                                //차량이 나타날 수 있도록 게임오브젝트(차량)을 소환해줌(주유기 부지 위치애)
                SoundController.Instance.소리재생("차량등장");
                carInstance1 = Instantiate(_randomCars, lub_cube_0_transform);
                SetHpBar();
                Debug.Log("차량 소환됨");
            }
            if (carInstance1 != null)
            {                                                                                //차량이 있으면 해당 내용 작동.
                carLimitRand = Random.Range(carInfo.MCarLimitTimeMin, carInfo.MCarLimitTimeMax);
                for (int a = 0; a <= carLimitRand; a++)
                {
                    _carWaitBarImage.fillAmount = (float)(carLimitRand - a) / (float)carLimitRand;
                    if (chargeStart == false)
                    {                                                                        // 차량이 주유중이 아니라면 차량 터치 체크를 하기 위해 터치체크를 참으로 바꾸고 업데이트 내용에 있는 이프문을 돌리게함.
                        Debug.Log("차량 떠나기까지 남은 시간 : " + (carLimitRand - a) + " 초");
                        touchCheck = true;                                                   // update로 가서 터치체크 이프문 작동.
                    }
                    if (touchCheck == false)
                    {
                        payCheck = false;
                        Debug.Log("차량 클릭한것 인식됨.(차지스타트 트루)");
                        _carWaitBarImage.fillAmount = 0;
                        SoundController.Instance.소리재생("주유시작");
                        goto StartGasCharge;
                    }
                    if (a == carLimitRand)
                    {
                        _carWaitBarImage.fillAmount = 0;
                        파괴();
                        goto ReapeatCarSpawn;
                    }
                    yield return new WaitForSeconds(1f);
                }
            }
            
            // 2. 차량 대기시간을 시각화한다.
            StartGasCharge:
            SoundController.Instance.소리재생("주유중");
            while (gasAmountRand > chargingTime)
            { // gasAMountRand = 기름 넣는 시간 = 기름 넣는 양
                chargingTime++;
                GameManager.Instance.gas -= 100;
                _carGageBarImage.fillAmount = ((float)chargingTime/(float)gasAmountRand);
                yield return new WaitForSeconds(1f);
            }
            _carGageBarImage.fillAmount = 0;
            
            SoundController.Instance.소리재생("주유완료");
            if (gasAmountRand == chargingTime)
            {
                
                Debug.Log("차 돈받을 이프문 작동");
                carLimitRand = Random.Range(carInfo.MCarLimitTimeMin, carInfo.MCarLimitTimeMax);
                Debug.Log("기름 다넣고 기다리는데 빡쳐서 갈 시간 : " + carLimitRand);           //차량이 기름 넣어달라고 기다리는 시간.
                for (int b = 0; b <= carLimitRand; b++)
                {
                    _carWaitBarImage2.fillAmount = (float)(carLimitRand - b) / (float)carLimitRand;
                    if (payCheck == false)
                    {
                        Debug.Log(" 기름 다넣고 빡쳐서 가기까지 남은 시간 : " + (carLimitRand - b) + " 초");
                        touchCheck = true;
                                                                                                 //update에 있는 이프구문 인식하러감.
                    }
                    if (touchCheck == false)
                    {                                                                            // 여기에 이제 차량 돈 받는거랑 만족도랑 기름 감소 넣으면 될듯
                        Debug.Log("차량 클릭한것 인식됨.(페이체크 트루");
                        _carWaitBarImage2.fillAmount = 0;
                        정산();
                        파괴();
                        goto ReapeatCarSpawn;
                    }
                    if (b == carLimitRand)
                    {
                        Debug.Log("차량 클릭 못함 파괴되러감.");
                        _carWaitBarImage2.fillAmount = 0;
                        파괴();
                        goto CarDistroy;   // 여기서 파괴될땐 나중에 만족도 같은거 깎거나 돈 제대로 안주고 가게 만들기.
                    }
                    yield return new WaitForSeconds(1f);
                }
                // 4. 차량 기름 충전 게이지를 시각화한다.
                // 5. 차량 기름 다 넣고 대기시간을 시각화한다.
                // 7. 돈이 오른다.

                if (payCheck == true)
                {
                    Debug.Log("파괴 전 정산 작동.");
                    정산();           // 기름 넣은 양 만큼 돈 값과 곱해서 지불시키기.
                    payCheck = false;
                }
                CarDistroy:
                파괴();
                break;
            }
        }
        goto ReapeatCarSpawn;
    }

    void Update()
    {
        #region 차량 클릭 인식 구문
        if (touchCheck == true)
        {
            if (Input.GetMouseButton(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                Debug.Log("충전 시작 여부 : "+ chargeStart);
                Debug.Log("터치 체크 여부 : "+ touchCheck);
                Debug.Log("돈 지불 여부 : "+ payCheck);
                if (Physics.Raycast(ray, out hit))
                {
                    Debug.Log("현재 터치되었다고 인식되는 오브젝트 : " + hit.transform.gameObject);
                    if (hit.transform.gameObject == carInstance1)
                    {
                        Debug.Log(carInfo.CarNames + " 차량 클릭됨.");

                        if (chargeStart == false)
                        {
                            chargeStart = true;
                            touchCheck = false;
                        }
                        if (payCheck == false)
                        {
                            payCheck = true;
                            touchCheck = false;
                        }
                    }
                }
            }
        }
        #endregion
    }
    void GetCarInfo() // 차량 값 받아주는 변수
    {
        var carArray = Enum.GetValues(typeof(CarNames));                                            // 차이름에 해당하는 값을 문자로 변경한 값으로 배열만들어서 담아줌.
        var chooseRandomIndex = Random.Range(0, carArray.Length);                                // 차를 랜덤으로 골라주기 위해 차량 배열의 수만큼을 가지고 랜덤 숫자를 뽑음.
        carNames = (CarNames)Enum.Parse(typeof(CarNames), chooseRandomIndex.ToString());
        gasAmountRand = Random.Range(carInfo.MCarGasAmountMin, carInfo.MCarGasAmountMax);
        Debug.Log("곧 주유할 지정된 차량 : " + carNames);
    }

    private int mSatis;
    void 파괴()
    {
        if (payCheck == true)
        {
            Debug.Log("파괴 내부 정산 작동.");
            mSatis = Random.Range(carInfo.MCarSatisfactionMin,carInfo.MCarSatisfactionMax);
            mSatis = mSatis - (int)GameManager.Instance.satisfaction;
            //만족도 감소시키려고 만든 체크문
            SoundController.Instance.소리재생("만족도하락");
            정산();
        }
        Debug.Log("차량 인스턴스 파괴.");
        Destroy(carInstance1);
        carInstance1 = null;
        _randomCars = null;
        chargeStart = false;
        touchCheck = false;                                                                      // 터치인식 켜져있을수도 있으니 초기화
        payCheck = false;
        _carGageBarImage.GetComponentsInParent<Image>()[1].color = Color.clear;
        Destroy(_carGageBar);
        SoundController.Instance.소리재생("차량떠남");
    }

    void 정산()
    { // 플레이어스크립트나 게임디렉터,게임제너레이터,게임매니저 등 다른데로 옮겨야할 내용.
        mSatis = Random.Range(carInfo.MCarSatisfactionMin,carInfo.MCarSatisfactionMax);
        mSatis = mSatis + (int)GameManager.Instance.satisfaction;
        SoundController.Instance.소리재생("만족도상승");

        var mMoney = (int)GameManager.Instance.m_Money;
        if (carInfo.MCarGasType == "Gasoline")
        {
            mMoney = mMoney + (int)(gasAmountRand * 1000);
        }
        else if (carInfo.MCarGasType == "Diesel")
        {
            mMoney = mMoney + (int)(gasAmountRand * 1000);
        }
        else if (carInfo.MCarGasType == "Bio")
        {
            mMoney = mMoney + (int)(gasAmountRand * 1000);
        }
        else if (carInfo.MCarGasType == "Lpg")
        {
            mMoney = mMoney + (int)(gasAmountRand * 1000);
        }
        else if (carInfo.MCarGasType == "Lpg")
        {
            mMoney = mMoney + (int)(gasAmountRand * 1000);
        }
        GameManager.Instance.m_Money = mMoney;
        GameManager.Instance.Ui글자초기화();
        SoundController.Instance.소리재생("돈받음");
        payCheck = false;
    }

    IEnumerator 숫자이펙트(double target, double current)
    {
        double duration = 5f;
        double offset = (target - current) / duration;

        while (current < target)
        {
            current += offset * Time.deltaTime;
            //아래 값을 하기 전에 빼고나서 0원 이하가 될땐 못하게 막을 예정임.
            //GameManager.Instance.gas = ((int)current);
            GameManager.Instance.GasText.text = ((int)current).ToString() + " L";
            yield return null;
        }

        current = target;
        //GameManager.Instance.gas = ((int)current);
        GameManager.Instance.GasText.text = ((int)current).ToString() + " L";
    }

    void SetHpBar()
    {
        _gageCanvas = GameObject.Find("Gage Canvas").GetComponent<Canvas>(); 
        _carGageBar = Instantiate<GameObject>(carGageBarPrefab, _gageCanvas.transform);
        _carGageBarImage = _carGageBar.GetComponentsInChildren<Image>()[1];
        _carWaitBarImage = _carGageBar.GetComponentsInChildren<Image>()[2];
        _carWaitBarImage2 = _carGageBar.GetComponentsInChildren<Image>()[3];
        // 0번째에는 자기자신 이미지를 뜻하니까 자식의 이미지를 불러올거라 1번을 불러오는거임.

        var gageBar = _carGageBar.GetComponent<CarGageBar>();
        gageBar.targetTransform = carInstance1.gameObject.transform;
        // 애너미 데미지를 가지고있는 게임오브젝트 트랜스폼은 적 캐릭터이다. 적캐릭터의 트랜스폼을 넣어주는거다.
        gageBar.offset = carGageBarOffset;
    }
    

}



// 차량 이넘으로 소환해서 값 넣어주는 자료 : https://parkshuan.tistory.com/entry/16%EC%9C%A0%EB%8B%88%ED%8B%B0%ED%8F%AC%ED%8F%B4-%EC%8A%A4%ED%83%AF-%EA%B5%AC%ED%98%84-%EB%B0%8F-%EC%A0%81%EC%9A%A9
// 이넘값을 배열로 받아와주기(?) : https://gigong.tistory.com/104
// 이넘 값 반환 자료 : https://medialink.tistory.com/151
// 일정시간마다 랜덤 생성 : https://coding-of-today.tistory.com/8
// 아이템 랜덤 드랍하기 : https://funfunhanblog.tistory.com/22
// 오브젝트 활성화 되었을때 이벤트 주기 : https://boxwitch.tistory.com/entry/%EC%9C%A0%EB%8B%88%ED%8B%B0-gameObject-%ED%99%9C%EC%84%B1-activeSelf
// 다른 스크립트 함수 땡겨오기 : https://dlemrcnd.tistory.com/8
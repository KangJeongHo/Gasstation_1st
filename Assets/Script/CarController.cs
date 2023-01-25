using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEditor;
using Random = UnityEngine.Random;

public class CarController : MonoBehaviour
{
    // 플레이어 움직임 관련 / 오브젝트를 움직이기 위한 대본
    
    //void start
    public GameObject LubCubeObject;
    private Transform lub_cube_0_transform;
    private IEnumerator LubCheckCorutine;
    //void LubCheck
    private IEnumerator CarSpawnCoroutine;
    //void CarSpawn
    private int CarSpawnbool;
    private int RandomSpawnTime;
    public GameObject RedCar; // 주유기 프리팹 소환하기 위한 오브젝트 선언
    private float RedCarTime;
    private IEnumerator RandomTimeCoroutine;
    void Start()
    {
        Debug.Log("CarController 시작");
        LubCubeObject = GameObject.Find("lub cube (0)");
        lub_cube_0_transform = GameObject.Find("Shop Controller").GetComponent<ShopController>().lub_cube_0_transform;
        
        Debug.Log("LubCheck를 위한 코루틴 준비 완료.");
        LubCheckCorutine = LubCheck();
        StartCoroutine(LubCheckCorutine);
        //InvokeRepeating("CarSpawn",Random.Range(1,10),1);
    }
    
    IEnumerator LubCheck()
    {
        Debug.Log("LubCheck 코루틴 진입.");
        Debug.Log(LubCubeObject.activeSelf);
        while(LubCubeObject.activeSelf == false)
        {
            Debug.Log("주유기 설치 인식 while문 작동중. (1초주기)");
            yield return new WaitForSeconds(1f);
        }
        Debug.Log("주유기 설치 인식 while문 사용 종료됨.");
        CarSpawnCoroutine = CarSpawn(); 
        StartCoroutine(CarSpawnCoroutine);
        
        Debug.Log("LubCheck 코루틴 진입. ");
    }
 
    IEnumerator CarSpawn()
    {
        CarSpawnbool = 1;
        RandomSpawnTime = Random.Range(1, 10);
        Debug.Log("현재 기다려야 하는 시간(랜덤) : " + RandomSpawnTime);
        yield return new WaitForSeconds(RandomSpawnTime);
        RedCarTime = 0;
        while (CarSpawnbool == 0)
        {
            GameObject carInstance1 = Instantiate(RedCar, lub_cube_0_transform);
            if (RedCarTime > Random.Range(1, 10))
            {
                CarSpawnbool = 0;
                Destroy(carInstance1);
            }
            
            yield return new WaitForSeconds(1f);
            RedCarTime++;
        }
        
        Debug.Log("차 떠나가서 다시 생성작업 들어감.");
        yield return StartCoroutine(CarSpawnCoroutine);
        // 차가 계속 스폰 될 수 있는 반복 시키기.
    }

}


// 일정시간마다 랜덤 생성 : https://coding-of-today.tistory.com/8
// 아이템 랜덤 드랍하기 : https://funfunhanblog.tistory.com/22
// 오브젝트 활성화 되었을때 이벤트 주기 : https://boxwitch.tistory.com/entry/%EC%9C%A0%EB%8B%88%ED%8B%B0-gameObject-%ED%99%9C%EC%84%B1-activeSelf
// 다른 스크립트 함수 땡겨오기 : https://dlemrcnd.tistory.com/8

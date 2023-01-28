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

    /// 플레이어 움직임 관련 / 오브젝트를 움직이기 위한 대본
public class CarController : MonoBehaviour
{
    
    /* void start */
    public GameObject LubCubeObject;
    private Transform lub_cube_0_transform;
    private IEnumerator LubCheckCorutine;

    /* void LubCheck */
    private int lubcount0;
    private IEnumerator CarSpawnCoroutine;

    /* void CarSpawn */
    private int RandomSpawnTime;
    private int CarWaitingTime;
    private float RedCarTime;
    public GameObject RedCar; // 주유기 프리팹 소환하기 위한 오브젝트 선언
    private GameObject carInstance1 = null;
    void Start()
    {
        LubCubeObject = GameObject.Find("lub cube (0)");
        lub_cube_0_transform = GameObject.Find("Shop Controller").GetComponent<ShopController>().lub_cube_0_transform;
        LubCheckCorutine = LubCheck();
        StartCoroutine(LubCheckCorutine);
        //InvokeRepeating("CarSpawn",Random.Range(1,10),1);
    }
    IEnumerator LubCheck()
    {
        while(GameObject.Find("EventSystem").GetComponent<GameManager>().lubCount0 == 0)
        {
            yield return new WaitForSeconds(1f);
        }
        CarSpawnCoroutine = CarSpawn(); 
        StartCoroutine(CarSpawnCoroutine);
        
    }
 
    IEnumerator CarSpawn()
    {
        RepeatCarSpawn:
        RandomSpawnTime = Random.Range(5, 15);
        CarWaitingTime = Random.Range(5, 15);
        RedCarTime = 0;
        yield return new WaitForSeconds(RandomSpawnTime);
        while (GameObject.Find("EventSystem").GetComponent<GameManager>().lubCount0 == 1)
        {
            if (carInstance1 == null)
            {
                 carInstance1 = Instantiate(RedCar, lub_cube_0_transform);
            }
            if (RedCarTime > CarWaitingTime)
            {
                
                Destroy(carInstance1);
                break;
            }
            RedCarTime++;
            yield return new WaitForSeconds(1f);
        }
        carInstance1 = null;
        goto RepeatCarSpawn;
    }

}


// 일정시간마다 랜덤 생성 : https://coding-of-today.tistory.com/8
// 아이템 랜덤 드랍하기 : https://funfunhanblog.tistory.com/22
// 오브젝트 활성화 되었을때 이벤트 주기 : https://boxwitch.tistory.com/entry/%EC%9C%A0%EB%8B%88%ED%8B%B0-gameObject-%ED%99%9C%EC%84%B1-activeSelf
// 다른 스크립트 함수 땡겨오기 : https://dlemrcnd.tistory.com/8

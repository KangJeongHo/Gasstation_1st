using System;
using UnityEngine;
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
    public GameObject RandomCars; // 주유기 프리팹 소환하기 위한 오브젝트 선언
    private GameObject carInstance1 = null;
    string[] Cars = { "RedCar", "BlueCar", "GreenCar", "YellowCar" };

    void Start()
    {
        LubCubeObject = GameObject.Find("lub cube (0)");
        lub_cube_0_transform = GameObject.Find("Shop Controller").GetComponent<ShopController>().lub_cube_0_transform;
        LubCheckCorutine = LubCheck();
        StartCoroutine(LubCheckCorutine);
    }

    public class CarInstanceManager
    {
        internal T Load<T>(string path) where T : Object
        {
            return Resources.Load<T>(path);
        }

        internal GameObject Instantiate(string path, Transform parent = null)
        {
            GameObject prefab = Load<GameObject>($"{path}");
            if (prefab == null)
            {
                Debug.Log($"프리팹 불러오는데 실패했습니다 : {path}");
                return null;
            }

            return Object.Instantiate(prefab, parent);
        }
    }

    IEnumerator LubCheck()
    {
        while (GameObject.Find("EventSystem").GetComponent<GameManager>().lubCount0 == 0)
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
        int CarsRandomRange = Random.Range(0, 3);
        if (RandomCars == null)
        {
            string CarsString = Cars[CarsRandomRange];
            RandomCars = Resources.Load<GameObject>(CarsString);
        }

        Debug.Log(RandomCars);
        yield return new WaitForSeconds(RandomSpawnTime);
        while (GameObject.Find("EventSystem").GetComponent<GameManager>().lubCount0 == 1)
        {
            if (carInstance1 == null)
            {
                carInstance1 = Instantiate(RandomCars, lub_cube_0_transform);
            }

            if (RedCarTime > CarWaitingTime)
            {
                Destroy(carInstance1);
                RandomCars = null;
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
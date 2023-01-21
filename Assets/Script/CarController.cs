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
    public double time;
    private Transform lub_cub_0_transform; // 주유기 설치를 위한 부모 찾기? 왜 main_stations와 중첩되는 것 같음.
    public GameObject RedCar; // 주유기 프리팹 소환하기 위한 오브젝트 선언
    private int flag;

    public GameObject a;

    // 플레이어 움직임 관련 / 오브젝트를 움직이기 위한 대본
    void Start()
    {
        a = transform.Find("lub cub (0)").gameObject;
        lub_cub_0_transform = GameObject.Find("Shop Controller").GetComponent<ShopController>().lub_cub_0_transform;

        //InvokeRepeating("CarSpawn",Random.Range(1,10),1);

    }

    // Update is called once per frame
    void Update()
    {
        if (a.activeSelf == true)
        {
            Debug.Log("카 스폰 작동했습니다.");
            StartCoroutine("CarSpawn");

        }
    }
    /*
     * 0. 어느 주기로 차가 생성되어야 할지 생각해야함.
     * 1. 자동차가 게임 속의 시간대비 주유기에 와야함.
     * 2. 주유기가 있는지 인지 후 차가 오기 시작해야함.
     * 3. 차가 왔을때 차를 눌러줘야함.
     * 4. 차를 눌러주면 차들의 원하는 수치(랜덤)에 맞게 기름을 넣어줘야함.
     * 5. 기름이 꽉 차면 시간안에 빼줘야함.
     * 6. 그 차를 클릭하여 보내고 나면 기름 넣은 양만큼 돈을 줌.
     * 7. 만족도나 행운수치, 인기도 등이 높으면 팁을 줌.
     * 8. 
     *
     * 
     */

    private float RedCarTime = 0f;
    private IEnumerator box;
    IEnumerator CarSpawn()
    {

        flag = 1;
        while (flag == 1)
        {
            GameObject carInstance1 = Instantiate(RedCar, lub_cub_0_transform);
            Debug.Log(carInstance1.activeSelf);
            RedCarTime += Time.deltaTime;
            if (RedCarTime > Random.Range(1, 10))
            {
                flag = 0;
            }
            
            Destroy(carInstance1);

        }

        box = randomtime();
        yield return StartCoroutine(box);
        // 코루틴을 ""로 불러 들어오면 탐색하는데 시간이 걸리고 메모리를 차지하니까 전에 선언해서 받아올것.
    }

     IEnumerator randomtime()
     {
         float b = Random.Range(1, 10);

         yield break;/*return new WaitForSeconds(b)*/;
     }
}


// 일정시간마다 랜덤 생성 : https://coding-of-today.tistory.com/8
// 아이템 랜덤 드랍하기 : https://funfunhanblog.tistory.com/22
// 오브젝트 활성화 되었을때 이벤트 주기 : https://boxwitch.tistory.com/entry/%EC%9C%A0%EB%8B%88%ED%8B%B0-gameObject-%ED%99%9C%EC%84%B1-activeSelf
// 다른 스크립트 함수 땡겨오기 : https://dlemrcnd.tistory.com/8

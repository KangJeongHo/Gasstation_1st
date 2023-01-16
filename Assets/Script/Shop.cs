using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{

    public GameObject mainCanvas; //메인 캔버스
    public GameObject shopCanvas; //상점 캔버스
    public GameObject mainCamera; //메인 카메라
    public GameObject buy_stations; // 주유기 설치 칸 보기(부모 오브젝트)
    public GameObject main_stations; // 주유기 설치 되고 나면 보이는 칸 (부모 오브젝트)
    public Ray ray; // 터치 인식을 위한 레이 선언
    public RaycastHit hit; //터치 인식을 한 위치 저장을 위한 선언.
    public GameObject normal_lub; // 주유기 프리팹 소환하기 위한 오브젝트 선언
    public Transform lub_cub_0; // 주유기 설치를 위한 부모 찾기? 왜 main_stations와 중첩되는 것 같음.

    //오브젝트들을 인스턴스화시켜서 선언을 줄여보자.
    //주유소 스테이션의 주유공간을 배열로 자동 프리팹 생성되게 바꿔보도록 하자.
    // 유니티 생명주기 잘 써보기.
    void start()
    {
        mainCanvas = GameObject.Find("Main Canvas");
        shopCanvas = GameObject.Find("Shop Canvas");
        mainCamera = GameObject.Find("Main Camera");

        ray = Camera.main.ScreenPointToRay(Input.mousePosition); // 화면 터치하는 위치 저장.
        
    }

    void Update()
    {
        
    }

    public void Setting_button()
    {
        
    }

    public void Shop_button()
    {
        mainCanvas.SetActive(false);
        shopCanvas.SetActive(true);
        mainCamera.transform.position = new Vector3(0, 8, -10);
        
    }

    public void Shop_exit_button()
    {
        shopCanvas.SetActive(false);
        mainCanvas.SetActive(true);
        mainCamera.transform.position = new Vector3(0, 0, -10);
    }

    public void Buy_lub()
    {
        shopCanvas.SetActive(false);
        Debug.Log("shop canvas false");
        mainCanvas.SetActive(true);
        Debug.Log("main canvas true");
        mainCamera.transform.position = new Vector2(0,0);
        Debug.Log("camera position come back");
        buy_stations.SetActive(true);
        Debug.Log("stations true");
        
        if (Physics.Raycast(ray, out hit))
        {
            /* 1. 주유소 칸 상자를 클릭한 값을 저장하게 함.
             * 1-1. 주유소 칸 상자를 나중에 어디 값에다가 카운드해서 저장해야겠음.
             *      ex: lub.buy.1 ++; , lubname.text = "기본주유기"
             * 2. 주유소 상자들을 눈앞에서 사라지게 해야함. 이게 사라지는 순서가 언제인지가 중요한듯. 사실상 빈공간 클릭했는데
             *      사라지면 안되니깐.
             * 3. 주유소 상자들이 사라진 후 그 자리에 주유기가 추가해야함 
                4. case로 항목을 선택지로 추가하되 case전에 공통된 항목으로 2번은 가능할듯.
             */
           
            Debug.Log(hit.transform.gameObject + "주유소 칸 터치된거임.");
            if (hit.transform.gameObject.tag == "buy area (0)") // 주유기 첫번째 칸을 클릭했을때
            {
                Debug.Log("주유기 1번 칸에 추가 및 주유기 구입 칸 항목 비활성화 숫자로 분간.");
                // GameObject myInstance = Instantiate(prefab); 부모 지정 없이 인스턴스화 하는 방법.

                GameObject lubInstance = Instantiate(normal_lub,lub_cub_0); // 부모 밑에 인스턴스하기.
                //lubInstance.transform.position = main_parent + new Vector3(0f,0f,0); 
                // 위 선언은 부모의 좌표 + 그 옆에 어디 위치로 소환하는 방법.
                main_stations.SetActive(true);
                lub_cub_0.GetComponent<GameObject>().SetActive(true);
            }
            buy_stations.SetActive(false);  // 
        }
        //상점에서 사기 위한 위치를 표히사는 루브 스테이션은 프리팹으로 만들어서 상점 물건 살때만 나왔다가 사라지도록 만들자.
        //instantiate(prefab, transform)을 사용하면 오브젝트 하위 요소로 소환할 수 있음.
        
    }

}
/*
참고 링크 
유니티 오브젝트 공간 클릭 인식 방법 : https://asxpyn.tistory.com/53
유니티 프리팹과 인스턴스 사용 내용 : https://notyu.tistory.com/35
*/
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

    //상점 버튼을 위한 스크립트
public class Shop : MonoBehaviour
{
    public GameObject mainCanvas; //메인 캔버스
    public GameObject shopCanvas; //상점 캔버스
    public GameObject mainCamera; //메인 카메라
    public GameObject buy_stations; // 주유기 설치 칸 보기(부모 오브젝트)
    //오브젝트들을 인스턴스화시켜서 선언을 줄여보자.
    //주유소 스테이션의 주유공간을 배열로 자동 프리팹 생성되게 바꿔보도록 하자.
    // 유니티 생명주기 잘 써보기.
    
    
    
    
    void start()
    {
        mainCanvas = GameObject.Find("Main Canvas");
        shopCanvas = GameObject.Find("Shop Canvas");
        mainCamera = GameObject.Find("Main Camera");
        buy_stations = GameObject.Find("Buy Station");
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

        }
        //상점에서 사기 위한 위치를 표히사는 루브 스테이션은 프리팹으로 만들어서 상점 물건 살때만 나왔다가 사라지도록 만들자.
        //instantiate(prefab, transform)을 사용하면 오브젝트 하위 요소로 소환할 수 있음.
        
    }


/*
참고 링크 
유니티 오브젝트 공간 클릭 인식 방법 : https://asxpyn.tistory.com/53
유니티 프리팹과 인스턴스 사용 내용 : https://notyu.tistory.com/35
*/
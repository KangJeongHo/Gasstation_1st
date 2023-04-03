using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShopButtonExample : MonoBehaviour
{

    public GameObject lc0;
    public void lubcube0on()
    {
        lc0.SetActive(true);
        GameObject.Find("EventSystem").GetComponent<GameManager>().LubCountAdd();
    }
    
    
}


/*
참고 링크 
유니티 오브젝트 공간 클릭 인식 방법 : https://asxpyn.tistory.com/53
유니티 프리팹과 인스턴스 사용 내용 : https://notyu.tistory.com/35
*/

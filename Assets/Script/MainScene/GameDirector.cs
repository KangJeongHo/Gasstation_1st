using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Serialization;

    /// 점수 계산 및 게임 진행 재시작 및 오버 여부
    /// ui 조작하거나 진행 상황을 판단
public class GameDirector : MonoBehaviour
{
    
    // 자동차가 생기는건 플레이어일까..? 아니면 유지 관리가 플레이어일까..?
    // 플레이어의 주체에 뇌절이 오기 시작함.
    // 플레이하는 우리를 위해서 우리 스스로를 플레이어로 지칭해야할까?
    //private static GameDirector Inst = null;       노명보 코드
    
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

    void Start()
    {
        
    }
    
    
    
    
    
    
    
    /*
    #region MenuValue

    public  static MenuLogicValue MenuValue
    {
        get { return _instance.m_MenuValue; }
    }
    [SerializeField] private MenuLogicValue m_MenuValue = new MenuLogicValue();
    [Serializable]
    public class MenuLogicValue
    {
        public static bool Is_Menu_Cancle = false;
        
        public GameObject Menu_Cancle_Panel;
        public float Menu_Down_Speed;
        public int Menu_Distance;
        public GameObject Button;
    }
    #endregion
    */
    // void Start() 노명보 코드
    // {
    //     Inst = this;
    // }
}
    /// 데이터 저장 방법  * https://devruby7777.tistory.com/entry/Unity-%EC%9C%A0%EB%8B%88%ED%8B%B0%EC%9D%98-%EB%8D%B0%EC%9D%B4%ED%84%B0-%EC%A0%80%EC%9E%A5-%EB%B0%A9%EB%B2%95%EB%93%A4%EA%B3%BC-%EA%B7%B8-%EA%B2%BD%EB%A1%9C
    /// 1. 데이터베이스에 연결해 저장
    /// 2. 유니티에서 제공하는 PlayerPrefs 이용
    /// 3. Json, Xml과 같은 파일에 저장
    ///  
    ///

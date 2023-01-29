using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;
using Script;
using Unity.VisualScripting;
using UnityEngine.UI;

// ### 검색해서 나중에 수정하면서 뺄 코드들 찾기

namespace Script
{
    public class MenuScript : MonoBehaviour
    {
        static MenuScript Inst = null;

        // UI 패널 사이즈 통일을 위해.
        public static Vector2 Main_Canvas_Size { get { return m_Main_Canvas_Size.sizeDelta; } }
        static RectTransform m_Main_Canvas_Size;

        #region GameObjects
        public static GameObject Shop_Canvas { get { return Inst.m_Shop_Canvas; } }
        public GameObject m_Shop_Canvas = null;

        #endregion

        //메뉴 버튼에 메뉴컴포넌트 삽입하기위해 선언  ### 나중에 메뉴이미지들 프리팹 따로따로 만들면 없앨 수 있음
        public GameObject Menu_Button = null;

        private void Awake()
        {
            //UI 패널 사이즈 통일을 위해
            m_Main_Canvas_Size = GetComponent<RectTransform>();


            Inst = this;
        }
        private void Start()
        {
            //메뉴 버튼에 메뉴컴포넌트 삽입  ### 나중에 메뉴이미지들 프리팹 따로따로 만들면 없앨 수 있음
            Menu_Button.AddComponent<Menu>();
            if (GameDirector.MenuValue.Menu_Cancle_Panel == null)
            {
                Debug.LogWarning("if (GameDirector.MenuValue.Menu_Cancle_Panel == null)");
            }
            GameDirector.MenuValue.Menu_Cancle_Panel.AddComponent<Menu_Cancle_Panel>();

        }
    }
}

#region Menu_Sub

enum MENU_LIST
{
    SETTING,
    SHOP,
}



// 상점버튼 기능 설정
/* 상점버튼들에는 클릭함수에
 * Menu_Cancle_Panel.Menu_Exit();
 * 를 꼭 추가한다. */
public class Menu_Shop : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        //Shop 입장 - 만들어진 상점 캔버스 열기
        MenuScript.Shop_Canvas.SetActive(true);
        Menu_Cancle_Panel.Menu_Exit();
    }
    void Awake()
    {
        // 디버깅용 ### 나중에 상점이미지 만들어 프리팹하면 지워도 됨
        GetComponentInChildren<Text>().text = "상점";
    }
}

public class Menu_Setting : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
    }
    void Awake()
    {
        // 디버깅용 ### 나중에 이미지 만들어 프리팹하면 지워도 됨
        GetComponentInChildren<Text>().text = "설정";
    }
}

#endregion

#region Menu


// 메뉴버튼 누른 후 버튼말고 빈화면 누르면 메뉴가 닫힘
public class Menu_Cancle_Panel : MonoBehaviour, IPointerClickHandler
{
    static Menu_Cancle_Panel Inst = null;
    public static void Menu_Exit()
    {
        GameDirector.MenuLogicValue.Is_Menu_Cancle = true;
        Inst.gameObject.SetActive(false);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Menu_Exit();
    }
    void Awake()
    {
        GetComponent<RectTransform>().sizeDelta = MenuScript.Main_Canvas_Size;
        Inst = this;
    }
}
public class Menu : MonoBehaviour,IPointerClickHandler
{
    

    // 메뉴 분류 (메뉴 생성 시 구분을 위해)
    MENU_LIST m_Menu_List = MENU_LIST.SETTING;

    private GameObject InstObject;

    private Vector3 Reset_Pre_InstObject_Pos = Vector3.zero;
    private Vector3 Pre_InstObject_Pos = Vector3.zero;
    // 생성된 메뉴 갯수 세는 함수 ( Open_Menu() 에서 사용 )  (건들필요없음)
    private int Menu_Count = 0;
    private List<GameObject> Exist_Menu = null;

    // 생성된 서브메뉴들이 내려오는 동안 메뉴가 클릭될 수 없게
    bool IsMenu_ClickAvailable = true;

    // 서브메뉴버튼에 (설정, 상점 등) 컴포넌트 삽입
    void class_In_InstObject(MENU_LIST _List)
    {
        if(_List == MENU_LIST.SETTING)
        {
            InstObject.AddComponent<Menu_Setting>();
        }
        else if (_List == MENU_LIST.SHOP)
        {
            InstObject.AddComponent<Menu_Shop>();
        }
    }

    // 메뉴 닫힘 함수
    void Menu_Detroy()
    {
        if (true == GameDirector.MenuLogicValue.Is_Menu_Cancle)
        {
            for (int i = 0; i < Exist_Menu.Count; i++)
            {
                Destroy(Exist_Menu[i]);
            }
            //메뉴생성변수들 초기화
            Menu_Count = 0;
            m_Menu_List = MENU_LIST.SETTING;

            Pre_InstObject_Pos = Reset_Pre_InstObject_Pos;

            GameDirector.MenuLogicValue.Is_Menu_Cancle = false;
            IsMenu_ClickAvailable = true;
        }
    }
        void Menu_Cancle_Panel_Active()
    {
        GameDirector.MenuValue.Menu_Cancle_Panel.SetActive(true);
    }
    void Open_Menu_Create() //메뉴칸 만들고 마지막 메뉴칸에 곂치게
    {
        InstObject = GameObject.Instantiate(GameDirector.MenuValue.Button);
        Exist_Menu.Add(InstObject);
        InstObject.transform.SetParent(this.transform);
        InstObject.transform.localScale = Vector3.one;
        InstObject.transform.localPosition = Pre_InstObject_Pos;
        
    }
    
    IEnumerator Open_Menu() // 메뉴 누르면 나오게 , 아래로 천천히 
    {
        Open_Menu_Create();
        
        Vector3 Origin = InstObject.transform.localPosition;

        while (Origin.y - GameDirector.MenuValue.Menu_Distance < InstObject.transform.localPosition.y)
        {
            InstObject.transform.localPosition += Vector3.down *Time.deltaTime * GameDirector.MenuValue.Menu_Down_Speed;
            if (Origin.y - GameDirector.MenuValue.Menu_Distance > InstObject.transform.localPosition.y)
            {
                InstObject.transform.localPosition = Origin + Vector3.down * GameDirector.MenuValue.Menu_Distance;
            }
            
            yield return null;
        }

        class_In_InstObject(m_Menu_List);
        m_Menu_List += 1;                // 다음 이넘으로 넘어가게 (Menu_Setting -> Menu_Shop -> ...)
        Menu_Count += 1;
        Pre_InstObject_Pos = InstObject.transform.localPosition;

        if (Menu_Count < GameDirector.MenuValue.Menu_Count) // 메뉴 갯수만큼 반복해서 생성
        {
            StartCoroutine(Open_Menu());
        }
    }
            
    // 메뉴버튼 클릭하면 취소패널만들고 상점 등 메뉴버튼 생성
    public void OnPointerClick(PointerEventData eventData)
    {
        if (true == IsMenu_ClickAvailable)
        {
            IsMenu_ClickAvailable = false;
            
            Menu_Cancle_Panel_Active();
            StartCoroutine(Open_Menu());
        }
    }

    private void Awake()
    {
        //생성된 메뉴 닫기 위해 모아둠
        Exist_Menu = new List<GameObject>();
        //메뉴 일정거리로 배열하기위해 선언
        Reset_Pre_InstObject_Pos = Pre_InstObject_Pos;
    }

    private void Update()
    {
        Menu_Detroy();
    }
}
#endregion
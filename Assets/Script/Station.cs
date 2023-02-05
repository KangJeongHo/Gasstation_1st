using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System.Text.RegularExpressions;
using Unity.Burst.CompilerServices;
using static Item;
using UnityEngine.UI;

// Station에서 선언되어 플레이어가 배열로 소유함
// 부지 위치는 station 에서 적용 ground에는 number만 number로 스테이션에서 자식으로찾는다 (나중에 패치로 부지 위치가 변경될 것을 대비)
public class Grounds
{
    // 이 부지를 획득 했는가?
    bool m_IsOn = false;                           
    public bool IsOn { get { return m_IsOn; } }
    //부지 위치
    Vector3 m_Position;
    public Vector3 Position { get { return m_Position; } }
    // 부지 가격
    int m_Value = 0;                                
    public int Value { get { return m_Value; } }
    // 설치된 주유기
    Item.Lubricator m_Installed_Lub = null;                
    public Item.Lubricator Installed_Lub { get { return m_Installed_Lub; } }
    // 주유기  설치 - station에서 처리
    internal void Install_Lub(Item.Lubricator _Lubricator) 
    {
        m_Installed_Lub = _Lubricator;
    }
    // 부지 획득시 적용
    internal void On_Grounds() 
    {
        m_IsOn = true;
    }
    //부지 위치 적용
    internal void Insert_Ground_Position( Vector3 _Position)
    {
        m_Position= _Position;
    }
    
    public Grounds(int _Value)
    {
        m_Value = _Value;
    }
}


public enum GROUNDS_MAIN_POP_UP_SELLECT
{
    BUY_GROUNDS,
    LUB_SETTING,
    LUB_MOVE,
    ON,
}
#region Grounds Menu Canvas

public enum ON_OFF
{
    ON,
    OFF,
}
# region Lub Inventory

public class Lub_Inventory_Install_Button : MonoBehaviour, IPointerClickHandler
{
    Grounds Sellected_Grounds;
    int Lub_Index;
    void Calculate()
    {
        if (Sellected_Grounds.Installed_Lub != null)
        {
            Lub_Index = Array.FindIndex(Lub_Inventory.Arr_Cur_Lub, x => x == Sellected_Grounds.Installed_Lub);
            Lub_Inventory.Arr_Cur_Lub[Lub_Index].Add_Installed_Amount(-1);
        } // 이미 설치되어있는 주유기 치우고 - Change
        Lub_Index = Array.FindIndex(Lub_Inventory.Arr_Cur_Lub, x => x == Lub_Inventory.Cur_Lub);
        Lub_Inventory.Arr_Cur_Lub[Lub_Index].Add_Installed_Amount(1);
        Sellected_Grounds.Install_Lub(Lub_Inventory.Cur_Lub);
    } // 설치된 수 하나 늘리기
    public void OnPointerClick(PointerEventData eventData)
    {
        Sellected_Grounds = PlayerScript.Instance.m_Grounds[Grounds_Menu_Canvas.Sellected_Grounds_Index];
        Calculate();
        Grounds_Menu_Canvas.Set_Lub_Inventory_Sellect = ON_OFF.OFF;
    }
}
public class Lub_Inventory_Install_Cancle_Panel : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Lub_Inventory.Set_Lub_Inventory_Pop_Sellect = LUB_INVENTORY_INSTALL_POP_UP.OFF;
    }
}
public enum LUB_INVENTORY_INSTALL_POP_UP
{
    ON,
    OFF,
}
public class Lub_Inventory_Install_Pop_Up : MonoBehaviour
{
    GameObject Cancle_Panel;
    TMP_Text Text; // (주유기)를 설치하시겠습니까?
    GameObject Install_Button;

    void Set_Child()
    {
        Cancle_Panel = transform.Find("Cancle Panel").gameObject;
        Cancle_Panel.AddComponent<Lub_Inventory_Install_Cancle_Panel>();
        Text = transform.Find("Text").GetComponent<TMP_Text>();
        Install_Button = transform.Find("Install Button").gameObject;
        Install_Button.AddComponent<Lub_Inventory_Install_Button>();
    }
    void Exit_Me()
    {
        if(Lub_Inventory.Lub_Inventory_Pop_Sellect == LUB_INVENTORY_INSTALL_POP_UP.OFF)
        {
            gameObject.SetActive(false);
        }
    }
    private void LateUpdate()
    {
        Exit_Me();
    }
    private void OnEnable()
    {
        if (Lub_Inventory.Cur_Lub.Name != null)
        {
            Text.text = Lub_Inventory.Cur_Lub.Name + "를" + "\n" + "설치하시겠습니까?";
        }
    }
    private void Awake()
    {
        Set_Child();
    }
    
}
public class Lub_Inventory_Button_In_Scroll : MonoBehaviour, IPointerClickHandler
{
    public Lubricator My_Lub;
    public int Number;
    TMP_Text Name;
    TMP_Text Has_Amount;
    TMP_Text Install_Available_Amount;
    
    void Button_Disable()
    {
        if( My_Lub.HasAmount - My_Lub.Installed_Amount <= 0)
        {
            GetComponent<Button>().enabled = false;
            GetComponent<Image>().color = new Color(0.3f, 0.3f, 0.3f);
        }
    }
    void Update()
    {
        if(Lub_Inventory.Is_Change == true)
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        Name.text = My_Lub.Name;
        Has_Amount.text = "보유 : " + My_Lub.HasAmount;
        Install_Available_Amount.text = "설치가능 : " + (My_Lub.HasAmount - My_Lub.Installed_Amount);
        Button_Disable(); // 설치가능수 0 이면 디폴트
    }
    void Awake()
    {
        Name = transform.Find("Name").GetComponent<TMP_Text>();
        Has_Amount = transform.Find("Amount").GetComponent<TMP_Text>();
        Install_Available_Amount = transform.Find("Install Available").GetComponent<TMP_Text>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if(My_Lub.HasAmount - My_Lub.Installed_Amount > 0)
        {
            Lub_Inventory.Set_Cur_Lub = My_Lub;
            Lub_Inventory.Set_Lub_Inventory_Pop_Sellect = LUB_INVENTORY_INSTALL_POP_UP.ON;
        }
    }
}
public class Lub_Inventory_Button_In_Main : MonoBehaviour, IPointerClickHandler
{
    public LUBRICATOR_TYPE Lub_Type; //인벤토리에서 add 로 추가해줌
    TMP_Text Type;
    void Set_Text()
    {
        switch (Lub_Type)
        {
            case LUBRICATOR_TYPE.GASOLINE:
                Type.text = "휘발유";
                break;
            case LUBRICATOR_TYPE.DIESEL:
                Type.text = "경유";
                break;
            case LUBRICATOR_TYPE.ELECTRIC:
                Type.text = "전기";
                break;
            case LUBRICATOR_TYPE.HYDROGEN:
                Type.text = "수소";
                break;
            case LUBRICATOR_TYPE.BIO:
                Type.text = "바이오";
                break;
            default:
                break;
        }
    }
    void Awake()
    {
        Type=transform.GetChild(0).GetComponent<TMP_Text>();
    }
    void Start()
    {
        Set_Text(); // Awake 에서 처리하면 Lub_Type에 값넣어주기전에 써져서 오류
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Lub_Inventory.Set_Lub_Type = Lub_Type;
        Lub_Inventory.Is_Change = true;
    }
}

public class Lub_Inventory_Cancle_Panel : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Grounds_Menu_Canvas.Set_Lub_Inventory_Sellect = ON_OFF.OFF;
    }
}
public class Lub_Inventory : MonoBehaviour
{
    // 고른 주유기 타입
    static LUBRICATOR_TYPE m_Lub_Type; 
    public static LUBRICATOR_TYPE Lub_Type { get { return m_Lub_Type; } }
    public static LUBRICATOR_TYPE Set_Lub_Type { set { m_Lub_Type = value; } }

    public static bool Is_Change = false; //휘발유, 경유 .. 눌렀을때 메뉴 변하는 트리거

    //고른 주유기
    static Lubricator m_Cur_Lub;
    public static Lubricator Cur_Lub { get { return m_Cur_Lub; } }
    public static Lubricator Set_Cur_Lub { set { m_Cur_Lub = value; } }

    //Lub_Inventory_Install_Pop_Up
    static LUB_INVENTORY_INSTALL_POP_UP m_Lub_Inventory_Pop_Sellect = LUB_INVENTORY_INSTALL_POP_UP.OFF;
    public static LUB_INVENTORY_INSTALL_POP_UP Lub_Inventory_Pop_Sellect { get { return m_Lub_Inventory_Pop_Sellect; } }
    public static LUB_INVENTORY_INSTALL_POP_UP Set_Lub_Inventory_Pop_Sellect { set { m_Lub_Inventory_Pop_Sellect = value; } }

    GameObject Main_Button;
    Transform Button_In_Main; // 휘발유 , 경유 ...

    GameObject Content; // 스크롤뷰 내용
    RectTransform Content_Size;
    Transform Button_In_Scroll; // 고급 주유기, 중급 주유기 ...
    static Lubricator[] m_Arr_Cur_Lub;
    public static Lubricator[] Arr_Cur_Lub { get { return m_Arr_Cur_Lub; } }

    GameObject Cancle_Panel;
    GameObject m_Lub_Install_Pop_Up;

    Vector3 Inst_Pos;
    Vector3 Inst_Size;
    float Inst_Distance;
    Transform Inst_Object;

    void Install_Pop_Up()
    {
        if (m_Lub_Inventory_Pop_Sellect == LUB_INVENTORY_INSTALL_POP_UP.ON
            && m_Lub_Install_Pop_Up.activeSelf == false)
        {
            m_Lub_Install_Pop_Up.SetActive(true);
        }
    }
    void Set_Content()
    {
        if (Is_Change == true)
        {
            Is_Change = false;
            Get_Cur_Lub();
            Content.transform.localPosition = Vector3.zero; // 스크롤 위치 초기화

            Inst_Pos = Button_In_Scroll.localPosition;
            Inst_Size = Button_In_Scroll.localScale;
            Inst_Distance = Button_In_Scroll.GetComponent<RectTransform>().sizeDelta.y;
            for (int i = 0; i < m_Arr_Cur_Lub.Length; i++)
            {
                Inst_Object = GameObject.Instantiate(Button_In_Scroll);
                Inst_Object.SetParent(Content.transform);
                Inst_Object.AddComponent<Lub_Inventory_Button_In_Scroll>().My_Lub = m_Arr_Cur_Lub[i];
                Inst_Object.localPosition = Inst_Pos;
                Inst_Object.localScale = Inst_Size;
                Inst_Pos += Vector3.down * Inst_Distance;
            }
            Content_Size.sizeDelta = new Vector2(0, Inst_Distance * m_Arr_Cur_Lub.Length);
        }
    }
    void Get_Cur_Lub()
    {
        switch (m_Lub_Type)
        {
            case LUBRICATOR_TYPE.GASOLINE:
                m_Arr_Cur_Lub = Item.ArrGasoline;
                break;
            case LUBRICATOR_TYPE.DIESEL:
                m_Arr_Cur_Lub = Item.ArrDissel;
                break;
            case LUBRICATOR_TYPE.ELECTRIC:
                m_Arr_Cur_Lub = Item.ArrElectric;
                break;
            case LUBRICATOR_TYPE.HYDROGEN:
                m_Arr_Cur_Lub = Item.ArrHydrogen;
                break;
            case LUBRICATOR_TYPE.BIO:
                m_Arr_Cur_Lub = Item.ArrBio;
                break;
            case LUBRICATOR_TYPE.NONE:
                m_Arr_Cur_Lub = null;
                break;
            default:
                break;
        }
    } // 현재 눌린 버튼에 따라 주유기 종류 받아옴 , 위에있는 Set_Content() 에서 사용
    void Set_Main_Button()
    {
        Button_In_Main.AddComponent<Lub_Inventory_Button_In_Main>().Lub_Type = LUBRICATOR_TYPE.GASOLINE;

        Inst_Pos = Button_In_Main.localPosition;
        Inst_Size = Button_In_Main.localScale;
        Inst_Distance = Button_In_Main.GetComponent<RectTransform>().sizeDelta.x;
        for (int i = 1; i < (int)LUBRICATOR_TYPE.NONE; i++)
        {
            Inst_Pos += Vector3.right * Inst_Distance;
            Inst_Object = GameObject.Instantiate(Button_In_Main);
            Inst_Object.SetParent(Main_Button.transform);
            Inst_Object.AddComponent<Lub_Inventory_Button_In_Main>().Lub_Type = (LUBRICATOR_TYPE)i;
            Inst_Object.localPosition = Inst_Pos;
            Inst_Object.localScale = Inst_Size;
        }
    }
    void Set_Child()
    {
        Cancle_Panel = transform.Find("Cancle Panel").gameObject;
        Cancle_Panel.AddComponent<Lub_Inventory_Cancle_Panel>();
        m_Lub_Install_Pop_Up = transform.Find("Lub Install Pop").gameObject;
        m_Lub_Install_Pop_Up.AddComponent<Lub_Inventory_Install_Pop_Up>();
        Main_Button = transform.Find("Main Button").gameObject;
        Button_In_Main = Main_Button.transform.GetChild(0); // 엔진에 만들어놓은 첫번째 버튼 가져옴 , 이거로 복사해서 나머지 버튼 만든다
        Content = transform.Find("Scroll View").GetChild(0).GetChild(0).gameObject;
        Content_Size = Content.GetComponent<RectTransform>();
        Button_In_Scroll = Content.transform.GetChild(0); // 엔진에 만들어 놓은 첫번쨰 스크롤 버튼
    }
    void Exit_Me()
    {
        if(Grounds_Menu_Canvas.Lub_Inventory_Sellect == ON_OFF.OFF)
        {
            m_Lub_Type = LUBRICATOR_TYPE.GASOLINE;
            m_Lub_Inventory_Pop_Sellect = LUB_INVENTORY_INSTALL_POP_UP.OFF;
            Pop_Up_Reset();
            gameObject.SetActive(false);
        }        
    }
    void Pop_Up_Reset()
    {
        m_Lub_Install_Pop_Up.SetActive(false);
    } // 종료되면 팝업 끄기
    private void Awake()
    {
        Set_Child();
        Set_Main_Button();
    }
    private void LateUpdate()
    {
        Exit_Me();
        Set_Content();
        Install_Pop_Up();
    }
}

#endregion

#region Lub Change Pop Up
public class Lub_Change_Store_Button : MonoBehaviour, IPointerClickHandler
{
    Grounds Sellected_Grounds;
    public void OnPointerClick(PointerEventData eventData)
    {
        Sellected_Grounds = PlayerScript.Instance.m_Grounds[Grounds_Menu_Canvas.Sellected_Grounds_Index]; // 선택된 부지 불러오고
        Sellected_Grounds.Installed_Lub.Add_Installed_Amount(-1); // 그 주유기의 설치되어있는 갯수 하나 뺴고
        Sellected_Grounds.Install_Lub(null); //부지에 설치된 주유기 없음으로 바꾸고
        Grounds_Menu_Canvas.Set_Lub_Change_Sellect = ON_OFF.OFF; //팝업닫고
    }
}

public class Lub_Change_Move_button : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Grounds_Menu_Canvas.Set_Ground_Menu_Sellect = GROUNDS_MAIN_POP_UP_SELLECT.LUB_MOVE;
        Grounds_Menu_Canvas.Set_Move_Grounds_Index = Grounds_Menu_Canvas.Sellected_Grounds_Index;
        Buy_Station.Reset_Grounds(); // Buy_Station 깜빡이는거 초기화
        Grounds_Menu_Canvas.Set_Lub_Change_Sellect = ON_OFF.OFF;
    }
}

public class Lub_Change_Change_Button : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Grounds_Menu_Canvas.Set_Lub_Change_Sellect = ON_OFF.OFF;
        Grounds_Menu_Canvas.Set_Lub_Inventory_Sellect = ON_OFF.ON;
        Lub_Inventory.Is_Change = true;
    }
}

public class Lub_Change_Cancle_Panel : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Grounds_Menu_Canvas.Set_Lub_Change_Sellect = ON_OFF.OFF;
        
    }
}

public class Lub_Change_Pop_Up : MonoBehaviour
{
    GameObject Cancle_Panel;
    GameObject Change_Button;
    GameObject Move_button;
    GameObject Store_Button;
    void Exit_Me()
    {
        if (Grounds_Menu_Canvas.Lub_Change_Sellect == ON_OFF.OFF)
        {
            gameObject.SetActive(false);
        }
    }
    void Child_Setting()
    {
        Cancle_Panel = transform.Find("Cancle Panel").gameObject;
        Cancle_Panel.AddComponent<Lub_Change_Cancle_Panel>();
        Change_Button = transform.Find("Change Button").gameObject;
        Change_Button.AddComponent<Lub_Change_Change_Button>();
        Move_button = transform.Find("Move Button").gameObject;
        Move_button.AddComponent<Lub_Change_Move_button>();
        Store_Button = transform.Find("Store Button").gameObject;
        Store_Button.AddComponent<Lub_Change_Store_Button>();
    }

    private void Awake()
    {
        Exit_Me();
        Child_Setting();
    }
    private void LateUpdate()
    {
        Exit_Me();
    }
}

#endregion

#region Lub Install Pop Up
public class Lub_Install_Install_Button : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Grounds_Menu_Canvas.Set_Lub_Install_Sellect = ON_OFF.OFF;
        Grounds_Menu_Canvas.Set_Lub_Inventory_Sellect = ON_OFF.ON;
        Lub_Inventory.Is_Change = true;
    }
}
public class Lub_Install_Cancle_Panel : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Grounds_Menu_Canvas.Set_Lub_Install_Sellect = ON_OFF.OFF;
    }
}
public class Lub_Install_Pop_Up : MonoBehaviour
{
    GameObject Cancle_Panel;
    GameObject Install_Button;

    void Exit_Me()
    {
        if(Grounds_Menu_Canvas.Lub_Install_Sellect == ON_OFF.OFF)
        {
            gameObject.SetActive(false);
        }
    }
    void Child_Setting()
    {
        Cancle_Panel = transform.Find("Cancle Panel").gameObject;
        Cancle_Panel.AddComponent<Lub_Install_Cancle_Panel>();
        Install_Button= transform.Find("Install Button").gameObject;
        Install_Button.AddComponent<Lub_Install_Install_Button>();
    }

    private void Awake()
    {
        Exit_Me();
        Child_Setting();
    }
    private void LateUpdate()
    {
        Exit_Me();
    }
}
#endregion

#region Buy Grounds Pop Up
public class Buy_Grounds_Buy_Button : MonoBehaviour, IPointerClickHandler
{
    TMP_Text Value;
    Grounds Sellected_Grounds; 
    
    void Calculate()
    {
        Sellected_Grounds.On_Grounds();
        PlayerScript.Instance.m_Money -= Sellected_Grounds.Value;
        Debug.Log(PlayerScript.Instance.m_Money);
    }
    void OnEnable()
    {
        Value = transform.GetChild(0).GetComponent<TMP_Text>();
        if (PlayerScript.Instance.m_Grounds == null)
        {
            Debug.Log("버그 ㅅㄱ람지");
        }
        Sellected_Grounds = PlayerScript.Instance.m_Grounds[Grounds_Menu_Canvas.Sellected_Grounds_Index];
        Value.text = Sellected_Grounds.Value.ToString();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Calculate();
        Grounds_Menu_Canvas.Set_Buy_Grounds_Sellect = ON_OFF.OFF;
    }
}
public class Buy_Grounds_Cancle_Panel : MonoBehaviour ,IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Grounds_Menu_Canvas.Set_Buy_Grounds_Sellect = ON_OFF.OFF;
    }

    
}
public class Buy_Grounds_Pop_Up : MonoBehaviour
{
    GameObject m_Cancle_Panel;
    GameObject m_Buy_Button;
    TMP_Text m_Gold_Text;
    public static GameObject Complete_Pop_Up;

    void Exit_Me()
    {
        if (Grounds_Menu_Canvas.Buy_Grounds_Sellect == ON_OFF.OFF)
        {
            gameObject.SetActive(false);
        }
    }
    void Insert_Component_In_Child()
    {
        m_Cancle_Panel.AddComponent<Buy_Grounds_Cancle_Panel>();
        m_Buy_Button.AddComponent<Buy_Grounds_Buy_Button>();
    }
    void Get_Child()
    {
        m_Cancle_Panel = transform.Find("Cancle Panel").gameObject;
        m_Buy_Button = transform.Find("Buy Button").gameObject;
        m_Gold_Text = transform.Find("Gold Text").gameObject.GetComponent<TMP_Text>();
    }
    private void Awake()
    {
        Get_Child();
        Insert_Component_In_Child();
        
    }
    private void OnEnable()
    {
        m_Gold_Text.text = "보유금액 : " + PlayerScript.Instance.m_Money;
    }
    private void LateUpdate()
    {
        Exit_Me();
    }
}

#endregion //Buy Grounds Pop Up 부지를 구매하시겠습니까?

//부지구매 주유기 설정 서로 변경 하는 버튼
public class Grounds_Main_Change_Button : MonoBehaviour, IPointerClickHandler
{
    TMP_Text Text;

    void Set_Text()
    {
        switch (Grounds_Menu_Canvas.Ground_Menu_Sellect)
        {
            case GROUNDS_MAIN_POP_UP_SELLECT.BUY_GROUNDS:
                Text.text = "주유기 설정";
                break;
            case GROUNDS_MAIN_POP_UP_SELLECT.LUB_SETTING:
                Text.text = "부지 구매";
                break;
            case GROUNDS_MAIN_POP_UP_SELLECT.LUB_MOVE:
                Text.text = "이동 취소";
                break;
            default:
                break;
        }
    } // 부지구매일 경우 주유기 설정으로 

    void Disable_Me()
    {
        if(Grounds_Menu_Canvas.Ground_Menu_Sellect == GROUNDS_MAIN_POP_UP_SELLECT.ON)
        {
            gameObject.SetActive(false);
        }
    }
    void LateUpdate()
    {
        Set_Text();
        Disable_Me();
    }
    void OnEnable()
    {
        Set_Text();
    }
    void Awake()
    {
        Text = transform.GetChild(0).GetComponent<TMP_Text>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Buy_Station.Reset_Grounds();
        switch (Grounds_Menu_Canvas.Ground_Menu_Sellect)
        {
            case GROUNDS_MAIN_POP_UP_SELLECT.BUY_GROUNDS:
                Grounds_Menu_Canvas.Set_Ground_Menu_Sellect = GROUNDS_MAIN_POP_UP_SELLECT.LUB_SETTING;
                break;
            case GROUNDS_MAIN_POP_UP_SELLECT.LUB_SETTING:
                Grounds_Menu_Canvas.Set_Ground_Menu_Sellect = GROUNDS_MAIN_POP_UP_SELLECT.BUY_GROUNDS;
                break;
            case GROUNDS_MAIN_POP_UP_SELLECT.LUB_MOVE:
                Grounds_Menu_Canvas.Set_Move_Grounds_Index = -1;
                Grounds_Menu_Canvas.Set_Ground_Menu_Sellect = GROUNDS_MAIN_POP_UP_SELLECT.LUB_SETTING;
                break;
            default:
                break;
        }
    }
}

//부지 구매 , 주유기 설정
#region Grounds Main Pop Up

public class Lub_Setting_Button : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Grounds_Menu_Canvas.Set_Ground_Menu_Sellect = GROUNDS_MAIN_POP_UP_SELLECT.LUB_SETTING;
    }
}
public class Buy_Grounds_Button : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Grounds_Menu_Canvas.Set_Ground_Menu_Sellect = GROUNDS_MAIN_POP_UP_SELLECT.BUY_GROUNDS;
    }
}
public class Grounds_Menu_Cancle_Panel : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Grounds_Menu_Canvas.IsReset= true;
    }
}
public class Grounds_Main_Pop_Up : MonoBehaviour
{
    GameObject m_Lub_Setting; // 주유기 세팅 버튼
    GameObject m_Buy_Grounds; // 부지 구매 버튼
    GameObject m_Cancle_Panel; // 빈공간 누르면 취소

    void Exit_Me()
    {
        if (Grounds_Menu_Canvas.Ground_Menu_Sellect == GROUNDS_MAIN_POP_UP_SELLECT.BUY_GROUNDS
            || Grounds_Menu_Canvas.Ground_Menu_Sellect == GROUNDS_MAIN_POP_UP_SELLECT.LUB_SETTING)
        {
            gameObject.SetActive(false);
        }
    }
    void AddComponent_In_Child()
    {
        m_Cancle_Panel.AddComponent<Grounds_Menu_Cancle_Panel>();
        m_Buy_Grounds.AddComponent<Buy_Grounds_Button>();
        m_Lub_Setting.AddComponent<Lub_Setting_Button>();
    }
    void Find_Child()
    {
        m_Cancle_Panel = transform.Find("Cancle Panel").gameObject;
        m_Buy_Grounds = transform.Find("Buy Grounds").gameObject;
        m_Lub_Setting = transform.Find("Lub Setting").gameObject;
    }
    private void Awake()
    {
        Exit_Me();
        Find_Child();
        AddComponent_In_Child();
    }

    private void LateUpdate()
    {
        Exit_Me(); // 선택되면 팝업 종료
    }
}

#endregion  //Grounds Main Pop Up

public class Grounds_Menu_Exit : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Grounds_Menu_Canvas.IsReset = true;
    }
}

// 부지 메뉴
public class Grounds_Menu_Canvas : MonoBehaviour
{
    //Lub_Inventory
    static ON_OFF m_Lub_Inventory_Sellect = ON_OFF.OFF;
    public static ON_OFF Lub_Inventory_Sellect { get { return m_Lub_Inventory_Sellect; } }
    public static ON_OFF Set_Lub_Inventory_Sellect { set { m_Lub_Inventory_Sellect = value; } }
    
    //Lub_Change_Pop_Up
    static ON_OFF m_Lub_Change_Sellect = ON_OFF.OFF;
    public static ON_OFF Lub_Change_Sellect { get { return m_Lub_Change_Sellect; } }
    public static ON_OFF Set_Lub_Change_Sellect { set { m_Lub_Change_Sellect= value; } }

    static int m_Move_Grounds_Index = -1; //Move_Grounds 에서 사용
    public static int Move_Grounds_Index { get { return m_Move_Grounds_Index; } }
    public static int Set_Move_Grounds_Index { set { m_Move_Grounds_Index= value; } }

    //Lub_Install_Pop_UP
    static ON_OFF m_Lub_Install_Sellect = ON_OFF.OFF;
    public static ON_OFF Lub_Install_Sellect { get { return m_Lub_Install_Sellect; } }
    public static ON_OFF Set_Lub_Install_Sellect { set { m_Lub_Install_Sellect = value; } }

    //Buy_Grounds_Pop_Up
    static ON_OFF m_Buy_Grounds_Sellect = ON_OFF.OFF;
    public static ON_OFF Buy_Grounds_Sellect { get { return m_Buy_Grounds_Sellect; } }
    public static ON_OFF Set_Buy_Grounds_Sellect { set { m_Buy_Grounds_Sellect = value; } }

    //부지 선택 - Buy_Station 클래스
    static bool m_Is_Input = false; //입력이 있었는가?
    public static bool Is_Input { get { return m_Is_Input; } }
    static int m_Sellected_Grounds_Index = -1; // 선택된 부지의 인덱스
    public static int Sellected_Grounds_Index { get { return m_Sellected_Grounds_Index; } }

    // 부지구매, 주유기설정 Change 버튼 - Grounds_Main_Change_Button

    // 부지구매, 주유기설정 - Main_Pop_Up
    static GROUNDS_MAIN_POP_UP_SELLECT m_Ground_Menu_Sellect = GROUNDS_MAIN_POP_UP_SELLECT.ON;
    public static GROUNDS_MAIN_POP_UP_SELLECT Ground_Menu_Sellect { get { return m_Ground_Menu_Sellect; } }
    public static GROUNDS_MAIN_POP_UP_SELLECT Set_Ground_Menu_Sellect { set { m_Ground_Menu_Sellect = value; } }
    

    public static bool IsReset; // 부지설정 끝나는 모든곳 enum static 초기화


    GameObject m_Main_Pop_Up;
    GameObject m_Main_Change_Button;
    GameObject m_Buy_Grounds_Pop_Up;
    GameObject m_Lub_Install_Pop_Up;
    GameObject m_Lub_Change_Pop_Up;
    GameObject m_Exit; // Main_Pop_Up 에서는 안뜨고 선택후부터 뜸 (돈, 메뉴버튼 안보이게 한 다음부터)
    GameObject m_Lub_Inventory;

    void Lub_Inventory()
    {
        if (m_Lub_Inventory_Sellect == ON_OFF.ON
            && m_Lub_Inventory.activeSelf == false)
        {
            m_Lub_Inventory.SetActive(true);
        }
    }

    void Lub_Move()
    {
        if (m_Ground_Menu_Sellect == GROUNDS_MAIN_POP_UP_SELLECT.LUB_MOVE
            && m_Is_Input == true)
        {
            m_Is_Input = false;
            if (Move_Grounds_Index != -1)
            {
                PlayerScript.Instance.m_Grounds[Sellected_Grounds_Index].Install_Lub(
                    PlayerScript.Instance.m_Grounds[Move_Grounds_Index].Installed_Lub); //선택한 땅에 주유기 추가하고
                PlayerScript.Instance.m_Grounds[Move_Grounds_Index].Install_Lub(null); //원래 땅에 주유기 없애고
                Buy_Station.Reset_Grounds();//Buy_Station 화면표시 리셋 (깜빡이는거 통일성을 위해)
                m_Move_Grounds_Index = -1; // 선택 초기화
                m_Ground_Menu_Sellect = GROUNDS_MAIN_POP_UP_SELLECT.LUB_SETTING; // 이동 완료했으니 주유기 세팅으로 되돌리기
            } // Move인 경우에는 선택시 주유기를 이동시키고 팝업은 열지않음
        }
    }
    void Lub_Install_Or_Change()
    {
        if (m_Ground_Menu_Sellect == GROUNDS_MAIN_POP_UP_SELLECT.LUB_SETTING
            && m_Is_Input == true)
        {
            m_Is_Input = false;
            if (PlayerScript.Instance.m_Grounds[Sellected_Grounds_Index].Installed_Lub == null)
            {
                m_Lub_Install_Sellect = ON_OFF.ON;
                m_Lub_Install_Pop_Up.SetActive(true);
            }
            else
            {
                m_Lub_Change_Sellect = ON_OFF.ON;
                m_Lub_Change_Pop_Up.SetActive(true);  
            }
            
            
        }
    }
    void Grounds_Buy_Pop_Up()
    {
        if (m_Ground_Menu_Sellect == GROUNDS_MAIN_POP_UP_SELLECT.BUY_GROUNDS
            && m_Is_Input == true)
        {
            m_Is_Input = false;
            m_Buy_Grounds_Sellect = ON_OFF.ON;
            m_Buy_Grounds_Pop_Up.SetActive(true);
        }
    }//부지구매 On - 메인에서 부지구매 누르고 , 부지  클릭되고 ,  BUY_GROUNDS_POP_UP.ON 이면 (스크립트 내에서 설정)

    // 부지 깜빡이는거 선택
    #region Sellect Actived Grounds
    
    string Number_Extract;
    Ray ray;
    RaycastHit hit;
    void Grounds_Sellect()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition); // 화면 터치하는 위치 저장.

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit))
            {
                /* 1. 주유소 칸 상자를 클릭한 값을 저장하게 함.
                 * 2. 주유소 상자들을 눈앞에서 사라지게 해야함. 이게 사라지는 순서가 언제인지가 중요한듯. 사실상 빈공간 클릭했는데
                 *      사라지면 안되니깐.
                 * 3. 주유소 상자들이 사라진 후 그 자리에 주유기가 추가해야함 
                    4. case로 항목을 선택지로 추가하되 case전에 공통된 항목으로 2번은 가능할듯.
                 */
                if (hit.transform.gameObject.GetComponent<Animator>() != null)
                {
                    if(EventSystem.current.IsPointerOverGameObject() == false)
                    {
                        Number_Extract = Regex.Replace(hit.transform.gameObject.name, @"\D", ""); // 게임오브젝트에서 숫자만 추출
                        m_Sellected_Grounds_Index = int.Parse(Number_Extract);
                        m_Is_Input = true;
                    }
                    
                }

            }
        }
    }
    #endregion  // 부지 깜빡이는거 선택
    
    void Main_Change_Button()
    {
        if(m_Ground_Menu_Sellect != GROUNDS_MAIN_POP_UP_SELLECT.ON
            && m_Main_Change_Button.activeSelf == false)
        {
            m_Main_Change_Button.SetActive(true);
        }
    }
    void Main_Pop_Up()
    {
        if (m_Ground_Menu_Sellect == GROUNDS_MAIN_POP_UP_SELLECT.ON
            && m_Main_Pop_Up.activeSelf == false)
        {
            m_Main_Pop_Up.SetActive(true);
        }
    } //메인팝업 On - GROUNDS_MAIN_POP_UP_SELLECT.ON 이면 (MenuScipt - Menu_Grounds 에서 설정)  

    void Main_Canvas_And_Exit_OnOff()
    {
        if (m_Ground_Menu_Sellect == GROUNDS_MAIN_POP_UP_SELLECT.ON 
            && Station.Instance.Main_Canvas.activeSelf == false)
        {
            Station.Instance.Main_Canvas.SetActive(true);
            m_Exit.SetActive(false);
        }
        
        else if(m_Ground_Menu_Sellect != GROUNDS_MAIN_POP_UP_SELLECT.ON
            && Station.Instance.Main_Canvas.activeSelf == true)
        {
            Station.Instance.Main_Canvas.SetActive(false);
            m_Exit.SetActive(true);
        }
    } // 부지구매나 주유기 설치할때 메인 캔버스 잠시 끄고 나가기 버튼 켬

    void Insert_Component_In_Child()
    {
        m_Main_Pop_Up = transform.Find("Main Pop Up").gameObject;
        m_Main_Pop_Up.AddComponent<Grounds_Main_Pop_Up>();
        m_Main_Change_Button = transform.Find("Change Button").gameObject;
        m_Main_Change_Button.AddComponent<Grounds_Main_Change_Button>();
        m_Buy_Grounds_Pop_Up = transform.Find("Buy Grounds Pop Up").gameObject;
        m_Buy_Grounds_Pop_Up.AddComponent<Buy_Grounds_Pop_Up>();
        m_Lub_Install_Pop_Up = transform.Find("Lub Install Pop Up").gameObject;
        m_Lub_Install_Pop_Up.AddComponent<Lub_Install_Pop_Up>();
        m_Lub_Change_Pop_Up = transform.Find("Lub Change Pop Up").gameObject;
        m_Lub_Change_Pop_Up.AddComponent<Lub_Change_Pop_Up>();
        m_Exit = transform.Find("Exit").gameObject;
        m_Exit.AddComponent<Grounds_Menu_Exit>();
        m_Lub_Inventory = transform.Find("Lub Inventory").gameObject;
        m_Lub_Inventory.AddComponent<Lub_Inventory>();
    } //자식들 컴포넌트 넣기
    

    void Reset_All_Pop_Up()
    {
        m_Ground_Menu_Sellect = GROUNDS_MAIN_POP_UP_SELLECT.ON;
        m_Buy_Grounds_Sellect = ON_OFF.OFF;
        m_Lub_Install_Sellect = ON_OFF.OFF;
        m_Lub_Change_Sellect = ON_OFF.OFF; 
        m_Sellected_Grounds_Index = -1;
        m_Move_Grounds_Index = -1;
        Main_Canvas_And_Exit_OnOff(); // 메인캔버스(돈, 메뉴버튼) 켜고 나가기버튼 없애고
    } // 부지설정 끝나는 모든곳 enum static 초기화
    void Reset_And_Exit()
    {
        if (IsReset == true)
        {
            IsReset = false;
            Reset_All_Pop_Up();
            gameObject.SetActive(false);
        }
    }

    private void Awake()
    {
        Insert_Component_In_Child();
        Reset_All_Pop_Up();
        m_Exit.SetActive(false); // Reset 에서는 메인 캔버스가 TRUE인 상태라 적용이 안됨
    }
    private void Update()
    {
        Main_Pop_Up();
    }
    private void LateUpdate()
    {
        Main_Change_Button();
        Main_Canvas_And_Exit_OnOff(); // 부지선택할때 메인캔버스(돈 메뉴버튼 등) 방해되서 끄고 나가기 버튼 켜는 기능
        Grounds_Sellect(); // 부지선택
        Grounds_Buy_Pop_Up(); //부지를 구매하시겟습니까?
        Lub_Install_Or_Change(); // 주유기 유무에 따라 설치 or 변경 팝업
            Lub_Move();
        Lub_Inventory(); // 주유기 인벤토리 띄우기
        Reset_And_Exit();
    }
}

#endregion //Grounds_Menu_Canvas 하위

// 선언은 여기서 되지만 사용은 위에 있는 Grounds_Menu_Canvas 에서 한다
public class Buy_Station : MonoBehaviour
{
    static GameObject[] Arr_Buy_Area;

    public static void Reset_Grounds()
    {
        for (int i = 0; i < PlayerScript.Instance.m_Grounds.Length; i++)
        {
            Arr_Buy_Area[i].SetActive(false);
        }
    }
    void Just_Available_Grounds()
    {
        if (Grounds_Menu_Canvas.Ground_Menu_Sellect == GROUNDS_MAIN_POP_UP_SELLECT.BUY_GROUNDS)
        {
            for (int i = 0; i < PlayerScript.Instance.m_Grounds.Length; i++)
            {
                if (PlayerScript.Instance.m_Grounds[i].IsOn == false
                    && Arr_Buy_Area[i].activeSelf == false)
                {
                    Arr_Buy_Area[i].SetActive(true);
                }
                else if(PlayerScript.Instance.m_Grounds[i].IsOn == true
                    && Arr_Buy_Area[i].activeSelf == true)
                {
                    Arr_Buy_Area[i].SetActive(false);
                }
            }
        }
        else if(Grounds_Menu_Canvas.Ground_Menu_Sellect == GROUNDS_MAIN_POP_UP_SELLECT.LUB_SETTING)
        {
            for (int i = 0; i < PlayerScript.Instance.m_Grounds.Length; i++)
            {
                if (PlayerScript.Instance.m_Grounds[i].IsOn == true
                    && Arr_Buy_Area[i].activeSelf == false)
                {
                    Arr_Buy_Area[i].gameObject.SetActive(true);
                }
                else if (PlayerScript.Instance.m_Grounds[i].IsOn == false
                    && Arr_Buy_Area[i].activeSelf == true)
                {
                    Arr_Buy_Area[i].gameObject.SetActive(false);
                }
            }
        }
        else if(Grounds_Menu_Canvas.Ground_Menu_Sellect == GROUNDS_MAIN_POP_UP_SELLECT.LUB_MOVE)
        {
            for (int i = 0; i < PlayerScript.Instance.m_Grounds.Length; i++)
            {
                if (PlayerScript.Instance.m_Grounds[i].IsOn ==true
                    && PlayerScript.Instance.m_Grounds[i].Installed_Lub == null
                    && Arr_Buy_Area[i].activeSelf == false)
                {
                    Arr_Buy_Area[i].gameObject.SetActive(true);
                }
            }
        }
        else
        {
            for (int i = 0; i < PlayerScript.Instance.m_Grounds.Length; i++)
            {
                if (Arr_Buy_Area[i].gameObject.activeSelf == true)
                {
                    Arr_Buy_Area[i].gameObject.SetActive(false);
                }
            }
        }

    } // Grounds_Main_Pop_Up 선택에 따라 클릭 가능한 부지를 활성화
    void Get_Child()
    {
        Arr_Buy_Area = new GameObject[transform.childCount];
        for (int i = 0; i < Arr_Buy_Area.Length; i++)
        {
            Arr_Buy_Area[i] = transform.GetChild(i).gameObject;
        }
        if (Arr_Buy_Area.Length != PlayerScript.Instance.m_Grounds.Length)
        {
            Debug.LogError("if (Arr_Buy_Area.Length != PlayerScript.Instance.m_Grounds.Length) 현재 부지 갯수와 플레이어에 입력된 부지 갯수가 다르다 (플레이어가 소유한 부지아님 false도 포함)");
        }
    } // 활성화 되어있는 자식들만 가져옴 (하위 오브젝트 활성화 필수)

    private void Start()
    {
        Get_Child(); // PlayerScript가 Station 이후에 Awake 되기 때문에
                     // 이 함수를 Awake 에서 처리하면 null 오류 발생
    }

    private void Update()
    {
        Just_Available_Grounds();
        
    }
}

public class Main_Station : MonoBehaviour
{
    Transform[] Children;

    void Station_Activate()
    {
        for (int i = 0; i < PlayerScript.Instance.m_Grounds.Length; i++)
        {
            if (PlayerScript.Instance.m_Grounds[i].IsOn == true
                && Children[i].gameObject.activeSelf == false)
            {
                Children[i].gameObject.SetActive(true);
            }
        }
       
    }
    void Insert_Ground_Position()
    {
        if (Children.Length != Station.Grounds.Length)
        {
            Debug.LogError("        if(Children.Length != Station.Grounds.Length)\r\n");
        }
        for (int i = 0; i < Station.Grounds.Length; i++)
        {
            Station.Grounds[i].Insert_Ground_Position(Children[i].position);
        }
    } // 만들어논 오브젝트 위치를 m_Groonds 에 넣으면 Player가 Awake 로 가져감

    void Get_Station()
    {
        Children = new Transform[transform.childCount];
        for (int i = 0; i < Children.Length; i++)
        {
            Children[i] = transform.GetChild(i);
        }
    } // Station(=자식) 가져옴

    private void Update()
    {
        Station_Activate(); // 게임 시작때 활성화 하고, 게임 중에는 부지를 구매하면 부지 활성화
    }
    private void Awake()
    {
        Get_Station();
        Insert_Ground_Position();
    }
}
public class Station : MonoBehaviour
{
    private static Station _instance;  //싱글톤
    public static Station Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(Station)) as Station;

                if (_instance == null)
                {
                    Debug.Log("싱글톤 오브젝트 없음");
                }
            }

            return _instance;
        }
    }  //싱글톤
    void SingleTone()
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
    } // 싱글톤

    
    public GameObject m_Grounds_Menu_Canvas; // 컴포넌트 적용위해 선언 
    public GameObject Main_Canvas; //부지 선택화면에서 메뉴버튼 끄려고 선언 

    public GameObject m_Buy_Station; // 주유기 설치 칸 보기(부모 오브젝트)
    public GameObject m_Main_Station; // 주유기 설치 되고 나면 보이는 칸 (부모 오브젝트)
    static Grounds[] m_Grounds; // 부지 설정
    public static Grounds[] Grounds { get { return m_Grounds; } } // 플레이어, Buy_Station에게 부지 정보 보냄

    

    void Setting_Grounds()
    {
        m_Grounds = new Grounds[]
        {
            new Grounds(1000), //0번 부지
            new Grounds(2000), //1
            new Grounds(3000), //2
            new Grounds(4000), //3
            new Grounds(5000), //4
            new Grounds(6000), //5
            new Grounds(7000), //6
            new Grounds(8000), //7
            new Grounds(9000), //8
        };
    }

    void Child_Setting()
    {
        m_Buy_Station.AddComponent<Buy_Station>();
        m_Main_Station.AddComponent<Main_Station>();
        m_Grounds_Menu_Canvas.AddComponent<Grounds_Menu_Canvas>();
    } //엔진에서 받아와서 넣어줌

    private void Awake()
    {
        SingleTone();
        Setting_Grounds(); //부지 설정
        Child_Setting();
    }
}

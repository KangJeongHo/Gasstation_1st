using Script;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static Item;
using static UnityEngine.RuleTile.TilingRuleOutput;

#region Pop_Up
public class Shop_Pop_Pop_Up_Panel : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Shop.m_Pop_Pop_Up.SetActive(false);
    }
}
public class Shop_Pop_Pop_Up : MonoBehaviour
{
    public static Item.Item_Info Info;
    static Text Name;
    static Text HasAmount;
    static Text Amount_Text;
    static int Amount_Number;
    GameObject m_Plus;
    GameObject m_Minus;
    GameObject m_Buy_Button;

    public static void Reset_Pop_Pop_Up()
    {
        Amount_Number = 0;
        Amount_Text.text = Amount_Number.ToString();
        UI_Change();
    }
    public static void Calculation()
    {
        Info.AddAmount(Amount_Number);
        Debug.Log(Info.Name + " 가격 : " + Info.Value + " 소지 수 : " + Info.HasAmount);
        Reset_Pop_Pop_Up();
    }
    void Insert_Component_Cancle_Panel()
    {
        transform.Find("Pop_Pop_Up_Cancle_Panel").gameObject.AddComponent<Shop_Pop_Pop_Up_Panel>();
    }
    #region Button
    void Insert_Component_Button()
    {
        m_Minus.AddComponent<Minus>();
        m_Plus.AddComponent<Plus>();
        m_Buy_Button.AddComponent<Buy_Button>();
    }
    class Buy_Button : MonoBehaviour, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            Shop_Pop_Pop_Up.Calculation();
        }
    }
    public static void Amount_Plus()
    {
        Amount_Number += Info.SellAmount;
    }
    public static void Amount_Minus()
    {
        Amount_Number -= Info.SellAmount;
    }
    class Plus : MonoBehaviour, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            Amount_Plus();
            Amount_Text.text = Amount_Number.ToString();
        }
    }
    class Minus : MonoBehaviour, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            if(Amount_Number > 0)
            {
                Amount_Minus();
            }
            Amount_Text.text = Amount_Number.ToString();
        }
    }

    #endregion
    public static void UI_Change()
    {
        Name.text = Info.Name;
        HasAmount.text = "보유 수량 : " + Info.HasAmount;
    }
    void Find_Child()
    {
        Name = transform.Find("Name").GetComponent<Text>();
        HasAmount = transform.Find("HasAmount").GetComponent<Text>();

        Amount_Text = transform.Find("Amount").GetComponent<Text>();
        m_Plus = Amount_Text.transform.Find("Plus").gameObject;
        m_Minus = Amount_Text.transform.Find("Minus").gameObject;

        m_Buy_Button = transform.Find("Buy Button").gameObject;
    }
    private void Awake()
    {
        Insert_Component_Cancle_Panel();
        Find_Child();
        Insert_Component_Button();
    }
}
//상점 계산기
public class Item_Icon : MonoBehaviour, IPointerClickHandler
{
    public Item.Item_Info Info;

    // 여기서 아이콘 이미지 넣거나 이름 바꾸거나 

    public void OnPointerClick(PointerEventData eventData)
    {
        Shop.m_Pop_Pop_Up.SetActive(true);
        //정보 삽입
        Shop_Pop_Pop_Up.Info = Info;
        //Pop_Pop_Up 초기화
        Shop_Pop_Pop_Up.Reset_Pop_Pop_Up();
    }

    // Awake 에서 처리하면 Info 값 불러오기전에 Awake 부터 실행되서 Info.Name 이 null 오류남
    void Start()
    {
        Debug.Log(GetComponentInChildren<Text>().name);
        Debug.Log(Info.Name);
        GetComponentInChildren<Text>().text = Info.Name;
    }
    
}

public class Shop_Merchandise : MonoBehaviour
{
    Text My_Name;

    static bool IsChange;

    float Contents_Height;
    int Col = 3;
    int Row;

    int Merchandise_DisY = 100;
    int Merchandise_DisX = 75;
    Vector3 Merchandise_Pos;

    // 버튼 포지션 초기값 = 첫번째 버튼
    Vector3 Reset_Merchandise_Pos;
    int Merchandise_PosY = -70;
    int Merchandise_PosX = -75;

    //버튼 갯수 == 상품 갯수 
    public static int Merchandise_Amount;

    Item_Icon m_Item_Icon;
    GameObject m_Merchandise;
    static GameObject Contents;

    // 상점이름 이미지로 바꾸면서 Type 오브젝트 지웠으면 삭제
    void Shop_Name_Change_Ready()
    {
        if(transform.parent.Find("Type") == null)
        {
            Debug.LogError("상점이름 이미지로 바꾸면서 Type 오브젝트 지웠으면 Shop_Name_Change() 삭제 필요");
        }
        My_Name = transform.parent.Find("Type").GetComponent<Text>();
    }
    // 상점 변경 허가 함수  각 상점 클릭버튼에서 선언
    public static void Change()
    {
        IsChange = true;
    }
    void Get_Part_Timer_Type()
    {
        switch (Shop.Which_Part_Timer_Type_Open)
        {
            case PART_TIMER_TYPE.NOMAL:
                My_Name.text = "일반 알바";
                break;
            case PART_TIMER_TYPE.ADVANCED:
                My_Name.text = "고급 알바";
                break;
            case PART_TIMER_TYPE.ZIZON:
                My_Name.text = "지존 알바";
                break;
            default:
                break;
        }
    }

   
    void Get_Lubricator_Type()
    {
        switch (Shop.Which_Lub_Type_Open)
        {
            case LUBRICATOR_TYPE.GASOLINE:
                My_Name.text = "가솔린";
                Merchandise_Amount = Item.ArrGasoline.Length;
                Create_Merchandise_Buttons(Item.ArrGasoline);
                break;
            case LUBRICATOR_TYPE.DIESEL:
                My_Name.text = "디젤";
                Merchandise_Amount = Item.ArrDissel.Length;
                Create_Merchandise_Buttons(Item.ArrDissel);
                break;
            case LUBRICATOR_TYPE.ELECTRIC:
                My_Name.text = "전기";
                Merchandise_Amount = Item.ArrElectric.Length;
                Create_Merchandise_Buttons(Item.ArrElectric);
                break;
            case LUBRICATOR_TYPE.HYDROGEN:
                My_Name.text = "수소";
                Merchandise_Amount = Item.ArrHydrogen.Length;
                Create_Merchandise_Buttons(Item.ArrHydrogen);
                break;
            case LUBRICATOR_TYPE.BIO:
                My_Name.text = "바이오";
                Merchandise_Amount = Item.ArrBio.Length;
                Create_Merchandise_Buttons(Item.ArrBio);
                break;
            default:
                break;
        }
    }
    
    void Create_Merchandise_Buttons(Item_Info[] _Info)
    {
        for (int i = 0; i < Merchandise_Amount; i++)
        {
            // 행이 다 차면 다음 행으로 이동
            if (i != 0 && i % Col == 0)
            {
                Merchandise_Pos.x = Reset_Merchandise_Pos.x;
                Merchandise_Pos.y -= Merchandise_DisY;
            }
            Contents_Size_Setting();

            m_Merchandise = GameObject.Instantiate(Shop.Inst.Pop_Up_Merchandise);
            m_Merchandise.transform.SetParent(Contents.transform);
            m_Merchandise.GetComponent<RectTransform>().anchoredPosition = Merchandise_Pos;
            Merchandise_Pos.x += Merchandise_DisX;
            m_Item_Icon = m_Merchandise.AddComponent<Item_Icon>();
            m_Item_Icon.Info = _Info[i];
        }
    }

    void Reset_Object()
    {
        for (int i = 0; i <Contents.transform.childCount ; i++)
        {
            Destroy(Contents.transform.GetChild(i).gameObject);
        }
        Merchandise_Pos = Reset_Merchandise_Pos;
    }
    void Create_Merchandise()
    {
        if (true == IsChange)
        {
            Reset_Object();
            // Shop_Type 확인
            switch (Shop.Witch_Shop_Open)
            {
                case SHOP_TYPE.LUBRICATOR:
                    Get_Lubricator_Type();
                    break;
                case SHOP_TYPE.PART_TIMER:
                    Get_Part_Timer_Type();
                    break;
                default:
                    break;
            }

            // 업데이트에서 한번만 바뀌도록 트리거
            IsChange = false;
        }
    }

    void Contents_Size_Setting()
    {
        Row = Merchandise_Amount / Col;
        if(0 != Merchandise_Amount % Col)
        {
            Row += 1;
        }
        Contents_Height = 130 + 100 * (Row - 1);
        Contents.GetComponent<RectTransform>().sizeDelta = new Vector2(0, Contents_Height);
    }

    private void Awake()
    {
        m_Merchandise = new GameObject();
        Contents = new GameObject();
        Reset_Merchandise_Pos = new Vector3(Merchandise_PosX, Merchandise_PosY, 0);
        Merchandise_Pos = new Vector3(Merchandise_PosX, Merchandise_PosY, 0);

        // 스크롤뷰 Content찾기
        Contents = transform.GetChild(0).GetChild(0).GetChild(0).gameObject;
        

        Shop_Name_Change_Ready();
    }

    
    private void LateUpdate()
    {
        // Update 에서 처리하니까 상점 내용이 변하는게 한프레임 보임
        Create_Merchandise();
    }
}

public class Shop_Pop_Up_Panel : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Shop.m_Pop_Up.SetActive(false);
    }
}

public class Shop_Pop_Up : MonoBehaviour
{

    void Insert_Component_Merchandise()
    {
        transform.Find("Merchandise").gameObject.AddComponent<Shop_Merchandise>();
    }
    void Insert_Component_Cancle_Panel()
    {
        transform.Find("Pop_Up_Cancle_Panel").gameObject.AddComponent<Shop_Pop_Up_Panel>();
    }
    private void Awake()
    {
        Insert_Component_Merchandise();
        Insert_Component_Cancle_Panel();
    }
}

#endregion
#region Part_Timer
public enum PART_TIMER_TYPE
{
    //NONE 항상 마지막에 두어야함
    NOMAL,
    ADVANCED,
    ZIZON,
    NONE,
}
public class Part_Timer_Type : MonoBehaviour,IShop_Type ,IPointerClickHandler
{

    //메뉴 순서
    public PART_TIMER_TYPE m_Part_Timer_Type;


    public void Open_Pop_Up()
    {
        Shop.m_Pop_Up.SetActive(true);
    }

    public void Request_Shop_Type_Change()
    {
        Shop_Merchandise.Change();
    }

    public void Select_Pop_Up_Menu()
    {
        switch (m_Part_Timer_Type)
        {
            case PART_TIMER_TYPE.NOMAL:
                Shop.Which_Part_Timer_Type_Open = PART_TIMER_TYPE.NOMAL;
                break;
            case PART_TIMER_TYPE.ADVANCED:
                Shop.Which_Part_Timer_Type_Open = PART_TIMER_TYPE.ADVANCED;
                break;
            case PART_TIMER_TYPE.ZIZON:
                Shop.Which_Part_Timer_Type_Open = PART_TIMER_TYPE.ZIZON;
                break;
            default:
                break;
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Open_Pop_Up();
        Request_Shop_Type_Change();
        Select_Pop_Up_Menu();
    }
    void Awake()
    {
    }
}
public class Part_Timer_Canvas : MonoBehaviour , IShop_Canvas
{
    GameObject Part_Timer_Type = null;

   
    public void Insert_Component_Type()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Part_Timer_Type = transform.GetChild(i).gameObject;

            Part_Timer_Type Cur_Type = Part_Timer_Type.AddComponent<Part_Timer_Type>();
            Cur_Type.m_Part_Timer_Type = (PART_TIMER_TYPE)i; // 메뉴 타입 넣어줌
        }
    }
    public void Awake()
    {
        Part_Timer_Type = new GameObject();
        Insert_Component_Type();
    }
    public void LateUpdate()
    {
        if (Shop.Witch_Shop_Open != SHOP_TYPE.PART_TIMER)
        {
            gameObject.SetActive(false);
        }
    }

}

public class Part_Timer_Button : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Shop.Witch_Shop_Open = SHOP_TYPE.PART_TIMER;
        Shop.Inst.m_Part_Shop_Canvas.SetActive(true);
    }
}

#endregion

#region Lubricator

public enum LUBRICATOR_TYPE
{
    //NONE 항상 마지막에 두어야함
    GASOLINE,
    DIESEL,
    ELECTRIC,
    HYDROGEN,
    BIO,
    NONE,
} 
public class Lubricator_Type : MonoBehaviour , IShop_Type, IPointerClickHandler
{
    public LUBRICATOR_TYPE Lub_Type;

    public void Open_Pop_Up()
    {
        Shop.m_Pop_Up.SetActive(true);
    }
    public void Select_Pop_Up_Menu()
    {
        switch (Lub_Type)
        {
            case LUBRICATOR_TYPE.GASOLINE:
                Shop.Which_Lub_Type_Open = LUBRICATOR_TYPE.GASOLINE;
                break;
            case LUBRICATOR_TYPE.DIESEL:
                Shop.Which_Lub_Type_Open = LUBRICATOR_TYPE.DIESEL;
                break;
            case LUBRICATOR_TYPE.ELECTRIC:
                Shop.Which_Lub_Type_Open = LUBRICATOR_TYPE.ELECTRIC;
                break;
            case LUBRICATOR_TYPE.HYDROGEN:
                Shop.Which_Lub_Type_Open = LUBRICATOR_TYPE.HYDROGEN;
                break;
            case LUBRICATOR_TYPE.BIO:
                Shop.Which_Lub_Type_Open = LUBRICATOR_TYPE.BIO;
                break;
            default:
                break;
        }
    }

    public void Request_Shop_Type_Change()
    {
        Shop_Merchandise.Change();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Select_Pop_Up_Menu();
        Open_Pop_Up();

        // 상점 내용 변경 허가
        Request_Shop_Type_Change();
    }
}
public class Lubricator_Canvas : MonoBehaviour , IShop_Canvas
{
    // 주유기 타입버튼 오브젝트 받기
    GameObject Lub_Type = null;

    

    public void Insert_Component_Type()
    {
        for (int i = 0; i < transform.childCount ; i++)
        {
            Lub_Type = transform.GetChild(i).gameObject;

            Lubricator_Type Cur_Type = Lub_Type.AddComponent<Lubricator_Type>();
            Cur_Type.Lub_Type = (LUBRICATOR_TYPE)i; // 메뉴 타입 넣어줌
        }
    }

    

    public void Awake()
    {
        
        Lub_Type = new GameObject();
        Insert_Component_Type();
       

    }

    public void LateUpdate()
    {
        if (Shop.Witch_Shop_Open != SHOP_TYPE.LUBRICATOR)
        {
            gameObject.SetActive(false);
        }
    }
}


public class Lubricator_Button : MonoBehaviour , IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Shop.Witch_Shop_Open = SHOP_TYPE.LUBRICATOR;
        Shop.Inst.m_Lub_Shop_Canvas.SetActive(true);
    }
}

#endregion


interface IShop_Type
{
    /*  인터페이스 예제
    //메뉴 순서
    public LUBRICATOR_TYPE Lub_Type;

    void Open_Pop_Up()
    {
        Shop.m_Pop_Up.SetActive(true);
    }
    void Select_Pop_Up_Menu()
    {
        switch (Lub_Type)
        {
            case LUBRICATOR_TYPE.GASOLINE:
                Shop.Which_Lub_Type_Open = LUBRICATOR_TYPE.GASOLINE;
                break;
            case LUBRICATOR_TYPE.DIESEL:
                Shop.Which_Lub_Type_Open = LUBRICATOR_TYPE.DIESEL;
                break;
            case LUBRICATOR_TYPE.ELECTRIC:
                Shop.Which_Lub_Type_Open = LUBRICATOR_TYPE.ELECTRIC;
                break;
            case LUBRICATOR_TYPE.HYDROGEN:
                Shop.Which_Lub_Type_Open = LUBRICATOR_TYPE.HYDROGEN;
                break;
            case LUBRICATOR_TYPE.BIO:
                Shop.Which_Lub_Type_Open = LUBRICATOR_TYPE.BIO;
                break;
            default:
                break;
        }
    }

    public void Request_Shop_Type_Change()
    {
        Shop_Merchandise.Change();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Select_Pop_Up_Menu();
        Open_Pop_Up();

        // 상점 내용 변경 허가
        Request_Shop_Type_Change();


    }      */
    void Open_Pop_Up();
    void Select_Pop_Up_Menu();
    void Request_Shop_Type_Change();
}
interface IShop_Canvas
{
    /*  예제
    // 주유기 타입버튼 오브젝트 받기
    GameObject Lub_Type = null;



    void Insert_Component_Type()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Lub_Type = transform.GetChild(i).gameObject;

            Lubricator_Type Cur_Type = Lub_Type.AddComponent<Lubricator_Type>();
            Cur_Type.Lub_Type = (LUBRICATOR_TYPE)i; // 메뉴 타입 넣어줌
        }
    }



    private void Awake()
    {

        Lub_Type = new GameObject();
        Insert_Component_Type();


    }

    private void LateUpdate()
    {
        if (Shop.Witch_Shop_Open != SHOP_TYPE.LUBRICATOR)
        {
            gameObject.SetActive(false);
        }
    }    */



    void Insert_Component_Type();
    void Awake();

    void LateUpdate();
}
public enum SHOP_TYPE 
{
    NONE,
    LUBRICATOR,
    PART_TIMER,
}

//상점 버튼을 위한 스크립트
public class Shop : MonoBehaviour
{
    static Shop m_Inst = null;
    public static Shop Inst { get { return m_Inst; } }

    //Pop_Pop_Up
    public static GameObject m_Pop_Pop_Up = null;
    //Pop_Up 오브젝트 쓰려고 선언
    public static GameObject m_Pop_Up = null;
    public GameObject Pop_Up_Merchandise = null;

    // UI 패널 사이즈 통일을 위해.
    public static Vector2 Main_Canvas_Size { get { return m_Main_Canvas_Size.sizeDelta; } }
    static RectTransform m_Main_Canvas_Size;

    //Part_Timer
    public static PART_TIMER_TYPE Which_Part_Timer_Type_Open = PART_TIMER_TYPE.NONE;
    //Lubricator
    public static LUBRICATOR_TYPE Which_Lub_Type_Open = LUBRICATOR_TYPE.NONE;

    //Main Menu
    public GameObject m_Lub_Shop_Canvas = null;
    public GameObject m_Part_Shop_Canvas = null;

    public GameObject m_Part_Timer_Button = null;
    public GameObject m_Lubricator_Button = null;
    //Exit버튼에 컴포넌트 추가하기위해 삽입 ###프리팹 만들면
    public GameObject m_Shop_Exit_Button = null;


    //다른 메뉴 클릭하면 현재 메뉴 닫히게 , 각 메뉴 캔버스 업데이트에서 처리
    public static SHOP_TYPE Witch_Shop_Open = SHOP_TYPE.NONE;

    void Insert_Component_Pop_Pop_Up()
    {
        m_Pop_Pop_Up = m_Pop_Up.transform.Find("Pop_Pop_Up").gameObject;
        m_Pop_Pop_Up.AddComponent<Shop_Pop_Pop_Up>();
    }
    void Insert_Component_Pop_Up()
    {
        m_Pop_Up = transform.Find("Pop_Up").gameObject;
        m_Pop_Up.AddComponent<Shop_Pop_Up>();
    }

    private void Awake()
    {
        m_Inst= this;

        //UI 패널 사이즈 통일을 위해
        m_Main_Canvas_Size = GetComponent<RectTransform>();

        //Exit 버튼에 컴포넌트 추가 ###프리팹 만들면
        m_Shop_Exit_Button.AddComponent<Shop_Exit>();

        m_Lubricator_Button.AddComponent<Lubricator_Button>();
        m_Part_Timer_Button.AddComponent<Part_Timer_Button>();

        m_Lub_Shop_Canvas.AddComponent<Lubricator_Canvas>();
        m_Part_Shop_Canvas.AddComponent<Part_Timer_Canvas>();

        //Pop_Up 컴포넌트 등록
        Insert_Component_Pop_Up();
        Insert_Component_Pop_Pop_Up();
    }
}

public class Shop_Exit : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        //상점캔버스 닫아서 상점에서 나가기
        MenuScript.Shop_Canvas.SetActive(false);
        //주유소 , 알바 선택 해서 나오는 서브창 초기화
        Shop.Witch_Shop_Open = SHOP_TYPE.NONE;
    }
}
